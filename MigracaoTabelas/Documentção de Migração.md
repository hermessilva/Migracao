# ETL para Migração de Seguro Prestamista e Parcelas (agnóstico)

Este documento descreve, de forma agnóstica de linguagem e framework, o processo de ETL que migra contratos de seguro prestamista e suas parcelas a partir de um banco de dados de origem (denominado aqui “fonte”) para um banco de dados de destino (denominado aqui “alvo”). A lógica foi extraída dos artefatos de referência e deve permitir a reimplementação por outra equipe em outra linguagem, preservando as mesmas regras de negócio e dependências.


Objetivo
- Migrar os contratos da tabela base ep_segprestamista (fonte) e suas parcelas (ep_segparcela) para as tabelas de destino correspondentes ao cabeçalho do seguro e suas parcelas.
- Criar sob demanda, no alvo, os registros mestres necessários (agência, ponto de atendimento, cooperado, vínculo cooperado?agência?conta, seguradora, apólice/grupo da seguradora) para manter integridade relacional e manter a base de destino “limpa” (somente o necessário).

Escopo e premissas
- Tabela base: ep_segprestamista (fonte). Todas as demais cargas dependem dela (registros derivados).
- Cada agência ativa na fonte possui seu próprio schema/namespace lógico; a migração percorre todas as agências ativas e processa seus dados.
- A origem de dados é baseada em um catálogo denominado aqui como “fonte”. O destino é baseado em um catálogo denominado “alvo”.
- A migração é transacional no alvo e faz inserções em lote para melhorar desempenho.
- O processo utiliza cache em memória (por execução) para evitar leituras/gravações repetidas de dimensões mestres no alvo.


1) Origem dos dados (fonte)
1.1 Descoberta de agências ativas e schemas
- As agências ativas são obtidas a partir de uma tabela corporativa comum, e para cada agência ativa se deriva um schema de dados específico onde residem as tabelas operacionais (inclusive ep_segprestamista e ep_segparcela).
- Consulta SQL utilizada para obter a lista de agências e seus schemas:

```
SELECT
 CONCAT('agencia_', CAST(AG_CODIGO AS CHAR(8))) AS TABLE_SCHEMA,
 AG_CODIGO AS Codigo
FROM unico.cd_agencia
WHERE AG_ATIVA =1;
```

- Para cada linha retornada, TABLE_SCHEMA define o schema onde as tabelas operacionais da agência serão lidas; Codigo é o identificador lógico da agência.

1.2 Tabelas/fontes utilizadas
- ep_segprestamista (no schema da agência)
 - Chave de agrupamento para localizar parcelas: CcoConta, SegContrato
 - Identificação do cooperado: SegCpf
 - Identificação da seguradora: PstCodigo
 - Atributos do seguro: SegBase (valor base), SegDps (flag DPS), SegModalidade, SegCancTipo, entre outros campos de cabeçalho do contrato.
 - Leitura representativa:
```
SELECT *
FROM {TABLE_SCHEMA}.ep_segprestamista;
```

- ep_segparcela (no schema da agência)
 - Relacionamento com ep_segprestamista pela dupla (CcoConta, SegContrato)
 - Leitura representativa:
```
SELECT *
FROM {TABLE_SCHEMA}.ep_segparcela;
```

- cc_conta (no schema da agência)
 - Utilizada para validar/obter a conta do cooperado por código de conta e extrair PaCodigo (ponto de atendimento).
 - Leitura representativa:
```
SELECT *
FROM {TABLE_SCHEMA}.cc_conta;
```

- unico.cd_cliente (catálogo corporativo)
 - Utilizada para obter dados do cooperado a partir de NumeroDocumento (CPF/CNPJ), nomes e e?mail.
 - Leitura representativa (por documento):
```
SELECT *
FROM unico.cd_cliente
WHERE CLI_CPFCNPJ = @NumeroDocumento;
```

- unico.cd_pa (catálogo corporativo)
 - Utilizada para validar/obter ponto de atendimento com base no código do ponto (PaCodigo) e no código da agência.
 - Leitura representativa:
```
SELECT *
FROM unico.cd_pa
WHERE AG_CODIGO = @AgenciaCodigo AND PA_CODIGO = @PontoCodigo
ORDER BY PA_CODIGO
FETCH FIRST1 ROW ONLY;
```

- unico.cd_prestamista (catálogo corporativo)
 - Utilizada para materializar a seguradora quando necessário, a partir de um código (PstCodigo) e atributos como razão social e CNPJ.
 - Leitura representativa:
```
SELECT *
FROM unico.cd_prestamista
WHERE PST_CODIGO = @SeguradoraCodigo;
```

Observação: Os nomes físicos das tabelas acima devem ser respeitados. Caso haja variações por ambiente, ajuste os nomes mantendo as chaves e atributos referenciados.


2) Destino dos dados (alvo)
2.1 Tabelas de destino envolvidas (nomes físicos)
- agencia
- ponto_atendimento
- cooperado
- cooperado_agencia_conta
- seguradora
- apolice_grupo_seguradora
- seguro
- parcela

2.2 Relacionamentos (visão lógica)
- cooperado_agencia_conta
 - Relaciona cooperado (por documento) à agência (por código) e armazena a conta corrente (código/char).
 - Unicidade esperada por (cooperado, agência, conta_corrente).

- seguro (cabeçalho)
 - Referencia cooperado_agencia_conta (FK).
 - Referencia ponto_atendimento (FK), derivado do PaCodigo da conta e do código da agência.
 - Referencia apolice_grupo_seguradora (FK), derivada do par (agência, seguradora).
 - Recebe atributos de ep_segprestamista (valor base, DPS, modalidade, cancelamento, contrato etc.).

- parcela (detalhe)
 - Filha de seguro (FK) e recebe atributos de ep_segparcela.

- seguradora
 - Criada sob demanda. Regra de CNPJ no alvo: concatenação (PstCodigo + CnpjOrigem) truncada em14 caracteres.

- ponto_atendimento
 - Chave lógica por (agencia.codigo, ponto.codigo). Nome composto: "{codigo} - {nome_origem}".

- apolice_grupo_seguradora
 - Chave lógica composta por (agencia_id, seguradora_id). Criada com valores padrão caso não exista quando da criação do seguro.


3) Fluxo de processamento do ETL
Passo0 — Inicialização
- Abrir conexão com a fonte e com o alvo.
- Iniciar transação no alvo (pode ser por toda a execução ou por agência).
- Preparar caches com leituras do alvo (agências, pontos, cooperados + contas, apólices/grupos) para reduzir consultas repetidas.

Passo1 — Descobrir agências e schemas
- Executar a consulta de descoberta (seção1.1) para obter a lista de agências ativas e seus schemas.
- Para cada agência ativa:
 - Definir o schema de leitura da fonte para este processamento.

Passo2 — Carregar base e detalhe por agência
- Carregar todos os registros de {schema}.ep_segprestamista.
 - Se não houver registros, registrar e seguir para a próxima agência.
- Carregar todas as {schema}.ep_segparcela e indexar por (CcoConta, SegContrato) para acesso O(1) por contrato.
 - Se não houver parcelas, registrar e seguir para a próxima agência.

Passo3 — Processar cada prestamista (contrato)
Para cada registro de ep_segprestamista:
- Localizar as parcelas vinculadas via chave (CcoConta, SegContrato). Se ausentes, pular o registro.
- Garantir no alvo:
1) agência: buscar por código; se não existir, criar minimalmente (codigo, nome).
2) conta e ponto de atendimento: localizar a conta em {schema}.cc_conta; extrair PaCodigo. Garantir ponto por (agencia.codigo, PaCodigo); criar se não existir (nome = "{codigo} - {nome_origem}").
3) cooperado: buscar por documento em unico.cd_cliente. Se não existir no alvo, criar (numero_documento, nome, tipo [“Física”/“Jurídica”], nome_fantasia, email). Em seguida, garantir o vínculo cooperado_agencia_conta (agencia_id, cooperado_id, conta_corrente).
4) seguradora: buscar por código em unico.cd_prestamista. Se não existir no alvo, criar com:
 - nome, razão_social e status = "Ativo".
 - cnpj = LEFT(CONCAT(PstCodigo, CnpjOrigem),14).
5) apolice_grupo_seguradora: buscar por (agencia_id, seguradora_id). Se não existir, criar com valores padrão (apolice="Apolice", grupo="Grupo", subgrupo="SubGrupo", tipo_capital="Variável", modalidade_unico="ModalidadeUnico", modalidade_avista=0, modalidade_parcelado=0, ordem=0).
- Construir o registro em seguro no alvo usando os campos do prestamista (valor base SegBase, DPS SegDps, modalidade SegModalidade, tipo de cancelamento SegCancTipo, contrato SegContrato etc.) e setar as FKs:
 - cooperado_agencia_conta_id (do vínculo criado/recuperado)
 - ponto_atendimento_id (derivado do PaCodigo)
 - agencia_seguradora/apólice_grupo id (derivado de agência x seguradora)
- Para cada parcela vinculada, criar o registro em parcela no alvo, atribuindo os atributos da origem (ex.: número, vencimento, valor, status). Vincular ao respectivo seguro.
- Inserir registros em memória para batching.

Passo4 — Batching e persistência
- A cada lote de ~100 seguros acumulados, efetivar as inserções no banco de destino e limpar o buffer. Ao final, inserir remanescentes.

Passo5 — Finalização
- Confirmar (commit) a transação no alvo se não houver falhas.
- Em caso de erro irrecuperável, reverter (rollback) a transação.


4) Regras de transformação e validações
- ep_segprestamista ? seguro
 - Mapear atributos diretos do contrato:
 - SegBase ? campo de valor base
 - SegDps ? indicador DPS
 - SegModalidade e SegCancTipo ? campos correspondentes
 - SegContrato ? número do contrato
 - Referências (FKs): cooperado_agencia_conta_id, ponto_atendimento_id, apolice_grupo_seguradora_id

- ep_segparcela ? parcela
 - Para cada parcela associada ao contrato, mapear1:1 os atributos relevantes (valor, vencimento, status etc.).

- seguradora
 - cnpj no alvo = LEFT(CONCAT(PstCodigo, CnpjOrigem),14)

- ponto_atendimento
 - nome no alvo = "{codigo} - {nome_origem}"

- cooperado_agencia_conta
 - deve existir uma única combinação por (cooperado, agência, conta_corrente). Criar se ausente.

Validações críticas (fonte):
- Conta do cooperado deve existir em {schema}.cc_conta; caso ausente, não migrar o registro e registrar o erro.
- Cooperado deve existir em unico.cd_cliente; caso ausente, não migrar o registro e registrar o erro.
- Ponto de atendimento deve existir em unico.cd_pa para a agência e o código; caso ausente, registrar erro.
- Seguradora deve existir em unico.cd_prestamista; caso ausente, registrar erro.


5) Estratégia de cache (em tempo de execução)
Para reduzir leituras repetidas e garantir consistência, manter em memória (do alvo):
- agências por código
- pontos de atendimento por (agencia.codigo, ponto.codigo)
- cooperados por documento (incluindo suas contas por agência)
- apólices/grupos de seguradora por (agencia_id, seguradora_id)

Atualizar imediatamente o cache após criar registros no alvo. Ao trocar de schema (entre agências), recarregar apenas o catálogo de contas da fonte para aquele schema.


6) Concorrência, transação e desempenho
- Transação: escopo amplo (ou por agência, para reduzir bloqueios).
- Batching: inserções em lotes de ~100 cabeçalhos de seguro.
- Leituras de origem preferencialmente em modo somente?leitura para reduzir overhead.
- Progresso: registrar métricas e ETA durante o processamento.


7) Tratamento de erros
- Nível do registro: erros ao processar um prestamista (contrato) são registrados e o processamento continua.
- Nível global: exceções não tratadas levam a rollback da transação no alvo e encerramento do processo.


8) Pseudocódigo de referência (agnóstico)

Descoberta de agências
```
for each row in (
 SELECT CONCAT('agencia_', CAST(AG_CODIGO AS CHAR(8))) AS TABLE_SCHEMA,
 AG_CODIGO AS Codigo
 FROM unico.cd_agencia WHERE AG_ATIVA =1)
 schema = row.TABLE_SCHEMA
 agencyCode = row.Codigo
 processAgency(schema, agencyCode)
```

Processamento por agência
```
load prestamistas = SELECT * FROM {schema}.ep_segprestamista
if prestamistas is empty: continue

load parcelas = SELECT * FROM {schema}.ep_segparcela
index parcelas by key (CcoConta, SegContrato)

buffer = []
for each p in prestamistas:
 key = (p.CcoConta, p.SegContrato)
 pParcelas = parcelas[key]
 if pParcelas is empty: continue

 agencia = ensureAgency(agencyCode)

 conta = ensureSourceAccount({schema}, p.CcoConta)
 ponto = ensurePointOfService(agencyCode, conta.PaCodigo)

 cooperado = ensureCooperadoFrom(unico.cd_cliente, p.SegCpf)
 vinculo = ensureCooperadoAgenciaConta(cooperado, agencia, conta.Codigo)

 seguradora = ensureSeguradoraFrom(unico.cd_prestamista, p.PstCodigo) // A "PK" DA SEGURADORA É O CÓDIGO ORIGINAL CONVERTIDO PARA ULONG
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
 insert buffer into target tables
 buffer.clear()

if buffer not empty: insert buffer
```


9) Considerações de idempotência e consistência
- Reutilizar registros mestres existentes no alvo por chaves de negócio e criar novos somente quando necessário.
- Recomenda-se garantir chaves/índices únicos no alvo para:
 - agencia (codigo)
 - ponto_atendimento (agencia_id + codigo)
 - cooperado (numero_documento)
 - cooperado_agencia_conta (cooperado_id + agencia_id + conta_corrente)
 - apolice_grupo_seguradora (agencia_id + seguradora_id)
 - seguro (chave natural a definir, se aplicável)


10) Métricas e auditoria
- Por agência: registrar início/fim, totais de prestamistas e parcelas migradas.
- Totais gerais: quantidade de contratos e parcelas migradas e tempo total decorrido.


11) Itens a observar/ajustes na reimplementação
- A criação de apolice_grupo_seguradora deve retornar seu identificador para ser usado como FK no seguro logo após a criação.
- Transações longas podem causar bloqueios; avaliar transações por agência.
- Validar/padronizar formatação de documentos (CPF/CNPJ) e strings de contas, se aplicável.
- Sanitização de entradas e validação de nulidade/tipos na camada de leitura.

---
Este documento encapsula a lógica do processo com base exclusiva nos artefatos analisados e está apto para guiar uma reimplementação em outra linguagem/plataforma, preservando regras, dependências e objetivos do ETL, sem referências a implementações específicas do projeto original.
