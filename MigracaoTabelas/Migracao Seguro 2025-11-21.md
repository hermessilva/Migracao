# Documentação Agnóstica de Migração: Seguro Prestamista

## 1. Visão Geral
Esta documentação descreve o processo de migração de dados de múltiplas bases de dados de origem (uma por agência) para uma única base de dados de destino centralizada. O foco principal é a migração dos dados de **Seguro Prestamista** e suas **Parcelas**.

O processo envolve a extração de dados de tabelas legadas (`ep_segprestamista`, `ep_segparcela`), a transformação conforme regras de negócio específicas e o carregamento em novas estruturas normalizadas (`seguro`, `parcela`), resolvendo dependências de chaves estrangeiras (Agência, Seguradora, Cooperado, Ponto de Atendimento).

## 2. Estrutura de Dados

### 2.1. Origem (Source)
Os dados de origem estão distribuídos em múltiplos schemas (bancos de dados), um para cada agência (ex: `agencia_0001`, `agencia_0002`).

**Tabelas Principais:**
*   **`ep_segprestamista`**: Contém os cabeçalhos dos contratos de seguro.
*   **`ep_segparcela`**: Contém as parcelas financeiras dos seguros.

**Tabelas de Apoio (Lookup):**
*   `unico.cd_agencia`: Lista de agências e seus códigos (usado para iterar sobre os schemas).
*   `unico.cd_prestamista`: Dados das seguradoras.
*   `unico.cd_pa`: Pontos de atendimento.
*   `cc_conta`: Contas correntes (vincula conta ao ponto de atendimento).
*   `unico.cd_cliente`: Dados dos clientes (cooperados).

### 2.2. Destino (Target)
Base de dados única e centralizada.

**Tabelas Principais:**
*   **`seguro`**: Tabela central dos contratos.
*   **`parcela`**: Tabela detalhe das parcelas.

**Tabelas de Dependência (Devem estar populadas):**
*   `agencia_seguradora`: Relacionamento N:N entre Agência e Seguradora.
*   `cooperado_agencia_conta`: Identificação única da conta do cooperado na agência.
*   `ponto_atendimento`: Locais de atendimento.
*   `usuario`: Usuários do sistema.

---

## 3. Mapeamento e Regras de Conversão

### 3.1. Tabela: `seguro`
Origem Principal: `ep_segprestamista`

| Coluna Destino (`seguro`) | Coluna Origem (`ep_segprestamista`) | Regra de Conversão / Observação |
| :--- | :--- | :--- |
| `id` | - | Gerado automaticamente (Auto Increment/Identity). |
| `agencia_seguradora_id` | `PST_CODIGO` + Contexto Agência | **Lookup**: Buscar ID em `agencia_seguradora` usando o ID da Agência atual e o ID da Seguradora (mapeado de `PST_CODIGO`). |
| `cooperado_agencia_conta_id` | `CCO_CONTA`, `SEG_CPF` | **Lookup**: Buscar ID em `cooperado_agencia_conta` usando CPF (`SEG_CPF`) e Conta (`CCO_CONTA`) na Agência atual. |
| `ponto_atendimento_id` | (Via `CCO_CONTA`) | **Lookup**: Buscar ID em `ponto_atendimento`. Obter `PA_CODIGO` da tabela `cc_conta` usando `CCO_CONTA`, depois buscar na tabela de destino. |
| `usuario_id` | - | Valor fixo: `21` (Usuário de Migração/Sistema). |
| `status` | `SEG_CANCTIPO` | **Mapeamento de Enum**: <br> `1` -> 'Cancelado pelo Cooperado' <br> `2` -> 'Cancelado pela Cooperativa' <br> `3` -> 'Sinistro' <br> `4` -> 'Recusado pela Seguradora' <br> `5` -> 'Cancelado por Aditivo' <br> `6` -> 'Liquidação Antecipada' <br> `Outros` -> 'Ativo' |
| `contrato` | `SEG_CONTRATO` | Cópia direta. Default: string vazia se nulo. |
| `inicio_vigencia` | `SEG_INICIO` | Cópia direta (`Date`). |
| `fim_vigencia` | `SEG_FIM` | Cópia direta (`Date`). |
| `codigo_grupo` | - | Valor fixo: `0` (Regra de negócio a definir). |
| `quantidade_parcelas` | `SEG_MESES` | Cópia direta. Se nulo, assumir `0`. |
| `vencimento` | `SEG_FIM` | Assume-se a data fim de vigência como vencimento inicial. |
| `capital_segurado` | `SEG_BASE` ou `SEG_VRCONTRATO` | Prioridade: `SEG_BASE`. Se nulo, usa `SEG_VRCONTRATO`. Default: `0.00`. |
| `premio_total` | `SEG_PREMIO` | Cópia direta. Default: `0.00`. |
| `tipo_pagamento` | `SEG_MODALIDADE` | **Mapeamento**: Default -> 'À Vista'. (Lógica atual simplificada). |
| `estorno_proporcional` | - | Valor fixo: `0.00`. |
| `valor_base` | `SEG_BASE` | Cópia direta. |
| `dps` | `SEG_DPS` | Conversão Booleana (1 -> true, 0 -> false). |
| `valor_iof` | `SEG_IOF` | Cópia direta. |

### 3.2. Tabela: `parcela`
Origem Principal: `ep_segparcela`

| Coluna Destino (`parcela`) | Coluna Origem (`ep_segparcela`) | Regra de Conversão / Observação |
| :--- | :--- | :--- |
| `id` | - | Gerado automaticamente. |
| `seguro_id` | (Relacionamento) | ID do `seguro` inserido anteriormente (FK). |
| `status` | - | Valor fixo: 'Ativo'. |
| `numero_parcela` | `seg_parcela` | Cópia direta. |
| `valor_parcela` | `seg_valor` | Cópia direta. |
| `valor_pago` | `seg_valor` (Condicional) | Se `seg_pgto` (Data Pagamento) não for nulo, assume-se pagamento integral (`seg_valor`). Caso contrário, `0.00`. |
| `vencimento` | `seg_vcto` | Cópia direta. |
| `liquidacao` | `seg_pgto` | Cópia direta (Data/Hora). |
| `data_ultimo_pagamento` | `seg_pgto` | Cópia direta. |

---

## 4. Consultas de Extração (Source Selects)

Para cada base de dados de agência (Schema), executar:

### 4.1. Seleção de Seguros (Prestamistas)
Filtra-se apenas a modalidade 4 (Prestamista).

```sql
SELECT 
    PST_CODIGO, 
    CCO_CONTA, 
    SEG_CPF, 
    SEG_CONTRATO, 
    SEG_MODALIDADE, 
    SEG_INICIO, 
    SEG_FIM, 
    SEG_MESES, 
    SEG_CANCTIPO, 
    SEG_BASE, 
    SEG_VRCONTRATO, 
    SEG_PREMIO, 
    SEG_IOF, 
    SEG_DPS 
FROM 
    ep_segprestamista 
WHERE 
    SEG_MODALIDADE = 4;
```

### 4.2. Seleção de Parcelas
Busca todas as parcelas para junção em memória ou via banco.

```sql
SELECT 
    cco_conta, 
    seg_contrato, 
    seg_parcela, 
    seg_valor, 
    seg_vcto, 
    seg_pgto 
FROM 
    ep_segparcela;
```

### 4.3. Consultas de Dependências (Lookups e Criação Dinâmica)

As tabelas abaixo são populadas dinamicamente se o registro não for encontrado no destino.

#### 4.3.1. Agência (`agencia`)
Origem: `unico.cd_agencia`

```sql
SELECT 
    AG_CODIGO as CODIGO, 
    AG_RAZAO as NOME 
FROM 
    unico.cd_agencia;
```

#### 4.3.2. Seguradora (`seguradora`)
Origem: `unico.cd_prestamista`

```sql
SELECT 
    PST_CODIGO as Codigo, 
    pst_nome as nome,
    '00000000000000' as cnpj, -- Valor padrão/fictício na query original
    pst_nome as razaosocial,
    '00000000' as cep,
    'RUA' as rua,
    'COMPLEMENTO' as complemento,
    '0' as numero,
    'BAIRRO' as bairro,
    'CIDADE' as cidade,
    'UF' as uf,
    'TELEFONE' as telefone,
    'EMAIL' as email 
FROM 
    unico.cd_prestamista;
```

#### 4.3.3. Ponto de Atendimento (`ponto_atendimento`)
Origem: `unico.cd_pa`

```sql
SELECT 
    cp.PA_CODIGO as CODIGO,
    cp.AG_CODIGO AS AGENCIA,
    cp.PA_SIGLA As NOME  
FROM 
    unico.cd_pa cp;
```

#### 4.3.4. Cooperado (`cooperado`)
Origem: `unico.cd_cliente`

```sql
SELECT 
    COALESCE(cl.CLI_CPFCNPJ, '') AS numerodocumento, 
    cl.CLI_TIPPES AS tipo, 
    COALESCE(cl.cli_nome, '') AS nome,
    cl.CLI_NFANTA AS nomefantasia,
    cl.CLI_EMAIL AS email,
    cl.AG_CODIGO AS Agencia
FROM 
    unico.cd_cliente cl;
```

#### 4.3.5. Conta Corrente (`cooperado_agencia_conta`)
Origem: `cc_conta`

```sql
SELECT 
    cc.CLI_CPFCNPJ CPFCNPJ,
    cc.CCO_CONTA Codigo,
    PA_CODIGO PaCodigo  
FROM 
    cc_conta cc;
```

---

## 5. Estratégia de Migração Eficiente (Pseudo-Código)

A estratégia mais eficiente envolve minimizar as viagens ao banco de dados (round-trips) e resolver dependências (Lookups) em memória (Cache), dado que as tabelas de domínio (Agências, Seguradoras) são pequenas.

### Algoritmo

```text
INICIO

    // 1. Carregar Cache de Dependências (Global)
    // Carrega mapas para resolução rápida de IDs de destino
    MAPA_AGENCIAS = SELECT Codigo, Id FROM Tx.Agencia
    MAPA_SEGURADORAS = SELECT CodigoLegado, Id FROM Tx.Seguradora
    MAPA_PONTOS_ATENDIMENTO = SELECT CodigoLegado, AgenciaId, Id FROM Tx.PontoAtendimento
    
    // Mapa complexo para Cooperado/Conta: Chave = (CPF, Conta, AgenciaId) -> Valor = Id
    MAPA_COOPERADO_CONTA = CarregarMapaCooperadoAgenciaConta() 
    
    // Mapa para AgenciaSeguradora: Chave = (AgenciaId, SeguradoraId) -> Valor = Id
    MAPA_AGENCIA_SEGURADORA = CarregarMapaAgenciaSeguradora()

    // 2. Identificar Bases de Origem
    LISTA_SCHEMAS = SELECT CONCAT('agencia_', AG_CODIGO), AG_CODIGO FROM unico.cd_agencia WHERE AG_ATIVA = 1

    PARA CADA SCHEMA EM LISTA_SCHEMAS:
    
        CONECTAR ORIGEM (SCHEMA)
        INICIAR TRANSACAO DESTINO (Batch Transaction)

        // 3. Extração (Bulk Read)
        // Ler todos os seguros e parcelas da agência atual de uma vez
        LISTA_SEGUROS_RAW = EXECUTAR QUERY 4.1 (Source)
        LISTA_PARCELAS_RAW = EXECUTAR QUERY 4.2 (Source)

        // Indexar parcelas por chave (Conta, Contrato) para acesso rápido O(1)
        DICIONARIO_PARCELAS = AGRUPAR LISTA_PARCELAS_RAW POR (cco_conta, seg_contrato)

        LISTA_SEGUROS_DESTINO = NOVA LISTA
        LISTA_PARCELAS_DESTINO = NOVA LISTA

        // 4. Transformação
        PARA CADA SEGURO_RAW EM LISTA_SEGUROS_RAW:
        
            // Resolver IDs (Lookup em Memória)
            ID_AGENCIA = MAPA_AGENCIAS[SCHEMA.AG_CODIGO]
            ID_SEGURADORA = MAPA_SEGURADORAS[SEGURO_RAW.PST_CODIGO]
            ID_AG_SEG = MAPA_AGENCIA_SEGURADORA[(ID_AGENCIA, ID_SEGURADORA)]
            
            ID_COOP_CONTA = MAPA_COOPERADO_CONTA[(SEGURO_RAW.SEG_CPF, SEGURO_RAW.CCO_CONTA, ID_AGENCIA)]
            
            // Obter PA da conta (pode exigir query auxiliar na cc_conta se não estiver cacheado)
            PA_CODIGO = OBTER_PA_DA_CONTA(SEGURO_RAW.CCO_CONTA) 
            ID_PA = MAPA_PONTOS_ATENDIMENTO[(PA_CODIGO, ID_AGENCIA)]

            // Criar Objeto Seguro
            OBJ_SEGURO = NOVO SEGURO
            OBJ_SEGURO.Preencher(SEGURO_RAW) // Aplicar regras de mapeamento
            OBJ_SEGURO.AgenciaSeguradoraId = ID_AG_SEG
            OBJ_SEGURO.CooperadoAgenciaContaId = ID_COOP_CONTA
            OBJ_SEGURO.PontoAtendimentoId = ID_PA
            
            ADICIONAR OBJ_SEGURO EM LISTA_SEGUROS_DESTINO

            // Processar Parcelas
            PARCELAS_DO_SEGURO = DICIONARIO_PARCELAS[(SEGURO_RAW.CCO_CONTA, SEGURO_RAW.SEG_CONTRATO)]
            
            PARA CADA PARCELA_RAW EM PARCELAS_DO_SEGURO:
                OBJ_PARCELA = NOVA PARCELA
                OBJ_PARCELA.Preencher(PARCELA_RAW) // Aplicar regras
                OBJ_PARCELA.Seguro = OBJ_SEGURO // Vínculo em memória
                ADICIONAR OBJ_PARCELA EM LISTA_PARCELAS_DESTINO

        FIM PARA

        // 5. Carga (Bulk Insert)
        // Usar Entity Framework Extensions (BulkInsert) ou SqlBulkCopy para alta performance
        // A ordem é importante para manter a integridade referencial se não usar grafo de objetos
        
        Tx.BulkInsert(LISTA_SEGUROS_DESTINO, IncludeGraph = True) 
        
        COMMIT TRANSACAO
        LOG "Schema " + SCHEMA + " migrado com sucesso. Registros: " + LISTA_SEGUROS_DESTINO.Count

    FIM PARA

FIM
```

### Considerações de Performance
1.  **Cache de Lookups**: Evita executar `SELECT` para buscar IDs de Agência, Seguradora ou Cooperado para cada registro de seguro. Isso reduz de N+1 queries para 1 query inicial.
2.  **Leitura em Memória**: Trazer todos os dados da agência (se couber em memória) é mais rápido do que cursores ou paginação excessiva, pois elimina latência de rede repetitiva.
3.  **Bulk Insert**: Inserções linha a linha são lentas. O uso de `BulkInsert` ou inserção em lotes (Batch Size ~1000-5000) acelera drasticamente a escrita.
4.  **Desabilitar Tracking**: No Entity Framework, usar `AsNoTracking()` na leitura da origem.
5.  **Transações por Agência**: Mantém o tamanho do log de transação gerenciável e permite retry granular em caso de falha em uma agência específica.
