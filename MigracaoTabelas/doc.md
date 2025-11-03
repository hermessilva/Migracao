# ETL para Migração de Seguro Prestamista e Parcelas (agnóstico)

Este documento descreve, de forma agnóstica de linguagem e framework, o processo de ETL que migra contratos de seguro prestamista e suas parcelas a partir de um banco de dados de origem (denominado aqui “fonte”) para um banco de dados de destino (denominado aqui “alvo”). A lógica foi extraída dos artefatos de referência e deve permitir a reimplementação por outra equipe em outra linguagem, preservando as mesmas regras de negócio e dependências.


Objetivo
- Migrar os contratos da tabela base ep_segprestamista (fonte) e suas parcelas (ep_segparcelas) para as tabelas de destino correspondentes a Seguro e Parcela.
- Criar sob demanda, no alvo, os registros mestres necessários (Agência, Ponto de Atendimento, Cooperado, Cooperado-Agência-Conta, Seguradora, Apólice/Grupo da Seguradora) para manter integridade relacional e manter a base de destino “limpa” (somente o necessário).

Escopo e premissas
- Tabela base: ep_segprestamista (fonte). Todas as demais cargas dependem dela (registros derivados).
- Cada agência ativa na fonte possui seu próprio schema/namespace lógico; a migração percorre todas as agências ativas e processa seus dados.
- A origem de dados é baseada em um contexto lógico denominado SxDbContext (fonte). O destino é baseado em TxDbContext (alvo).
- A migração é transacional no alvo e faz inserções em lote para melhorar desempenho.
- O processo utiliza cache em memória (por execução) para evitar leituras/gravac?o?es repetidas de dimensões mestres no alvo.


1) Origem dos dados (fonte)
1.1 Descoberta de agências ativas e schemas
- As agências ativas são obtidas a partir de uma tabela corporativa comum, e para cada agência ativa se deriva um schema de dados específico onde residem as tabelas operacionais (inclusive ep_segprestamista e ep_segparcelas).
- Consulta SQL utilizada para obter a lista de agências e seus schemas:

```
SELECT
 CONCAT('agencia_', CAST(AG_CODIGO AS CHAR(8))) AS TABLE_SCHEMA,
 AG_CODIGO AS Codigo
FROM unico.cd_agencia
WHERE AG_ATIVA =1;
```

- Para cada linha retornada, TABLE_SCHEMA define o schema onde as tabelas operacionais da agência serão lidas; Codigo é o identificador lógico da agência.

1.2 Tabelas/fontes utilizadas por agência (no schema da agência)
- ep_segprestamista (base da migração)
 - Campos de interesse (nomes lógicos conforme observados pelo processo):
 - Chave de agrupamento para localizar parcelas: CcoConta, SegContrato
 - Identificação do cooperado: SegCpf
 - Identificação da seguradora: PstCodigo
 - Atributos do seguro: SegBase (valor base), SegDps (flag DPS), SegModalidade, SegCancTipo
 - Outros campos de cabeçalho do contrato (não listados explicitamente aqui) são mapeados para Seguro.
 - Leitura efetiva: todos os registros da agência são materializados para migração.
 - Consulta representativa (agnóstica):

```
SELECT *
FROM {TABLE_SCHEMA}.ep_segprestamista;
```

- ep_segparcelas (detalhe, dependente de ep_segprestamista)
 - Relacionamento com ep_segprestamista por dupla (CcoConta, SegContrato)
 - Leitura: todas as parcelas da agência são carregadas e agrupadas por (CcoConta, SegContrato) para posterior associação aos respectivos contratos.
 - Consulta representativa (agnóstica):

```
SELECT *
FROM {TABLE_SCHEMA}.ep_segparcelas;
```

- Cooperados/Clientes (ex.: cooperados, clientes)
 - Utilizados para obter dados do cooperado a partir de NumeroDocumento (CPF/CNPJ), nomes e e-mail.
 - Consulta representativa para busca específica:

```
SELECT *
FROM {TABLE_SCHEMA}.cooperados
WHERE NumeroDocumento = @NumeroDocumento;
```

- Contas (ex.: contas correntes)
 - Utilizadas para validar/obter a conta do cooperado por código de conta.
 - Consulta representativa para cache local por schema:

```
SELECT *
FROM {TABLE_SCHEMA}.contas;
```

- Ponto de Atendimento (ex.: tabela de pontos vinculada à agência)
 - Utilizado para obter nome e validar existência do ponto com base no código informado em conta (PaCodigo) e no código da agência.
 - Consulta representativa:

```
SELECT *
FROM {TABLE_SCHEMA}.ponto_atendimento
WHERE Agencia = @AgenciaCodigo AND Codigo = @PontoCodigo
ORDER BY Codigo
FETCH FIRST1 ROW ONLY;
```

- Seguradoras
 - Utilizadas para materializar a seguradora no alvo quando necessário, a partir de um código (PstCodigo) e atributos como Razão Social e CNPJ.
 - Consulta representativa:

```
SELECT *
FROM {TABLE_SCHEMA}.seguradoras
WHERE Codigo = @SeguradoraCodigo;
```

Observação: Os nomes físicos das tabelas auxiliares podem variar; use o mapeamento de origem para localizar os equivalentes de cooperados, contas, pontos de atendimento e seguradoras. O que importa para este ETL são as chaves e atributos referenciados acima.


2) Destino dos dados (alvo)
2.1 Entidades/tabelas de destino envolvidas
- Agência
- Ponto de Atendimento
- Cooperado
- Cooperado-Agência-Conta (ligação entre cooperado, agência e conta corrente)
- Seguradora
- Apólice/Grupo da Seguradora (chave composta por Agência e Seguradora)
- Seguro (fato/cabeçalho)
- Parcela (detalhe, filha de Seguro)

2.2 Nomes físicos das tabelas de destino
- Os nomes físicos no banco de destino devem ser obtidos das configurações de mapeamento da camada de persistência (por exemplo, marcações do tipo builder.ToTable("nome_tabela")).
- Exemplo de referência: uma configuração do tipo IEntityTypeConfiguration<GestaoDocumento> pode declarar builder.ToTable("gestao_documento"). O mesmo padrão se aplica às entidades deste processo (Agência, Ponto de Atendimento, Cooperado, etc.). Consulte as respectivas configurações para obter os nomes exatos de cada tabela física no alvo.


3) Relacionamentos e chaves (visão lógica)
- Cooperado-Agência-Conta
 - Relaciona Cooperado (por Id/Documento) à Agência (por Id/Código) e armazena a Conta Corrente (texto/código).
 - Unicidade esperada por (Cooperado, Agência, Conta).

- Seguro
 - Referencia Cooperado-Agência-Conta (CHAVE ESTRANGEIRA).
 - Referencia Ponto de Atendimento (CHAVE ESTRANGEIRA) derivado do PaCodigo da conta e do código da agência.
 - Referencia Apólice/Grupo da Seguradora (CHAVE ESTRANGEIRA), derivada do par (Agência, Seguradora).
 - Recebe atributos de ep_segprestamista (valor base, DPS, modalidade, cancelamento, contrato etc.).

- Parcela
 - Filha de Seguro (CHAVE ESTRANGEIRA) e recebe atributos de ep_segparcelas.

- Seguradora
 - Criada/atualizada sob demanda. Regra de CNPJ: concatenação (Codigo + CnpjOrigem) truncada em14 caracteres.

- Ponto de Atendimento
 - Chave lógica por (Agência.Codigo, Ponto.Codigo). Nome composto: "{Codigo} - {NomeOrigem}".

- Apólice/Grupo da Seguradora
 - Chave lógica composta por (AgênciaId, SeguradoraId). Criada com valores padrão caso não exista ao criar o Seguro.


4) Fluxo de processamento do ETL
Passo0 — Inicialização
- Abrir conexão com a fonte e com o alvo.
- Iniciar uma transação no alvo (escopo de toda a execução). Opcionalmente, considerar transação por agência para reduzir janela transacional.
- Preparar caches locais com leituras do alvo (Agências, Pontos de Atendimento, Cooperados + suas contas, Apólices/Grupos de Seguradora) para reduzir consultas repetidas.

Passo1 — Descobrir agências e schemas
- Executar a consulta de descoberta (seção1.1) para obter a lista de agências ativas e seus schemas.
- Para cada agência ativa:
 - Selecionar TABLE_SCHEMA correspondente (ex.: agencia_00001234).
 - Definir o contexto de leitura da fonte para este schema.

Passo2 — Carregar base e detalhe da agência
- Carregar todos os registros de ep_segprestamista do schema da agência (seção1.2).
 - Se não houver registros, registrar e seguir para a próxima agência.
- Carregar todas as ep_segparcelas do schema da agência e agrupar por (CcoConta, SegContrato) para acesso O(1) por contrato.
 - Se não houver parcelas, registrar e seguir para a próxima agência.

Passo3 — Processar cada prestamista (contrato)
Para cada registro de ep_segprestamista:
- Identificar as parcelas vinculadas via chave (CcoConta, SegContrato). Se ausentes, pular o registro.
- Obter/garantir no alvo:
1) Agência: buscar por Código; se não existir, criar minimalmente (Codigo, Nome) e armazenar em cache.
2) Conta e Ponto de Atendimento: localizar a conta no repositório de contas da agência (fonte); extrair PaCodigo (ponto). Garantir/obter Ponto por (AgenciaCodigo, PaCodigo); criar se não existir (Nome = "{Codigo} - {NomeOrigem}").
3) Cooperado: buscar por NumeroDocumento. Se não existir, criar (NumeroDocumento, Nome, Tipo ["Física" ou "Jurídica"], NomeFantasia, Email). Em seguida, garantir o vínculo Cooperado-Agência-Conta (AgenciaId, CooperadoId, ContaCorrente).
4) Seguradora: buscar por Codigo. Se não existir, criar com:
 - Nome, Razão Social e Status = "Ativo".
 - CNPJ = (Codigo + CnpjOrigem) truncado para14 caracteres.
5) Apólice/Grupo da Seguradora: buscar por (AgenciaId, SeguradoraId). Se não existir, criar com valores padrão (Apolice="Apolice", Grupo="Grupo", SubGrupo="SubGrupo", TipoCapital="Variável", ModalidadeUnico="ModalidadeUnico", ModalidadeAvista=0, ModalidadeParcelado=0, Ordem=0).
- Construir o registro Seguro no alvo usando os campos do prestamista (atribuição direta dos atributos do contrato, incluindo Valor Base, DPS, Modalidade, Tipo de Cancelamento, Contrato etc.) e setar as chaves estrangeiras:
 - CooperadoAgenciaContaId (do vínculo criado/recuperado)
 - PontoAtendimentoId (derivado do PaCodigo)
 - ApoliceGrupoSeguradoraId (derivado de Agência x Seguradora)
- Para cada parcela vinculada, criar o registro de Parcela no alvo, atribuindo os atributos da origem (ex.: número, vencimento, valor, status, etc., conforme disponíveis).
- Inserir os registros de Seguro e Parcela em memória para batching.

Passo4 — Batching e persistência
- A cada lote de ~100 Seguros acumulados, efetivar as inserções no banco de destino e limpar o buffer de lote. Ao final do processamento da agência, inserir eventuais remanescentes.

Passo5 — Finalização
- Confirmar (commit) a transação no alvo se toda a execução completar sem falhas catastróficas.
- Em caso de erro irrecuperável, reverter (rollback) a transação.


5) Regras de transformação e validações
- ep_segprestamista ? Seguro (alvo)
 - Mapear atributos diretos do contrato:
 - Valor Base (SegBase) ? campo correspondente do Seguro.
 - DPS (SegDps) ? flag/indicador no Seguro.
 - Modalidade (SegModalidade) e Tipo de Cancelamento (SegCancTipo) ? campos correspondentes.
 - Contrato/identificadores correlatos (por exemplo, SegContrato) ? campo de contrato do Seguro.
 - Referências (FKs):
 - CooperadoAgenciaContaId, PontoAtendimentoId, ApóliceGrupoSeguradoraId conforme seção4.

- ep_segparcelas ? Parcela (alvo)
 - Para cada parcela associada ao contrato, mapear1:1 os atributos relevantes (ex.: valor, vencimento, status etc.).
 - Vincular ao Seguro criado.

- Seguradora
 - Regra de CNPJ: CNPJ_alvo = LEFT(Concat(Codigo, CnpjOrigem),14)

- Ponto de Atendimento
 - Nome_alvo = "{Codigo} - {NomeOrigem}" (com Código e Nome da origem)

- Cooperado-Agência-Conta
 - Deve existir uma e apenas uma combinação por (Cooperado, Agência, ContaCorrente). Criar se ausente.

Validações críticas (fonte):
- Conta do cooperado deve existir na tabela de contas da agência; caso ausente, o registro não é migrado e o erro é registrado.
- Cooperado deve existir na tabela de cooperados; caso ausente, o registro não é migrado e o erro é registrado.
- Ponto de Atendimento deve existir para a agência e o código; caso ausente, registrar erro.
- Seguradora deve existir na tabela de seguradoras da origem; caso ausente, registrar erro.


6) Estratégia de cache (somente durante a execução)
Para reduzir leituras repetidas e garantir consistência:
- Carregar e manter em memória (do alvo):
 - Agências por Código
 - Pontos de Atendimento por (AgenciaCodigo, PontoCodigo)
 - Cooperados por NumeroDocumento (incluindo suas contas por agência)
 - Apólices/Grupos de Seguradora por (AgenciaId, SeguradoraId)
- Para cada novo registro criado no alvo, atualizar imediatamente o cache em memória, evitando duplicidade.
- Ao trocar de schema (entre agências), recarregar o cache apenas do catálogo de Contas da fonte para aquele schema.


7) Concorrência, transação e desempenho
- Transação: escopo amplo envolvendo toda a execução. Alternativamente, pode ser feito por agência para reduzir tempo de bloqueios.
- Batching: inserções de Seguro (e suas Parcelas) em lotes de ~100.
- Leitura sem rastreamento (conceito agnóstico: leitura somente-leitura) para dados da fonte, quando aplicável, para reduzir overhead.
- Progresso: registrar métricas de progresso e ETA (tempo restante estimado) durante o processamento, especialmente em execuções longas.


8) Tratamento de erros
- Nível do registro: erros durante o processamento de um prestamista (contrato) são registrados, e o processamento continua para os próximos registros.
- Nível global: exceções não tratadas levam a rollback da transação no alvo e encerramento do processo com log de erro fatal.


9) Pseudocódigo de referência (agnóstico)

Descoberta de agências
```
for each row in (SELECT ... FROM unico.cd_agencia WHERE AG_ATIVA =1)
 schema = row.TABLE_SCHEMA
 agencyCode = row.Codigo
 processAgency(schema, agencyCode)
```

Processamento por agência
```
load prestamistas = SELECT * FROM {schema}.ep_segprestamista
if prestamistas is empty: continue

load parcelas = SELECT * FROM {schema}.ep_segparcelas
index parcelas by key (CcoConta, SegContrato)

buffer = []
for each p in prestamistas:
 key = (p.CcoConta, p.SegContrato)
 pParcelas = parcelas[key]
 if pParcelas is empty: continue

 agencia = ensureAgency(agencyCode)

 conta = ensureSourceAccount(schema, p.CcoConta)
 ponto = ensurePointOfService(agencyCode, conta.PaCodigo)

 cooperado = ensureCooperado(p.SegCpf)
 vinculo = ensureCooperadoAgenciaConta(cooperado, agencia, conta.Codigo)

 seguradora = ensureSeguradora(p.PstCodigo)
 apoliceGrupo = ensureApoliceGrupo(agencia.Id, seguradora.Id)

 seguro = mapPrestamistaToSeguro(p)
 seguro.CooperadoAgenciaContaId = vinculo.Id
 seguro.PontoAtendimentoId = ponto.Id
 seguro.ApoliceGrupoSeguradoraId = apoliceGrupo.Id

 for each par in pParcelas:
 parcela = mapParcela(par)
 seguro.Parcelas.add(parcela)

 buffer.add(seguro)
 if buffer.size >=100:
 insert buffer into target
 buffer.clear()

if buffer not empty: insert buffer
```


10) Considerações de idempotência e consistência
- O processo busca reutilizar registros mestres existentes no alvo (por chaves de negócio) e só cria novos quando necessário, o que reduz duplicidade.
- Recomenda-se garantir chaves/índices únicos no alvo para:
 - Agência (Código)
 - Ponto de Atendimento (AgenciaId + Codigo)
 - Cooperado (NumeroDocumento)
 - Cooperado-Agência-Conta (CooperadoId + AgenciaId + ContaCorrente)
 - Apólice/Grupo de Seguradora (AgenciaId + SeguradoraId)
 - Seguro (chave natural a definir, se aplicável)


11) Saídas e métricas
- Por agência: registrar início/fim, totais de prestamistas e parcelas migradas.
- Totais gerais ao final: número de contratos (prestamistas) e parcelas migradas e tempo total decorrido.


12) Itens a observar/ajustes na reimplementação
- A criação de Apólice/Grupo da Seguradora deve retornar seu identificador para ser usado como FK no Seguro logo após a criação.
- Transações longas podem causar bloqueios; avaliar transações por agência.
- Validar e padronizar formatação de documentos (CPF/CNPJ) e strings de contas, se aplicável.
- Sanitização de entradas e validação de nulidade/tipos na camada de leitura.

---
Este documento encapsula a lógica do processo com base exclusiva nos artefatos analisados e está apto para guiar uma reimplementação em outra linguagem/plataforma, preservando regras, dependências e objetivos do ETL.
