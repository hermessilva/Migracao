using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MigracaoTabelas.DEVDB.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "acao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Descrição da ação disponível (ex.: Visualizar, Editar, Excluir, Aprovar)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Catálogo de ações que podem ser executadas nas telas do sistema")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agencia",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    codigo = table.Column<string>(type: "char(4)", fixedLength: true, maxLength: 4, nullable: false, comment: "Código único da agência no formato de 4 caracteres"),
                    nome = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Nome completo da agência"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "'now()'", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Armazena informações cadastrais das agências da cooperativa")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cooperado",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    numero_documento = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false, comment: "Documento de identificação do cooperado (CPF com 11 dígitos ou CNPJ com 14 dígitos, sem formatação)"),
                    tipo = table.Column<string>(type: "enum('Física','Jurídica')", nullable: false, comment: "Tipo de pessoa: Física (CPF) ou Jurídica (CNPJ)"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome completo (pessoa física) ou razão social (pessoa jurídica) do cooperado"),
                    nome_fantasia = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Nome fantasia do cooperado (aplicável apenas para pessoa jurídica)"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Endereço de e-mail para contato e comunicações com o cooperado"),
                    telefone = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    data_expedicao_rg = table.Column<DateTime>(type: "datetime(6)", maxLength: 6, nullable: true),
                    data_nascimento = table.Column<DateTime>(type: "datetime(6)", maxLength: 6, nullable: true),
                    estado_civil = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    matricula = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    nacionalidade = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    nome_social = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true),
                    orgao_expedidor_rg = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    pais_expedicao_rg = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    patrimonio_estimado = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    profissao = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true),
                    renda_mensal = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    rg = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    sexo = table.Column<string>(type: "varchar(1)", maxLength: 1, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Cadastro de cooperados (clientes) da cooperativa que podem contratar seguros")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "evento_outbox",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela.")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    chave_idempotente = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Chave de idempotência para evitar processamento duplicado."),
                    tipo = table.Column<string>(type: "enum('Processar débito da parcela','Processar lançamento contábil da parcela')", nullable: false, comment: "Tipo do evento de negócio."),
                    chave_negocio = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Chave de negócio associada como forma de identificação do evento."),
                    payload = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: false, comment: "Payload do evento (JSON serializado). Armazerna informações do evento."),
                    status = table.Column<string>(type: "enum('Pendente','Processando','Sucesso','Falha')", nullable: false, comment: "Status do evento na fila Outbox."),
                    tentativas = table.Column<byte>(type: "tinyint unsigned", nullable: false, comment: "Número de tentativas de processamento."),
                    ultima_atualizacao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Mensagem do último erro ocorrido."),
                    identificador_externo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Identificador externo associado ao evento."),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora de criação do evento."),
                    processado_em = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora de processamento do evento.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabela de eventos para padrão Outbox que armazena um fila de processamento de forma geral.")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "gestao_documento",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<long>(type: "bigint", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    validade = table.Column<DateTime>(type: "date", nullable: false, comment: "Data inicial de validade"),
                    status = table.Column<string>(type: "enum('Ativo','Inativo')", nullable: false, comment: "Indica se um documento está disponível para uso"),
                    modelo = table.Column<byte[]>(type: "mediumblob", nullable: false, comment: "Modelo que será usado para gerar o documento"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "'now()'", comment: "Data e hora de criação do registro"),
                    extensao = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "Extensão do documento"),
                    tipo = table.Column<string>(type: "enum('Termo de Adesão com DPS','Termo de Adesão sem DPS')", nullable: false, comment: "Topo do documento/modelo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parametrizacao",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    identificador = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Nome do parâmetro que será usando no código"),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição do campo de parametrização configurável"),
                    valor = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Valor atribuido ao parametro"),
                    tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Tipo de dados do campo valor")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "perfil",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Nome descritivo do perfil de acesso"),
                    slug = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Identificador amigável do perfil para uso em URLs e código")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Tabela de perfis que agrupam permissões de acesso ao sistema")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome fantasia da seguradora"),
                    cnpj = table.Column<string>(type: "char(14)", fixedLength: true, maxLength: 14, nullable: false, comment: "CNPJ da seguradora sem formatação (apenas números)"),
                    razao_social = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Razão social completa da seguradora"),
                    status = table.Column<string>(type: "enum('Ativo','Inativo')", nullable: false, comment: "Status atual da seguradora: Ativo ou Inativo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Armazena os dados cadastrais das seguradoras parceiras")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguro_parametro",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tipo_capital = table.Column<string>(type: "enum('Fixo','Variável')", nullable: false, comment: "Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)"),
                    periodicidade_30dias = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'false'", comment: "Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)"),
                    coeficiente = table.Column<decimal>(type: "decimal(8,7)", precision: 8, scale: 7, nullable: false, comment: "Coeficiente multiplicador utilizado para cálculo do prêmio e estornos"),
                    porcentual_iof = table.Column<decimal>(type: "decimal(10,4)", precision: 10, scale: 4, nullable: false),
                    porcentagem_comissao_corretora = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false, comment: "Percentual de comissão destinado à corretora (ex: 0.1500 = 15%)"),
                    porcentagem_comissao_cooperativa = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false, comment: "Percentual de comissão destinado à cooperativa (ex: 0.0500 = 5%)"),
                    capital_invalidez = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    capital_morte = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    porcentagem_cobertura_invalidez = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    porcentagem_cobertura_morte = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false),
                    premio_invalidez = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    premio_morte = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Parâmetros de contratação do seguro utilizados para cálculos de parcelas, prêmios e cancelamentos")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    slug = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Identificador amigável da tela para uso em URLs e código"),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome ou descrição completa da tela")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "Catálogo de telas (módulos/páginas) disponíveis no sistema")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "integracao_senior",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela agencia"),
                    conta_contabil_credito = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Código da conta contábil de crédito para o lançamento"),
                    conta_contabil_debito = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Código da conta contábil de débito para o lançamento"),
                    status = table.Column<string>(type: "enum('Enviado','Falha')", nullable: false, comment: "Status da integração: Enviado (sucesso) ou Falha (erro no envio)"),
                    data_movimentacao = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora da movimentação a ser integrada"),
                    valor = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor monetário do lançamento a ser integrado"),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição detalhada do lançamento para identificação"),
                    tipo_lancamento_contabil = table.Column<string>(type: "enum('Seguro Prestamista Contratado','Comissão Seguro Prestamista Contratado','Cancelamento Seguro Prestamista Parcelado','Cancelamento Seguro Prestamista Parcelado Comissão','Cancelamento Seguro Prestamista À Vista Proporcional Comissão','Pagamento Seguro Prestamista','Recebimento Comissão Seguro Prestamista','Recebimento Premio Seguro Prestamista Parcelado','Recebimento Comissão Seguro Prestamista Parcelado')", nullable: false, comment: "Tipo do lançamento contábil conforme enum tipo_lancamento"),
                    codigo_pa = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false, comment: "Código do ponto de atendimento de origem do lançamento"),
                    numero_lancamento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Número do lançamento (ID do lançamento de origem)"),
                    visualizar = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'1'", comment: "Indica se o registro deve ser visualizado nas consultas")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "integracao_senior_ibfk_1",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                },
                comment: "Fila de controle de integrações contábeis com o sistema Sênior (ERP)")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ponto_atendimento",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela agencia"),
                    codigo = table.Column<string>(type: "char(3)", fixedLength: true, maxLength: 3, nullable: false, comment: "Código do ponto de atendimento no formato de 3 caracteres, único dentro de uma agência"),
                    nome = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Nome completo do ponto de atendimento"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "'now()'", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "ponto_atendimento_ibfk_1",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                },
                comment: "Armazena informações dos pontos de atendimento (PAs) vinculados a cada agência")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cooperado_agencia_conta",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cooperado_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela cooperado"),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela agencia"),
                    conta_corrente = table.Column<string>(type: "char(9)", fixedLength: true, maxLength: 9, nullable: false, comment: "Número da conta corrente do cooperado na agência (9 caracteres)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "cooperado_agencia_conta_ibfk_1",
                        column: x => x.cooperado_id,
                        principalTable: "cooperado",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "cooperado_agencia_conta_ibfk_2",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                },
                comment: "Tabela de junção que vincula cooperados às suas contas correntes em cada agência")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "lancamento_efetivar",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela agencia"),
                    cooperado_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela cooperado"),
                    conta_corrente = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Número da conta corrente do cooperado para débito/crédito do lançamento"),
                    data_movimentacao = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora da movimentação financeira no sistema de origem"),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição detalhada do lançamento a ser efetivado"),
                    valor = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor monetário do lançamento a ser efetivado"),
                    data_lancamento = table.Column<DateTime>(type: "date", nullable: true, comment: "Data programada para efetivação do lançamento no sistema")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "lancamento_efetivar_ibfk_1",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "lancamento_efetivar_ibfk_2",
                        column: x => x.cooperado_id,
                        principalTable: "cooperado",
                        principalColumn: "id");
                },
                comment: "Fila de lançamentos financeiros pendentes de efetivação nas contas dos cooperados")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agencia_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela agencia"),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    ordem = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "Ordem de prioridade da seguradora dentro da agência (menor = maior prioridade)"),
                    tipo_capital = table.Column<string>(type: "enum('Fixo','Variável')", nullable: false, defaultValueSql: "'Fixo'", comment: "Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "agencia_seguradora_ibfk_1",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "agencia_seguradora_ibfk_2",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Tabela de vínculo que relaciona agências com seguradoras autorizadas e define prioridade")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comissao_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    porcentagem_comissao_corretora = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false, comment: "Percentual de comissão destinado à corretora (ex: 0.1500 = 15%)"),
                    porcentagem_comissao_cooperativa = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false, comment: "Percentual de comissão destinado à cooperativa (ex: 0.0500 = 5%)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "comissao_seguradora_ibfk_1",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Configuração de percentuais de comissão por seguradora para corretora e cooperativa")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "condicao_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    max_meses_contrato = table.Column<short>(type: "smallint", nullable: false, comment: "Quantidade máxima de meses permitidos para vigência do contrato"),
                    max_idade = table.Column<short>(type: "smallint", nullable: false, comment: "Idade máxima permitida do proponente para contratação do seguro"),
                    porcentagem_cobertura_morte = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false, comment: "Percentual de cobertura para sinistro por morte (ex: 1.0000 = 100%)"),
                    porcentagem_cobertura_invalidez = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false, comment: "Percentual de cobertura para sinistro por invalidez (ex: 0.5000 = 50%)"),
                    porcentagem_cobertura_perda_renda = table.Column<decimal>(type: "decimal(5,4)", precision: 5, scale: 4, nullable: false, comment: "Percentual de cobertura para sinistro por perda de renda (ex: 0.3000 = 30%)"),
                    periodicidade_30dias = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValueSql: "'false'", comment: "Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "condicao_seguradora_ibfk_1",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Parâmetros de condições operacionais e financeiras aplicados por seguradora")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "conta_corrente_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    conta_corrente_prestamista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Número da conta corrente para operações de seguro prestamista"),
                    descricao_conta_corrente_prestamista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta corrente para operações de seguro prestamista"),
                    conta_cancelamento_prestamista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Número da conta para lançamentos de cancelamento de seguro prestamista"),
                    descricao_conta_cancelamento_prestamista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta para lançamentos de cancelamento de seguro prestamista"),
                    conta_estorno_prestamista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Número da conta para lançamentos de estorno de seguro prestamista"),
                    descricao_conta_estorno_prestamista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta para lançamentos de estorno de seguro prestamista")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "conta_corrente_seguradora_ibfk_1",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Configuração de contas correntes por seguradora para operações de seguro prestamista")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contabilizacao_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    credito_premio_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de crédito para lançamento do prêmio na contratação"),
                    descricao_credito_premio_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de crédito do prêmio na contratação"),
                    debito_premio_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de débito para lançamento do prêmio na contratação"),
                    descricao_debito_premio_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de débito do prêmio na contratação"),
                    credito_comissao_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de crédito para lançamento da comissão na contratação"),
                    descricao_credito_comissao_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de crédito da comissão na contratação"),
                    debito_comissao_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de débito para lançamento da comissão na contratação"),
                    descricao_debito_comissao_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de débito da comissão na contratação"),
                    credito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de crédito para cancelamento de comissão parcial ou total"),
                    descricao_credito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de crédito para cancelamento de comissão parcial ou total"),
                    debito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de débito para cancelamento de comissão parcial ou total"),
                    descricao_debito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de débito para cancelamento de comissão parcial ou total"),
                    credito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de crédito para cancelamento de comissão de seguro à vista"),
                    descricao_credito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de crédito para cancelamento de comissão de seguro à vista"),
                    debito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de débito para cancelamento de comissão de seguro à vista"),
                    descricao_debito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de débito para cancelamento de comissão de seguro à vista"),
                    credito_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de crédito para registro de valor pago"),
                    descricao_credito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de crédito para registro de valor pago"),
                    debito_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de débito para registro de valor pago"),
                    descricao_debito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de débito para registro de valor pago"),
                    credito_comissao_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de crédito para comissão sobre valor pago"),
                    descricao_comissao_credito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de crédito para comissão sobre valor pago"),
                    debito_comissao_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil de débito para comissão sobre valor pago"),
                    descricao_comissao_debito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de débito para comissão sobre valor pago"),
                    debito_premio_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil debito premio parcela"),
                    descricao_debito_premio_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil de débito para comissão sobre valor pago"),
                    credito_premio_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil credito premio parcela"),
                    descricao_credito_premio_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil credito premio parcela"),
                    debito_comissao_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil debito comissao parcela"),
                    descricao_debito_comissao_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil debito comissao parcela"),
                    credito_comissao_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil credito comissao parcela"),
                    descricao_credito_comissao_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil credito comissao parcela")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "contabilizacao_seguradora_ibfk_1",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Configuração de contas contábeis por seguradora para lançamentos de prêmios, comissões e cancelamentos")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "proposta_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    descricao_sequencial = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Descrição ou prefixo do formato do número sequencial da proposta"),
                    numero_sequencial = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Último número sequencial utilizado para geração de propostas")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "proposta_seguradora_ibfk_1",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Controle de numeração sequencial de propostas por seguradora")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguradora_limite",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    idade_inicial = table.Column<short>(type: "smallint", nullable: false, comment: "Idade inicial da faixa etária para aplicação da regra"),
                    idade_final = table.Column<short>(type: "smallint", nullable: false, comment: "Idade final da faixa etária para aplicação da regra"),
                    valor_maximo = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor máximo de capital segurado permitido para a faixa"),
                    coeficiente = table.Column<decimal>(type: "decimal(8,7)", precision: 8, scale: 7, nullable: false, comment: "Coeficiente multiplicador para cálculo do prêmio"),
                    limite_dps = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor limite de capital segurado que exige Declaração Pessoal de Saúde (DPS)"),
                    descricao_regra = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição textual detalhada da regra aplicada para o limite e DPS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "seguradora_limite_ibfk_1",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Define faixas etárias, coeficientes e limites de DPS por seguradora para cálculo de prêmios")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela_acao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tela_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela tela"),
                    acao_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela acao")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "tela_acao_ibfk_1",
                        column: x => x.tela_id,
                        principalTable: "tela",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tela_acao_ibfk_2",
                        column: x => x.acao_id,
                        principalTable: "acao",
                        principalColumn: "id");
                },
                comment: "Tabela de junção N:N que define quais ações estão disponíveis em cada tela")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela_acao_perfil",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tela_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela tela"),
                    acao_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela acao"),
                    perfil_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela perfil"),
                    TelaAcaoId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "tela_acao_perfil_ibfk_1",
                        column: x => x.tela_id,
                        principalTable: "tela",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tela_acao_perfil_ibfk_2",
                        column: x => x.acao_id,
                        principalTable: "acao",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "tela_acao_perfil_ibfk_3",
                        column: x => x.perfil_id,
                        principalTable: "perfil",
                        principalColumn: "id");
                },
                comment: "Tabela de permissões que define quais perfis podem executar determinadas ações em telas específicas")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuario",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ponto_atendimento_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela ponto_atendimento"),
                    perfil_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira opcional referenciando a tabela perfil para o perfil principal do usuário"),
                    login = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Login de acesso do usuário ao sistema"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome completo do usuário"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Endereço de e-mail do usuário para contato e notificações"),
                    status = table.Column<string>(type: "enum('Ativo','Inativo')", nullable: false, comment: "Indica o status do perfil"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "'now()'", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "usuario_ibfk_2",
                        column: x => x.ponto_atendimento_id,
                        principalTable: "ponto_atendimento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "usuario_ibfk_3",
                        column: x => x.perfil_id,
                        principalTable: "perfil",
                        principalColumn: "id");
                },
                comment: "Tabela de usuários do sistema com suas credenciais e vínculos organizacionais")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "apolice_grupo_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela agencia_seguradora"),
                    apolice = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Número ou código da apólice contratada"),
                    grupo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Código do grupo dentro da apólice"),
                    subgrupo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Código do subgrupo dentro do grupo da apólice"),
                    tipo_capital = table.Column<string>(type: "enum('Fixo','Variável')", nullable: false, comment: "Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)"),
                    modalidade_unico = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Identificador ou código da modalidade de pagamento único"),
                    modalidade_avista = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: true, comment: "Valor ou taxa para modalidade de pagamento à vista"),
                    modalidade_parcelado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: true, comment: "Valor ou taxa para modalidade de pagamento parcelado")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "apolice_grupo_seguradora_ibfk_1",
                        column: x => x.agencia_seguradora_id,
                        principalTable: "agencia_seguradora",
                        principalColumn: "id");
                },
                comment: "Configurações de apólices e grupos por vínculo agência-seguradora")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "auditoria",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira referenciando a tabela usuario que realizou a ação"),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira referenciando a tabela agencia onde a ação foi realizada"),
                    modulo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Nome do módulo ou área do sistema onde a operação foi realizada"),
                    tabela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Nome da tabela auditada"),
                    rota = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Nome do módulo ou área do sistema onde a operação foi realizada"),
                    chave = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave primária da tabela auditada"),
                    operacao = table.Column<string>(type: "enum('Inserção','Atualização','Deleção','Login','Refresh Token')", nullable: false, comment: "Tipo da operação realizada: Delete (exclusão) ou Update (atualização)"),
                    dados_anteriores = table.Column<string>(type: "varchar(8000)", maxLength: 8000, nullable: false, comment: "Dados do registro antes da alteração em formato JSON ou serializado"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora em que a ação foi registrada")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "auditoria_ibfk_1",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "auditoria_ibfk_2",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                },
                comment: "Tabela de auditoria para rastreamento de todas as operações realizadas no sistema")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguro",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira referenciando a tabela usuario responsável pela contratação"),
                    ponto_atendimento_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela ponto_atendimento onde o seguro foi contratado"),
                    cooperado_agencia_conta_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela cooperado_agencia_conta"),
                    apolice_grupo_seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela apolice_grupo_seguradora indicando a apolice contratada"),
                    seguro_parametro_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguro_parametro com os parâmetros de cálculo"),
                    contrato = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "Número do contrato de crédito vinculado ao seguro"),
                    inicio_vigencia = table.Column<DateTime>(type: "date", nullable: true, comment: "Data de início da vigência do seguro"),
                    fim_vigencia = table.Column<DateTime>(type: "date", nullable: true, comment: "Data de término da vigência do seguro"),
                    codigo_grupo = table.Column<int>(type: "int", nullable: false, comment: "Código identificador do grupo/produto do seguro"),
                    quantidade_parcelas = table.Column<short>(type: "smallint", nullable: false, comment: "Quantidade total de parcelas do seguro"),
                    vencimento = table.Column<DateTime>(type: "date", nullable: true, comment: "Data de vencimento base do contrato ou da próxima parcela"),
                    capital_segurado = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor total do capital segurado (valor coberto em caso de sinistro)"),
                    premio_total = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor total do prêmio do seguro a ser pago"),
                    tipo_pagamento = table.Column<string>(type: "enum('À Vista','Parcelado','Único')", nullable: false, comment: "Modalidade de pagamento: À Vista, Parcelado ou Único"),
                    estorno_proporcional = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor de estorno proporcional em caso de cancelamento"),
                    valor_base = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: true, comment: "Valor base utilizado para cálculo do seguro (saldo devedor ou valor financiado)"),
                    declaracao_pessoal_saude = table.Column<bool>(type: "tinyint(1)", nullable: true),
                    valor_iof = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: true, comment: "Valor do IOF (Imposto sobre Operações Financeiras) incidente sobre o prêmio"),
                    numero_contrato_emprestimo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "Número do contrato de crédito do empréstimo"),
                    motivo = table.Column<string>(type: "enum('Em analise na seguradora','Aguardando faturamento','Aguardando documentação','Pagamento à vista','Pagamento parcelado','Inadimplente','Regular','Recusado pela seguradora','Expiração da vigência do seguro','Aditivo','Cancelamento por prejuízo','Renegociação','Sinistro','Solicitado pela cooperativa','Solicitado pelo cooperado','Liquidação antecipada')", nullable: false),
                    status = table.Column<string>(type: "enum('Pendente','Ativo','Recusado','Expirado','Cancelado')", nullable: false),
                    contrato_sequencia = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false, defaultValueSql: "'00'", comment: "Numero sequêncial do contrato")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "seguro_ibfk_1",
                        column: x => x.apolice_grupo_seguradora_id,
                        principalTable: "apolice_grupo_seguradora",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "seguro_ibfk_2",
                        column: x => x.cooperado_agencia_conta_id,
                        principalTable: "cooperado_agencia_conta",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "seguro_ibfk_3",
                        column: x => x.ponto_atendimento_id,
                        principalTable: "ponto_atendimento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "seguro_ibfk_4",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "seguro_ibfk_5",
                        column: x => x.seguro_parametro_id,
                        principalTable: "seguro_parametro",
                        principalColumn: "id");
                },
                comment: "Contratos de seguros prestamista com informações de vigência, valores e relacionamentos")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parcela",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguro_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguro"),
                    status = table.Column<string>(type: "enum('Pendente','Em Aberto','Pago','Cancelada')", nullable: false, comment: "Status atual da parcela conforme enum status_seguro"),
                    numero_parcela = table.Column<short>(type: "smallint", nullable: false, comment: "Número sequencial da parcela dentro do seguro (1, 2, 3...)"),
                    valor_parcela = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor nominal atual da parcela a ser cobrado"),
                    valor_original = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor original da parcela calculado na contratação"),
                    valor_pago = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor total efetivamente pago na parcela"),
                    vencimento = table.Column<DateTime>(type: "date", nullable: false, comment: "Data de vencimento da parcela"),
                    liquidacao = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora de liquidação/quitação da parcela"),
                    data_ultimo_pagamento = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora do último pagamento parcial ou total registrado"),
                    comissao_corretora = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor de comissão destinado à corretora"),
                    comissao_cooperativa = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor de comissão destinado à coperativa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "parcela_ibfk_1",
                        column: x => x.seguro_id,
                        principalTable: "seguro",
                        principalColumn: "id");
                },
                comment: "Parcelas financeiras de prêmio vinculadas a um contrato de seguro")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguro_cancelamento",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguro_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguro"),
                    data = table.Column<DateTime>(type: "date", nullable: false, defaultValueSql: "'now()'", comment: "Data efetiva do cancelamento do seguro"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "'now()'", comment: "Data e hora de criação do registro de cancelamento"),
                    motivo = table.Column<string>(type: "enum('Aditivo','Cancelamento por prejuízo','Renegociaçao','Sinistro','Solicitado pela cooperativa','Solicitado pelo cooperado','Liquidação Antecipada')", nullable: false),
                    valor_restituir = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor do prêmio a ser restituído ao cooperado"),
                    valor_comissao = table.Column<decimal>(type: "decimal(10,2)", precision: 10, nullable: false, comment: "Valor da comissão a ser estornada devido ao cancelamento"),
                    dias_utilizados = table.Column<int>(type: "int", nullable: false, comment: "Quantidade de dias de vigência utilizados até o cancelamento")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "seguro_cancelamento_ibfk_1",
                        column: x => x.seguro_id,
                        principalTable: "seguro",
                        principalColumn: "id");
                },
                comment: "Registro de cancelamentos de seguros com cálculo de restituição e estorno de comissão")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "uq_acao_descricao",
                table: "acao",
                column: "descricao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "codigo",
                table: "agencia",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "nome",
                table: "agencia",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "agencia_id",
                table: "agencia_seguradora",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "seguradora_id",
                table: "agencia_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "agencia_seguradora_id",
                table: "apolice_grupo_seguradora",
                column: "agencia_seguradora_id");

            migrationBuilder.CreateIndex(
                name: "agencia_id1",
                table: "auditoria",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id",
                table: "auditoria",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "seguradora_id1",
                table: "comissao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "seguradora_id2",
                table: "condicao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "seguradora_id3",
                table: "conta_corrente_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "seguradora_id4",
                table: "contabilizacao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "agencia_id2",
                table: "cooperado_agencia_conta",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "idx_conta_corrente",
                table: "cooperado_agencia_conta",
                column: "conta_corrente");

            migrationBuilder.CreateIndex(
                name: "idx_cooperado_id_agencia_id_conta_corrente",
                table: "cooperado_agencia_conta",
                columns: new[] { "cooperado_id", "agencia_id", "conta_corrente" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "unique_chave_idempotente",
                table: "evento_outbox",
                column: "chave_idempotente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "agencia_id3",
                table: "integracao_senior",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "agencia_id4",
                table: "lancamento_efetivar",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "cooperado_id",
                table: "lancamento_efetivar",
                column: "cooperado_id");

            migrationBuilder.CreateIndex(
                name: "seguro_id",
                table: "parcela",
                column: "seguro_id");

            migrationBuilder.CreateIndex(
                name: "nome1",
                table: "perfil",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "slug",
                table: "perfil",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_agencia_id_codigo",
                table: "ponto_atendimento",
                columns: new[] { "agencia_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "nome2",
                table: "ponto_atendimento",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "seguradora_id5",
                table: "proposta_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "cnpj",
                table: "seguradora",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "seguradora_id6",
                table: "seguradora_limite",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "apolice_grupo_seguradora_id",
                table: "seguro",
                column: "apolice_grupo_seguradora_id");

            migrationBuilder.CreateIndex(
                name: "cooperado_agencia_conta_id",
                table: "seguro",
                column: "cooperado_agencia_conta_id");

            migrationBuilder.CreateIndex(
                name: "idx_seguro_parametro_id",
                table: "seguro",
                column: "seguro_parametro_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ponto_atendimento_id",
                table: "seguro",
                column: "ponto_atendimento_id");

            migrationBuilder.CreateIndex(
                name: "usuario_id1",
                table: "seguro",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "seguro_id1",
                table: "seguro_cancelamento",
                column: "seguro_id");

            migrationBuilder.CreateIndex(
                name: "slug1",
                table: "tela",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "acao_id",
                table: "tela_acao",
                column: "acao_id");

            migrationBuilder.CreateIndex(
                name: "tela_id",
                table: "tela_acao",
                column: "tela_id");

            migrationBuilder.CreateIndex(
                name: "acao_id1",
                table: "tela_acao_perfil",
                column: "acao_id");

            migrationBuilder.CreateIndex(
                name: "perfil_id",
                table: "tela_acao_perfil",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "tela_id1",
                table: "tela_acao_perfil",
                column: "tela_id");

            migrationBuilder.CreateIndex(
                name: "perfil_id1",
                table: "usuario",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "ponto_atendimento_id1",
                table: "usuario",
                column: "ponto_atendimento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "auditoria");

            migrationBuilder.DropTable(
                name: "comissao_seguradora");

            migrationBuilder.DropTable(
                name: "condicao_seguradora");

            migrationBuilder.DropTable(
                name: "conta_corrente_seguradora");

            migrationBuilder.DropTable(
                name: "contabilizacao_seguradora");

            migrationBuilder.DropTable(
                name: "evento_outbox");

            migrationBuilder.DropTable(
                name: "gestao_documento");

            migrationBuilder.DropTable(
                name: "integracao_senior");

            migrationBuilder.DropTable(
                name: "lancamento_efetivar");

            migrationBuilder.DropTable(
                name: "parametrizacao");

            migrationBuilder.DropTable(
                name: "parcela");

            migrationBuilder.DropTable(
                name: "proposta_seguradora");

            migrationBuilder.DropTable(
                name: "seguradora_limite");

            migrationBuilder.DropTable(
                name: "seguro_cancelamento");

            migrationBuilder.DropTable(
                name: "tela_acao");

            migrationBuilder.DropTable(
                name: "tela_acao_perfil");

            migrationBuilder.DropTable(
                name: "seguro");

            migrationBuilder.DropTable(
                name: "tela");

            migrationBuilder.DropTable(
                name: "acao");

            migrationBuilder.DropTable(
                name: "apolice_grupo_seguradora");

            migrationBuilder.DropTable(
                name: "cooperado_agencia_conta");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "seguro_parametro");

            migrationBuilder.DropTable(
                name: "agencia_seguradora");

            migrationBuilder.DropTable(
                name: "cooperado");

            migrationBuilder.DropTable(
                name: "ponto_atendimento");

            migrationBuilder.DropTable(
                name: "perfil");

            migrationBuilder.DropTable(
                name: "seguradora");

            migrationBuilder.DropTable(
                name: "agencia");
        }
    }
}
