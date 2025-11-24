# ETL para Migração de Seguro Prestamista e Parcelas (agnóstico)

Este documento descreve, de forma totalmente agnóstica de linguagem e framework, o processo de ETL que migra contratos de seguro prestamista e suas parcelas a partir de múltiplos schemas de origem (um por agência ativa) para um banco de dados de destino unificado. As consultas de origem abaixo estão EXATAMENTE como no código deste projeto.

Conteúdo consolidado de:
- Regras extraídas dos artefatos de referência originais.
- Análise detalhada da classe `MigratorWorker` e das consultas definidas em `SxDbContext`.
- Recomendações de otimização, robustez e consistência (agnósticas).

---
## 0. Visão Geral

Origem (SOURCE): Múltiplos schemas nomeados como `agencia_{AG_CODIGO}` dentro de um mesmo servidor/catálogo, mais schemas corporativos compartilhados (`unico`). O código alterna dinamicamente o schema de conexão substituindo `unico` pelo schema da agência selecionada.

Destino (TARGET): Base unificada com tabelas normalizadas e chaves artificiais/internal IDs (`Id` numérico/ulong) e chaves naturais para deduplicação.

Fluxo macro:
1. Descoberta de agências ativas e seus schemas.
2. Para cada schema de agência: leitura de contratos (prestamista) e parcelas associadas.
3. Para cada contrato: garantir dimensões (agência, ponto atendimento, cooperado + vínculo, seguradora, apólice/grupo seguradora).
4. Construir entidade cabeçalho `seguro` e suas `parcelas` dependentes.
5. Persistir em lotes (batch) de tamanho configurável (implementação atual usa 10; recomendado ~100).
6. Registrar métricas e erros; seguir processamento em caso de falhas de registro.
7. Commit transacional (implementação atual: transação única global; recomendado: por agência).

---
## 1. Origem dos dados (SOURCE) – Consultas exatamente como no código

### 1.1 Descoberta de agências e schemas (MigratorWorker)
Consulta (exata):
```
SELECT CONCAT('agencia_', CAST(AG_CODIGO AS CHAR(8))) TABLE_SCHEMA,AG_CODIGO Codigo FROM unico.cd_agencia WHERE AG_ATIVA = 1
```
Significado:
- `TABLE_SCHEMA`: nome do schema operacional da agência.
- `Codigo`: código lógico da agência (chave de negócio).

### 1.2 Tabelas/fontes por schema de agência (SxDbContext)

1) Contratos (prestamista)
- Entidade: `SxEpSegPrestamista`
- Consulta (exata):
```
select pm.* from ep_segprestamista pm where pm.SEG_MODALIDADE = 4
```
Observações:
- Filtro aplicado: apenas `SEG_MODALIDADE = 4` serão migrados pela implementação atual.

2) Parcelas
- Entidade: `SxEpSegParcela`
- Consulta (exata):
```
select * from ep_segparcela
```

3) Contas (para extrair `PaCodigo` e normalizar conta)
- Entidade: `SxContas`
- Consulta (exata):
```
select cc.CLI_CPFCNPJ CPFCNPJ ,cc.CCO_CONTA Codigo,PA_CODIGO PaCodigo  from cc_conta cc
```

### 1.3 Tabelas corporativas (`unico`) – SxDbContext

1) Agências (dados mestres de agência – usado para nomes/descrições)
- Entidade: `SxAgencia`
- Consulta (exata):
```
select ca.AG_CODIGO as CODIGO ,ca.AG_RAZAO as NOME from unico.cd_agencia ca
```

2) Pontos de atendimento
- Entidade: `SxPontoAtendimento`
- Consulta (exata):
```
select cp.PA_CODIGO as CODIGO,cp.AG_CODIGO AS AGENCIA,cp.PA_SIGLA As NOME  from unico.cd_pa cp
```

3) Cooperados (clientes por documento)
- Entidade: `SxClientes`
- Consulta (exata):
```
SELECT COALESCE(cl.CLI_CPFCNPJ, '') AS numerodocumento, cl.CLI_TIPPES AS tipo, COALESCE(cl.cli_nome, '') AS nome,
                                    cl.CLI_NFANTA AS nomefantasia,cl.CLI_EMAIL AS email,cl.AG_CODIGO  AS Agencia
                                    FROM unico.cd_cliente cl
```

4) Seguradoras (prestamistas)
- Entidade: `SxSeguradoras`
- Consulta (exata):
```
select PST_CODIGO as Codigo, pst_nome as nome,'00000000000000' as cnpj,pst_nome  as razaosocial,'00000000' as cep,
                                    'RUA' as rua,'COMPLEMENTO' as complemento,'0' as numero,  'BAIRRO' as bairro,
                                    'CIDADE' as cidade,  'UF' as uf,  'TELEFONE' as telefone,  'EMAIL' as email from unico.cd_prestamista 
```
Observações:
- A consulta retorna valores default para diversos campos (ex.: `cnpj` fixo `00000000000000`). Caso o destino requeira o CNPJ real, será necessário ajustar a consulta de origem ou complementar a regra na camada de transformação.

---
## 2. Destino dos dados (TARGET)

Tabelas físicas de destino envolvidas (nomes inferidos pelo domínio de destino):
- `agencia`
- `ponto_atendimento`
- `cooperado`
- `cooperado_agencia_conta`
- `seguradora`
- `apolice_grupo_seguradora`
- `seguro`
- `parcela`

Relacionamentos (visão lógica):
- `cooperado_agencia_conta`: unicidade por (cooperado, agência, conta_corrente).
- `seguro`: FK para `cooperado_agencia_conta`, `ponto_atendimento`, `apolice_grupo_seguradora`.
- `parcela`: FK para `seguro`.
- `apolice_grupo_seguradora`: chave lógica por (agencia_id, seguradora_id).

---
## 3. Mapeamento Source → Target (Regras)

- `ep_segprestamista` → `seguro`
  - Atributos: `SegBase` → `valor_base`, `SegDps` → `indicador_dps`, `SegModalidade` → `modalidade`, `SegCancTipo` → `tipo_cancelamento`, `SegContrato` → `numero_contrato`.
  - FKs: `cooperado_agencia_conta_id`, `ponto_atendimento_id`, `apolice_grupo_seguradora_id`.
  - Observação: Apenas modalidade 4 é migrada na implementação atual.

- `ep_segparcela` → `parcela`
  - 1:1 por cada parcela do contrato.

- `unico.cd_prestamista` → `seguradora`
  - Código: `PST_CODIGO`.
  - Demais campos conforme consulta atual (valores default). Caso necessário CNPJ real, ajustar consulta.

- `unico.cd_pa` + contas → `ponto_atendimento`
  - Código: `PA_CODIGO`.
  - Nome: composto `"{codigo} - {PA_SIGLA}"` (conforme regra de negócio; a consulta de origem provê `PA_SIGLA`).

- `unico.cd_cliente` → `cooperado`
  - Documento: `numerodocumento` (normalizado).
  - Tipo: `CLI_TIPPES`.
  - Nome/fantasia/email conforme origem.

- `cooperado_agencia_conta`
  - Chave: (cooperado, agência, `CCO_CONTA`).

---
## 4. Fluxo do ETL (Aderente ao Código + Recomendações)

1) Inicialização
- Abrir conexões source/target.
- Carregar caches do destino (agências, pontos, cooperados, vínculos, seguradoras, apólices/grupos).

2) Descoberta de agências
- Executar a consulta exata de descoberta (seção 1.1).
- Para cada agência: trocar o schema de conexão de origem (`SxDbContext.Schema = TABLE_SCHEMA`).

3) Leitura de dados da agência
- Ler contratos com filtro `SEG_MODALIDADE = 4` conforme consulta exata.
- Ler todas as parcelas e indexar por `(CcoConta, SegContrato)`.

4) Transformação e garantias
- Para cada contrato:
  - Localizar parcelas; se ausentes, ignorar e logar.
  - Garantir no destino: agência, ponto (via `PA_SIGLA` e `PA_CODIGO`), cooperado, vínculo cooperado–agência–conta, seguradora, apólice/grupo.
  - Construir `seguro` e associar `parcelas`.

5) Persistência em lote
- Inserir seguros em batches (atual: 10; recomendado: 100 configurável).

6) Finalização
- Commit transacional (atual: global; recomendado: por agência).

---
## 5. Validações e Regras Críticas

- Conta do cooperado deve existir em `cc_conta` (consulta exata na seção 1.2).
- Cooperado deve existir em `unico.cd_cliente` (consulta exata na seção 1.3).
- Ponto de atendimento deve existir em `unico.cd_pa` (consulta exata na seção 1.3).
- Seguradora deve existir em `unico.cd_prestamista` (consulta exata na seção 1.3). Observação: consulta atual retorna `cnpj` default; se o CNPJ real for obrigatório no destino, a transformação deve lidar com isso.

---
## 6. Estratégia de Cache (Execução)

Em memória (por execução ou por agência):
- Agências por código.
- Pontos por (agencia.codigo, ponto.codigo).
- Cooperados por documento (e suas contas por agência).
- Seguradoras por código.
- Apólice/grupo por (agencia_id, seguradora_id).

Atualizar cache imediatamente após criação no destino. Recarregar dados sensíveis a cada troca de agência.

---
## 7. Concorrência, Transação e Performance

- Transação: recomendada por agência (código atual usa uma transação global).
- Batching: recomendado ~100 cabeçalhos; código atual usa 10.
- Projeção: as consultas são exatamente as do código; otimizações futuras devem manter equivalência funcional.
- Paralelismo: possível por agência com caches isolados e transações independentes.

---
## 8. Pseudocódigo Otimizado (Agnóstico)
```
const BATCH_SIZE = 100
agencias = query("SELECT CONCAT('agencia_', CAST(AG_CODIGO AS CHAR(8))) TABLE_SCHEMA,AG_CODIGO Codigo FROM unico.cd_agencia WHERE AG_ATIVA = 1")
for ag in agencias:
  setSchema(ag.TABLE_SCHEMA)
  beginTransaction()
  prestamistas = query("select pm.* from ep_segprestamista pm where pm.SEG_MODALIDADE = 4")
  if prestamistas.empty(): commit(); continue
  parcelas = query("select * from ep_segparcela")
  indexParcelas = indexByKey(parcelas, (p) => (p.CcoConta, p.SegContrato))
  buffer = []
  for p in prestamistas:
    key = (p.CcoConta, p.SegContrato)
    if !indexParcelas.contains(key): logInfo("SEM_PARCELAS", key); continue
    conta = queryOne("select cc.CLI_CPFCNPJ CPFCNPJ ,cc.CCO_CONTA Codigo,PA_CODIGO PaCodigo  from cc_conta cc where cc.CCO_CONTA = @c", {c:p.CcoConta})
    if conta == null: logErr("CONTA_NAO_ENCONTRADA", key); continue
    cliente = queryOne("SELECT COALESCE(cl.CLI_CPFCNPJ, '') AS numerodocumento, cl.CLI_TIPPES AS tipo, COALESCE(cl.cli_nome, '') AS nome, cl.CLI_NFANTA AS nomefantasia,cl.CLI_EMAIL AS email,cl.AG_CODIGO  AS Agencia FROM unico.cd_cliente cl WHERE cl.CLI_CPFCNPJ = @doc", {doc:p.SegCpf})
    if cliente == null: logErr("COOPERADO_NAO_ENCONTRADO", key); continue
    ponto = queryOne("select cp.PA_CODIGO as CODIGO,cp.AG_CODIGO AS AGENCIA,cp.PA_SIGLA As NOME  from unico.cd_pa cp WHERE cp.AG_CODIGO=@ag AND cp.PA_CODIGO=@pa", {ag: ag.Codigo, pa: conta.PaCodigo})
    if ponto == null: logErr("PONTO_ATENDIMENTO_NAO_ENCONTRADO", key); continue
    seguradora = queryOne("select PST_CODIGO as Codigo, pst_nome as nome,'00000000000000' as cnpj,pst_nome  as razaosocial,'00000000' as cep, 'RUA' as rua,'COMPLEMENTO' as complemento,'0' as numero,  'BAIRRO' as bairro, 'CIDADE' as cidade,  'UF' as uf,  'TELEFONE' as telefone,  'EMAIL' as email from unico.cd_prestamista WHERE PST_CODIGO=@pst", {pst: p.PstCodigo})
    if seguradora == null: logErr("SEGURADORA_NAO_ENCONTRADA", key); continue
    vinculo = ensureCooperadoAgenciaConta(cliente, ag, conta)
    apoliceGrupo = ensureApoliceGrupo(ag.Id, seguradora.Id)
    seguro = mapPrestamista(p, vinculo.Id, ponto.Id, apoliceGrupo.Id)
    for parSrc in indexParcelas[key]:
      seguro.Parcelas.add(mapParcela(parSrc))
    buffer.add(seguro)
    if buffer.size == BATCH_SIZE:
      insertBatch(buffer)
      buffer.clear()
  if buffer.size > 0: insertBatch(buffer)
  commit()
```

---
## 9. Métricas, Idempotência e Consistência

- Reutilizar registros mestres existentes no destino por chaves de negócio (agência, ponto, cooperado, vínculo, seguradora, apólice/grupo).
- Índices únicos recomendados: `agencia(codigo)`, `ponto_atendimento(agencia_id,codigo)`, `cooperado(numero_documento)`, `cooperado_agencia_conta(cooperado_id,agencia_id,conta_corrente)`, `apolice_grupo_seguradora(agencia_id,seguradora_id)`.
- Para `seguro`, considerar chave natural para idempotência (ex.: `numero_contrato` + `cooperado_agencia_conta_id`).

---
## 10. Observações finais

- Todas as consultas de origem listadas estão exatamente como no código (`SxDbContext` e `MigratorWorker`).
- Quaisquer otimizações futuras devem preservar a compatibilidade funcional e, se alterarem as consultas, este documento deve ser atualizado para refletir exatamente as novas consultas.
