# Documento de Reconstrução ETL - Geração de Apólices/Grupos Seguradora

## Documento Complementar: Tabela `apolice_grupo_seguradora`

---

## 1. Contexto

A tabela `apolice_grupo_seguradora` é uma **tabela de relacionamento** que vincula agências a seguradoras, armazenando as configurações de apólice, grupo e modalidades de pagamento para cada combinação.

### Problema Identificado

A **base de dados de origem não possui** uma tabela equivalente com esses dados. No sistema legado, as informações de apólice e grupo não são armazenadas de forma estruturada ou simplesmente não existem.

No entanto, o sistema de destino **exige** que exista um registro nesta tabela para cada combinação (agência, seguradora) que possua contratos de seguro, pois:
- A tabela `seguro` possui uma chave estrangeira obrigatória (`apolice_grupo_seguradora_id`)
- Não é possível criar um seguro sem referenciar uma apólice/grupo válido

---

## 2. Solução Implementada

### Estratégia: Geração Automática com Valores Padrão

O ETL implementa uma estratégia de **criação sob demanda** (lazy creation): quando um contrato de seguro é processado e não existe um registro de apólice/grupo para a combinação (agência, seguradora), o sistema cria automaticamente um registro com **valores fictícios padronizados**.

### Momento da Criação

A criação ocorre durante o processamento de cada contrato de seguro:

```
PARA CADA contrato de seguro:
  1. Obtém o ID da agência
  2. Obtém o ID da seguradora (via código PST_CODIGO)
  3. Busca apólice/grupo para (agencia_id, seguradora_id)
  4. SE não existe:
     → Cria registro com valores padrão
     → Persiste no banco
     → Adiciona ao cache
  5. Retorna o ID do registro
```

---

## 3. Valores Padrão Utilizados

| Campo | Valor Padrão | Justificativa |
|-------|--------------|---------------|
| `apolice` | "APO-001" | Identificador genérico de apólice |
| `grupo` | "GRP-001" | Identificador genérico de grupo |
| `subgrupo` | "SUB-001" | Identificador genérico de subgrupo |
| `tipo_capital` | **Varia conforme nome da seguradora** | Ver regra crítica abaixo |
| `modalidade_unico` | "Unico" | Texto identificador da modalidade |
| `modalidade_avista` | 1.5 | Taxa/valor para pagamento à vista |
| `modalidade_parcelado` | 10 | Taxa/valor para pagamento parcelado |
| `ordem` | 0 (padrão do banco) | Prioridade não definida |

---

## 4. Regra Crítica: Tipo de Capital por Nome da Seguradora

### ⚠️ ATENÇÃO: Regra de Negócio Fundamental

O campo `tipo_capital` da tabela `apolice_grupo_seguradora` **NÃO é um valor fixo**. Ele é determinado dinamicamente com base no **nome da seguradora**:

### Regra Lógica

```
SE nome_seguradora CONTÉM "VARIAVEL" (case-insensitive)
    ENTÃO tipo_capital = "Variável"
SENÃO
    tipo_capital = "Fixo"
```

### Tabela de Decisão

| Nome da Seguradora (exemplo) | Contém "VARIAVEL"? | Tipo Capital |
|------------------------------|-------------------|--------------|
| "SEGURADORA ABC" | Não | **Fixo** |
| "SEGURADORA XYZ VARIAVEL" | Sim | **Variável** |
| "PRESTAMISTA VARIAVEL PLUS" | Sim | **Variável** |
| "SEGURO VIDA TRADICIONAL" | Não | **Fixo** |

### Impacto no Sistema

Esta configuração afeta diretamente:
- **Cálculo do prêmio** do seguro (coeficientes diferentes)
- **Cálculo de estornos** em cancelamentos
- **Comportamento do capital segurado** ao longo da vigência:
  - **Fixo**: valor permanece constante
  - **Variável**: valor acompanha o saldo devedor (decresce)

### Para Reimplementação

É **obrigatório** implementar esta regra ao criar registros na tabela `apolice_grupo_seguradora`:

```
1. Obter o nome da seguradora (via seguradora_id)
2. Verificar se o nome contém a string "VARIAVEL"
3. Se sim: definir tipo_capital = "Variável"
4. Se não: definir tipo_capital = "Fixo"
```

> **Importante**: A verificação deve ser case-insensitive (ignorar maiúsculas/minúsculas).

---

## 5. Regra de Unicidade

A tabela garante que existe **no máximo um registro** para cada combinação:

| Chave Composta | Descrição |
|----------------|-----------|
| `(agencia_id, seguradora_id)` | Identifica unicamente o vínculo |

### Verificação Antes da Criação

```
SE existe registro com (agencia_id, seguradora_id) no cache
    ENTÃO retorna ID existente
SENÃO
    cria novo registro
    adiciona ao cache
    retorna novo ID
```

---

## 6. Diagrama de Fluxo

```
┌─────────────────────────────────────────────────────────────────┐
│                 PROCESSAMENTO DE CONTRATO                       │
└─────────────────────────────────────────────────────────────────┘
                              │
                              ▼
              ┌───────────────────────────────┐
              │ Obtém agencia_id e            │
              │ seguradora_id do contrato     │
              └───────────────────────────────┘
                              │
                              ▼
              ┌───────────────────────────────┐
              │ Busca no cache:               │
              │ (agencia_id, seguradora_id)   │
              └───────────────────────────────┘
                              │
              ┌───────────────┴───────────────┐
              │                               │
              ▼                               ▼
    ┌─────────────────┐             ┌─────────────────┐
    │    ENCONTROU    │             │  NÃO ENCONTROU  │
    └─────────────────┘             └─────────────────┘
              │                               │
              ▼                               ▼
    ┌─────────────────┐             ┌─────────────────┐
    │ Retorna ID      │             │ Cria registro   │
    │ existente       │             │ com valores     │
    └─────────────────┘             │ padrão          │
                                    └─────────────────┘
                                              │
                                              ▼
                                    ┌─────────────────┐
                                    │ Persiste no     │
                                    │ banco de dados  │
                                    └─────────────────┘
                                              │
                                              ▼
                                    ┌─────────────────┐
                                    │ Adiciona ao     │
                                    │ cache           │
                                    └─────────────────┘
                                              │
                                              ▼
                                    ┌─────────────────┐
                                    │ Retorna novo ID │
                                    └─────────────────┘
```

## 7. Estrutura da Tabela de Destino

| Coluna | Tipo | Obrigatório | Descrição |
|--------|------|-------------|-----------|
| id | BIGINT UNSIGNED | Sim | Identificador único (auto-incremento) |
| agencia_id | BIGINT UNSIGNED | Sim | FK para tabela `agencia` |
| seguradora_id | BIGINT UNSIGNED | Sim | FK para tabela `seguradora` |
| ordem | INT | Sim | Ordem de prioridade (padrão: 0) |
| apolice | VARCHAR(255) | Não | Número/código da apólice |
| grupo | VARCHAR(255) | Não | Código do grupo |
| subgrupo | VARCHAR(255) | Não | Código do subgrupo |
| tipo_capital | ENUM('Fixo', 'Variável') | Sim | Tipo de capital segurado |
| modalidade_unico | VARCHAR(50) | Não | Identificador modalidade única |
| modalidade_avista | DECIMAL(10,2) | Não | Taxa para pagamento à vista |
| modalidade_parcelado | DECIMAL(10,2) | Não | Taxa para pagamento parcelado |

---

## 8. Implicações e Recomendações

### 8.1 Dados Fictícios

Os valores gerados são **placeholder** e **não refletem** a realidade contratual entre agência e seguradora. Após a migração, recomenda-se:

1. **Levantamento manual** dos dados reais de apólice junto às seguradoras
2. **Atualização dos registros** com os valores corretos
3. **Validação** das modalidades e taxas aplicáveis

### 8.2 Consistência

Mesmo sendo dados fictícios, o ETL garante:
- ✅ Integridade referencial (FKs válidas)
- ✅ Unicidade por combinação agência-seguradora
- ✅ Todos os seguros têm uma apólice/grupo vinculada

### 8.3 Para Reimplementação

Ao reimplementar o ETL, garantir que:
- [ ] Verificação de existência antes de criar novo registro
- [ ] Uso de cache para evitar consultas repetidas ao banco
- [ ] Thread-safety se houver processamento paralelo
- [ ] Valores padrão idênticos para consistência com migração original

---

## 9. Exemplo de Dados Gerados

Após processar contratos de 3 agências com 2 seguradoras cada (uma com nome contendo "VARIAVEL"):

| id | agencia_id | seguradora_id | apolice | grupo | subgrupo | tipo_capital | nome_seguradora (ref) |
|----|------------|---------------|---------|-------|----------|--------------|----------------------|
| 1 | 1 | 1 | APO-001 | GRP-001 | SUB-001 | Fixo | SEGURADORA ABC |
| 2 | 1 | 2 | APO-001 | GRP-001 | SUB-001 | **Variável** | PRESTAMISTA VARIAVEL |
| 3 | 2 | 1 | APO-001 | GRP-001 | SUB-001 | Fixo | SEGURADORA ABC |
| 4 | 2 | 2 | APO-001 | GRP-001 | SUB-001 | **Variável** | PRESTAMISTA VARIAVEL |
| 5 | 3 | 1 | APO-001 | GRP-001 | SUB-001 | Fixo | SEGURADORA ABC |
| 6 | 3 | 2 | APO-001 | GRP-001 | SUB-001 | **Variável** | PRESTAMISTA VARIAVEL |

---

*Fim do documento complementar*
