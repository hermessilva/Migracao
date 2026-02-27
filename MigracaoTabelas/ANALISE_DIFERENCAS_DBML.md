# Análise de Diferenças: Modelo.dbml vs Classes C#

**Data da análise:** 26/02/2026  
**Escopo:** Pasta `MigracaoTabelas\Target` e `MigracaoTabelas\Target\EntityConfiguration`

---

## Sumário

1. [Diferenças em Enumerados](#1-diferenças-em-enumerados)
2. [Diferenças em Tipos de Dados](#2-diferenças-em-tipos-de-dados)
3. [Diferenças em Configurações de Conversão (EntityConfiguration)](#3-diferenças-em-configurações-de-conversão-entityconfiguration)
4. [Recomendações de Correção](#4-recomendações-de-correção)

---

## 1. Diferenças em Enumerados

### 1.1 ⚠️ `sexo_cooperado` - **VALOR FALTANTE**

| DBML | C# `SexoCooperado` |
|------|---------------------|
| "Masculino" | Masculino = 1 ✅ |
| "Feminino" | Feminino = 2 ✅ |
| **"Outro"** | ❌ **NÃO EXISTE** |

**Arquivo:** [Target/Cooperado.cs](Target/Cooperado.cs)

**Correção necessária:**
```csharp
public enum SexoCooperado
{
    [Description("Masculino")]
    Masculino = 1,

    [Description("Feminino")]
    Feminino = 2,

    [Description("Outro")]  // ADICIONAR
    Outro = 3
}
```

---

### 1.2 ⚠️ `status_seguro` - **VALOR EXTRA E ORDENAÇÃO DIFERENTE**

| DBML | C# `StatusSeguro` |
|------|-------------------|
| - | NaoPermitido = 0 ❌ **EXTRA** |
| "Pendente" | Pendente = 1 ✅ |
| "Ativo" | Ativo = 2 ✅ |
| "Recusado" | Recusado = 3 ✅ |
| "Cancelado" | Cancelado = 5 ⚠️ (ordem) |
| "Expirado" | Expirado = 4 ⚠️ (ordem) |

**Arquivo:** [Target/Seguro.cs](Target/Seguro.cs#L170-L182)

**Observação:** O valor `NaoPermitido` não existe no DBML. Se for usado internamente, considere documentar ou remover se não for necessário para persistência.

---

### 1.3 ⚠️ `motivo_seguro` - **VALORES EXTRAS NO C#**

| DBML | C# `MotivoSeguro` |
|------|-------------------|
| - | NaoPermitido = 0 ❌ **EXTRA** |
| "Em analise na seguradora" | EmAnaliseNaSeguradora = 1 ✅ |
| "Aguardando faturamento" | AguardandoFaturamento = 2 ✅ |
| "Aguardando documentação" | AguardandoDocumentacao = 3 ✅ |
| "Pagamento à vista" | PagamentoAVista = 4 ✅ |
| "Pagamento parcelado" | PagamentoParcelado = 5 ✅ |
| "Inadimplente" | Inadimplente = 6 ✅ |
| "Regular" | Regular = 7 ✅ |
| "Recusado pela seguradora" | RecusadoPelaSeguradora = 8 ✅ |
| "Expiração da vigência do seguro" | ExpiracaoVigenciaSeguro = 9 ✅ |
| - | Aditivo = 10 ❌ **EXTRA** |
| - | CancelamentoPorPrejuizo = 11 ❌ **EXTRA** |
| - | Renegociacao = 12 ❌ **EXTRA** |
| - | Sinistro = 13 ❌ **EXTRA** |
| - | SolicitadoPelaCooperativa = 14 ❌ **EXTRA** |
| - | SolicitadoPeloCooperado = 15 ❌ **EXTRA** |
| - | LiquidacaoAntecipada = 16 ❌ **EXTRA** |

**Arquivo:** [Target/Seguro.cs](Target/Seguro.cs#L184-L215)

**Análise:** O C# possui valores adicionais relacionados a cancelamento que parecem estar sendo usados no fluxo de negócio (verificar propriedade `Motivo` com setter que altera `Status`). Estes valores devem ser adicionados ao DBML:

**Correção no DBML:**
```sql
enum motivo_seguro {
  "Em analise na seguradora"
  "Aguardando faturamento"
  "Aguardando documentação"
  "Pagamento à vista"
  "Pagamento parcelado"
  "Inadimplente"
  "Regular"
  "Recusado pela seguradora"
  "Expiração da vigência do seguro"
  "Aditivo"
  "Cancelamento por prejuízo"
  "Renegociação"
  "Sinistro"
  "Solicitado pela cooperativa"
  "Solicitado pelo cooperado"
  "Liquidação antecipada"
}
```

---

### 1.4 ⚠️ `motivo_cancel` vs `MotivoSeguroCancelamento` - **DIFERENÇAS SIGNIFICATIVAS**

| DBML `motivo_cancel` | C# `MotivoSeguroCancelamento` |
|----------------------|-------------------------------|
| "Em análise pela Seguradora" | ❌ NÃO EXISTE |
| "Pendente de documentação" | ❌ NÃO EXISTE |
| "Ativo" | ❌ NÃO EXISTE |
| "Expiração da vigência do seguro" | ❌ NÃO EXISTE |
| "Cancelado pelo cooperado" | SolicitadoPeloCooperado = 6 ⚠️ (texto diferente) |
| "Cancelado pela cooperativa" | SolicitadoPelaCooperativa = 5 ⚠️ (texto diferente) |
| "Sinistro" | Sinistro = 4 ✅ |
| "Recusado pela seguradora" | RecusadoPelaSeguradora = 8 ✅ |
| "Cancelamento por prejuízo" | CancelamentoPorPrejuizo = 2 ✅ |
| "Liquidação antecipada" | LiquidacaoAntecipada = 7 ✅ |
| "Cancelado por renegociação" | Renegociacao = 3 ⚠️ (texto diferente) |
| "Cancelado por aditivo" | Aditivo = 1 ⚠️ (texto diferente) |
| - | NãoPermitido = 0 ❌ **EXTRA** |

**Arquivo:** [Target/SeguroCancelamento.cs](Target/SeguroCancelamento.cs#L78-L98)

**Problemas identificados:**
1. Textos com nomenclatura diferente no `[Description]`
2. Valores do DBML que não fazem sentido para cancelamento ("Em análise pela Seguradora", "Pendente de documentação", "Ativo")
3. C# tem `NãoPermitido` que não existe no DBML

**Recomendação:** Alinhar o DBML com os valores reais de cancelamento no C#, removendo os que não fazem sentido.

---

### 1.5 ⚠️ `tipo_faturamento_parcela_status` - **VALOR EXTRA NO C#**

| DBML | C# `TipoFaturamentoParcelaStatus` |
|------|-----------------------------------|
| "Faturado" | Faturado = 1 ✅ |
| "Cancelado" | Cancelado = 2 ✅ |
| "Não Faturado" | NaoFaturado = 3 ✅ |
| - | EmProcessamento = 4 ❌ **EXTRA** |

**Arquivo:** [Target/FaturamentoParcela.cs](Target/FaturamentoParcela.cs#L19-L32)

**Correção no DBML:**
```sql
enum tipo_faturamento_parcela_status {
  "Faturado"
  "Cancelado"
  "Não Faturado"
  "EmProcessamento"
}
```

---

### 1.6 ⚠️ `tipo_documento_armazenado` - **DIFERENÇAS SIGNIFICATIVAS**

| DBML | C# `TipoDocumentoArmazenado` |
|------|------------------------------|
| "Termo de Adesão com DPS" | TermoAdesaoComDps = 1 ✅ |
| "Termo de Adesão sem DPS" | TermoAdesaoSemDps = 2 ✅ |
| "Comprovante de Pagamento" | ❌ NÃO EXISTE |
| "Documento de Identificação" | ❌ NÃO EXISTE |
| "Comprovante de Residência" | ❌ NÃO EXISTE |
| "Laudo Médico" | ❌ NÃO EXISTE |
| "Outros" | ❌ NÃO EXISTE |
| - | DocumentacaoComplementar = 3 ❌ **EXTRA** |

**Arquivo:** [Target/ArmazenamentoDocumento.cs](Target/ArmazenamentoDocumento.cs#L37-L50)

**Correção necessária no C#:**
```csharp
public enum TipoDocumentoArmazenado
{
    [Description("Termo de Adesão com DPS")]
    TermoAdesaoComDps = 1,

    [Description("Termo de Adesão sem DPS")]
    TermoAdesaoSemDps = 2,

    [Description("Comprovante de Pagamento")]
    ComprovantePagamento = 3,

    [Description("Documento de Identificação")]
    DocumentoIdentificacao = 4,

    [Description("Comprovante de Residência")]
    ComprovanteResidencia = 5,

    [Description("Laudo Médico")]
    LaudoMedico = 6,

    [Description("Outros")]
    Outros = 7
}
```

---

## 2. Diferenças em Tipos de Dados

### 2.1 ✅ Tipos Conferidos e Corretos

| Tabela | Campo | DBML | C# | Status |
|--------|-------|------|-----|--------|
| agencia | id | bigint unsigned | ulong | ✅ |
| agencia | codigo | char(4) | string (config: char(4)) | ✅ |
| agencia | nome | varchar(255) | string (config: varchar(255)) | ✅ |
| cooperado | numero_documento | varchar(14) | string (config: MaxLength(14)) | ✅ |
| seguradora | cnpj | char(14) | string (config: char(14)) | ✅ |
| seguro_parametro | coeficiente | decimal(8,7) | decimal (config: decimal(8,7)) | ✅ |
| seguro_parametro | porcentual_iof | decimal(5,4) | decimal (config: decimal(5,4)) | ✅ |
| parcela | valor_parcela | decimal(10,2) | decimal (config: decimal(10,2)) | ✅ |
| evento_outbox | tentativas | tinyint | int (config: tinyint) | ✅ |

### 2.2 ⚠️ Campo `criado_em` em `agencia`

| DBML | C# |
|------|-----|
| `datetime [not null, default: now()]` | `DateTime CriadoEm` (sem default no código) |

**Arquivo:** [Target/Agencia.cs](Target/Agencia.cs)

**Observação:** O valor default `now()` deve estar configurado na EntityConfiguration. Verificar se `HasDefaultValueSql("CURRENT_TIMESTAMP")` está presente.

---

## 3. Diferenças em Configurações de Conversão (EntityConfiguration)

### 3.1 🚨 **CRÍTICO** - `GestaoDocumentoConfiguration` - **VALORES ENUM ERRADOS**

**Arquivo:** [Target/EntityConfiguration/GestaoDocumentoConfiguration.cs](Target/EntityConfiguration/GestaoDocumentoConfiguration.cs#L30-L36)

**Código atual (ERRADO):**
```csharp
ConfigureEnum(builder.Property(x => x.Tipo)
    .HasColumnName("tipo"), "Termo de Adesão", "DPS")  // ❌ ERRADO
```

**Código correto:**
```csharp
ConfigureEnum(builder.Property(x => x.Tipo)
    .HasColumnName("tipo"), "Termo de Adesão com DPS", "Termo de Adesão sem DPS")  // ✅
```

**Impacto:** Os valores gravados no banco não correspondem aos valores do enum. Inconsistência de dados.

---

### 3.2 🚨 **CRÍTICO** - `FaturamentoParcelaConfiguration` - **VALOR ENUM FALTANTE**

**Arquivo:** [Target/EntityConfiguration/FaturamentoParcelaConfiguration.cs](Target/EntityConfiguration/FaturamentoParcelaConfiguration.cs#L42-L50)

**Código atual (INCOMPLETO):**
```csharp
ConfigureEnum(
    builder.Property(x => x.Status)...,
    "Faturado", "Cancelado", "EmProcessamento"  // ❌ Faltou "Não Faturado"
)
```

**Código correto:**
```csharp
ConfigureEnum(
    builder.Property(x => x.Status)...,
    "Faturado", "Cancelado", "Não Faturado", "EmProcessamento"  // ✅
)
```

**Impacto:** O status "Não Faturado" não será aceito como valor válido no ConfigureEnum.

---

### 3.3 🚨 **CRÍTICO** - `IntegracaoSeniorConfiguration` - **VALORES ENUM FALTANTES**

**Arquivo:** [Target/EntityConfiguration/IntegracaoSeniorConfiguration.cs](Target/EntityConfiguration/IntegracaoSeniorConfiguration.cs#L64-L74)

**Código atual (INCOMPLETO):**
```csharp
ConfigureEnum(builder.Property(x => x.TipoLancamentoContabil)
    .HasColumnName("tipo_lancamento_contabil"), 
    "Seguro Prestamista Contratado", 
    "Comissão Seguro Prestamista Contratado",
    "Cancelamento Seguro Prestamista Parcelado Comissão", 
    "Cancelamento Seguro Prestamista À Vista Proporcional Comissão",
    "Pagamento Seguro Prestamista", 
    "Recebimento Comissão Seguro Prestamista",
    "Recebimento Premio Seguro Prestamista Parcelado", 
    "Recebimento Comissão Seguro Prestamista Parcelado",
    "Faturamento Prêmio Seguro Prestamista")
    // ❌ Faltam: "Faturamento Comissão Seguro Prestamista", "Faturamento IRRF Seguro Prestamista"
```

**Código correto:**
```csharp
ConfigureEnum(builder.Property(x => x.TipoLancamentoContabil)
    .HasColumnName("tipo_lancamento_contabil"), 
    "Seguro Prestamista Contratado", 
    "Comissão Seguro Prestamista Contratado",
    "Cancelamento Seguro Prestamista Parcelado Comissão", 
    "Cancelamento Seguro Prestamista À Vista Proporcional Comissão",
    "Pagamento Seguro Prestamista", 
    "Recebimento Comissão Seguro Prestamista",
    "Recebimento Premio Seguro Prestamista Parcelado", 
    "Recebimento Comissão Seguro Prestamista Parcelado",
    "Faturamento Prêmio Seguro Prestamista",
    "Faturamento Comissão Seguro Prestamista",      // ✅ ADICIONAR
    "Faturamento IRRF Seguro Prestamista")          // ✅ ADICIONAR
```

**Impacto:** Lançamentos do tipo "Faturamento Comissão" e "Faturamento IRRF" falharão na persistência.

---

## 4. Recomendações de Correção

### 4.1 Prioridade CRÍTICA (Erros de Persistência)

| # | Arquivo | Problema | Ação |
|---|---------|----------|------|
| 1 | GestaoDocumentoConfiguration.cs | Valores enum errados | Corrigir para "Termo de Adesão com DPS", "Termo de Adesão sem DPS" |
| 2 | FaturamentoParcelaConfiguration.cs | Falta "Não Faturado" | Adicionar valor |
| 3 | IntegracaoSeniorConfiguration.cs | Falta 2 valores de tipo_lancamento | Adicionar "Faturamento Comissão Seguro Prestamista" e "Faturamento IRRF Seguro Prestamista" |

### 4.2 Prioridade ALTA (Inconsistência de Modelo)

| # | Arquivo | Problema | Ação |
|---|---------|----------|------|
| 4 | Cooperado.cs | Falta valor "Outro" em SexoCooperado | Adicionar enum value |
| 5 | ArmazenamentoDocumento.cs | Faltam 5 tipos de documento | Completar enum com valores do DBML |
| 6 | Modelo.dbml | Faltam valores em motivo_seguro | Atualizar DBML com valores do C# |
| 7 | Modelo.dbml | Falta "EmProcessamento" em tipo_faturamento_parcela_status | Atualizar DBML |

### 4.3 Prioridade MÉDIA (Alinhamento de Nomenclatura)

| # | Arquivo | Problema | Ação |
|---|---------|----------|------|
| 8 | SeguroCancelamento.cs e Modelo.dbml | Textos diferentes em motivo_cancel | Decidir nomenclatura e alinhar |
| 9 | Seguro.cs | Status "NaoPermitido" não existe no DBML | Documentar ou remover se desnecessário |

### 4.4 Prioridade BAIXA (Melhoria de Documentação)

| # | Arquivo | Problema | Ação |
|---|---------|----------|------|
| 10 | Enums/*.cs | Enumerados duplicados em pasta Enums e Target | Consolidar em um único local |

---

## Resumo de Enums Corretos ✅

Os seguintes enumerados estão corretos e alinhados entre DBML e C#:

- `ativo_inativo` ↔ `StatusUsuario`, `StatusSeguradora`, `StatusGestaoDocumento`
- `auditoria_operacao` ↔ `OperacaoAuditoria`
- `tipo_capital_apolice_grupo_seguradora` ↔ `TipoCapitalApolice`
- `tipo_cooperado` ↔ `TipoPessoaCooperado`
- `tipo_pagamento` ↔ `TipoPagamentoSeguro`
- `status_parcela` ↔ `StatusParcela`
- `tipo_evento_outbox` ↔ `TipoEvento`
- `status_evento_outbox` ↔ `OutboxStatus`
- `tipo_faturamento_parcela_origem` ↔ `TipoFaturamentoParcelaOrigem`
- `status_importacao_faturamento` ↔ `StatusImportacaoFaturamento`
- `tipo_lancamento` ↔ `TipoLancamentoContabilIntegracaoSenior`
- `status_integracao_senior` ↔ `StatusEnvioIntegracaoSenior`
- `tipo_documento` ↔ `TipoGestaoDocumento`
- `local_armazenamento_documento` ↔ `LocalArmazenamentoDocumento`
- `status_armazenamento_documento` ↔ `StatusArmazenamentoDocumento`
- `tipo_capital_seguro_parametro` ↔ `TipoCapitalApolice` (reutilizado)

---

**Nota:** Esta análise foi realizada comparando o arquivo `Modelo.dbml` com as classes C# na pasta `Target` e suas configurações em `Target/EntityConfiguration`. Recomenda-se executar os testes automatizados após as correções para garantir a integridade do sistema.
