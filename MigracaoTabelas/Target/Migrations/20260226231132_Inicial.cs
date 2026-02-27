using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MigracaoTabelas.Target.Migrations
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
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da ação (ex.: Visualizar, Editar, Excluir)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_acao", x => x.id);
                },
                comment: "Catálogo de ações que podem ser executadas nas telas")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agencia",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    codigo = table.Column<string>(type: "char(4)", nullable: false, comment: "Código único da agência no formato de 4 caracteres"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome completo da agência"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agencia", x => x.id);
                },
                comment: "Armazena informações cadastrais das agências da cooperativa")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "auditoria",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    chave = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave primária da tabela auditada"),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira referenciando a tabela usuario que realizou a ação"),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira referenciando a tabela agencia onde a ação foi realizada"),
                    modulo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Nome do módulo ou área do sistema onde a operação foi realizada"),
                    rota = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Rota onde a operação foi realizada"),
                    tabela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome da tabela auditada"),
                    operacao = table.Column<string>(type: "enum('Inserção','Atualização','Deleção','Login','Refresh Token')", nullable: false, comment: "Tipo da operação realizada: Insert (inserção), Delete (exclusão) ou Update (atualização)"),
                    dados_anteriores = table.Column<string>(type: "varchar(8000)", maxLength: 8000, nullable: false, comment: "Dados do registro antes da alteração em formato JSON ou serializado"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora em que a ação foi registrada")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditoria", x => x.id);
                },
                comment: "Tabela de auditoria para rastreamento de todas as operações realizadas no sistema")
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
                    nome_social = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Nome social do cooperado (nome pelo qual a pessoa prefere ser chamada)"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Endereço de e-mail para contato e comunicações com o cooperado"),
                    data_nascimento = table.Column<DateTime>(type: "datetime(6)", nullable: true, comment: "Data de nascimento do cooperado"),
                    sexo = table.Column<string>(type: "enum('Masculino','Feminino')", nullable: true, comment: "Sexo do cooperado: Masculino, Feminino ou Juridico (para pessoa jurídica)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cooperado", x => x.id);
                },
                comment: "Cadastro de cooperados (clientes) da cooperativa que podem contratar seguros")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "evento_outbox",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    chave_idempotente = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Chave de idempotência para evitar processamento duplicado."),
                    tipo = table.Column<string>(type: "enum('Processar débito da parcela','Processar lançamento contábil da parcela')", nullable: false, comment: "Tipo do evento de negócio."),
                    chave_negocio = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false, comment: "Chave de negócio associada como forma de identificação do evento."),
                    payload = table.Column<string>(type: "varchar(3000)", maxLength: 3000, nullable: false, comment: "Payload do evento (JSON serializado). Armazerna informações do evento."),
                    status = table.Column<string>(type: "enum('Pendente','Processando','Sucesso','Falha')", nullable: false, comment: "Status do evento na fila Outbox"),
                    tentativas = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "Número de tentativas de processamento."),
                    ultima_atualizacao = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "Mensagem do último erro ocorrido."),
                    identificador_externo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Identificador externo associado ao evento."),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora de criação do evento."),
                    processado_em = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora de processamento do evento.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_evento_outbox", x => x.id);
                },
                comment: "Tabela de eventos para padrão Outbox que armazena um fila de processamento de forma geral.")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parametrizacao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição do item"),
                    identificador = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Nome do parâmetro que será usando no código"),
                    valor = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Valor atribuido ao parametro"),
                    tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Tipo de dados do campo valor")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parametrizacao", x => x.id);
                },
                comment: "Catálogo de campos de parametrização do sistema para configurações dinâmicas")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "perfil",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome descritivo do perfil de acesso"),
                    slug = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Identificador amigável do perfil para uso em URLs e código")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_perfil", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome fantasia da seguradora"),
                    cnpj = table.Column<string>(type: "char(14)", nullable: false, comment: "CNPJ da seguradora sem formatação (apenas números)"),
                    razao_social = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Razão social completa da seguradora"),
                    status = table.Column<string>(type: "enum('Ativo','Inativo')", nullable: false, comment: "Status atual da seguradora: Ativo ou Inativo")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguradora", x => x.id);
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
                    periodicidade_30dias = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)"),
                    coeficiente = table.Column<decimal>(type: "decimal(8,7)", nullable: false, comment: "Coeficiente multiplicador utilizado para cálculo do prêmio e estornos"),
                    porcentual_iof = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Porcentual de IOF cobrado no seguro"),
                    porcentagem_comissao_corretora = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de comissão destinado à corretora (ex: 0.1500 = 15%)"),
                    porcentagem_comissao_cooperativa = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de comissão destinado à cooperativa (ex: 0.0500 = 5%)"),
                    porcentagem_cobertura_morte = table.Column<decimal>(type: "decimal(5,4)", nullable: false, defaultValue: 0m, comment: "Percentual de cobertura por morte (ex: 1.0000 = 100%)"),
                    capital_morte = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Valor do capital segurado por morte"),
                    premio_morte = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Valor do prêmio referente à cobertura por morte"),
                    porcentagem_cobertura_invalidez = table.Column<decimal>(type: "decimal(5,4)", nullable: false, defaultValue: 0m, comment: "Percentual de cobertura por invalidez permanente total por acidente - IPTA (ex: 1.0000 = 100%)"),
                    capital_invalidez = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Valor do capital segurado por invalidez permanente total por acidente - IPTA"),
                    premio_invalidez = table.Column<decimal>(type: "decimal(18,2)", nullable: false, defaultValue: 0m, comment: "Valor do prêmio referente à cobertura por invalidez - IPTA")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguro_parametro", x => x.id);
                },
                comment: "Parâmetros de contratação do seguro utilizados para cálculos de parcelas, prêmios e cancelamentos")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome ou descrição completa da tela"),
                    slug = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Identificador amigável da tela para uso em URLs e código")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tela", x => x.id);
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
                    status = table.Column<string>(type: "enum('Enviado','Falha')", nullable: false, comment: "Status da integração: Enviado (sucesso) ou Falha (erro no envio)"),
                    tipo_lancamento_contabil = table.Column<string>(type: "enum('Seguro Prestamista Contratado','Comissão Seguro Prestamista Contratado','Cancelamento Seguro Prestamista Parcelado Comissão','Cancelamento Seguro Prestamista À Vista Proporcional Comissão','Pagamento Seguro Prestamista','Recebimento Comissão Seguro Prestamista','Recebimento Premio Seguro Prestamista Parcelado','Recebimento Comissão Seguro Prestamista Parcelado','Faturamento Prêmio Seguro Prestamista','Faturamento Comissão Seguro Prestamista','Faturamento IRRF Seguro Prestamista')", nullable: false, comment: "Tipo do lançamento contábil conforme enum tipo_lancamento"),
                    codigo_pa = table.Column<string>(type: "char(3)", nullable: false, comment: "Código do ponto de atendimento de origem do lançamento"),
                    conta_contabil_debito = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Código da conta contábil de débito para o lançamento"),
                    conta_contabil_credito = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Código da conta contábil de crédito para o lançamento"),
                    data_movimentacao = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora da movimentação a ser integrada"),
                    valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor monetário do lançamento a ser integrado"),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição detalhada do lançamento para identificação"),
                    numero_lancamento = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Número do lançamento (ID do lançamento de origem)"),
                    visualizar = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true, comment: "Indica se o registro deve ser visualizado nas consultas")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_integracao_senior", x => x.id);
                    table.ForeignKey(
                        name: "FK_integracao_senior_agencia_agencia_id",
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
                    codigo = table.Column<string>(type: "char(3)", nullable: false, comment: "Código do ponto de atendimento no formato de 3 caracteres, único dentro de uma agência"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome completo do ponto de atendimento"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ponto_atendimento", x => x.id);
                    table.ForeignKey(
                        name: "FK_ponto_atendimento_agencia_agencia_id",
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
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cooperado_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela cooperado"),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela agencia"),
                    conta_corrente = table.Column<string>(type: "char(9)", nullable: false, comment: "Código da conta corrente")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cooperado_agencia_conta", x => x.id);
                    table.ForeignKey(
                        name: "FK_cooperado_agencia_conta_agencia_agencia_id",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_cooperado_agencia_conta_cooperado_cooperado_id",
                        column: x => x.cooperado_id,
                        principalTable: "cooperado",
                        principalColumn: "id");
                },
                comment: "Junção entre cooperados, agencias e contas")
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
                    valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor monetário do lançamento a ser efetivado"),
                    data_lancamento = table.Column<DateTime>(type: "date", nullable: true, comment: "Data programada para efetivação do lançamento no sistema")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lancamento_efetivar", x => x.id);
                    table.ForeignKey(
                        name: "FK_lancamento_efetivar_agencia_agencia_id",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_lancamento_efetivar_cooperado_cooperado_id",
                        column: x => x.cooperado_id,
                        principalTable: "cooperado",
                        principalColumn: "id");
                },
                comment: "Fila de lançamentos financeiros pendentes de efetivação nas contas dos cooperados")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "apolice_grupo_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela agencia"),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    ordem = table.Column<int>(type: "int", nullable: false, comment: "Ordem de prioridade da seguradora dentro da agência (menor = maior prioridade)"),
                    apolice = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Número ou código da apólice contratada"),
                    grupo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Código do grupo dentro da apólice"),
                    subgrupo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Código do subgrupo dentro do grupo da apólice"),
                    tipo_capital = table.Column<string>(type: "enum('Fixo','Variável')", nullable: false, comment: "Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)"),
                    modalidade_unico = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Identificador ou código da modalidade de pagamento único"),
                    modalidade_avista = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor ou taxa para modalidade de pagamento à vista"),
                    modalidade_parcelado = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor ou taxa para modalidade de pagamento parcelado")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_apolice_grupo_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_apolice_grupo_seguradora_agencia_agencia_id",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_apolice_grupo_seguradora_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Configurações de apólices e grupos por vínculo agência-seguradora")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "comissao_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    porcentagem_comissao_corretora = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de comissão da corretora"),
                    porcentagem_comissao_cooperativa = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de comissão da cooperativa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_comissao_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_comissao_seguradora_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Configurações de comissões por seguradora")
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
                    porcentagem_cobertura_morte = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de cobertura para sinistro por morte (ex: 1.0000 = 100%)"),
                    porcentagem_cobertura_invalidez = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de cobertura para sinistro por invalidez (ex: 0.5000 = 50%)"),
                    porcentagem_cobertura_perda_renda = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de cobertura para sinistro por perda de renda (ex: 0.3000 = 30%)"),
                    periodicidade_30dias = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condicao_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_condicao_seguradora_seguradora_seguradora_id",
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
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    conta_corrente_prestamista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Numero da conta corrente de seguro prestamista"),
                    descricao_conta_corrente_prestamista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta corrente de seguro prestamista"),
                    conta_cancelamento_prestamista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Numero da conta de cancelamento de seguro prestamista"),
                    descricao_conta_cancelamento_prestamista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descricao da conta de cancelamento de seguro prestamista"),
                    conta_estorno_prestamista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Numero da conta de estorno de seguro prestamista"),
                    descricao_conta_estorno_prestamista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descricao da conta de estorno de seguro prestamista")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conta_corrente_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_conta_corrente_seguradora_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Junção entre parâmetros de seguradora e contas correntes")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contabilizacao_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    credito_premio_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil premio de credito"),
                    descricao_credito_premio_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil premio de credito"),
                    debito_premio_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil premio de debito"),
                    descricao_debito_premio_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil premio de debito"),
                    credito_comissao_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil comissao de credito "),
                    descricao_credito_comissao_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil comissao de credito"),
                    debito_comissao_contratacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil comissao de debito"),
                    descricao_debito_comissao_contratacao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil comissao de debito"),
                    credito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil cancelamento parcial total de credito"),
                    descricao_credito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil cancelamento parcial total de credito"),
                    debito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil cancelamento parcial total de debito"),
                    descricao_debito_cancelamento_comissao_parc_tot = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil cancelamento parcial total de debito"),
                    credito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil cancelamento comissao a vista credito"),
                    descricao_credito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil cancelamento comissao a vista credito"),
                    debito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil cancelamento comissao a vista debito"),
                    descricao_debito_cancelamento_comissao_avista = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil cancelamento comissao a vista debito"),
                    credito_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil valor pago credito"),
                    descricao_credito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil valor pago credito"),
                    debito_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil valor pago debito"),
                    descricao_debito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil valor pago debito"),
                    credito_comissao_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil comissao valor pago credito"),
                    descricao_comissao_credito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil comissao valor pago credito"),
                    debito_comissao_valor_pago = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil comissao valor pago debito"),
                    descricao_comissao_debito_valor_pago = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil comissao valor pago debito"),
                    debito_premio_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil debito premio parcela"),
                    descricao_debito_premio_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil debito premio parcela"),
                    credito_premio_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil credito premio parcela"),
                    descricao_credito_premio_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil credito premio parcela"),
                    debito_comissao_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil debito comissao parcela"),
                    descricao_debito_comissao_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil debito comissao parcela"),
                    credito_comissao_parcela = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil credito comissao parcela"),
                    descricao_credito_comissao_parcela = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil credito comissao parcela"),
                    debito_faturamento_premio = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil debito faturamento premio"),
                    descricao_debito_faturamento_premio = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil debito faturamento premio"),
                    credito_faturamento_premio = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil credito faturamento premio"),
                    descricao_credito_faturamento_premio = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil credito faturamento premio"),
                    debito_faturamento_comissao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil débito faturamento comissão"),
                    descricao_debito_faturamento_comissao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil débito faturamento comissão"),
                    credito_faturamento_comissao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil crédito faturamento comissão"),
                    descricao_credito_faturamento_comissao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil crédito faturamento comissão"),
                    debito_faturamento_irrf = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil débito faturamento IRRF"),
                    descricao_debito_faturamento_irrf = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil débito faturamento IRRF"),
                    credito_faturamento_irrf = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "Código da conta contábil crédito faturamento IRRF"),
                    descricao_credito_faturamento_irrf = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil crédito faturamento IRRF")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contabilizacao_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_contabilizacao_seguradora_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "faturamento_exportacao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false),
                    usuario_nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    usuario_email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    tipo_pagamento = table.Column<string>(type: "enum('À Vista','Parcelado')", nullable: false, comment: "Tipo de pagamento do seguro: À Vista ou Parcelado"),
                    periodo_inicio = table.Column<DateTime>(type: "datetime", nullable: false),
                    periodo_fim = table.Column<DateTime>(type: "datetime", nullable: false),
                    quantidade_registros = table.Column<int>(type: "int", nullable: false),
                    status = table.Column<string>(type: "enum('Sucesso','Erro')", nullable: false, comment: "Status da exportação: Sucesso ou Erro"),
                    nome_arquivo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false),
                    arquivo_hash = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    mensagem_erro = table.Column<string>(type: "text", nullable: true),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faturamento_exportacao", x => x.id);
                    table.ForeignKey(
                        name: "FK_faturamento_exportacao_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "gestao_documento",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    tipo = table.Column<string>(type: "enum('Termo de Adesão com DPS','Termo de Adesão sem DPS','Documentação Complementar')", nullable: false, comment: "Tipo do documento/modelo"),
                    validade = table.Column<DateTime>(type: "date", nullable: false, comment: "Data inicial de validade"),
                    status = table.Column<string>(type: "enum('Ativo','Inativo')", nullable: false, comment: "Indica se um documento está disponível para uso"),
                    modelo = table.Column<string>(type: "mediumtext", nullable: false, comment: "Modelo que será usado para gerar o documento"),
                    extensao = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "Extensão do documento"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gestao_documento", x => x.id);
                    table.ForeignKey(
                        name: "FK_gestao_documento_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                },
                comment: "Armazena os documentos de gestão por seguradora")
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
                    table.PrimaryKey("PK_proposta_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_proposta_seguradora_seguradora_seguradora_id",
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
                    valor_maximo = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor máximo de capital segurado permitido para a faixa"),
                    coeficiente = table.Column<decimal>(type: "decimal(8,7)", nullable: false, comment: "Coeficiente multiplicador para cálculo do prêmio"),
                    limite_dps = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor limite de capital segurado que exige Declaração Pessoal de Saúde (DPS)"),
                    descricao_regra = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição textual detalhada da regra aplicada para o limite e DPS")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguradora_limite", x => x.id);
                    table.ForeignKey(
                        name: "FK_seguradora_limite_seguradora_seguradora_id",
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
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tela_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela tela"),
                    acao_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela acao")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tela_acao", x => x.id);
                    table.UniqueConstraint("AK_tela_acao_tela_id_acao_id", x => new { x.tela_id, x.acao_id });
                    table.ForeignKey(
                        name: "FK_tela_acao_acao_acao_id",
                        column: x => x.acao_id,
                        principalTable: "acao",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tela_acao_tela_tela_id",
                        column: x => x.tela_id,
                        principalTable: "tela",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela_acao_perfil",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tela_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela tela"),
                    acao_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela acao"),
                    perfil_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela perfil")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tela_acao_perfil", x => x.id);
                    table.UniqueConstraint("AK_tela_acao_perfil_tela_id_acao_id_perfil_id", x => new { x.tela_id, x.acao_id, x.perfil_id });
                    table.ForeignKey(
                        name: "FK_tela_acao_perfil_acao_acao_id",
                        column: x => x.acao_id,
                        principalTable: "acao",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tela_acao_perfil_perfil_perfil_id",
                        column: x => x.perfil_id,
                        principalTable: "perfil",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tela_acao_perfil_tela_tela_id",
                        column: x => x.tela_id,
                        principalTable: "tela",
                        principalColumn: "id");
                })
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
                    status = table.Column<string>(type: "enum('Ativo','Inativo')", nullable: false, comment: "Status do usuário: Ativo ou Inativo"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuario", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuario_perfil_perfil_id",
                        column: x => x.perfil_id,
                        principalTable: "perfil",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_usuario_ponto_atendimento_ponto_atendimento_id",
                        column: x => x.ponto_atendimento_id,
                        principalTable: "ponto_atendimento",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Tabela de usuários do sistema com suas credenciais e vínculos organizacionais")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "baixa_comissao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro de baixa de comissão")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguradora"),
                    lote = table.Column<string>(type: "varchar(50)", nullable: false, comment: "Número do lote/fatura oriundo do faturamento"),
                    competencia = table.Column<DateTime>(type: "date", nullable: false, comment: "Mês e ano correspondente ao movimento do faturamento (vencimento da parcela)"),
                    tipo_pagamento = table.Column<string>(type: "enum('À Vista','Parcelado','Único')", nullable: false, comment: "Tipo de pagamento do seguro: À Vista, Parcelado ou Único"),
                    data_emissao = table.Column<DateTime>(type: "date", nullable: false, comment: "Data de expectativa de pagamento de comissão indicada no faturamento"),
                    valor_comissao = table.Column<decimal>(type: "decimal(14,2)", nullable: false, comment: "Valor total de comissão provisionada para o lote/fatura"),
                    quantidade_cooperados = table.Column<int>(type: "int", nullable: false, comment: "Número total de movimentos faturados no lote"),
                    data_recebimento = table.Column<DateTime>(type: "date", nullable: true, comment: "Data em que a comissão foi efetivamente recebida/baixada"),
                    situacao = table.Column<string>(type: "enum('Pendente','Recebida')", nullable: false, comment: "Situação da comissão: Pendente ou Recebida"),
                    usuario_baixa_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira referenciando o usuário que realizou a baixa"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_baixa_comissao", x => x.id);
                    table.ForeignKey(
                        name: "FK_baixa_comissao_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_baixa_comissao_usuario_usuario_baixa_id",
                        column: x => x.usuario_baixa_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                },
                comment: "Registros de baixa de comissão de faturamento de seguro prestamista, agrupados por seguradora, lote e competência")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "faturamento_importacao_historico",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro de importação")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a seguradora do arquivo importado"),
                    tipo_pagamento = table.Column<string>(type: "enum('À Vista','Parcelado','Único')", nullable: false, comment: "Tipo de pagamento: À Vista, Parcelado ou Único"),
                    lote = table.Column<string>(type: "varchar(50)", nullable: false, comment: "Número do lote/fatura identificado no arquivo importado"),
                    nome_arquivo = table.Column<string>(type: "varchar(255)", nullable: false, comment: "Nome do arquivo importado pelo usuário"),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando o usuário que realizou a importação"),
                    origem = table.Column<string>(type: "enum('Manual','Automático')", nullable: false, comment: "Origem da importação: Manual ou Automático"),
                    status_importacao = table.Column<string>(type: "enum('Processado','Erro','Processado Parcialmente')", nullable: false, comment: "Status da importação: Processado, Erro ou Processado Parcialmente"),
                    total_registros_arquivo = table.Column<int>(type: "int", nullable: false, comment: "Total de registros/linhas encontrados no arquivo"),
                    quantidade_seguros_aceitos = table.Column<int>(type: "int", nullable: false, comment: "Quantidade de seguros com status aceito no arquivo"),
                    quantidade_seguros_nao_aceitos = table.Column<int>(type: "int", nullable: false, comment: "Quantidade de seguros com status recusado ou pendente no arquivo"),
                    valor_total_seguros_aceitos = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Valor total dos prêmios dos seguros aceitos"),
                    valor_total_seguros_nao_aceitos = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Valor total dos prêmios dos seguros não aceitos"),
                    valor_total_arquivo = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Valor total geral de todos os registros do arquivo"),
                    valor_total_faturar = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Valor total a ser faturado (aceitos)"),
                    observacao = table.Column<string>(type: "mediumtext", nullable: true, comment: "Observações adicionais ou mensagem de erro da importação"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora de criação do registro de importação")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faturamento_importacao_historico", x => x.id);
                    table.ForeignKey(
                        name: "FK_faturamento_importacao_historico_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_faturamento_importacao_historico_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                },
                comment: "Histórico de importações de arquivos de retorno das seguradoras para faturamento")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguro",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    cooperado_agencia_conta_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela cooperado_agencia_conta"),
                    ponto_atendimento_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela ponto_atendimento onde o seguro foi contratado"),
                    apolice_grupo_seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela apolice_grupo_seguradora indicando a apolice contratada"),
                    seguro_parametro_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguro_parametro com os parâmetros de cálculo"),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira referenciando a tabela usuario responsável pela contratação"),
                    contrato_sequencia = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true, defaultValue: "00", comment: "Numero sequêncial do contrato"),
                    status = table.Column<string>(type: "enum('Pendente','Ativo','Recusado','Expirado','Cancelado')", nullable: false, comment: "Status do seguro"),
                    motivo = table.Column<string>(type: "enum('Em analise na seguradora','Aguardando faturamento','Aguardando documentação','Pagamento à vista','Pagamento parcelado','Inadimplente','Regular','Recusado pela seguradora','Expiração da vigência do seguro','Aditivo','Cancelamento por prejuízo','Renegociação','Sinistro','Solicitado pela cooperativa','Solicitado pelo cooperado','Liquidação antecipada')", nullable: false, comment: "Motivo do seguro"),
                    contrato = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "Número do contrato de seguro"),
                    numero_contrato_emprestimo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "Número do contrato de crédito do empréstimo"),
                    inicio_vigencia = table.Column<DateTime>(type: "date", nullable: true, comment: "Data de início da vigência do seguro"),
                    fim_vigencia = table.Column<DateTime>(type: "date", nullable: true, comment: "Data de término da vigência do seguro"),
                    codigo_grupo = table.Column<int>(type: "int", nullable: false, comment: "Código identificador do grupo/produto do seguro"),
                    quantidade_parcelas = table.Column<short>(type: "smallint", nullable: false, comment: "Quantidade total de parcelas do seguro"),
                    vencimento = table.Column<DateTime>(type: "date", nullable: true, comment: "Data de vencimento base do contrato ou da próxima parcela"),
                    capital_segurado = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor total do capital segurado (valor coberto em caso de sinistro)"),
                    premio_total = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor total do prêmio do seguro a ser pago"),
                    tipo_pagamento = table.Column<string>(type: "enum('À Vista','Parcelado')", nullable: false, comment: "Modalidade de pagamento: À Vista ou Parcelado"),
                    estorno_proporcional = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor de estorno proporcional em caso de cancelamento"),
                    valor_base = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor base utilizado para cálculo do seguro (saldo devedor ou valor financiado)"),
                    declaracao_pessoal_saude = table.Column<bool>(type: "tinyint(1)", nullable: true, comment: "Indica se foi exigida Declaração Pessoal de Saúde (true/false)"),
                    valor_iof = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor do IOF (Imposto sobre Operações Financeiras) incidente sobre o prêmio"),
                    upload_documento = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora do upload dos documentos relacionados ao seguro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguro", x => x.id);
                    table.ForeignKey(
                        name: "FK_seguro_apolice_grupo_seguradora_apolice_grupo_seguradora_id",
                        column: x => x.apolice_grupo_seguradora_id,
                        principalTable: "apolice_grupo_seguradora",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_seguro_cooperado_agencia_conta_cooperado_agencia_conta_id",
                        column: x => x.cooperado_agencia_conta_id,
                        principalTable: "cooperado_agencia_conta",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_seguro_ponto_atendimento_ponto_atendimento_id",
                        column: x => x.ponto_atendimento_id,
                        principalTable: "ponto_atendimento",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_seguro_seguro_parametro_seguro_parametro_id",
                        column: x => x.seguro_parametro_id,
                        principalTable: "seguro_parametro",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_seguro_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                },
                comment: "Contratos de seguros e seus metadados financeiros e relacionamentos")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "armazenamento_documento",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguro_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela seguro"),
                    nome_original = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome original do arquivo no momento do upload"),
                    extensao_arquivo = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false, comment: "Extensão do arquivo (ex: pdf, png, jpg)"),
                    tipo = table.Column<string>(type: "enum('Termo de Adesão com DPS','Termo de Adesão sem DPS','Documentação Complementar')", nullable: false, comment: "Tipo/finalidade do documento armazenado"),
                    finalidade = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true, comment: "Descrição da finalidade do documento quando Tipo for Documentação Complementar"),
                    local = table.Column<string>(type: "enum('Amazon Web Services S3','Microsoft Azure Blob Storage','Google Cloud Platform Storage','Oracle Cloud Infrastructure Object Storage','Armazenamento On-Premises')", nullable: false, comment: "Local de armazenamento (AWS, Azure, GCP, OCI, OnPrem)"),
                    unidade_armazenamento = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Nome do Bucket (S3/GCP/MinIO) ou Container (Azure/OCI)"),
                    caminho_arquivo = table.Column<string>(type: "varchar(450)", maxLength: 450, nullable: false, comment: "Caminho completo do objeto (Key/Path) no provedor de armazenamento"),
                    codigo_regiao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "Código da região do provedor (ex: us-east-1, eastus, southamerica-east1)"),
                    endpoint_alias = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "URL base customizada para CDN, proxy ou endpoint personalizado"),
                    tipo_mime = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Tipo MIME do arquivo (ex: application/pdf, image/png)"),
                    tamanho_bytes = table.Column<long>(type: "bigint", nullable: false, comment: "Tamanho do arquivo em bytes"),
                    hash_controle = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, comment: "Hash SHA-256 do arquivo para verificação de integridade e auditoria"),
                    status = table.Column<string>(type: "enum('Ativo','Inativo','Pendente de Processamento','Erro no Upload','Excluído')", nullable: false, defaultValue: "Ativo", comment: "Status atual do documento armazenado"),
                    criado_por = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Identificador do usuário que criou o registro"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_armazenamento_documento", x => x.id);
                    table.ForeignKey(
                        name: "FK_armazenamento_documento_seguro_seguro_id",
                        column: x => x.seguro_id,
                        principalTable: "seguro",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                },
                comment: "Armazena referências de documentos em provedores de nuvem ou on-premises vinculados a seguros")
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
                    valor_parcela = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor nominal atual da parcela a ser cobrado"),
                    valor_original = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor original da parcela calculado na contratação"),
                    valor_pago = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor total efetivamente pago na parcela"),
                    vencimento = table.Column<DateTime>(type: "date", nullable: false, comment: "Data de vencimento da parcela"),
                    liquidacao = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora de liquidação/quitação da parcela"),
                    data_ultimo_pagamento = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora do último pagamento parcial ou total registrado"),
                    comissao_corretora = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor da comissão do corretor sobre a parcela"),
                    comissao_cooperativa = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor da comissão da cooperativa sobre a parcela")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parcela", x => x.id);
                    table.ForeignKey(
                        name: "FK_parcela_seguro_seguro_id",
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
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguro_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguro"),
                    data = table.Column<DateTime>(type: "date", nullable: false, comment: "Data do cancelamento"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data/hora de criação do registro"),
                    motivo = table.Column<string>(type: "enum('Cancelado pelo cooperado','Cancelado pela cooperativa','Sinistro','Recusado pela seguradora','Cancelamento por prejuízo','Liquidação antecipada','Cancelado por renegociação','Cancelado por aditivo')", nullable: false, comment: "Motivo do cancelamento"),
                    valor_restituir = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor que foi restituido ao segurado"),
                    valor_comissao = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor que foi laçando de abatimento de comissão"),
                    dias_utilizados = table.Column<int>(type: "int", nullable: false, comment: "Quantidade de dias que foi utilizado o seguro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguro_cancelamento", x => x.id);
                    table.ForeignKey(
                        name: "FK_seguro_cancelamento_seguro_seguro_id",
                        column: x => x.seguro_id,
                        principalTable: "seguro",
                        principalColumn: "id");
                },
                comment: "Registro de cancelamento de seguro")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "faturamento_parcela",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro de faturamento")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    parcela_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela parcela"),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira referenciando a tabela usuario que realizou a operação"),
                    lote = table.Column<string>(type: "varchar(50)", nullable: false, comment: "Número do lote/fatura informado pelo usuário no momento do faturamento"),
                    status = table.Column<string>(type: "enum('Faturado','Cancelado','Não Faturado','Em Processamento')", nullable: false, comment: "Status do faturamento: Faturado, Cancelado, Não Faturado ou Em Processamento"),
                    origem = table.Column<string>(type: "enum('Manual','Automático')", nullable: false, comment: "Origem do faturamento: Manual ou Automático"),
                    data_pagamento = table.Column<DateTime>(type: "date", nullable: false, comment: "Data de pagamento informada no momento do faturamento"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora de criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_faturamento_parcela", x => x.id);
                    table.ForeignKey(
                        name: "FK_faturamento_parcela_parcela_parcela_id",
                        column: x => x.parcela_id,
                        principalTable: "parcela",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_faturamento_parcela_usuario_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuario",
                        principalColumn: "id");
                },
                comment: "Registros de faturamento e cancelamento de parcelas do seguro prestamista")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "ux_acao_descricao",
                table: "acao",
                column: "descricao",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_agencia_codigo",
                table: "agencia",
                column: "codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_agencia_nome",
                table: "agencia",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_apolice_grupo_seguradora_seguradora_id",
                table: "apolice_grupo_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_ApoliceGrupoSeguradora_Agencia_Seguradora_TipoCapital",
                table: "apolice_grupo_seguradora",
                columns: new[] { "agencia_id", "seguradora_id", "tipo_capital" });

            migrationBuilder.CreateIndex(
                name: "IX_ApoliceGrupoSeguradora_Ordem",
                table: "apolice_grupo_seguradora",
                column: "ordem");

            migrationBuilder.CreateIndex(
                name: "ix_armazenamento_documento_hash_controle",
                table: "armazenamento_documento",
                column: "hash_controle");

            migrationBuilder.CreateIndex(
                name: "ix_armazenamento_documento_local_caminho",
                table: "armazenamento_documento",
                columns: new[] { "local", "unidade_armazenamento", "caminho_arquivo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_armazenamento_documento_seguro_id",
                table: "armazenamento_documento",
                column: "seguro_id");

            migrationBuilder.CreateIndex(
                name: "ix_armazenamento_documento_status",
                table: "armazenamento_documento",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "ix_armazenamento_documento_tipo",
                table: "armazenamento_documento",
                column: "tipo");

            migrationBuilder.CreateIndex(
                name: "idx_baixa_comissao_competencia",
                table: "baixa_comissao",
                column: "competencia");

            migrationBuilder.CreateIndex(
                name: "idx_baixa_comissao_lote",
                table: "baixa_comissao",
                column: "lote");

            migrationBuilder.CreateIndex(
                name: "idx_baixa_comissao_seguradora_id",
                table: "baixa_comissao",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "idx_baixa_comissao_seguradora_lote_competencia_tipo_pagamento",
                table: "baixa_comissao",
                columns: new[] { "seguradora_id", "lote", "competencia", "tipo_pagamento" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_baixa_comissao_situacao",
                table: "baixa_comissao",
                column: "situacao");

            migrationBuilder.CreateIndex(
                name: "IX_baixa_comissao_usuario_baixa_id",
                table: "baixa_comissao",
                column: "usuario_baixa_id");

            migrationBuilder.CreateIndex(
                name: "comissao_seguradora_index_5",
                table: "comissao_seguradora",
                column: "seguradora_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "condicao_seguradora_index_1",
                table: "condicao_seguradora",
                column: "seguradora_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "conta_corrente_seguradora_index_4",
                table: "conta_corrente_seguradora",
                column: "seguradora_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "contabilizacao_seguradora_index_3",
                table: "contabilizacao_seguradora",
                column: "seguradora_id",
                unique: true);

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
                name: "IX_cooperado_agencia_conta_agencia_id",
                table: "cooperado_agencia_conta",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_evento_outbox_chave_idempotente",
                table: "evento_outbox",
                column: "chave_idempotente",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_exportacao_arquivo_hash",
                table: "faturamento_exportacao",
                column: "arquivo_hash");

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_exportacao_criado_em",
                table: "faturamento_exportacao",
                column: "criado_em");

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_exportacao_filtros",
                table: "faturamento_exportacao",
                columns: new[] { "seguradora_id", "tipo_pagamento", "periodo_inicio", "periodo_fim" });

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_exportacao_seguradora_id",
                table: "faturamento_exportacao",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_exportacao_usuario_email",
                table: "faturamento_exportacao",
                column: "usuario_email");

            migrationBuilder.CreateIndex(
                name: "idx_fat_imp_hist_criado_em",
                table: "faturamento_importacao_historico",
                column: "criado_em");

            migrationBuilder.CreateIndex(
                name: "idx_fat_imp_hist_lote",
                table: "faturamento_importacao_historico",
                column: "lote");

            migrationBuilder.CreateIndex(
                name: "idx_fat_imp_hist_seguradora_id",
                table: "faturamento_importacao_historico",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "idx_fat_imp_hist_status_importacao",
                table: "faturamento_importacao_historico",
                column: "status_importacao");

            migrationBuilder.CreateIndex(
                name: "idx_fat_imp_hist_usuario_id",
                table: "faturamento_importacao_historico",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_parcela_criado_em",
                table: "faturamento_parcela",
                column: "criado_em");

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_parcela_lote",
                table: "faturamento_parcela",
                column: "lote");

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_parcela_parcela_id",
                table: "faturamento_parcela",
                column: "parcela_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_faturamento_parcela_status",
                table: "faturamento_parcela",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_faturamento_parcela_usuario_id",
                table: "faturamento_parcela",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_gestao_documento_seguradora_id",
                table: "gestao_documento",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_integracao_senior_agencia_id",
                table: "integracao_senior",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_efetivar_agencia_id",
                table: "lancamento_efetivar",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_lancamento_efetivar_cooperado_id",
                table: "lancamento_efetivar",
                column: "cooperado_id");

            migrationBuilder.CreateIndex(
                name: "IX_parcela_seguro_id",
                table: "parcela",
                column: "seguro_id");

            migrationBuilder.CreateIndex(
                name: "IX_perfil_nome",
                table: "perfil",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_perfil_slug",
                table: "perfil",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_agencia_id_codigo",
                table: "ponto_atendimento",
                columns: new[] { "agencia_id", "codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ponto_atendimento_nome",
                table: "ponto_atendimento",
                column: "nome",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_proposta_seguradora_seguradora_id",
                table: "proposta_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_seguradora_cnpj",
                table: "seguradora",
                column: "cnpj",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seguradora_limite_seguradora_id",
                table: "seguradora_limite",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "idx_seguro_parametro_id",
                table: "seguro",
                column: "seguro_parametro_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seguro_apolice_grupo_seguradora_id",
                table: "seguro",
                column: "apolice_grupo_seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_seguro_cooperado_agencia_conta_id",
                table: "seguro",
                column: "cooperado_agencia_conta_id");

            migrationBuilder.CreateIndex(
                name: "IX_seguro_ponto_atendimento_id",
                table: "seguro",
                column: "ponto_atendimento_id");

            migrationBuilder.CreateIndex(
                name: "IX_seguro_usuario_id",
                table: "seguro",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_seguro_cancelamento_seguro_id",
                table: "seguro_cancelamento",
                column: "seguro_id");

            migrationBuilder.CreateIndex(
                name: "IX_tela_slug",
                table: "tela",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "tela_acao_index_2",
                table: "tela_acao",
                column: "tela_id");

            migrationBuilder.CreateIndex(
                name: "tela_acao_index_3",
                table: "tela_acao",
                column: "acao_id");

            migrationBuilder.CreateIndex(
                name: "tela_acao_perfil_index_4",
                table: "tela_acao_perfil",
                column: "tela_id");

            migrationBuilder.CreateIndex(
                name: "tela_acao_perfil_index_5",
                table: "tela_acao_perfil",
                column: "acao_id");

            migrationBuilder.CreateIndex(
                name: "tela_acao_perfil_index_6",
                table: "tela_acao_perfil",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_perfil_id",
                table: "usuario",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuario_ponto_atendimento_id",
                table: "usuario",
                column: "ponto_atendimento_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "armazenamento_documento");

            migrationBuilder.DropTable(
                name: "auditoria");

            migrationBuilder.DropTable(
                name: "baixa_comissao");

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
                name: "faturamento_exportacao");

            migrationBuilder.DropTable(
                name: "faturamento_importacao_historico");

            migrationBuilder.DropTable(
                name: "faturamento_parcela");

            migrationBuilder.DropTable(
                name: "gestao_documento");

            migrationBuilder.DropTable(
                name: "integracao_senior");

            migrationBuilder.DropTable(
                name: "lancamento_efetivar");

            migrationBuilder.DropTable(
                name: "parametrizacao");

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
                name: "parcela");

            migrationBuilder.DropTable(
                name: "acao");

            migrationBuilder.DropTable(
                name: "tela");

            migrationBuilder.DropTable(
                name: "seguro");

            migrationBuilder.DropTable(
                name: "apolice_grupo_seguradora");

            migrationBuilder.DropTable(
                name: "cooperado_agencia_conta");

            migrationBuilder.DropTable(
                name: "seguro_parametro");

            migrationBuilder.DropTable(
                name: "usuario");

            migrationBuilder.DropTable(
                name: "seguradora");

            migrationBuilder.DropTable(
                name: "cooperado");

            migrationBuilder.DropTable(
                name: "perfil");

            migrationBuilder.DropTable(
                name: "ponto_atendimento");

            migrationBuilder.DropTable(
                name: "agencia");
        }
    }
}
