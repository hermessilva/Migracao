# Documento de Reconstrução ETL - Migração de Seguros Prestamista

# Parte 1: Visão Geral, Arquitetura e Catálogo de Fontes

---

## 1. Resumo Executivo

Este documento descreve as regras, definições e lógica de um processo de ETL (Extract-Transform-Load) para migração de dados de **Seguros Prestamista** de um sistema legado para um novo sistema de gestão de seguros.

### Objetivo de Negócio
O ETL realiza a migração de:
- **Contratos de seguro prestamista** vinculados a operações de empréstimo
- **Parcelas financeiras** dos seguros
- **Cadastros de cooperados** (clientes) e suas contas correntes
- **Cadastros de agências** e pontos de atendimento
- **Cadastros de seguradoras** com suas configurações de comissões, limites por faixa etária e contas contábeis

### Contexto do Domínio
O seguro prestamista é um produto financeiro vinculado a operações de crédito (empréstimos), onde o cooperado (tomador do empréstimo) contrata um seguro que cobre o saldo devedor em caso de sinistro (morte, invalidez, etc.). As cooperativas de crédito atuam como intermediárias entre cooperados e seguradoras.

---

## 2. Escopo e Limites

### O que está incluído
1. **Contratos de seguro prestamista** da modalidade "Empréstimo" (modalidade = 4)
2. **Contratos ativos** (não cancelados, com vigência futura)
3. **Contratos de contas correntes ativas** (situação = 1)
4. **Parcelas de seguro** vinculadas aos contratos elegíveis
5. **Dados cadastrais** de:
   - Agências ativas
   - Pontos de atendimento
   - Cooperados (clientes)
   - Seguradoras
   - Limites e coeficientes por faixa etária
   - Configurações de comissões
   - Contas contábeis para contabilização

### O que NÃO está incluído
1. Contratos cancelados (tipo de cancelamento ≠ 0)
2. Contratos com vigência expirada (fim de vigência anterior a 01/01/2026)
3. Contratos já pagos/liquidados (data de pagamento preenchida)
4. Modalidades de seguro diferentes de "Empréstimo" (cheque especial, ECR, desconto)
5. Contas correntes inativas (situação ≠ 1)
6. Histórico de auditoria do sistema legado
7. Documentos digitalizados/anexos

### Ambientes
- **Origem**: Múltiplos bancos de dados MySQL, um por agência (padrão: `agencia_XXXX`)
- **Destino**: Banco de dados MySQL único consolidado

---

## 3. Arquitetura Lógica do Fluxo

### Diagrama de Fluxo (ASCII)

```
┌─────────────────────────────────────────────────────────────────────────────┐
│                           BASE CENTRAL (unico)                               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐     │
│  │ cd_agencia   │  │ cd_pa        │  │ cd_cliente   │  │ cd_prestamista│     │
│  │ (Agências)   │  │ (Pontos Atend)│ │ (Clientes)   │  │ (Seguradoras) │     │
│  └──────────────┘  └──────────────┘  └──────────────┘  └──────────────┘     │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                    DESCOBERTA DE AGÊNCIAS ATIVAS                             │
│         Consulta cd_agencia onde AG_ATIVA = 1 → Lista de schemas            │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
              ┌───────────────────────┼───────────────────────┐
              ▼                       ▼                       ▼
┌──────────────────────┐  ┌──────────────────────┐  ┌──────────────────────┐
│   agencia_0001       │  │   agencia_0002       │  │   agencia_NNNN       │
│  ┌────────────────┐  │  │  ┌────────────────┐  │  │  ┌────────────────┐  │
│  │ep_segprestamista│ │  │  │ep_segprestamista│ │  │  │ep_segprestamista│ │
│  │ep_segparcela    │ │  │  │ep_segparcela    │ │  │  │ep_segparcela    │ │
│  │ep_contrato      │ │  │  │ep_contrato      │ │  │  │ep_contrato      │ │
│  │ep_parcela       │ │  │  │ep_parcela       │ │  │  │ep_parcela       │ │
│  │cc_conta         │ │  │  │cc_conta         │ │  │  │cc_conta         │ │
│  └────────────────┘  │  │  └────────────────┘  │  │  └────────────────┘  │
└──────────────────────┘  └──────────────────────┘  └──────────────────────┘
              │                       │                       │
              └───────────────────────┼───────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                         PROCESSO DE MIGRAÇÃO                                 │
│                                                                              │
│  ┌─────────────┐    ┌─────────────┐    ┌─────────────┐    ┌─────────────┐   │
│  │  EXTRAÇÃO   │ ─► │ PREPARAÇÃO  │ ─► │ENRIQUECIM.  │ ─► │   CARGA     │   │
│  │             │    │    CACHE    │    │  LOOKUP     │    │             │   │
│  └─────────────┘    └─────────────┘    └─────────────┘    └─────────────┘   │
│                                                                              │
└─────────────────────────────────────────────────────────────────────────────┘
                                      │
                                      ▼
┌─────────────────────────────────────────────────────────────────────────────┐
│                           BASE DESTINO                                       │
│  ┌────────────┐ ┌────────────┐ ┌────────────┐ ┌────────────┐ ┌───────────┐  │
│  │  agencia   │ │ cooperado  │ │  seguro    │ │  parcela   │ │ seguradora│  │
│  └────────────┘ └────────────┘ └────────────┘ └────────────┘ └───────────┘  │
│  ┌────────────────────┐ ┌───────────────────────┐ ┌───────────────────────┐ │
│  │ponto_atendimento   │ │cooperado_agencia_conta│ │apolice_grupo_seguradora││
│  └────────────────────┘ └───────────────────────┘ └───────────────────────┘ │
│  ┌────────────────┐ ┌──────────────────┐ ┌─────────────────┐                │
│  │seguro_parametro│ │seguradora_limite │ │comissao_seguradora│              │
│  └────────────────┘ └──────────────────┘ └─────────────────┘                │
└─────────────────────────────────────────────────────────────────────────────┘
```

### Etapas do Processo

#### Etapa 1: Descoberta de Agências
- Consulta a base central para identificar todas as agências ativas
- Gera a lista de schemas de banco de dados a serem processados
- Formato do schema: `agencia_` + código da agência (4 dígitos)

#### Etapa 2: Preparação de Cache (por execução)
- Carrega em memória todos os registros existentes no destino:
  - Seguradoras (indexadas por CNPJ)
  - Agências (indexadas por código)
  - Pontos de atendimento (indexados por código da agência + código do PA)
  - Cooperados (indexados por número do documento)
  - Apólices/grupos de seguradoras (indexadas por ID da agência + ID da seguradora)

#### Etapa 3: Extração (por agência)
- Para cada agência:
  - Carrega todos os contratos de seguro prestamista elegíveis
  - Carrega todas as parcelas de seguro
  - Agrupa parcelas por chave (conta corrente, número do contrato)
  - Carrega cooperados (clientes) da agência
  - Carrega contas correntes da agência

#### Etapa 4: Enriquecimento e Lookup
- Para cada contrato:
  - Localiza ou cria a agência no destino
  - Localiza ou cria o ponto de atendimento no destino
  - Localiza ou cria o cooperado no destino
  - Localiza ou cria o vínculo cooperado-agência-conta
  - Localiza ou cria a seguradora no destino (com limites e comissões)
  - Localiza ou cria a apólice/grupo no destino

#### Etapa 5: Transformação e Carga
- Transforma os dados do contrato para o formato destino
- Cria o registro de parâmetros do seguro
- Transforma e vincula as parcelas
- Persiste em lotes de 100 registros

---

## 4. Catálogo de Fontes (MySQL)

### 4.1 Base Central: Schema `unico`

#### Tabela: `cd_agencia`
**Descrição**: Cadastro de agências da cooperativa

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| AG_CODIGO | Código da agência (4 caracteres) | Chave natural, usado para identificar o schema da agência |
| AG_SIGLA | Nome/sigla da agência | Migrado para nome da agência |
| AG_ATIVA | Flag de agência ativa (1=ativa, 0=inativa) | Filtro de elegibilidade |

**Critério de seleção**: `AG_ATIVA = 1`

---

#### Tabela: `cd_pa`
**Descrição**: Cadastro de pontos de atendimento

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| PA_CODIGO | Código do ponto de atendimento (3 caracteres) | Chave natural (junto com agência) |
| AG_CODIGO | Código da agência | Chave de relacionamento |
| PA_SIGLA | Nome/sigla do ponto de atendimento | Migrado para nome |

---

#### Tabela: `cd_cliente`
**Descrição**: Cadastro de clientes/cooperados

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| CLI_CPFCNPJ | CPF ou CNPJ do cliente (sem formatação) | Chave natural de identificação |
| CLI_TIPPES | Tipo de pessoa: F (Física) ou J (Jurídica) | Migrado como tipo de pessoa |
| CLI_NOME | Nome completo ou razão social | Migrado para nome |
| CLI_NFANTA | Nome fantasia | Migrado (opcional) |
| CLI_EMAIL | E-mail de contato | Migrado (opcional) |
| AG_CODIGO | Código da agência | Informativo |

---

#### Tabela: `cd_prestamista`
**Descrição**: Cadastro de seguradoras prestamista

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| PST_CODIGO | Código da seguradora (4 caracteres) | Chave natural |
| PST_NOME | Nome da seguradora | Migrado para nome e razão social |
| PST_ATIVO | Status ativo/inativo | Migrado como status |
| PST_MAXMESES | Número máximo de meses do contrato | Migrado para condição da seguradora |
| PST_LIMITEIDADE | Limite máximo de idade | Migrado para condição da seguradora |
| PST_VALORDPS | Valor mínimo para exigir DPS | Base para limites de DPS |
| PORCENTAGEM_COMISSAO | Percentual de comissão da cooperativa | Migrado para comissão |
| PST_LIMITE30 a PST_LIMITE85 | Limites por faixa etária | Migrado para limites da seguradora |
| PST_COEF30 a PST_COEF85 | Coeficientes por faixa etária | Migrado para limites da seguradora |
| PST_DPS30 a PST_DPS85 | Exigência de DPS por faixa | Combinado com VALORDPS |
| CONTA_CONTABIL_CREDITO | Conta contábil de crédito | Migrado para contabilização |
| CONTA_CONTABIL_DEBITO | Conta contábil de débito | Migrado para contabilização |
| CONTA_CONTABIL_CREDITO_COMISSAO | Conta crédito comissão | Migrado para contabilização |
| CONTA_CONTABIL_DEBITO_COMISSAO | Conta débito comissão | Migrado para contabilização |
| CONTA_CONTABIL_CREDITO_4966 | Conta crédito cancelamento | Migrado para contabilização |
| CONTA_CONTABIL_DEBITO_4966 | Conta débito cancelamento | Migrado para contabilização |
| CONTA_CONTABIL_CREDITO_COMISSAO_4966 | Conta crédito comissão cancelamento | Migrado para contabilização |
| CONTA_CONTABIL_DEBITO_COMISSAO_4966 | Conta débito comissão cancelamento | Migrado para contabilização |

---

### 4.2 Base por Agência: Schema `agencia_XXXX`

#### Tabela: `ep_segprestamista`
**Descrição**: Contratos de seguro prestamista

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| PST_CODIGO | Código da seguradora | Chave de lookup para seguradora |
| CCO_CONTA | Código da conta corrente (9 caracteres) | Chave de relacionamento com conta/cooperado |
| SEG_CPF | CPF do segurado (11 caracteres) | Chave de lookup para cooperado |
| SEG_NOME | Nome do segurado | Informativo (não migrado) |
| SEG_NASC | Data de nascimento | Não migrado diretamente |
| SEG_CONTRATO | Número do contrato (10 caracteres) | Migrado como identificador do contrato |
| SEG_MODALIDADE | Modalidade: 1=Ch.Especial, 2=ECR, 3=Desconto, 4=Empréstimo | Filtro de elegibilidade |
| SEG_TIPOCONTA | Tipo de conta: F=Física, J=Jurídica | Informativo |
| SEG_INICIO | Data de início da vigência | Migrado como início de vigência |
| SEG_FIM | Data de fim da vigência | Migrado como fim de vigência e vencimento |
| SEG_MESES | Número de meses do seguro | Migrado como quantidade de parcelas |
| SEG_CANCELAMENTO | Data do cancelamento | Não migrado (filtro de exclusão) |
| SEG_CANCTIPO | Tipo de cancelamento: 0=Não cancelado, 1-4=Tipos de cancelamento | Filtro de elegibilidade |
| SEG_VRCONTRATO | Valor total do contrato | Informativo |
| SEG_BASE | Valor base segurado | Migrado como capital segurado e valor base |
| SEG_PREMIO | Valor do prêmio | Migrado como prêmio total |
| SEG_IOF | Valor do IOF | Migrado |
| SEG_DPS | Flag de exigência de DPS | Migrado |
| SEG_EFETIVACAO | Data de efetivação | Não migrado |
| CON_SEQ | Sequencial do contrato (aditivos) | Migrado como sequência do contrato |
| sql_rowid | ID técnico (auto-incremento) | Não migrado |
| sql_deleted | Flag de exclusão lógica (F/T) | Filtro de elegibilidade |

**Critérios de seleção (inferido)**:
- `SEG_MODALIDADE = 4` (somente empréstimos)
- `SEG_CANCTIPO = 0` (não cancelados)
- `SEG_FIM >= '2025-12-10'` (vigência futura)
- `sql_deleted = 'F'` (não excluídos logicamente)
- `SEG_FIM > '2026-01-01'` (vigência posterior a data de corte)

**Junção com tabela `ep_contrato`**:
- Chave: `SEG_CONTRATO = CON_NDOC` e `CON_SEQ`
- Critério adicional: `CON_PGTO IS NULL` (contrato não liquidado)

**Junção com tabela `cc_conta`**:
- Chave: `CCO_CONTA = CCO_CONTA`
- Critério adicional: `CCO_SITUACAO = 1` (conta ativa)

---

#### Tabela: `ep_segparcela`
**Descrição**: Parcelas dos contratos de seguro

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| cco_conta | Conta corrente do cooperado | Chave de agrupamento |
| seg_contrato | Número do contrato | Chave de agrupamento |
| con_seq | Sequencial do contrato | Chave de agrupamento |
| seg_parcela | Número da parcela | Migrado como número da parcela |
| seg_valor | Valor da parcela | Migrado como valor da parcela e valor original |
| seg_vcto | Data de vencimento | Migrado como vencimento |
| seg_pgto | Data de pagamento | Migrado como data de liquidação |
| seg_cancelado | Data de cancelamento | Usado para determinar status |
| seg_liberado | Flag de liberação (0=mesa, 1=liberado) | Não migrado diretamente |
| sql_rowid | ID técnico | Não migrado |

---

#### Tabela: `ep_contrato`
**Descrição**: Contratos de empréstimo (usada apenas em junção)

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| CON_NDOC | Número do documento/contrato | Chave de junção |
| CON_SEQ | Sequencial do contrato | Chave de junção |
| CON_DEBSEGURO | Tipo de débito do seguro: 2=À Vista, 1=Parcelado | Determina tipo de pagamento |
| CON_PARCELAS | Quantidade de parcelas do empréstimo | Informativo |
| MOD_CALCULO | Modalidade de cálculo: 2,3=PRICE/SAC (variável), outros=fixo | Determina tipo de capital (saldo) |
| CON_PGTO | Data de pagamento/liquidação | Filtro de elegibilidade |

---

#### Tabela: `ep_parcela`
**Descrição**: Parcelas dos empréstimos (usada em agregação)

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| cco_conta | Conta corrente | Chave de agregação |
| con_ndoc | Número do contrato | Chave de agregação |
| con_seq | Sequencial | Chave de agregação |
| emp_vlrseg | Valor do seguro na parcela | Agregado para validação |
| emp_parcela | Número da parcela | Contagem para validação |

---

#### Tabela: `cc_conta`
**Descrição**: Contas correntes dos cooperados

| Campo | Descrição | Uso no ETL |
|-------|-----------|------------|
| CCO_CONTA | Código da conta corrente | Chave de junção |
| CLI_CPFCNPJ | CPF/CNPJ do titular | Lookup para cooperado |
| PA_CODIGO | Código do ponto de atendimento | Determina PA do seguro |
| CCO_SITUACAO | Situação da conta: 1=Ativa | Filtro de elegibilidade |

---

### 4.3 Campos Calculados na Extração

A consulta de extração de contratos realiza cálculos e agregações:

| Campo Calculado | Descrição | Regra |
|-----------------|-----------|-------|
| tipo_seguro | Tipo de pagamento do seguro | Se `CON_DEBSEGURO = 2` ou `CON_PARCELAS = 1` → 1 (À Vista); Se há mais de uma fonte com valor > 0 → 2 (Parcelado); Caso contrário → 3 (Único) |
| tipo_saldo | Tipo de capital segurado | Se `MOD_CALCULO IN (2, 3)` → 1 (Variável/PRICE/SAC); Caso contrário → 2 (Fixo) |
| Soma_Das_Parcelas | Soma dos valores das parcelas | Primeiro tenta do módulo de seguro; fallback para tabela de parcelas |
| Qtd_Parcelas_Seguro | Quantidade de parcelas | Primeiro tenta do módulo de seguro; fallback para tabela de parcelas |

---

## 5. Regras de Extração

### 5.1 Descoberta de Agências

**Fonte**: Schema `unico`, tabela `cd_agencia`

**Regras**:
- Seleciona todas as agências onde o status é ativo (`AG_ATIVA = 1`)
- Gera o nome do schema concatenando `agencia_` com o código da agência
- O código da agência é convertido para texto (8 caracteres máximo)

**Saída**: Lista de schemas para iteração (ex: `agencia_0001`, `agencia_0012`, etc.)

---

### 5.2 Extração de Contratos de Seguro

**Fonte Principal**: Tabela `ep_segprestamista`

**Junções**:
1. **INNER JOIN** com `ep_contrato`:
   - Chave: `SEG_CONTRATO = CON_NDOC` e `CON_SEQ = CON_SEQ`
   - Propósito: Obter modalidade de cálculo e tipo de débito do seguro
   
2. **JOIN** com `cc_conta`:
   - Chave: `CCO_CONTA = CCO_CONTA`
   - Propósito: Verificar situação da conta

3. **LEFT JOIN** com subconsulta `ResumoFinanceiro`:
   - Chave: `SEG_CONTRATO = contrato` e `CON_SEQ = con_seq`
   - Propósito: Obter totais agregados de parcelas

**Agregação (ResumoFinanceiro)**:
A subconsulta combina duas fontes de dados de parcelas:
1. **Fonte 1 (MODULO_SEGURO)**: Tabela `ep_segparcela`
   - Agrupa por conta, contrato, sequencial
   - Soma valores (`seg_valor`)
   - Conta parcelas (`seg_parcela`)
   
2. **Fonte 2 (TABELA_PARCELAS)**: Tabela `ep_parcela`
   - Filtra onde `emp_vlrseg > 0`
   - Agrupa por conta, contrato, sequencial
   - Soma valores de seguro (`emp_vlrseg`)
   - Conta parcelas com seguro

**Filtros de Elegibilidade**:
| Filtro | Valor | Justificativa |
|--------|-------|---------------|
| SEG_MODALIDADE | = 4 | Somente modalidade Empréstimo |
| CCO_SITUACAO | = 1 | Somente contas ativas |
| SEG_CANCTIPO | = 0 | Somente contratos não cancelados |
| SEG_FIM | >= '2025-12-10' | Vigência futura (data parametrizada - inferido) |
| CON_PGTO | IS NULL | Contrato não liquidado |
| sql_deleted | = 'F' | Não excluídos logicamente |
| SEG_FIM | > '2026-01-01' | Data de corte para migração |

**Agrupamento Final**:
- Agrupa por: `SEG_CONTRATO`, `CON_SEQ`, `SEG_PREMIO`, `CON_DEBSEGURO`, `CON_PARCELAS`, `MOD_CALCULO`, `SEG_FIM`

---

### 5.3 Extração de Parcelas

**Fonte**: Tabela `ep_segparcela`

**Regras**:
- Extrai todas as parcelas (sem filtro específico)
- Agrupa em memória por chave composta: (conta corrente, número do contrato)
- Associa a cada contrato durante o processamento

**Campos extraídos**: Todos os campos da tabela

---

### 5.4 Extração de Cooperados

**Fonte**: Tabela `cd_cliente` (schema `unico`)

**Regras**:
- Extrai todos os cooperados cadastrados
- Aplica tratamento de nulos com COALESCE para CPF/CNPJ e nome
- Carrega em cache indexado por número do documento

---

### 5.5 Extração de Seguradoras

**Fonte**: Tabela `cd_prestamista` (schema `unico`)

**Regras**:
- Extrai todas as configurações de seguradoras
- Inclui todos os campos de limites por faixa etária
- Inclui todas as contas contábeis
- Indexa em cache por CNPJ (derivado do código)

---

### 5.6 Extração de Contas Correntes

**Fonte**: Tabela `cc_conta` (por agência)

**Regras**:
- Carrega quando o schema muda
- Extrai conta corrente, CPF/CNPJ do titular e código do PA
- Usado para determinar o ponto de atendimento do seguro

# Parte 2: Catálogo de Destinos e Matriz de Mapeamento

---

## 6. Catálogo de Destinos (MySQL)

### 6.1 Tabela: `agencia`
**Descrição**: Armazena informações cadastrais das agências da cooperativa

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único, auto-incremento |
| codigo | CHAR(4) | Sim | Código único da agência |
| nome | VARCHAR(255) | Sim | Nome completo da agência |
| criado_em | DATETIME | Sim | Data/hora de criação (automático) |

**Chave Primária**: `id`
**Índices Únicos**: `codigo`, `nome`
**Papel**: Dimensão - Cadastro de agências

---

### 6.2 Tabela: `ponto_atendimento`
**Descrição**: Armazena informações dos pontos de atendimento vinculados às agências

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único, auto-incremento |
| agencia_id | BIGINT UNSIGNED | Sim | FK para tabela agencia |
| codigo | CHAR(3) | Sim | Código do PA (único por agência) |
| nome | VARCHAR(255) | Sim | Nome completo do PA |
| criado_em | DATETIME | Não | Data/hora de criação |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `agencia_id` → `agencia.id`
**Índices Únicos**: `nome`, `(agencia_id, codigo)`
**Papel**: Dimensão - Local de atendimento

---

### 6.3 Tabela: `cooperado`
**Descrição**: Cadastro de cooperados (clientes) da cooperativa

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único, auto-incremento |
| numero_documento | VARCHAR(14) | Sim | CPF (11) ou CNPJ (14), sem formatação |
| tipo | ENUM('Física', 'Jurídica') | Sim | Tipo de pessoa |
| nome | VARCHAR(255) | Sim | Nome completo ou razão social |
| nome_fantasia | VARCHAR(255) | Não | Nome fantasia (PJ) |
| email | VARCHAR(255) | Não | E-mail de contato |

**Chave Primária**: `id`
**Papel**: Dimensão - Cadastro de clientes

---

### 6.4 Tabela: `cooperado_agencia_conta`
**Descrição**: Junção entre cooperados, agências e contas correntes

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único, auto-incremento |
| cooperado_id | BIGINT UNSIGNED | Sim | FK para tabela cooperado |
| agencia_id | BIGINT UNSIGNED | Sim | FK para tabela agencia |
| conta_corrente | CHAR(9) | Sim | Código da conta corrente |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `cooperado_id` → `cooperado.id`, `agencia_id` → `agencia.id`
**Índices Únicos**: `(cooperado_id, agencia_id, conta_corrente)`
**Papel**: Ponte - Relaciona cooperado, agência e conta

---

### 6.5 Tabela: `seguradora`
**Descrição**: Cadastro das seguradoras parceiras

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único, auto-incremento |
| nome | VARCHAR(255) | Sim | Nome fantasia |
| cnpj | CHAR(14) | Sim | CNPJ sem formatação |
| razao_social | VARCHAR(255) | Sim | Razão social completa |
| status | ENUM('Ativo', 'Inativo') | Sim | Status da seguradora |

**Chave Primária**: `id`
**Índices Únicos**: `cnpj`
**Papel**: Dimensão - Cadastro de seguradoras

---

### 6.6 Tabela: `seguradora_limite`
**Descrição**: Faixas etárias, coeficientes e limites de DPS por seguradora

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| seguradora_id | BIGINT UNSIGNED | Sim | FK para tabela seguradora |
| idade_inicial | SMALLINT | Sim | Idade inicial da faixa |
| idade_final | SMALLINT | Sim | Idade final da faixa |
| valor_maximo | DECIMAL(10,2) | Sim | Valor máximo de capital segurado |
| coeficiente | DECIMAL(8,7) | Sim | Coeficiente multiplicador para cálculo |
| limite_dps | DECIMAL(10,2) | Sim | Limite para exigir DPS |
| descricao_regra | VARCHAR(255) | Sim | Descrição textual da regra |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `seguradora_id` → `seguradora.id`
**Papel**: Configuração - Regras por faixa etária

---

### 6.7 Tabela: `comissao_seguradora`
**Descrição**: Configurações de comissões por seguradora

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| seguradora_id | BIGINT UNSIGNED | Sim | FK para tabela seguradora |
| porcentagem_comissao_corretora | DECIMAL(5,4) | Sim | Percentual corretora (ex: 0.1500 = 15%) |
| porcentagem_comissao_cooperativa | DECIMAL(5,4) | Sim | Percentual cooperativa |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `seguradora_id` → `seguradora.id`
**Índices Únicos**: `seguradora_id`
**Papel**: Configuração - Percentuais de comissão

---

### 6.8 Tabela: `condicao_seguradora`
**Descrição**: Parâmetros de condições operacionais por seguradora

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| seguradora_id | BIGINT UNSIGNED | Sim | FK para tabela seguradora |
| max_meses_contrato | SMALLINT | Sim | Máximo de meses de vigência |
| max_idade | SMALLINT | Sim | Idade máxima permitida |
| porcentagem_cobertura_morte | DECIMAL(5,4) | Sim | % cobertura morte |
| porcentagem_cobertura_invalidez | DECIMAL(5,4) | Sim | % cobertura invalidez |
| porcentagem_cobertura_perda_renda | DECIMAL(5,4) | Sim | % cobertura perda de renda |
| periodicidade_30dias | TINYINT(1) | Sim | Se vencimento é a cada 30 dias |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `seguradora_id` → `seguradora.id`
**Índices Únicos**: `seguradora_id`
**Papel**: Configuração - Condições contratuais

---

### 6.9 Tabela: `contabilizacao_seguradora`
**Descrição**: Contas contábeis para lançamentos da seguradora

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| seguradora_id | BIGINT UNSIGNED | Sim | FK para tabela seguradora |
| credito_premio_contratacao | VARCHAR(50) | Sim | Conta crédito prêmio |
| descricao_credito_premio_contratacao | VARCHAR(255) | Sim | Descrição |
| debito_premio_contratacao | VARCHAR(50) | Sim | Conta débito prêmio |
| descricao_debito_premio_contratacao | VARCHAR(255) | Sim | Descrição |
| credito_comissao_contratacao | VARCHAR(50) | Sim | Conta crédito comissão |
| descricao_credito_comissao_contratacao | VARCHAR(255) | Sim | Descrição |
| debito_comissao_contratacao | VARCHAR(50) | Sim | Conta débito comissão |
| descricao_debito_comissao_contratacao | VARCHAR(255) | Sim | Descrição |
| credito_cancelamento_comissao_parc_tot | VARCHAR(50) | Sim | Conta crédito cancelamento |
| descricao_credito_cancelamento_comissao_parc_tot | VARCHAR(255) | Sim | Descrição |
| debito_cancelamento_comissao_parc_tot | VARCHAR(50) | Sim | Conta débito cancelamento |
| descricao_debito_cancelamento_comissao_parc_tot | VARCHAR(255) | Sim | Descrição |
| credito_cancelamento_comissao_avista | VARCHAR(50) | Sim | Conta crédito cancel. à vista |
| descricao_credito_cancelamento_comissao_avista | VARCHAR(255) | Sim | Descrição |
| debito_cancelamento_comissao_avista | VARCHAR(50) | Sim | Conta débito cancel. à vista |
| descricao_debito_cancelamento_comissao_avista | VARCHAR(255) | Sim | Descrição |
| credito_valor_pago | VARCHAR(50) | Sim | Conta crédito valor pago |
| descricao_credito_valor_pago | VARCHAR(255) | Sim | Descrição |
| debito_valor_pago | VARCHAR(50) | Sim | Conta débito valor pago |
| descricao_debito_valor_pago | VARCHAR(255) | Sim | Descrição |
| credito_comissao_valor_pago | VARCHAR(50) | Sim | Conta crédito comissão pago |
| descricao_comissao_credito_valor_pago | VARCHAR(255) | Sim | Descrição |
| debito_comissao_valor_pago | VARCHAR(50) | Sim | Conta débito comissão pago |
| descricao_comissao_debito_valor_pago | VARCHAR(255) | Sim | Descrição |
| debito_premio_parcela | VARCHAR(50) | Sim | Conta débito parcela |
| descricao_debito_premio_parcela | VARCHAR(255) | Sim | Descrição |
| credito_premio_parcela | VARCHAR(50) | Sim | Conta crédito parcela |
| descricao_credito_premio_parcela | VARCHAR(255) | Sim | Descrição |
| debito_comissao_parcela | VARCHAR(50) | Sim | Conta débito comissão parcela |
| descricao_debito_comissao_parcela | VARCHAR(255) | Sim | Descrição |
| credito_comissao_parcela | VARCHAR(50) | Sim | Conta crédito comissão parcela |
| descricao_credito_comissao_parcela | VARCHAR(255) | Sim | Descrição |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `seguradora_id` → `seguradora.id`
**Papel**: Configuração - Plano de contas contábeis

---

### 6.10 Tabela: `apolice_grupo_seguradora`
**Descrição**: Configurações de apólices e grupos por vínculo agência-seguradora

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| agencia_id | BIGINT UNSIGNED | Sim | FK para tabela agencia |
| seguradora_id | BIGINT UNSIGNED | Sim | FK para tabela seguradora |
| ordem | INT | Sim | Ordem de prioridade |
| apolice | VARCHAR(255) | Não | Número da apólice |
| grupo | VARCHAR(255) | Não | Código do grupo |
| subgrupo | VARCHAR(255) | Não | Código do subgrupo |
| tipo_capital | ENUM('Fixo', 'Variável') | Sim | Tipo de capital segurado |
| modalidade_unico | VARCHAR(50) | Não | Identificador modalidade única |
| modalidade_avista | DECIMAL(10,2) | Não | Taxa para à vista |
| modalidade_parcelado | DECIMAL(10,2) | Não | Taxa para parcelado |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `agencia_id` → `agencia.id`, `seguradora_id` → `seguradora.id`
**Papel**: Ponte - Relaciona agência com seguradora e apólice

---

### 6.11 Tabela: `seguro`
**Descrição**: Contratos de seguros e seus metadados financeiros

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| cooperado_agencia_conta_id | BIGINT UNSIGNED | Sim | FK para cooperado_agencia_conta |
| ponto_atendimento_id | BIGINT UNSIGNED | Sim | FK para ponto_atendimento |
| apolice_grupo_seguradora_id | BIGINT UNSIGNED | Sim | FK para apolice_grupo_seguradora |
| seguro_parametro_id | BIGINT UNSIGNED | Sim | FK para seguro_parametro |
| usuario_id | BIGINT UNSIGNED | Não | FK para usuario (quem contratou) |
| contrato_sequencia | VARCHAR(2) | Sim | Sequência do contrato (00, 01, etc.) |
| status | ENUM('Pendente', 'Ativo', 'Recusado', 'Expirado', 'Cancelado') | Sim | Status atual do seguro |
| motivo | ENUM (ver lista completa abaixo) | Sim | Motivo do status |
| contrato | VARCHAR(10) | Sim | Número do contrato |
| inicio_vigencia | DATE | Não | Data de início |
| fim_vigencia | DATE | Não | Data de término |
| codigo_grupo | INT | Sim | Código do grupo/produto |
| quantidade_parcelas | SMALLINT | Sim | Quantidade de parcelas |
| vencimento | DATE | Não | Data de vencimento base |
| capital_segurado | DECIMAL(10,2) | Sim | Valor do capital segurado |
| premio_total | DECIMAL(10,2) | Sim | Valor do prêmio total |
| tipo_pagamento | ENUM('À Vista', 'Parcelado', 'Único') | Sim | Modalidade de pagamento |
| estorno_proporcional | DECIMAL(10,2) | Sim | Valor de estorno (cancelamento) |
| valor_base | DECIMAL(10,2) | Não | Valor base para cálculo |
| declaracao_pessoal_saude | TINYINT(1) | Não | Se exigiu DPS |
| valor_iof | DECIMAL(10,2) | Não | Valor do IOF |
| numero_contrato_emprestimo | VARCHAR(20) | Não | Número do contrato de crédito |

**Valores do campo `motivo`**:
- `Em analise na seguradora`
- `Aguardando faturamento`
- `Aguardando documentação`
- `Pagamento à vista`
- `Pagamento parcelado`
- `Inadimplente`
- `Regular`
- `Recusado pela seguradora`
- `Expiração da vigência do seguro`
- `Aditivo`
- `Cancelamento por prejuízo`
- `Renegociação`
- `Sinistro`
- `Solicitado pela cooperativa`
- `Solicitado pelo cooperado`
- `Liquidação antecipada`

**Chave Primária**: `id`
**Chaves Estrangeiras**: Todas as FKs referenciadas acima
**Papel**: Fato - Registro principal de contratos de seguro

---

### 6.12 Tabela: `seguro_parametro`
**Descrição**: Parâmetros de contratação do seguro para cálculos

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| tipo_capital | ENUM('Fixo', 'Variável') | Sim | Tipo de capital |
| periodicidade_30dias | TINYINT(1) | Sim | Se periodicidade é 30 dias |
| coeficiente | DECIMAL(8,7) | Sim | Coeficiente multiplicador |
| porcentual_iof | DECIMAL(5,4) | Sim | Percentual de IOF |
| porcentagem_comissao_corretora | DECIMAL(5,4) | Sim | % comissão corretora |
| porcentagem_comissao_cooperativa | DECIMAL(5,4) | Sim | % comissão cooperativa |
| porcentagem_cobertura_morte | DECIMAL(5,4) | Não | % cobertura morte |
| capital_morte | DECIMAL(18,2) | Não | Capital por morte |
| premio_morte | DECIMAL(18,2) | Não | Prêmio por morte |
| porcentagem_cobertura_invalidez | DECIMAL(5,4) | Não | % cobertura invalidez |
| capital_invalidez | DECIMAL(18,2) | Não | Capital por invalidez |
| premio_invalidez | DECIMAL(18,2) | Não | Prêmio por invalidez |

**Chave Primária**: `id`
**Relacionamento**: 1:1 com `seguro`
**Papel**: Configuração - Parâmetros de cálculo por contrato

---

### 6.13 Tabela: `parcela`
**Descrição**: Parcelas financeiras de prêmio vinculadas a contratos de seguro

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único |
| seguro_id | BIGINT UNSIGNED | Sim | FK para tabela seguro |
| status | ENUM('Pendente', 'Em Aberto', 'Pago', 'Cancelada') | Sim | Status da parcela |
| numero_parcela | SMALLINT | Sim | Número sequencial (1, 2, 3...) |
| valor_parcela | DECIMAL(10,2) | Sim | Valor atual da parcela |
| valor_original | DECIMAL(10,2) | Sim | Valor original calculado |
| valor_pago | DECIMAL(10,2) | Sim | Valor efetivamente pago |
| vencimento | DATE | Sim | Data de vencimento |
| liquidacao | DATETIME | Não | Data/hora de quitação |
| data_ultimo_pagamento | DATETIME | Não | Data/hora do último pagamento |
| comissao_corretora | DECIMAL(10,2) | Sim | Valor da comissão do corretor |
| comissao_cooperativa | DECIMAL(10,2) | Sim | Valor da comissão da cooperativa |

**Chave Primária**: `id`
**Chaves Estrangeiras**: `seguro_id` → `seguro.id`
**Papel**: Fato - Parcelas de pagamento

---

## 7. Regras de Transformação

### 7.1 Transformação de Agências

| Aspecto | Regra |
|---------|-------|
| **Origem** | `cd_agencia.AG_CODIGO`, `cd_agencia.AG_SIGLA` |
| **Destino** | Tabela `agencia` |
| **Código** | Copia diretamente `AG_CODIGO` |
| **Nome** | Copia diretamente `AG_SIGLA` |
| **Deduplicação** | Agrupa por código para evitar duplicatas |

---

### 7.2 Transformação de Pontos de Atendimento

| Aspecto | Regra |
|---------|-------|
| **Origem** | `cd_pa.PA_CODIGO`, `cd_pa.PA_SIGLA`, `cd_pa.AG_CODIGO` |
| **Destino** | Tabela `ponto_atendimento` |
| **AgenciaId** | Lookup por código da agência |
| **Código** | Copia `PA_CODIGO` |
| **Nome** | Concatena: código + " - " + sigla |
| **Verificação** | Verifica existência antes de inserir (agencia_id + codigo) |

---

### 7.3 Transformação de Cooperados

| Aspecto | Regra |
|---------|-------|
| **Origem** | `cd_cliente` |
| **Destino** | Tabela `cooperado` |
| **NumeroDocumento** | Copia `CLI_CPFCNPJ` (COALESCE com vazio) |
| **Tipo** | Converte: "F" → "Física", demais → "Jurídica" |
| **Nome** | Copia `CLI_NOME` (COALESCE com vazio) |
| **NomeFantasia** | Copia `CLI_NFANTA` (pode ser nulo) |
| **Email** | Copia `CLI_EMAIL` (pode ser nulo) |

---

### 7.4 Transformação de Seguradoras

| Aspecto | Regra |
|---------|-------|
| **Origem** | `cd_prestamista` |
| **Destino** | Tabela `seguradora` + tabelas relacionadas |
| **CNPJ** | Deriva do código: preenche com zeros à direita até 14 caracteres, depois trunca em 14 |
| **Nome** | Copia `PST_NOME` |
| **RazaoSocial** | Copia `PST_NOME` (mesmo valor) |
| **Status** | Converte: `PST_ATIVO = true` → "Ativo", false → "Inativo" |

#### Criação de Comissões
| Aspecto | Regra |
|---------|-------|
| **PorcentagemComissaoCorretora** | Valor fixo: 0.15 (15%) |
| **PorcentagemComissaoCooperativa** | Copia `PORCENTAGEM_COMISSAO` |

#### Criação de Condições
| Aspecto | Regra |
|---------|-------|
| **MaxMesesContrato** | Copia `PST_MAXMESES` |
| **MaxIdade** | Copia `PST_LIMITEIDADE` |
| **PorcentagemCoberturaMorte** | Valor fixo: 0.1 (10%) |
| **PorcentagemCoberturaInvalidez** | Valor fixo: 0.1 (10%) |
| **PorcentagemCoberturaPerdaRenda** | Valor fixo: 0.1 (10%) |
| **Periodicidade30Dias** | Condição: código ≠ "0005" → true, senão false |

#### Criação de Contabilização
| Campo Destino | Campo Origem |
|---------------|--------------|
| CreditoPremioContratacao | CONTA_CONTABIL_CREDITO |
| DebitoPremioContratacao | CONTA_CONTABIL_DEBITO |
| CreditoComissaoContratacao | CONTA_CONTABIL_CREDITO_COMISSAO |
| DebitoComissaoContratacao | CONTA_CONTABIL_DEBITO_COMISSAO |
| CreditoCancelamentoComissaoParcTot | CONTA_CONTABIL_CREDITO_4966 |
| DebitoCancelamentoComissaoParcTot | CONTA_CONTABIL_DEBITO_4966 |
| CreditoCancelamentoComissaoAVista | CONTA_CONTABIL_CREDITO_COMISSAO_4966 |
| DebitoCancelamentoComissaoAVista | CONTA_CONTABIL_DEBITO_COMISSAO_4966 |
| CreditoValorPago | CONTA_CONTABIL_CREDITO |
| DebitoValorPago | CONTA_CONTABIL_DEBITO |
| CreditoComissaoValorPago | CONTA_CONTABIL_CREDITO_COMISSAO |
| DebitoComissaoValorPago | CONTA_CONTABIL_DEBITO_COMISSAO |
| Demais campos | String vazia (não migrados) |

#### Criação de Limites por Faixa Etária
Para cada faixa etária (30, 35, 40, 45, 50, 55, 60, 65, 70, 75, 80, 85):
- **Condição de criação**: Limite > 0 OU Coeficiente > 0 para a faixa
- **IdadeInicial**: Idade anterior + 1 (ex: faixa 35 → idade inicial 31)
- **IdadeFinal**: Valor da faixa (ex: faixa 35 → idade final 35)
- **Coeficiente**: Valor do campo `PST_COEFXX`
- **ValorMaximo**: Valor do campo `PST_LIMITEXX`
- **LimiteDps**: Se `PST_DPSXX = true` → `PST_VALORDPS`, senão 0
- **DescricaoRegra**: Texto descritivo (ex: "Faixa 31 a 35 anos")

**Faixa padrão**: Se nenhuma faixa definida, cria uma faixa de 18 até `LimiteIdade` com valores padrão.

---

### 7.5 Transformação de Apólice/Grupo Seguradora

| Aspecto | Regra |
|---------|-------|
| **Quando criar** | Quando não existe vínculo (agencia_id, seguradora_id) |
| **Apolice** | Valor fixo: "APO-001" |
| **Grupo** | Valor fixo: "GRP-001" |
| **SubGrupo** | Valor fixo: "SUB-001" |
| **TipoCapital** | Valor fixo: "Fixo" |
| **ModalidadeUnico** | Valor fixo: "Unico" |
| **ModalidadeAVista** | Valor fixo: 1.5 |
| **ModalidadeParcelado** | Valor fixo: 10 |

---

### 7.6 Transformação de Seguros

| Aspecto | Regra |
|---------|-------|
| **Status** | Derivado do Motivo (ver regra de derivação) |
| **Motivo** | Valor fixo: "Regular" (para contratos migrados) |
| **Contrato** | Copia `SEG_CONTRATO` (COALESCE com vazio) |
| **InicioVigencia** | Copia `SEG_INICIO` |
| **FimVigencia** | Copia `SEG_FIM` |
| **QuantidadeParcelas** | Copia `SEG_MESES` (COALESCE com 0), converte para short |
| **Vencimento** | Copia `SEG_FIM` |
| **CapitalSegurado** | Copia `SEG_BASE` (COALESCE com 0.00) |
| **PremioTotal** | Copia `SEG_PREMIO` (COALESCE com 0.00) |
| **ValorBase** | Copia `SEG_BASE` |
| **ValorIof** | Copia `SEG_IOF` |
| **ContratoSequencia** | Formata `CON_SEQ` como 2 dígitos (ex: "00", "01") |
| **TipoPagamento** | Converte: tipo_seguro = 1 → "À Vista", demais → "Parcelado" |
| **EstornoProporcional** | Valor fixo: 0 |
| **CodigoGrupo** | Valor fixo: 0 (inferido) |

#### Regra de Derivação de Status pelo Motivo
| Motivo | Status Resultante |
|--------|-------------------|
| EmAnaliseNaSeguradora, AguardandoFaturamento, AguardandoDocumentacao | Pendente |
| PagamentoAVista, PagamentoParcelado, Inadimplente, Regular | Ativo |
| RecusadoPelaSeguradora | Recusado |
| ExpiracaoVigenciaSeguro | Expirado |
| Aditivo, CancelamentoPorPrejuizo, Renegociacao, Sinistro, SolicitadoPelaCooperativa, SolicitadoPeloCooperado, LiquidacaoAntecipada | Cancelado |

---

### 7.7 Transformação de Parâmetros do Seguro

| Campo Destino | Regra de Transformação |
|---------------|------------------------|
| TipoCapital | Converte: tipo_saldo = 1 → "Variável", demais → "Fixo" |
| Coeficiente | Condicional: Se Variável → 0.0003511, Se Fixo → 0.0005945 |
| Periodicidade30Dias | Valor fixo: true |
| PorcentualIof | Valor fixo: 0.0038 (0.38%) |
| PorcentagemComissaoCorretora | Da comissão da seguradora ou padrão 0.45 (45%) |
| PorcentagemComissaoCooperativa | Da comissão da seguradora ou padrão 0.20 (20%) |

---

### 7.8 Transformação de Parcelas

| Campo Destino | Regra de Transformação |
|---------------|------------------------|
| Status | Se `seg_cancelado` preenchido → "Pago", senão → "Pendente" |
| NumeroParcela | Copia `seg_parcela`, converte para ushort |
| ValorParcela | Copia `seg_valor` |
| ValorOriginal | Copia `seg_valor` (mesmo valor) |
| Vencimento | Copia `seg_vcto` |
| Liquidacao | Copia `seg_pgto` |
| DataUltimoPagamento | Copia `seg_pgto` |
| ValorPago | Se `seg_pgto` preenchido → `seg_valor`, senão → 0.00 |
| ComissaoCorretora | Valor fixo: 0 (não calculado na migração) |
| ComissaoCooperativa | Valor fixo: 0 (não calculado na migração) |

---

## 8. Matriz de Mapeamento (Linhagem)

### 8.1 Tabela `agencia`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| codigo | cd_agencia.AG_CODIGO | Cópia direta | Chave natural |
| nome | cd_agencia.AG_SIGLA | Cópia direta | - |
| criado_em | - | CURRENT_TIMESTAMP | Automático |

---

### 8.2 Tabela `ponto_atendimento`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| agencia_id | agencia.id | Lookup por cd_pa.AG_CODIGO | Busca ID da agência |
| codigo | cd_pa.PA_CODIGO | Cópia direta | 3 caracteres |
| nome | cd_pa.PA_CODIGO + cd_pa.PA_SIGLA | Concatenação: código + " - " + sigla | - |
| criado_em | - | CURRENT_TIMESTAMP | Automático |

---

### 8.3 Tabela `cooperado`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| numero_documento | cd_cliente.CLI_CPFCNPJ | COALESCE(valor, '') | Nunca nulo |
| tipo | cd_cliente.CLI_TIPPES | "F" → "Física", outros → "Jurídica" | Enumerado |
| nome | cd_cliente.cli_nome | COALESCE(valor, '') | Nunca nulo |
| nome_fantasia | cd_cliente.CLI_NFANTA | Cópia direta | Pode ser nulo |
| email | cd_cliente.CLI_EMAIL | Cópia direta | Pode ser nulo |

---

### 8.4 Tabela `cooperado_agencia_conta`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| cooperado_id | cooperado.id | Lookup por CPF/CNPJ | Busca ou cria cooperado |
| agencia_id | agencia.id | Lookup por código | Busca agência existente |
| conta_corrente | cc_conta.CCO_CONTA | Cópia direta | 9 caracteres |

---

### 8.5 Tabela `seguradora`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| nome | cd_prestamista.PST_NOME | Cópia direta | - |
| cnpj | cd_prestamista.PST_CODIGO | PadRight(14,'0').Substring(0,14) | Derivado do código |
| razao_social | cd_prestamista.PST_NOME | Cópia direta | Mesmo valor do nome |
| status | cd_prestamista.PST_ATIVO | true → "Ativo", false → "Inativo" | Enumerado |

---

### 8.6 Tabela `seguradora_limite`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| seguradora_id | seguradora.id | FK gerado na criação | - |
| idade_inicial | Constante | Idade anterior + 1 | Ex: 31 para faixa 35 |
| idade_final | cd_prestamista.PST_LIMITEXX | Valor da faixa (30, 35, 40...) | - |
| valor_maximo | cd_prestamista.PST_LIMITEXX | Cópia direta | - |
| coeficiente | cd_prestamista.PST_COEFXX | Cópia direta | 7 casas decimais |
| limite_dps | cd_prestamista.PST_DPSXX + PST_VALORDPS | Se DPS=true → VALORDPS, senão 0 | Condicional |
| descricao_regra | Constante | Texto descritivo gerado | Ex: "Faixa 31 a 35 anos" |

---

### 8.7 Tabela `seguro`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| cooperado_agencia_conta_id | cooperado_agencia_conta.id | Lookup por CPF + conta + agência | - |
| ponto_atendimento_id | ponto_atendimento.id | Lookup por código do PA da conta | Via cc_conta.PA_CODIGO |
| apolice_grupo_seguradora_id | apolice_grupo_seguradora.id | Lookup por agência + seguradora | Cria se não existir |
| seguro_parametro_id | seguro_parametro.id | FK do parâmetro criado | Criado junto |
| usuario_id | - | NULL | Não migrado |
| contrato_sequencia | ep_segprestamista.CON_SEQ | ToString("00") | Formato 2 dígitos |
| status | ep_segprestamista (derivado) | Derivado do Motivo | Ver regra de status |
| motivo | Constante | "Regular" | Contratos ativos |
| contrato | ep_segprestamista.SEG_CONTRATO | COALESCE(valor, '') | 10 caracteres |
| inicio_vigencia | ep_segprestamista.SEG_INICIO | Cópia direta | DATE |
| fim_vigencia | ep_segprestamista.SEG_FIM | Cópia direta | DATE |
| codigo_grupo | Constante | 0 | Não mapeado |
| quantidade_parcelas | ep_segprestamista.SEG_MESES | COALESCE(valor, 0) | SMALLINT |
| vencimento | ep_segprestamista.SEG_FIM | Cópia direta | DATE |
| capital_segurado | ep_segprestamista.SEG_BASE | COALESCE(valor, 0.00) | DECIMAL(10,2) |
| premio_total | ep_segprestamista.SEG_PREMIO | COALESCE(valor, 0.00) | DECIMAL(10,2) |
| tipo_pagamento | ep_segprestamista.tipo_seguro | 1 → "À Vista", outros → "Parcelado" | Campo calculado |
| estorno_proporcional | Constante | 0 | Não migrado |
| valor_base | ep_segprestamista.SEG_BASE | Cópia direta | Pode ser nulo |
| declaracao_pessoal_saude | ep_segprestamista.SEG_DPS | Cópia direta | BOOLEAN |
| valor_iof | ep_segprestamista.SEG_IOF | Cópia direta | DECIMAL(10,2) |
| numero_contrato_emprestimo | - | NULL | Não migrado |

---

### 8.8 Tabela `seguro_parametro`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| tipo_capital | ep_segprestamista.tipo_saldo | 1 → "Variável", outros → "Fixo" | Campo calculado |
| periodicidade_30dias | Constante | true | Fixo na migração |
| coeficiente | ep_segprestamista.tipo_saldo | Variável: 0.0003511, Fixo: 0.0005945 | Condicional |
| porcentual_iof | Constante | 0.0038 | 0.38% |
| porcentagem_comissao_corretora | comissao_seguradora.porcentagem_comissao_corretora | COALESCE(valor, 0.45) | Fallback 45% |
| porcentagem_comissao_cooperativa | comissao_seguradora.porcentagem_comissao_cooperativa | COALESCE(valor, 0.20) | Fallback 20% |
| porcentagem_cobertura_morte | Constante | 0 | Não migrado |
| capital_morte | Constante | 0 | Não migrado |
| premio_morte | Constante | 0 | Não migrado |
| porcentagem_cobertura_invalidez | Constante | 0 | Não migrado |
| capital_invalidez | Constante | 0 | Não migrado |
| premio_invalidez | Constante | 0 | Não migrado |

---

### 8.9 Tabela `parcela`

| Coluna Destino | Origem (Tabela.Coluna) | Regra/Transformação | Observações |
|----------------|------------------------|---------------------|-------------|
| id | - | Auto-incremento | Gerado pelo banco |
| seguro_id | seguro.id | FK do seguro pai | Vinculado na criação |
| status | ep_segparcela.seg_cancelado | Se preenchido → "Pago", senão → "Pendente" | **Nota: inferido que usa seg_cancelado, ver observação** |
| numero_parcela | ep_segparcela.seg_parcela | Cast para ushort | SMALLINT |
| valor_parcela | ep_segparcela.seg_valor | Cópia direta | DECIMAL(10,2) |
| valor_original | ep_segparcela.seg_valor | Cópia direta | Mesmo valor |
| valor_pago | ep_segparcela.seg_pgto | Se preenchido → seg_valor, senão 0.00 | Condicional |
| vencimento | ep_segparcela.seg_vcto | Cópia direta | DATE |
| liquidacao | ep_segparcela.seg_pgto | Cópia direta | DATETIME |
| data_ultimo_pagamento | ep_segparcela.seg_pgto | Cópia direta | DATETIME |
| comissao_corretora | Constante | 0 | Não calculado |
| comissao_cooperativa | Constante | 0 | Não calculado |

**Observação sobre status da parcela**: A lógica usa o campo `seg_cancelado` para determinar status "Pago", o que parece ser uma inversão semântica (cancelado usado para pago). Isto deve ser validado na reimplementação.

# Parte 3: Regras de Carga, Controles Operacionais e Critérios de Aceitação

---

## 9. Regras de Carga e Consistência

### 9.1 Estratégia Geral de Carga

| Aspecto | Estratégia |
|---------|------------|
| **Tipo de Carga** | Carga inicial completa (full load) |
| **Modo de Operação** | Incremental por agência (processa uma agência por vez) |
| **Persistência** | Em lotes de 100 registros de seguro |
| **Transações** | Não utiliza transação global (comentado no código original) |
| **Idempotência** | Parcial - verifica existência antes de inserir para dimensões |

---

### 9.2 Estratégias por Tabela de Destino

#### Tabela: `agencia`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Upsert lógico (busca ou cria) |
| **Chave de Identificação** | `codigo` (4 caracteres) |
| **Verificação** | Busca em cache por código |
| **Se não existe** | Insere todos os registros de agências distintas e atualiza cache |
| **Se existe** | Retorna do cache, não atualiza |
| **Campos atualizáveis** | Nenhum (insert-only) |

#### Tabela: `ponto_atendimento`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Upsert lógico (busca ou cria) |
| **Chave de Identificação** | `(agencia_id, codigo)` |
| **Verificação** | Busca em cache por (código_agência, código_pa) |
| **Se não existe** | Insere e adiciona ao cache |
| **Se existe** | Retorna do cache |
| **Verificação adicional** | Antes do insert, verifica se já existe no banco |
| **Campos atualizáveis** | Nenhum (insert-only) |

#### Tabela: `cooperado`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Upsert lógico (busca ou cria) |
| **Chave de Identificação** | `numero_documento` (CPF/CNPJ) |
| **Verificação** | Busca em cache por documento |
| **Se não existe** | Busca na origem, insere no destino, adiciona ao cache |
| **Se existe** | Retorna do cache |
| **Campos atualizáveis** | Nenhum (insert-only) |

#### Tabela: `cooperado_agencia_conta`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Upsert lógico (busca ou cria) |
| **Chave de Identificação** | `(cooperado_id, agencia_id, conta_corrente)` |
| **Verificação** | Verifica nas contas do cooperado em cache |
| **Se não existe** | Insere e recarrega cache do cooperado |
| **Se existe** | Retorna o vínculo existente |
| **Campos atualizáveis** | Nenhum (insert-only) |

#### Tabela: `seguradora`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Upsert lógico (busca ou cria) |
| **Chave de Identificação** | `cnpj` (derivado do código, 14 caracteres) |
| **Verificação** | Busca em cache por CNPJ |
| **Se não existe** | Cria seguradora com todas as tabelas relacionadas (comissão, condição, contabilização, limites) |
| **Se existe** | Retorna do cache |
| **Campos atualizáveis** | Nenhum (insert-only) |
| **Cascata de criação** | Sim - cria automaticamente registros em tabelas dependentes |

#### Tabela: `apolice_grupo_seguradora`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Upsert lógico (busca ou cria) |
| **Chave de Identificação** | `(agencia_id, seguradora_id)` |
| **Verificação** | Busca em cache por (agencia_id, seguradora_id) |
| **Se não existe** | Insere com valores padrão e adiciona ao cache |
| **Se existe** | Retorna ID do cache |
| **Campos atualizáveis** | Nenhum (insert-only) |

#### Tabela: `seguro`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Insert only (carga inicial) |
| **Verificação de duplicidade** | Não há verificação explícita |
| **Persistência** | Em lotes de 100 registros |
| **Criação em cascata** | Cria `seguro_parametro` automaticamente |
| **Campos atualizáveis** | N/A (insert-only) |

#### Tabela: `seguro_parametro`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Insert junto com seguro |
| **Relacionamento** | 1:1 com seguro |
| **Criação** | Automática ao criar seguro |

#### Tabela: `parcela`

| Aspecto | Regra |
|---------|-------|
| **Estratégia** | Insert only (carga inicial) |
| **Verificação** | Não há |
| **Criação** | Em cascata com seguro (via navegação) |
| **Persistência** | Junto com o seguro (AddRange) |

---

### 9.3 Tratamento de Exclusões

| Tipo | Comportamento |
|------|---------------|
| **Hard Delete na origem** | Registros com `sql_deleted = 'T'` são ignorados na extração |
| **Soft Delete** | Não implementado no destino |
| **Espelhamento de status** | Não implementado |
| **Sincronia de exclusões** | Não há - registros excluídos na origem após migração inicial permanecerão no destino |

---

### 9.4 Garantias de Idempotência

| Cenário | Comportamento | Idempotente? |
|---------|---------------|--------------|
| Reexecução completa (tabelas vazias) | Cria todos os registros | ✅ Sim |
| Reexecução com dados existentes - Dimensões | Busca em cache, não duplica | ✅ Sim |
| Reexecução com dados existentes - Seguros | Cria todos os registros | ✅ Sim |
| Reexecução com dados existentes - Parcelas | Cria todos os registros | ✅ Sim |


---

### 9.5 Consistência e Integridade

| Aspecto | Implementação |
|---------|---------------|
| **Integridade referencial** | Garantida pelo processo (cria dependências antes) |
| **Ordem de criação** | Agência → PA → Cooperado → CoopAgConta → Seguradora → ApóliceGrupo → Seguro → Parcelas |
| **Transações** | deve utilizar transação englobante |
| **Rollback em caso de erro** | Total, deve reiniciar os Generator de PK  |
| **Consistência eventual** | Processo síncrono, sem filas |

---

## 10. Controles Operacionais

### 10.1 Logs e Auditoria

| Tipo de Log | Informação Registrada |
|-------------|----------------------|
| **Início da aplicação** | Timestamp de início |
| **Agências encontradas** | Lista de schemas a processar |
| **Início por agência** | Nome, código e ID da agência |
| **Nenhum contrato** | Mensagem quando base não tem contratos elegíveis |
| **Nenhuma parcela** | Mensagem quando não há parcelas |
| **Progresso** | Processados/Total, percentual, tempo restante estimado |
| **Conclusão por agência** | Quantidade de prestamistas e parcelas migrados |
| **Erro por registro** | Conta, contrato e mensagem de erro |
| **Erro fatal** | Stack trace completo |
| **Conclusão geral** | Total migrado vs total geral |

**Formato de log**: JSON compacto (via Serilog)
**Destino**: Console + Arquivo diário (Logs/log-YYYYMMDD.txt)
**Enriquecimento**: Nome da máquina, ID do processo, ID da thread

---

### 10.2 Métricas Coletadas

| Métrica | Descrição |
|---------|-----------|
| **TotalContratos** | Contador de contratos encontrados (todas agências) |
| **TotalMigrado** | Contador de seguros efetivamente persistidos |
| **Tempo por lote** | Calculado para estimativa de tempo restante |
| **Tempo total** | Cronometrado por agência |

---

### 10.3 Tratamento de Falhas

| Cenário | Comportamento |
|---------|---------------|
| **Erro em um registro** | Log de erro, continua para próximo registro |
| **Erro fatal** | Log fatal, encerra execução, faz rollback |
| **Agência sem dados** | Log informativo, continua para próxima |
| **Lookup não encontrado** | Lança exceção (ex: cooperado não encontrado) |

**Estratégia de retry**: Não implementada
**Backoff**: Não implementado
**Circuit breaker**: Não implementado

---

### 10.4 Parâmetros de Execução

| Parâmetro | Fonte | Descrição |
|-----------|-------|-----------|
| **SOURCE_DB** | Variável de ambiente | Connection string do banco de origem |
| **TARGET_DB** | Variável de ambiente | Connection string do banco de destino |
| **Schema dinâmico** | Calculado | `agencia_XXXX` derivado do código |

**Observação**: Não há parâmetros para data de corte, modo dry-run ou filtros adicionais no código analisado. As datas de filtro estão hardcoded nas queries.

---

### 10.5 Concorrência e Paralelismo

| Aspecto | Implementação |
|---------|---------------|
| **Processamento de agências** | Sequencial (uma por vez) |
| **Processamento de contratos** | Sequencial com preparação paralela (código comentado) |
| **Acesso a cache** | Thread-safe via `lock` |
| **Conexões de banco** | Pool gerenciado pelo driver |
| **MaxDegreeOfParallelism** | Definido como número de processadores (mas não utilizado ativamente) |

---

## 11. Casos Especiais e Exceções

### 11.1 Regras Condicionais

#### Determinação do Tipo de Seguro (tipo_pagamento)
```
SE (CON_DEBSEGURO = 2) OU (CON_PARCELAS = 1)
    ENTÃO tipo_seguro = 1 (À Vista)
SENÃO SE (há mais de uma fonte com valor > 0 no ResumoFinanceiro)
    ENTÃO tipo_seguro = 2 (Parcelado)
SENÃO
    tipo_seguro = 3 (Único/À Vista Lançamento Único)
```

#### Determinação do Tipo de Saldo (capital)
```
SE MOD_CALCULO IN (2, 3)
    ENTÃO tipo_saldo = 1 (Variável - PRICE/SAC)
SENÃO
    tipo_saldo = 2 (Fixo)
```

#### Determinação do Coeficiente
```
SE tipo_capital = Variável
    ENTÃO coeficiente = 0.0003511
SENÃO
    coeficiente = 0.0005945
```

#### Periodicidade por Seguradora
```
SE codigo_seguradora = "0005"
    ENTÃO Periodicidade30Dias = false
SENÃO
    Periodicidade30Dias = true
```

---

### 11.2 Tratamento de Dados Fora do Padrão

| Situação | Tratamento |
|----------|------------|
| **CPF/CNPJ nulo ou vazio** | COALESCE com string vazia |
| **Nome nulo** | COALESCE com string vazia |
| **Data de nascimento nula** | Aceita (não validado) |
| **Valor de prêmio nulo** | COALESCE com 0.00 |
| **Valor base nulo** | COALESCE com 0.00 |
| **Meses nulo** | COALESCE com 0 |
| **Conta contábil nula** | COALESCE com string vazia |
| **Comissão não encontrada** | Usa valores padrão (45%/20%) |

---

### 11.3 Ramos Alternativos

#### Cooperado Não Encontrado no Cache
1. Busca lista de cooperados da origem (se ainda não carregada)
2. Procura por número do documento
3. Se não encontrado: **lança exceção**
4. Se encontrado: cria no destino e adiciona ao cache

#### Conta Não Encontrada
1. Carrega lista de contas da agência (se schema mudou)
2. Procura por código da conta
3. Se não encontrada: **lança exceção**

#### Contrato Sem Parcelas
1. Verifica se há parcelas agrupadas para (conta, contrato)
2. Se não há parcelas: **retorna 0, não cria seguro**

---

## 12. Critérios de Aceitação para Reimplementação

### 12.1 Checklist de Equivalência Funcional

#### Descoberta e Iteração
- [ ] Conecta-se à base central (schema `unico`)
- [ ] Identifica todas as agências ativas (`AG_ATIVA = 1`)
- [ ] Itera sobre cada agência em sequência
- [ ] Alterna dinamicamente o schema de conexão para cada agência

#### Extração de Dados
- [ ] Extrai contratos de seguro prestamista com todos os filtros:
  - [ ] Modalidade = 4 (Empréstimo)
  - [ ] Não cancelado (tipo_cancelamento = 0)
  - [ ] Vigência futura (fim_vigencia >= data de corte)
  - [ ] Não excluído logicamente (sql_deleted = 'F')
  - [ ] Conta ativa (situação = 1)
  - [ ] Contrato não liquidado (data_pagamento IS NULL)
- [ ] Calcula campos derivados (tipo_seguro, tipo_saldo)
- [ ] Agrupa parcelas por (conta, contrato)

#### Transformações
- [ ] Converte tipo de pessoa corretamente (F/J → Física/Jurídica)
- [ ] Deriva CNPJ da seguradora a partir do código
- [ ] Aplica coeficientes condicionais por tipo de capital
- [ ] Formata sequência do contrato com 2 dígitos
- [ ] Determina tipo de pagamento corretamente

#### Lookup e Criação de Dimensões
- [ ] Cria agências não existentes
- [ ] Cria pontos de atendimento não existentes
- [ ] Cria cooperados não existentes
- [ ] Cria vínculos cooperado-agência-conta não existentes
- [ ] Cria seguradoras não existentes (com toda a cascata)
- [ ] Cria limites por faixa etária para cada seguradora
- [ ] Cria apólice/grupo não existente

#### Carga de Dados
- [ ] Insere seguros com todos os campos mapeados
- [ ] Insere parâmetros de seguro
- [ ] Insere parcelas vinculadas ao seguro
- [ ] Persiste em lotes para performance

#### Logs e Métricas
- [ ] Registra início e fim de processamento
- [ ] Registra progresso por agência
- [ ] Conta total de contratos processados
- [ ] Registra erros individuais sem interromper o fluxo

---

### 12.2 Números Esperados por Etapa

Os números abaixo devem ser validados comparando a execução da implementação original com a nova implementação, utilizando a mesma base de dados de origem:

| Etapa | Métrica | Como Validar |
|-------|---------|--------------|
| Descoberta | Quantidade de agências | Contar resultado da query de agências ativas |
| Extração | Contratos por agência | Contar registros retornados pela query principal |
| Extração | Parcelas por agência | Contar registros da tabela ep_segparcela |
| Transformação | Cooperados criados | Contar INSERT na tabela cooperado |
| Transformação | Seguradoras criadas | Contar INSERT na tabela seguradora |
| Carga | Seguros inseridos | Contar INSERT na tabela seguro |
| Carga | Parcelas inseridas | Contar INSERT na tabela parcela |
| Final | Total migrado | Deve igualar soma dos seguros por agência |

---

### 12.3 Testes de Validação Sugeridos

#### Teste de Integridade Referencial
```
Para cada seguro:
  - Existe cooperado_agencia_conta com o ID referenciado
  - Existe ponto_atendimento com o ID referenciado
  - Existe apolice_grupo_seguradora com o ID referenciado
  - Existe seguro_parametro com o ID referenciado
  
Para cada parcela:
  - Existe seguro com o ID referenciado
```

#### Teste de Valores Calculados
```
Para amostras de registros:
  - tipo_pagamento corresponde à regra de tipo_seguro
  - coeficiente corresponde ao tipo_capital
  - valor_pago = valor_parcela quando liquidacao preenchido
  - valor_pago = 0 quando liquidacao nulo
```

#### Teste de Completude
```
- Total de seguros no destino >= total de contratos elegíveis na origem
- Para cada seguro, quantidade de parcelas no destino = quantidade na origem
- Todas as agências ativas foram processadas
```

---

## 13. Glossário

| Termo | Definição |
|-------|-----------|
| **Agência** | Unidade operacional da cooperativa de crédito, identificada por código de 4 dígitos |
| **Apólice** | Documento que formaliza o contrato de seguro entre seguradora e segurado |
| **Capital Segurado** | Valor máximo coberto pelo seguro em caso de sinistro |
| **Capital Fixo** | Modalidade onde o valor segurado permanece constante durante a vigência |
| **Capital Variável** | Modalidade onde o valor segurado acompanha o saldo devedor (decresce) |
| **Coeficiente** | Fator multiplicador usado no cálculo do prêmio do seguro |
| **Comissão** | Percentual do prêmio destinado à corretora e/ou cooperativa como remuneração |
| **Cooperado** | Cliente/associado da cooperativa de crédito |
| **DPS** | Declaração Pessoal de Saúde - documento exigido acima de certos valores ou idades |
| **ECR** | Empréstimo com Consignação em Renda |
| **Grupo** | Subdivisão da apólice que agrupa riscos similares |
| **IOF** | Imposto sobre Operações Financeiras - tributo incidente sobre o prêmio |
| **Liquidação** | Quitação/pagamento de uma parcela ou contrato |
| **Modalidade** | Tipo de produto de crédito: Cheque Especial, ECR, Desconto ou Empréstimo |
| **PA** | Ponto de Atendimento - subdivisão da agência |
| **Periodicidade 30 Dias** | Regime onde os vencimentos são a cada 30 dias corridos (vs mensal no mesmo dia) |
| **Prêmio** | Valor pago pelo segurado à seguradora pela cobertura do seguro |
| **Prestamista** | Tipo de seguro vinculado a operações de crédito |
| **Renegociação** | Nova negociação das condições de um contrato existente |
| **Seguradora** | Empresa que assume o risco e paga a indenização em caso de sinistro |
| **Sequencial** | Número que identifica aditivos/renegociações de um mesmo contrato |
| **Sinistro** | Ocorrência do evento coberto pelo seguro (morte, invalidez, etc.) |
| **Vigência** | Período durante o qual o seguro está ativo e garante cobertura |

---

## 14. Assunções e Invariantes

### 14.1 Assunções do ETL

| Assunção | Evidência |
|----------|-----------|
| O código da seguradora pode ser convertido em CNPJ preenchendo com zeros | Lógica de conversão explícita |
| Todos os cooperados da base de origem possuem CPF/CNPJ válido | Query não trata inconsistências |
| As contas correntes sempre têm PA_CODIGO preenchido | Usado sem verificação de nulo |
| A seguradora código "0005" tem periodicidade diferente das demais | Condição explícita no código |
| Contratos sem parcelas devem ser ignorados | Retorna 0 se não há parcelas |
| O campo sql_deleted com valor 'F' indica registro ativo | Filtro explícito na extração |

### 14.2 Invariantes Esperadas

| Invariante | Descrição |
|------------|-----------|
| Unicidade de agência por código | Código de 4 dígitos é único |
| Unicidade de PA por agência | Código de 3 dígitos é único dentro da agência |
| Unicidade de cooperado por documento | CPF/CNPJ identifica unicamente um cooperado |
| Unicidade de vínculo coop-ag-conta | Combinação é única |
| Unicidade de seguradora por CNPJ | CNPJ identifica unicamente uma seguradora |
| Relação 1:1 seguro-parametro | Cada seguro tem exatamente um registro de parâmetros |

### 14.3 Limitações Conhecidas

| Limitação | Impacto |
|-----------|---------|
| Datas de filtro hardcoded | Requer modificação de código para alterar janela de migração |
| Não idempotente para fatos | Segunda execução cria duplicatas |
| Sem transação global | Falha parcial deixa dados inconsistentes |
| Sem retry automático | Erros transitórios podem causar perda de dados |
| Comissões fixas na criação | Não reflete alterações posteriores na seguradora |
| Valores de apólice fixos | Não captura configuração real da apólice |
| Usuário não migrado | Campo usuario_id sempre NULL |
| Histórico de auditoria não migrado | Não há rastreabilidade de alterações anteriores |

---

## 15. Apêndice: Queries de Validação

### 15.1 Validação de Completude (executar após migração)

**Contar seguros por agência no destino:**
```
Selecionar agência, contar seguros
Agrupar por agência
Ordenar por agência
```

**Verificar seguros sem parcelas:**
```
Selecionar seguros onde não existe nenhuma parcela vinculada
```

**Verificar parcelas órfãs:**
```
Selecionar parcelas onde não existe seguro com o ID referenciado
```

### 15.2 Validação de Integridade

**Cooperados sem vínculo agência-conta:**
```
Selecionar cooperados onde não existe vínculo na tabela cooperado_agencia_conta
```

**Apólices sem seguros:**
```
Selecionar apólice_grupo_seguradora onde não existe seguro vinculado
```

### 15.3 Validação de Valores

**Parcelas com valor pago inconsistente:**
```
Selecionar parcelas onde:
  (liquidação preenchida E valor_pago = 0)
  OU
  (liquidação nula E valor_pago > 0)
```

**Seguros com tipo de pagamento inconsistente:**
```
Selecionar seguros onde:
  (tipo_pagamento = 'À Vista' E quantidade_parcelas > 1 E existe mais de uma parcela)
```

---

*Fim do documento de reconstrução ETL - Parte 3*

---

## Histórico de Versões

| Versão | Data | Descrição |
|--------|------|-----------|
| 1.0 | 09/01/2026 | Versão inicial - Análise completa do projeto de migração |
