# Migração de Tabelas

Este projeto é uma aplicação de console em .NET 9.0 para migrar dados de seguros prestamistas de um banco de dados de origem (ERP) para um novo banco de dados de destino.

## Pré-requisitos

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)

## Configuração

A aplicação utiliza variáveis de ambiente para configurar as strings de conexão com os bancos de dados de origem e destino.

### Variáveis de Ambiente

- `SOURCE_DB`: String de conexão para o banco de dados de origem (ERP).
  - **Exemplo**: `Server=my_source_server;Database=unico;User=my_user;Password=my_password;`
- `TARGET_DB`: String de conexão para o banco de dados de destino.
  - **Exemplo**: `Server=my_target_server;Database=my_target_db;User=my_user;Password=my_password;`

**Nota**: O processo de migração itera sobre diferentes schemas no banco de dados de origem, substituindo a parte `Database` da string de conexão `SOURCE_DB` dinamicamente para cada agência (ex: `agencia_1234`).

## Execução

1. Clone o repositório.
2. Configure as variáveis de ambiente `SOURCE_DB` e `TARGET_DB` no seu ambiente de desenvolvimento ou no servidor de execução.
3. Navegue até a pasta do projeto e execute o seguinte comando:

```bash
dotnet run
```

A aplicação irá iniciar, conectar-se aos bancos de dados e começar o processo de migração, exibindo o progresso no console.

## Mapeamento de Dados

### Tabelas de Origem e Consultas

Os dados são lidos a partir de um banco de dados MySQL. As seguintes "tabelas" (definidas como `DbSet` no `SxDbContext`) são consultadas usando SQL bruto (`ToSqlQuery`).

- **`SxAgencia`**: `select ca.AG_CODIGO as CODIGO ,ca.AG_RAZAO as NOME from unico.cd_agencia ca`
- **`SxClientes`**: `SELECT COALESCE(cl.CLI_CPFCNPJ, '') AS numerodocumento, cl.CLI_TIPPES AS tipo, COALESCE(cl.cli_nome, '') AS nome, cl.CLI_NFANTA AS nomefantasia,cl.CLI_EMAIL AS email,cl.AG_CODIGO AS Agencia FROM unico.cd_cliente cl`
- **`SxContas`**: `select cc.CLI_CPFCNPJ CPFCNPJ ,cc.CCO_CONTA Codigo,PA_CODIGO PaCodigo from cc_conta cc`
- **`SxEpSegParcela`**: `select * from ep_segparcela`
- **`SxEpSegPrestamista`**: `select pm.* from ep_segprestamista pm where pm.SEG_MODALIDADE = 4`
- **`SxPontoAtendimento`**: `select cp.PA_CODIGO as CODIGO,cp.AG_CODIGO AS AGENCIA,cp.PA_SIGLA As NOME from unico.cd_pa cp`
- **`SxSeguradoras`**: `select PST_CODIGO as Codigo, pst_nome as nome,'00000000000000' as cnpj,pst_nome as razaosocial,'00000000' as cep, 'RUA' as rua,'COMPLEMENTO' as complemento,'0' as numero, 'BAIRRO' as bairro, 'CIDADE' as cidade, 'UF' as uf, 'TELEFONE' as telefone, 'EMAIL' as email from unico.cd_prestamista`

### Tabelas de Destino

Os dados migrados são gravados nas seguintes tabelas no banco de dados de destino. Estas são as tabelas que a aplicação de migração popula ativamente.

- `agencias`
- `agencias_seguradoras`
- `cooperados`
- `cooperados_agencias_contas`
- `parcelas`
- `pontos_atendimento`
- `seguradoras`
- `seguros`
