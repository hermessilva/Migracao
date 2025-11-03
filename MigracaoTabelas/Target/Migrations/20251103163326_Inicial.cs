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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "agencia",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    codigo = table.Column<string>(type: "char(4)", nullable: false, comment: "Código único da agência"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome da agência"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data/hora da criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_agencia", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "conta_contabil",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da conta contábil"),
                    Conta = table.Column<string>(type: "longtext", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_conta_contabil", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "cooperado",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    numero_documento = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false, comment: "Documento do cooperado (CPF/CNPJ sem formatação)"),
                    tipo = table.Column<string>(type: "enum('Física','Jurídica')", nullable: false, comment: "Tipo de pessoa"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome/Razão social do cooperado"),
                    nome_fantasia = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Nome fantasia (para PJ)"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "E-mail de contato")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cooperado", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parametrizacao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição do item")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parametrizacao", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "perfil",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome do perfil (único)")
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
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome fantasia da seguradora"),
                    cnpj = table.Column<string>(type: "char(14)", nullable: false, comment: "CNPJ da seguradora"),
                    razao_social = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Razão social da seguradora"),
                    status = table.Column<string>(type: "enum('Ativo','Inativo')", nullable: false, comment: "Status da seguradora")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguradora", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição/nome da tela")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tela", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "integracao_senior",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela agencia"),
                    conta_credito = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica se é uma conta de crédito"),
                    conta_debito = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica se é uma conta de débito"),
                    conta_credito_comissao = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica se é uma conta de crédito de comissão"),
                    conta_debito_comissao = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "Indica se é uma conta de débito de comissão"),
                    status = table.Column<string>(type: "enum('Enviado', 'Erro')", nullable: false, comment: "Status integração"),
                    data_movimentacao = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data e hora da movimentação"),
                    valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor da movimentação"),
                    lancamento = table.Column<int>(type: "int", nullable: false, comment: "Número do lançamento no sistema Senior"),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição da integração")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_integracao_senior", x => x.id);
                    table.ForeignKey(
                        name: "FK_integracao_senior_agencia_agencia_id",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ponto_atendimento",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela agencia"),
                    codigo = table.Column<string>(type: "char(3)", nullable: false, comment: "Código único do ponto de atendimento"),
                    nome = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome do ponto de atendimento"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data/hora da criação do registro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ponto_atendimento", x => x.id);
                    table.ForeignKey(
                        name: "FK_ponto_atendimento_agencia_agencia_id",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                })
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "lancamento_efetivar",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela agencia"),
                    cooperado_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela cooperado"),
                    conta_corrente = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Número da conta corrente do cooperado"),
                    data_movimentacao = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora da movimentação"),
                    descricao = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição do lançamento a efetivar"),
                    valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor do lançamento"),
                    data_lancamento = table.Column<DateTime>(type: "date", nullable: true, comment: "Data prevista para o lançamento")
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parametrizacao_resposta",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    parametrizacao_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela parametrizacao"),
                    resposta = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Valor de resposta do parâmetro")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parametrizacao_resposta", x => x.id);
                    table.ForeignKey(
                        name: "FK_parametrizacao_resposta_parametrizacao_parametrizacao_id",
                        column: x => x.parametrizacao_id,
                        principalTable: "parametrizacao",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "apolice_grupo_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela agencia"),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    apolice = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Código da apólice"),
                    grupo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Código do grupo da apólice"),
                    subgrupo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "Código do subgrupo da apólice"),
                    tipo_capital = table.Column<string>(type: "enum('Fixo','Variável')", nullable: false, comment: "Tipo de capital"),
                    modalidade_unico = table.Column<string>(type: "varchar(50)", nullable: true, comment: "Valor da modalidade único"),
                    modalidade_avista = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor da modalidade à vista"),
                    modalidade_parcelado = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor da modalidade parcelado"),
                    ordem = table.Column<byte>(type: "tinyint unsigned", nullable: true, comment: "Ordem de priorização da seguradora")
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
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "condicao_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    max_meses_contrato = table.Column<short>(type: "smallint", nullable: false, comment: "Quantidade máxima de meses permitido para o contrato"),
                    max_idade = table.Column<short>(type: "smallint", nullable: false, comment: "Idade máxima do proponente"),
                    porcentagem_cobertura_morte = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de cobertura para morte"),
                    porcentagem_cobertura_invalidez = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de cobertura para invalidez"),
                    porcentagem_cobertura_perda_renda = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de cobertura para perda de renda"),
                    porcentagem_comissao_corretora = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de comissão da corretora"),
                    porcentagem_comissao_cooperativa = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Percentual de comissão da cooperativa")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_condicao_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_condicao_seguradora_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "contabilizacao_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    conta_contabil_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela conta_contabil"),
                    tipo = table.Column<string>(type: "enum('Crédito','Débito','Crédito Comissão', 'Débito Comissão')", nullable: false, comment: "Tipo de conta")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_contabilizacao_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_contabilizacao_seguradora_conta_contabil_conta_contabil_id",
                        column: x => x.conta_contabil_id,
                        principalTable: "conta_contabil",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_contabilizacao_seguradora_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "gestao_documento",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    nome_documento = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome/título do documento"),
                    versao = table.Column<short>(type: "smallint", nullable: false, comment: "Versão do documento"),
                    label = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Nome amigável do parâmetro"),
                    campo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Identificador do parâmetro"),
                    valor = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Valor de resposta do parâmetro"),
                    ordem = table.Column<int>(type: "int", nullable: false, defaultValue: 0, comment: "Ordem de exibição"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data/hora de criação")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_gestao_documento", x => x.id);
                    table.ForeignKey(
                        name: "FK_gestao_documento_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "proposta_seguradora",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguradora"),
                    descricao_sequencial = table.Column<string>(type: "longtext", nullable: false, comment: "Descrição sequencial da proposta"),
                    numero_sequencial = table.Column<string>(type: "longtext", nullable: false, comment: "Número sequencial da proposta")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_proposta_seguradora", x => x.id);
                    table.ForeignKey(
                        name: "FK_proposta_seguradora_seguradora_seguradora_id",
                        column: x => x.seguradora_id,
                        principalTable: "seguradora",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela_acao",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tela_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela tela"),
                    acao_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela ação"),
                    TelaId1 = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    AcaoId1 = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tela_acao", x => x.id);
                    table.ForeignKey(
                        name: "FK_tela_acao_acao_AcaoId1",
                        column: x => x.AcaoId1,
                        principalTable: "acao",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tela_acao_acao_acao_id",
                        column: x => x.acao_id,
                        principalTable: "acao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tela_acao_tela_TelaId1",
                        column: x => x.TelaId1,
                        principalTable: "tela",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_tela_acao_tela_tela_id",
                        column: x => x.tela_id,
                        principalTable: "tela",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "tela_acao_perfil",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tela_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela tela"),
                    acao_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela ação"),
                    perfil_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela perfil")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tela_acao_perfil", x => x.id);
                    table.ForeignKey(
                        name: "FK_tela_acao_perfil_acao_acao_id",
                        column: x => x.acao_id,
                        principalTable: "acao",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tela_acao_perfil_perfil_perfil_id",
                        column: x => x.perfil_id,
                        principalTable: "perfil",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_tela_acao_perfil_tela_tela_id",
                        column: x => x.tela_id,
                        principalTable: "tela",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "usuarios",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Login de acesso do usuário"),
                    nome = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "Nome completo do usuário"),
                    email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "E-mail do usuário"),
                    ativo = table.Column<bool>(type: "tinyint(1)", nullable: false, comment: "ENUM('Ativo', 'Inativo') — Indica o status do perfil"),
                    criado_em = table.Column<DateTime>(type: "datetime(6)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(6)", comment: "Data/hora de criação do registro"),
                    AgenciaId = table.Column<ulong>(type: "bigint unsigned", nullable: true),
                    PontoAtendimentoId = table.Column<ulong>(type: "bigint unsigned", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_usuarios", x => x.id);
                    table.ForeignKey(
                        name: "FK_usuarios_agencia_AgenciaId",
                        column: x => x.AgenciaId,
                        principalTable: "agencia",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_usuarios_ponto_atendimento_PontoAtendimentoId",
                        column: x => x.PontoAtendimentoId,
                        principalTable: "ponto_atendimento",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "limite",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    condicao_seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela condicao_seguradora"),
                    idade_inicial = table.Column<short>(type: "smallint", nullable: false, comment: "Idade mínima para aplicação do limite"),
                    idade_final = table.Column<short>(type: "smallint", nullable: false, comment: "Idade máxima para aplicação do limite"),
                    valor = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor máximo permitido para o limite"),
                    coeficiente = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Coeficiente aplicado ao cálculo do limite"),
                    limite_dps = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor limite para exigência de DPS"),
                    descricao_regra = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Descrição das regras de aplicação do limite")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_limite", x => x.id);
                    table.ForeignKey(
                        name: "FK_limite_condicao_seguradora_condicao_seguradora_id",
                        column: x => x.condicao_seguradora_id,
                        principalTable: "condicao_seguradora",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "auditoria",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela usuario"),
                    agencia_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela agencia"),
                    modulo = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Módulo/área do sistema afetado"),
                    operacao = table.Column<string>(type: "enum('Insert','Delete','Update')", nullable: false, comment: "Tipo da operação"),
                    antes = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Payload de dados antes da operação"),
                    depois = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "Payload de dados depois da alteração"),
                    criado_em = table.Column<DateTime>(type: "datetime", nullable: false, comment: "Data/hora da ação registrada")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_auditoria", x => x.id);
                    table.ForeignKey(
                        name: "FK_auditoria_agencia_agencia_id",
                        column: x => x.agencia_id,
                        principalTable: "agencia",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_auditoria_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "seguro",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    agencia_seguradora_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira na tabela agencia_seguradora"),
                    cooperado_agencia_conta_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira na tabela cooperado_agencia_conta"),
                    ponto_atendimento_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira na tabela ponto de atendimento"),
                    usuario_id = table.Column<ulong>(type: "bigint unsigned", nullable: true, comment: "Chave estrangeira na tabela usuario"),
                    status = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "Identificador do status (ex.: 1=aberto, 2=quitado, 3=cancelado)"),
                    contrato = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, comment: "Número do contrato do seguro"),
                    inicio_vigencia = table.Column<DateTime>(type: "date", nullable: true, comment: "Início de vigência do seguro"),
                    fim_vigencia = table.Column<DateTime>(type: "date", nullable: true, comment: "Fim de vigência do seguro"),
                    codigo_grupo = table.Column<int>(type: "int", nullable: false, comment: "Identificador do grupo (código) do contrato"),
                    quantidade_parcelas = table.Column<short>(type: "smallint", nullable: false, comment: "Quantidade total de parcelas do seguro"),
                    vencimento = table.Column<DateTime>(type: "date", nullable: true, comment: "Data de vencimento (padrão do contrato ou próxima parcela)"),
                    capital_segurado = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor do capital segurado"),
                    premio_total = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor do prêmio total do seguro"),
                    tipo_pagamento = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "Identificador do tipo de pagamento (ex.: 1=à vista, 2=parcelado)"),
                    estorno_proporcional = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor de estorno proporcional quando aplicável"),
                    valor_base = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor Base Segurado"),
                    dps = table.Column<bool>(type: "tinyint(1)", nullable: true, comment: "Informação de Exigência de DPS"),
                    valor_iof = table.Column<decimal>(type: "decimal(10,2)", nullable: true, comment: "Valor de IOF")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguro", x => x.id);
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
                        name: "FK_seguro_usuarios_usuario_id",
                        column: x => x.usuario_id,
                        principalTable: "usuarios",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "parcela",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    seguro_id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Chave estrangeira da tabela seguro"),
                    status = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "Status da parcela (1 - Pendente, 2 - Pago, 3 - Cancelado)"),
                    numero_parcela = table.Column<short>(type: "smallint", nullable: false, comment: "Número sequencial da parcela"),
                    valor_parcela = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor original da parcela"),
                    valor_pago = table.Column<decimal>(type: "decimal(10,2)", nullable: false, comment: "Valor efetivamente pago da parcela"),
                    vencimento = table.Column<DateTime>(type: "date", nullable: false, comment: "Data de vencimento da parcela"),
                    liquidacao = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora da liquidação da parcela"),
                    data_ultimo_pagamento = table.Column<DateTime>(type: "datetime", nullable: true, comment: "Data e hora do último pagamento realizado")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parcela", x => x.id);
                    table.ForeignKey(
                        name: "FK_parcela_seguro_seguro_id",
                        column: x => x.seguro_id,
                        principalTable: "seguro",
                        principalColumn: "id");
                })
                .Annotation("MySQL:Charset", "utf8mb4");

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
                name: "IX_apolice_grupo_seguradora_agencia_id",
                table: "apolice_grupo_seguradora",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_apolice_grupo_seguradora_seguradora_id",
                table: "apolice_grupo_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_auditoria_agencia_id",
                table: "auditoria",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_auditoria_usuario_id",
                table: "auditoria",
                column: "usuario_id");

            migrationBuilder.CreateIndex(
                name: "IX_condicao_seguradora_seguradora_id",
                table: "condicao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_contabilizacao_seguradora_conta_contabil_id",
                table: "contabilizacao_seguradora",
                column: "conta_contabil_id");

            migrationBuilder.CreateIndex(
                name: "IX_contabilizacao_seguradora_seguradora_id",
                table: "contabilizacao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_contabilizacao_seguradora_seguradora_id_conta_contabil_id",
                table: "contabilizacao_seguradora",
                columns: new[] { "seguradora_id", "conta_contabil_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_cooperado_agencia_conta_agencia_id",
                table: "cooperado_agencia_conta",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_cooperado_agencia_conta_conta_corrente",
                table: "cooperado_agencia_conta",
                column: "conta_corrente");

            migrationBuilder.CreateIndex(
                name: "IX_cooperado_agencia_conta_cooperado_id",
                table: "cooperado_agencia_conta",
                column: "cooperado_id");

            migrationBuilder.CreateIndex(
                name: "IX_cooperado_agencia_conta_cooperado_id_agencia_id_conta_corren~",
                table: "cooperado_agencia_conta",
                columns: new[] { "cooperado_id", "agencia_id", "conta_corrente" },
                unique: true);

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
                name: "IX_limite_condicao_seguradora_id",
                table: "limite",
                column: "condicao_seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_parametrizacao_resposta_parametrizacao_id",
                table: "parametrizacao_resposta",
                column: "parametrizacao_id");

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
                name: "IX_ponto_atendimento_agencia_id",
                table: "ponto_atendimento",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_ponto_atendimento_agencia_id_codigo",
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
                name: "idx_tela_acao_acao_id",
                table: "tela_acao",
                column: "acao_id");

            migrationBuilder.CreateIndex(
                name: "idx_tela_acao_tela_id",
                table: "tela_acao",
                column: "tela_id");

            migrationBuilder.CreateIndex(
                name: "IX_tela_acao_AcaoId1",
                table: "tela_acao",
                column: "AcaoId1");

            migrationBuilder.CreateIndex(
                name: "IX_tela_acao_TelaId1",
                table: "tela_acao",
                column: "TelaId1");

            migrationBuilder.CreateIndex(
                name: "idx_tela_acao_perfil_acao_id",
                table: "tela_acao_perfil",
                column: "acao_id");

            migrationBuilder.CreateIndex(
                name: "idx_tela_acao_perfil_perfil_id",
                table: "tela_acao_perfil",
                column: "perfil_id");

            migrationBuilder.CreateIndex(
                name: "idx_tela_acao_perfil_tela_id",
                table: "tela_acao_perfil",
                column: "tela_id");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_AgenciaId",
                table: "usuarios",
                column: "AgenciaId");

            migrationBuilder.CreateIndex(
                name: "IX_usuarios_PontoAtendimentoId",
                table: "usuarios",
                column: "PontoAtendimentoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "apolice_grupo_seguradora");

            migrationBuilder.DropTable(
                name: "auditoria");

            migrationBuilder.DropTable(
                name: "contabilizacao_seguradora");

            migrationBuilder.DropTable(
                name: "gestao_documento");

            migrationBuilder.DropTable(
                name: "integracao_senior");

            migrationBuilder.DropTable(
                name: "lancamento_efetivar");

            migrationBuilder.DropTable(
                name: "limite");

            migrationBuilder.DropTable(
                name: "parametrizacao_resposta");

            migrationBuilder.DropTable(
                name: "parcela");

            migrationBuilder.DropTable(
                name: "proposta_seguradora");

            migrationBuilder.DropTable(
                name: "tela_acao");

            migrationBuilder.DropTable(
                name: "tela_acao_perfil");

            migrationBuilder.DropTable(
                name: "conta_contabil");

            migrationBuilder.DropTable(
                name: "condicao_seguradora");

            migrationBuilder.DropTable(
                name: "parametrizacao");

            migrationBuilder.DropTable(
                name: "seguro");

            migrationBuilder.DropTable(
                name: "acao");

            migrationBuilder.DropTable(
                name: "perfil");

            migrationBuilder.DropTable(
                name: "tela");

            migrationBuilder.DropTable(
                name: "seguradora");

            migrationBuilder.DropTable(
                name: "cooperado_agencia_conta");

            migrationBuilder.DropTable(
                name: "usuarios");

            migrationBuilder.DropTable(
                name: "cooperado");

            migrationBuilder.DropTable(
                name: "ponto_atendimento");

            migrationBuilder.DropTable(
                name: "agencia");
        }
    }
}
