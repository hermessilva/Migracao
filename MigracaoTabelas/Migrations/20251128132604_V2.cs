using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace MigracaoTabelas.Migrations
{
    /// <inheritdoc />
    public partial class V2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apolice_grupo_seguradora_agencia_agencia_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropForeignKey(
                name: "FK_apolice_grupo_seguradora_seguradora_seguradora_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropForeignKey(
                name: "FK_parametrizacao_resposta_parametrizacao_parametrizacao_id",
                table: "parametrizacao_resposta");

            migrationBuilder.DropForeignKey(
                name: "FK_seguro_agencia_seguradora_agencia_seguradora_id",
                table: "seguro");

            migrationBuilder.DropIndex(
                name: "IX_seguro_agencia_seguradora_id",
                table: "seguro");

            migrationBuilder.DropIndex(
                name: "ponto_atendimento_index_1",
                table: "ponto_atendimento");

            migrationBuilder.DropIndex(
                name: "cooperado_agencia_conta_index_8",
                table: "cooperado_agencia_conta");

            migrationBuilder.DropIndex(
                name: "IX_contabilizacao_seguradora_seguradora_id",
                table: "contabilizacao_seguradora");

            migrationBuilder.DropIndex(
                name: "IX_conta_corrente_seguradora_seguradora_id",
                table: "conta_corrente_seguradora");

            migrationBuilder.DropIndex(
                name: "IX_condicao_seguradora_seguradora_id",
                table: "condicao_seguradora");

            migrationBuilder.DropIndex(
                name: "IX_comissao_seguradora_seguradora_id",
                table: "comissao_seguradora");

            migrationBuilder.DropIndex(
                name: "IX_apolice_grupo_seguradora_agencia_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropIndex(
                name: "IX_apolice_grupo_seguradora_seguradora_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropColumn(
                name: "agencia_seguradora_id",
                table: "seguro");

            migrationBuilder.DropColumn(
                name: "depois",
                table: "auditoria");

            migrationBuilder.DropColumn(
                name: "Ordem",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropColumn(
                name: "agencia_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropColumn(
                name: "seguradora_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.RenameIndex(
                name: "cooperado_agencia_conta_index_9",
                table: "cooperado_agencia_conta",
                newName: "IX_cooperado_agencia_conta_agencia_id");

            migrationBuilder.RenameIndex(
                name: "cooperado_agencia_conta_index_7",
                table: "cooperado_agencia_conta",
                newName: "cooperado_agencia_conta_index_6");

            migrationBuilder.RenameIndex(
                name: "cooperado_agencia_conta_index_10",
                table: "cooperado_agencia_conta",
                newName: "cooperado_agencia_conta_index_7");

            migrationBuilder.AlterTable(
                name: "usuario",
                comment: "Tabela de usuários do sistema com suas credenciais e vínculos organizacionais")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "tela",
                comment: "Catálogo de telas (módulos/páginas) disponíveis no sistema")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "seguradora_limite",
                comment: "Define faixas etárias, coeficientes e limites de DPS por seguradora para cálculo de prêmios")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "seguradora",
                comment: "Armazena os dados cadastrais das seguradoras parceiras")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "proposta_seguradora",
                comment: "Controle de numeração sequencial de propostas por seguradora")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "ponto_atendimento",
                comment: "Armazena informações dos pontos de atendimento (PAs) vinculados a cada agência")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "parcela",
                comment: "Parcelas financeiras de prêmio vinculadas a um contrato de seguro",
                oldComment: "Parcelas financeiras vinculadas a um seguro")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "parametrizacao_resposta",
                comment: "Opções de resposta disponíveis para cada campo de parametrização",
                oldComment: "Resposta dos campos de parametrização")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "lancamento_efetivar",
                comment: "Fila de lançamentos financeiros pendentes de efetivação nas contas dos cooperados",
                oldComment: "Lançamentos financeiros que serão/são efetivados")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "integracao_senior",
                comment: "Fila de controle de integrações contábeis com o sistema Sênior (ERP)")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "gestao_documento",
                comment: "Gestão de templates e campos de documentos por seguradora para geração automática",
                oldComment: "Gestão de documentos")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "cooperado",
                comment: "Cadastro de cooperados (clientes) da cooperativa que podem contratar seguros",
                oldComment: "Cadastro de cooperados vinculados a uma agência")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "condicao_seguradora",
                comment: "Parâmetros de condições operacionais e financeiras aplicados por seguradora")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "auditoria",
                comment: "Tabela de auditoria para rastreamento de todas as operações realizadas no sistema")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "apolice_grupo_seguradora",
                comment: "Configurações de apólices e grupos por vínculo agência-seguradora")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "agencia_seguradora",
                comment: "Tabela de vínculo que relaciona agências com seguradoras autorizadas e define prioridade")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "agencia",
                comment: "Armazena informações cadastrais das agências da cooperativa")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "usuario",
                type: "enum('Ativo','Inativo')",
                nullable: false,
                comment: "Status do usuário: Ativo ou Inativo",
                oldClrType: typeof(string),
                oldType: "enum('Ativo','Inativo')",
                oldComment: "Indica o status do usuário");

            migrationBuilder.AlterColumn<ulong>(
                name: "ponto_atendimento_id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela ponto_atendimento",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira para Ponto de Atendimento");

            migrationBuilder.AlterColumn<ulong>(
                name: "perfil_id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: true,
                comment: "Chave estrangeira opcional referenciando a tabela perfil para o perfil principal do usuário",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira para a Perfil ");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "usuario",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome completo do usuário",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "Nome completo do usuário");

            migrationBuilder.AlterColumn<string>(
                name: "login",
                table: "usuario",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Login de acesso do usuário ao sistema",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldComment: "Login de acesso do usuário");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "usuario",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Endereço de e-mail do usuário para contato e notificações",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "E-mail do usuário");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "usuario",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data e hora de criação do registro",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data/hora de criação do registro");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira para a Agência");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "tela",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Identificador amigável da tela para uso em URLs e código",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição amigavel da tela");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "tela",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome ou descrição completa da tela",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição/nome da tela");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "tela",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "vencimento",
                table: "seguro",
                type: "date",
                nullable: true,
                comment: "Data de vencimento base do contrato ou da próxima parcela",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Data de vencimento (padrão do contrato ou próxima parcela)");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_iof",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor do IOF (Imposto sobre Operações Financeiras) incidente sobre o prêmio",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor de IOF");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_base",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor base utilizado para cálculo do seguro (saldo devedor ou valor financiado)",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor Base Segurado");

            migrationBuilder.AlterColumn<ulong>(
                name: "usuario_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: true,
                comment: "Chave estrangeira referenciando a tabela usuario responsável pela contratação",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldNullable: true,
                oldComment: "Chave estrangeira na tabela usuario");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_pagamento",
                table: "seguro",
                type: "enum('À Vista','Parcelado','Único')",
                nullable: false,
                comment: "Modalidade de pagamento: À Vista, Parcelado ou Único",
                oldClrType: typeof(string),
                oldType: "enum('À Vista','Parcelado','Único')",
                oldComment: "Identificador do tipo de pagamento (1=à vista, 2=parcelado, 3=Única)");

            migrationBuilder.AlterColumn<decimal>(
                name: "premio_total",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor total do prêmio do seguro a ser pago",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor do prêmio total do seguro");

            migrationBuilder.AlterColumn<ulong>(
                name: "ponto_atendimento_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela ponto_atendimento onde o seguro foi contratado",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira na tabela ponto de atendimento");

            migrationBuilder.AlterColumn<DateTime>(
                name: "inicio_vigencia",
                table: "seguro",
                type: "date",
                nullable: true,
                comment: "Data de início da vigência do seguro",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Início de vigência do seguro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fim_vigencia",
                table: "seguro",
                type: "date",
                nullable: true,
                comment: "Data de término da vigência do seguro",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Fim de vigência do seguro");

            migrationBuilder.AlterColumn<decimal>(
                name: "estorno_proporcional",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor de estorno proporcional em caso de cancelamento",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor de estorno proporcional quando aplicável");

            migrationBuilder.AlterColumn<bool>(
                name: "dps",
                table: "seguro",
                type: "tinyint(1)",
                nullable: true,
                comment: "Indica se foi exigida Declaração Pessoal de Saúde (true/false)",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true,
                oldComment: "Informação de Exigência de DPS");

            migrationBuilder.AlterColumn<ulong>(
                name: "cooperado_agencia_conta_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela cooperado_agencia_conta",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira na tabela cooperado_agencia_conta");

            migrationBuilder.AlterColumn<string>(
                name: "contrato",
                table: "seguro",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                comment: "Número do contrato de crédito vinculado ao seguro",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldComment: "Número do contrato do seguro");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_grupo",
                table: "seguro",
                type: "int",
                nullable: false,
                comment: "Código identificador do grupo/produto do seguro",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Identificador do grupo (código) do contrato");

            migrationBuilder.AlterColumn<decimal>(
                name: "capital_segurado",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor total do capital segurado (valor coberto em caso de sinistro)",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor do capital segurado");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<ulong>(
                name: "apolice_grupo_seguradora_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                comment: "Chave estrangeira referenciando a tabela apolice_grupo_seguradora indicando a apolice contratada");

            migrationBuilder.AddColumn<ulong>(
                name: "seguro_parametro_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                comment: "Chave estrangeira referenciando a tabela seguro_parametro com os parâmetros de cálculo");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_maximo",
                table: "seguradora_limite",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor máximo de capital segurado permitido para a faixa",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor associado à faixa");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "seguradora_limite",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela seguradora");

            migrationBuilder.AlterColumn<decimal>(
                name: "limite_dps",
                table: "seguradora_limite",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor limite de capital segurado que exige Declaração Pessoal de Saúde (DPS)",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor limite de exigibilidade para DPS");

            migrationBuilder.AlterColumn<short>(
                name: "idade_inicial",
                table: "seguradora_limite",
                type: "smallint",
                nullable: false,
                comment: "Idade inicial da faixa etária para aplicação da regra",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Idade inicial da faixa");

            migrationBuilder.AlterColumn<short>(
                name: "idade_final",
                table: "seguradora_limite",
                type: "smallint",
                nullable: false,
                comment: "Idade final da faixa etária para aplicação da regra",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Idade final da faixa");

            migrationBuilder.AlterColumn<string>(
                name: "descricao_regra",
                table: "seguradora_limite",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição textual detalhada da regra aplicada para o limite e DPS",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição detalhada da regra para o limite DPS");

            migrationBuilder.AlterColumn<decimal>(
                name: "coeficiente",
                table: "seguradora_limite",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Coeficiente multiplicador para cálculo do prêmio",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Coeficiente aplicado para cálculos");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "seguradora_limite",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "seguradora",
                type: "enum('Ativo','Inativo')",
                nullable: false,
                comment: "Status atual da seguradora: Ativo ou Inativo",
                oldClrType: typeof(string),
                oldType: "enum('Ativo','Inativo')",
                oldComment: "Status da seguradora");

            migrationBuilder.AlterColumn<string>(
                name: "razao_social",
                table: "seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Razão social completa da seguradora",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Razão social da seguradora");

            migrationBuilder.AlterColumn<string>(
                name: "cnpj",
                table: "seguradora",
                type: "char(14)",
                nullable: false,
                comment: "CNPJ da seguradora sem formatação (apenas números)",
                oldClrType: typeof(string),
                oldType: "char(14)",
                oldComment: "CNPJ da seguradora");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "proposta_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela seguradora");

            migrationBuilder.AlterColumn<string>(
                name: "numero_sequencial",
                table: "proposta_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Último número sequencial utilizado para geração de propostas",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "Número sequencial da proposta");

            migrationBuilder.AlterColumn<string>(
                name: "descricao_sequencial",
                table: "proposta_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Descrição ou prefixo do formato do número sequencial da proposta",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "Descrição sequencial da proposta");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "proposta_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "ponto_atendimento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome completo do ponto de atendimento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome do ponto de atendimento");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "ponto_atendimento",
                type: "datetime(6)",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data e hora de criação do registro",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data/hora da criação do registro");

            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                table: "ponto_atendimento",
                type: "char(3)",
                nullable: false,
                comment: "Código do ponto de atendimento no formato de 3 caracteres, único dentro de uma agência",
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldComment: "Código único do ponto de atendimento");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "ponto_atendimento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "ponto_atendimento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "perfil",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Identificador amigável do perfil para uso em URLs e código",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome amigavel do perfil");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "perfil",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome descritivo do perfil de acesso",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome do perfil");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_parcela",
                table: "parcela",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor nominal atual da parcela a ser cobrado",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor nominal da parcela");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_pago",
                table: "parcela",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor total efetivamente pago na parcela",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor total pago na parcela");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "parcela",
                type: "enum('Em Aberto','Pago','Cancelada')",
                nullable: false,
                comment: "Status atual da parcela conforme enum status_seguro",
                oldClrType: typeof(string),
                oldType: "enum('Em Aberto','Pago','Cancelada')",
                oldComment: "Identificador do status da parcela");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguro_id",
                table: "parcela",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela seguro",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela seguro");

            migrationBuilder.AlterColumn<short>(
                name: "numero_parcela",
                table: "parcela",
                type: "smallint",
                nullable: false,
                comment: "Número sequencial da parcela dentro do seguro (1, 2, 3...)",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Número sequencial da parcela dentro do seguro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "liquidacao",
                table: "parcela",
                type: "datetime",
                nullable: true,
                comment: "Data e hora de liquidação/quitação da parcela",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data de liquidação da parcela");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_ultimo_pagamento",
                table: "parcela",
                type: "datetime",
                nullable: true,
                comment: "Data e hora do último pagamento parcial ou total registrado",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data do último pagamento registrado");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "parcela",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "valor_original",
                table: "parcela",
                type: "decimal(10,2)",
                nullable: false,
                defaultValue: 0m,
                comment: "Valor original da parcela calculado na contratação");

            migrationBuilder.AlterColumn<string>(
                name: "resposta",
                table: "parametrizacao_resposta",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Valor de resposta ou opção disponível para o campo de parametrização",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Valor de resposta do parâmetro");

            migrationBuilder.AlterColumn<ulong>(
                name: "parametrizacao_id",
                table: "parametrizacao_resposta",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela parametrizacao",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela parametrizacao");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "parametrizacao_resposta",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "lancamento_efetivar",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor monetário do lançamento a ser efetivado",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor do lançamento");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "lancamento_efetivar",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição detalhada do lançamento a ser efetivado",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição do lançamento");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_movimentacao",
                table: "lancamento_efetivar",
                type: "datetime",
                nullable: true,
                comment: "Data e hora da movimentação financeira no sistema de origem",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data de movimentação financeira");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_lancamento",
                table: "lancamento_efetivar",
                type: "date",
                nullable: true,
                comment: "Data programada para efetivação do lançamento no sistema",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Data de lançamento/registro no sistema");

            migrationBuilder.AlterColumn<ulong>(
                name: "cooperado_id",
                table: "lancamento_efetivar",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela cooperado",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela cooperado");

            migrationBuilder.AlterColumn<string>(
                name: "conta_corrente",
                table: "lancamento_efetivar",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Número da conta corrente do cooperado para débito/crédito do lançamento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Conta corrente associada ao lançamento");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "lancamento_efetivar",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "lancamento_efetivar",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "integracao_senior",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor monetário do lançamento a ser integrado",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor da movimentação");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_lancamento_contabil",
                table: "integracao_senior",
                type: "enum('Seguro Prestamista Contratado','Comissão Seguro Prestamista Contratado','Cancelamento Seguro Prestamista Parcelado','Cancelamento Seguro Prestamista Parcelado Comissão','Cancelamento Seguro Prestamista À Vista Proporcional Comissão','Pagamento Seguro Prestamista','Recebimento Comissão Seguro Prestamista')",
                nullable: false,
                comment: "Tipo do lançamento contábil conforme enum tipo_lancamento",
                oldClrType: typeof(string),
                oldType: "enum('Seguro Prestamista Contratado', 'Comissão Seguro Prestamista Contratado', 'Cancelamento Seguro Prestamista Parcelado Comissão', 'Cancelamento Seguro Prestamista À Vista Proporcional Comissão', 'Pagamento Seguro Prestamista', 'Recebimento Comissão Seguro Prestamista')",
                oldComment: "Tipo de lançamento contábil");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "integracao_senior",
                type: "enum('Enviado','Falha')",
                nullable: false,
                comment: "Status da integração: Enviado (sucesso) ou Falha (erro no envio)",
                oldClrType: typeof(string),
                oldType: "enum('Enviado', 'Falha')",
                oldComment: "Status integração");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "integracao_senior",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição detalhada do lançamento para identificação",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição da integração");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_movimentacao",
                table: "integracao_senior",
                type: "datetime",
                nullable: false,
                comment: "Data e hora da movimentação a ser integrada",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldComment: "Data e hora da movimentação");

            migrationBuilder.AlterColumn<string>(
                name: "conta_contabil_debito",
                table: "integracao_senior",
                type: "varchar(255)",
                nullable: false,
                comment: "Código da conta contábil de débito para o lançamento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "Conta contábil de débito");

            migrationBuilder.AlterColumn<string>(
                name: "conta_contabil_credito",
                table: "integracao_senior",
                type: "varchar(255)",
                nullable: false,
                comment: "Código da conta contábil de crédito para o lançamento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "Conta contábil de crédito");

            migrationBuilder.AlterColumn<string>(
                name: "codigo_pa",
                table: "integracao_senior",
                type: "char(3)",
                nullable: false,
                comment: "Código do ponto de atendimento de origem do lançamento",
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldComment: "Código único do ponto de atendimento");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "integracao_senior",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "integracao_senior",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<int>(
                name: "numero_lancamento",
                table: "integracao_senior",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Número sequencial do lançamento no sistema de origem");

            migrationBuilder.AlterColumn<short>(
                name: "versao",
                table: "gestao_documento",
                type: "smallint",
                nullable: false,
                comment: "Número da versão do documento para controle de alterações",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Versão do documento");

            migrationBuilder.AlterColumn<string>(
                name: "valor",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Valor padrão ou resposta configurada para o campo",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Valor de resposta do parâmetro");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "gestao_documento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela seguradora");

            migrationBuilder.AlterColumn<int>(
                name: "ordem",
                table: "gestao_documento",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Ordem de exibição do campo no documento",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0,
                oldComment: "Ordem de exibição");

            migrationBuilder.AlterColumn<string>(
                name: "nome_documento",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome ou título do documento a ser gerado",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome/título do documento");

            migrationBuilder.AlterColumn<string>(
                name: "label",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Rótulo amigável do campo para exibição ao usuário",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome amigável do parâmetro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "gestao_documento",
                type: "datetime(6)",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data e hora de criação do registro",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data/hora de criação");

            migrationBuilder.AlterColumn<string>(
                name: "campo",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Identificador técnico do campo no documento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Identificador do parâmetro");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "gestao_documento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "cooperado",
                type: "enum('Física','Jurídica')",
                nullable: false,
                comment: "Tipo de pessoa: Física (CPF) ou Jurídica (CNPJ)",
                oldClrType: typeof(string),
                oldType: "enum('Física','Jurídica')",
                oldComment: "Tipo de pessoa");

            migrationBuilder.AlterColumn<string>(
                name: "numero_documento",
                table: "cooperado",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                comment: "Documento de identificação do cooperado (CPF com 11 dígitos ou CNPJ com 14 dígitos, sem formatação)",
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14,
                oldComment: "Documento do cooperado (CPF/CNPJ sem formatação)");

            migrationBuilder.AlterColumn<string>(
                name: "nome_fantasia",
                table: "cooperado",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Nome fantasia do cooperado (aplicável apenas para pessoa jurídica)",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Nome fantasia (para PJ)");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "cooperado",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome completo (pessoa física) ou razão social (pessoa jurídica) do cooperado",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome/Razão social do cooperado");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "cooperado",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Endereço de e-mail para contato e comunicações com o cooperado",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "E-mail de contato");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "cooperado",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "condicao_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela seguradora");

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentagem_cobertura_perda_renda",
                table: "condicao_seguradora",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Percentual de cobertura para sinistro por perda de renda (ex: 0.3000 = 30%)",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Percentual de cobertura para perda de renda");

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentagem_cobertura_morte",
                table: "condicao_seguradora",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Percentual de cobertura para sinistro por morte (ex: 1.0000 = 100%)",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Percentual de cobertura para morte");

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentagem_cobertura_invalidez",
                table: "condicao_seguradora",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Percentual de cobertura para sinistro por invalidez (ex: 0.5000 = 50%)",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Percentual de cobertura para invalidez");

            migrationBuilder.AlterColumn<bool>(
                name: "periodicidade_30dias",
                table: "condicao_seguradora",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false,
                comment: "Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldComment: "Indica se a periodicidade de vencimento é mensal ou a cada 30 dias");

            migrationBuilder.AlterColumn<short>(
                name: "max_meses_contrato",
                table: "condicao_seguradora",
                type: "smallint",
                nullable: false,
                comment: "Quantidade máxima de meses permitidos para vigência do contrato",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Quantidade máxima de meses permitidos para o contrato");

            migrationBuilder.AlterColumn<short>(
                name: "max_idade",
                table: "condicao_seguradora",
                type: "smallint",
                nullable: false,
                comment: "Idade máxima permitida do proponente para contratação do seguro",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Idade máxima do proponente");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "condicao_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "usuario_id",
                table: "auditoria",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela usuario que realizou a ação",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela usuario");

            migrationBuilder.AlterColumn<string>(
                name: "operacao",
                table: "auditoria",
                type: "enum('Insert','Delete','Update')",
                nullable: false,
                comment: "Tipo da operação realizada: Insert (inserção), Delete (exclusão) ou Update (atualização)",
                oldClrType: typeof(string),
                oldType: "enum('Insert','Delete','Update')",
                oldComment: "Tipo da operação");

            migrationBuilder.AlterColumn<string>(
                name: "modulo",
                table: "auditoria",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome do módulo ou área do sistema onde a operação foi realizada",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Módulo/área do sistema afetado");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "auditoria",
                type: "datetime",
                nullable: true,
                comment: "Data e hora em que a ação foi registrada",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data/hora da ação registrada");

            migrationBuilder.AlterColumn<string>(
                name: "antes",
                table: "auditoria",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Dados do registro antes da alteração em formato JSON ou serializado",
                oldClrType: typeof(string),
                oldType: "longtext",
                oldComment: "Payload de dados antes da operação");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "auditoria",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela agencia onde a ação foi realizada",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "auditoria",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "tipo_capital",
                table: "apolice_grupo_seguradora",
                type: "enum('Fixo','Variável')",
                nullable: false,
                comment: "Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)",
                oldClrType: typeof(string),
                oldType: "enum('Fixo','Variável')",
                oldComment: "Tipo de capital");

            migrationBuilder.AlterColumn<string>(
                name: "subgrupo",
                table: "apolice_grupo_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Código do subgrupo dentro do grupo da apólice",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Código do subgrupo da apólice");

            migrationBuilder.AlterColumn<string>(
                name: "modalidade_unico",
                table: "apolice_grupo_seguradora",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "Identificador ou código da modalidade de pagamento único",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Valor modalidade unico");

            migrationBuilder.AlterColumn<decimal>(
                name: "modalidade_parcelado",
                table: "apolice_grupo_seguradora",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor ou taxa para modalidade de pagamento parcelado",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor modalidade parcelado");

            migrationBuilder.AlterColumn<decimal>(
                name: "modalidade_avista",
                table: "apolice_grupo_seguradora",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor ou taxa para modalidade de pagamento à vista",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor modalidade avista");

            migrationBuilder.AlterColumn<string>(
                name: "grupo",
                table: "apolice_grupo_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Código do grupo dentro da apólice",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Código do grupo da apólice");

            migrationBuilder.AlterColumn<string>(
                name: "apolice",
                table: "apolice_grupo_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Número ou código da apólice contratada",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Código da apólice");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "apolice_grupo_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<ulong>(
                name: "agencia_seguradora_id",
                table: "apolice_grupo_seguradora",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                comment: "Chave estrangeira referenciando a tabela agencia_seguradora");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "agencia_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela seguradora");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "agencia_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira referenciando a tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira da tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "agencia_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<sbyte>(
                name: "ordem",
                table: "agencia_seguradora",
                type: "tinyint",
                nullable: false,
                defaultValue: (sbyte)0,
                comment: "Ordem de prioridade da seguradora dentro da agência (menor = maior prioridade)");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "agencia",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome completo da agência",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome da agência");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "agencia",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data e hora de criação do registro",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data/hora da criação do registro");

            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                table: "agencia",
                type: "char(4)",
                nullable: false,
                comment: "Código único da agência no formato de 4 caracteres",
                oldClrType: typeof(string),
                oldType: "char(4)",
                oldComment: "Código único da agência");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "agencia",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador único do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateTable(
                name: "seguro_parametro",
                columns: table => new
                {
                    id = table.Column<ulong>(type: "bigint unsigned", nullable: false, comment: "Identificador único do registro na tabela")
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    tipo_capital = table.Column<string>(type: "enum('Fixo','Variável')", nullable: false, comment: "Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)"),
                    periodicidade_30dias = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false, comment: "Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)"),
                    coeficiente = table.Column<decimal>(type: "decimal(5,4)", nullable: false, comment: "Coeficiente multiplicador utilizado para cálculo do prêmio e estornos")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_seguro_parametro", x => x.id);
                },
                comment: "Parâmetros de contratação do seguro utilizados para cálculos de parcelas, prêmios e cancelamentos")
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_tela_slug",
                table: "tela",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_seguro_apolice_grupo_seguradora_id",
                table: "seguro",
                column: "apolice_grupo_seguradora_id");

            migrationBuilder.CreateIndex(
                name: "seguro_index_8",
                table: "seguro",
                column: "seguro_parametro_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_perfil_slug",
                table: "perfil",
                column: "slug",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "contabilizacao_seguradora_index_3",
                table: "contabilizacao_seguradora",
                column: "seguradora_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "conta_corrente_seguradora_index_4",
                table: "conta_corrente_seguradora",
                column: "seguradora_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "condicao_seguradora_index_1",
                table: "condicao_seguradora",
                column: "seguradora_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "comissao_seguradora_index_5",
                table: "comissao_seguradora",
                column: "seguradora_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_apolice_grupo_seguradora_agencia_seguradora_id",
                table: "apolice_grupo_seguradora",
                column: "agencia_seguradora_id");

            migrationBuilder.AddForeignKey(
                name: "FK_apolice_grupo_seguradora_agencia_seguradora_agencia_segurado~",
                table: "apolice_grupo_seguradora",
                column: "agencia_seguradora_id",
                principalTable: "agencia_seguradora",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_parametrizacao_resposta_parametrizacao_parametrizacao_id",
                table: "parametrizacao_resposta",
                column: "parametrizacao_id",
                principalTable: "parametrizacao",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_seguro_apolice_grupo_seguradora_apolice_grupo_seguradora_id",
                table: "seguro",
                column: "apolice_grupo_seguradora_id",
                principalTable: "apolice_grupo_seguradora",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_seguro_seguro_parametro_seguro_parametro_id",
                table: "seguro",
                column: "seguro_parametro_id",
                principalTable: "seguro_parametro",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_apolice_grupo_seguradora_agencia_seguradora_agencia_segurado~",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropForeignKey(
                name: "FK_parametrizacao_resposta_parametrizacao_parametrizacao_id",
                table: "parametrizacao_resposta");

            migrationBuilder.DropForeignKey(
                name: "FK_seguro_apolice_grupo_seguradora_apolice_grupo_seguradora_id",
                table: "seguro");

            migrationBuilder.DropForeignKey(
                name: "FK_seguro_seguro_parametro_seguro_parametro_id",
                table: "seguro");

            migrationBuilder.DropTable(
                name: "seguro_parametro");

            migrationBuilder.DropIndex(
                name: "IX_tela_slug",
                table: "tela");

            migrationBuilder.DropIndex(
                name: "IX_seguro_apolice_grupo_seguradora_id",
                table: "seguro");

            migrationBuilder.DropIndex(
                name: "seguro_index_8",
                table: "seguro");

            migrationBuilder.DropIndex(
                name: "IX_perfil_slug",
                table: "perfil");

            migrationBuilder.DropIndex(
                name: "contabilizacao_seguradora_index_3",
                table: "contabilizacao_seguradora");

            migrationBuilder.DropIndex(
                name: "conta_corrente_seguradora_index_4",
                table: "conta_corrente_seguradora");

            migrationBuilder.DropIndex(
                name: "condicao_seguradora_index_1",
                table: "condicao_seguradora");

            migrationBuilder.DropIndex(
                name: "comissao_seguradora_index_5",
                table: "comissao_seguradora");

            migrationBuilder.DropIndex(
                name: "IX_apolice_grupo_seguradora_agencia_seguradora_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropColumn(
                name: "apolice_grupo_seguradora_id",
                table: "seguro");

            migrationBuilder.DropColumn(
                name: "seguro_parametro_id",
                table: "seguro");

            migrationBuilder.DropColumn(
                name: "valor_original",
                table: "parcela");

            migrationBuilder.DropColumn(
                name: "numero_lancamento",
                table: "integracao_senior");

            migrationBuilder.DropColumn(
                name: "agencia_seguradora_id",
                table: "apolice_grupo_seguradora");

            migrationBuilder.DropColumn(
                name: "ordem",
                table: "agencia_seguradora");

            migrationBuilder.RenameIndex(
                name: "IX_cooperado_agencia_conta_agencia_id",
                table: "cooperado_agencia_conta",
                newName: "cooperado_agencia_conta_index_9");

            migrationBuilder.RenameIndex(
                name: "cooperado_agencia_conta_index_7",
                table: "cooperado_agencia_conta",
                newName: "cooperado_agencia_conta_index_10");

            migrationBuilder.RenameIndex(
                name: "cooperado_agencia_conta_index_6",
                table: "cooperado_agencia_conta",
                newName: "cooperado_agencia_conta_index_7");

            migrationBuilder.AlterTable(
                name: "usuario",
                oldComment: "Tabela de usuários do sistema com suas credenciais e vínculos organizacionais")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "tela",
                oldComment: "Catálogo de telas (módulos/páginas) disponíveis no sistema")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "seguradora_limite",
                oldComment: "Define faixas etárias, coeficientes e limites de DPS por seguradora para cálculo de prêmios")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "seguradora",
                oldComment: "Armazena os dados cadastrais das seguradoras parceiras")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "proposta_seguradora",
                oldComment: "Controle de numeração sequencial de propostas por seguradora")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "ponto_atendimento",
                oldComment: "Armazena informações dos pontos de atendimento (PAs) vinculados a cada agência")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "parcela",
                comment: "Parcelas financeiras vinculadas a um seguro",
                oldComment: "Parcelas financeiras de prêmio vinculadas a um contrato de seguro")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "parametrizacao_resposta",
                comment: "Resposta dos campos de parametrização",
                oldComment: "Opções de resposta disponíveis para cada campo de parametrização")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "lancamento_efetivar",
                comment: "Lançamentos financeiros que serão/são efetivados",
                oldComment: "Fila de lançamentos financeiros pendentes de efetivação nas contas dos cooperados")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "integracao_senior",
                oldComment: "Fila de controle de integrações contábeis com o sistema Sênior (ERP)")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "gestao_documento",
                comment: "Gestão de documentos",
                oldComment: "Gestão de templates e campos de documentos por seguradora para geração automática")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "cooperado",
                comment: "Cadastro de cooperados vinculados a uma agência",
                oldComment: "Cadastro de cooperados (clientes) da cooperativa que podem contratar seguros")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "condicao_seguradora",
                oldComment: "Parâmetros de condições operacionais e financeiras aplicados por seguradora")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "auditoria",
                oldComment: "Tabela de auditoria para rastreamento de todas as operações realizadas no sistema")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "apolice_grupo_seguradora",
                oldComment: "Configurações de apólices e grupos por vínculo agência-seguradora")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "agencia_seguradora",
                oldComment: "Tabela de vínculo que relaciona agências com seguradoras autorizadas e define prioridade")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterTable(
                name: "agencia",
                oldComment: "Armazena informações cadastrais das agências da cooperativa")
                .Annotation("MySQL:Charset", "utf8mb4")
                .OldAnnotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "usuario",
                type: "enum('Ativo','Inativo')",
                nullable: false,
                comment: "Indica o status do usuário",
                oldClrType: typeof(string),
                oldType: "enum('Ativo','Inativo')",
                oldComment: "Status do usuário: Ativo ou Inativo");

            migrationBuilder.AlterColumn<ulong>(
                name: "ponto_atendimento_id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira para Ponto de Atendimento",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela ponto_atendimento");

            migrationBuilder.AlterColumn<ulong>(
                name: "perfil_id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                comment: "Chave estrangeira para a Perfil ",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldNullable: true,
                oldComment: "Chave estrangeira opcional referenciando a tabela perfil para o perfil principal do usuário");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "usuario",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Nome completo do usuário",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome completo do usuário");

            migrationBuilder.AlterColumn<string>(
                name: "login",
                table: "usuario",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Login de acesso do usuário",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Login de acesso do usuário ao sistema");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "usuario",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "E-mail do usuário",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Endereço de e-mail do usuário para contato e notificações");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "usuario",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data/hora de criação do registro",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data e hora de criação do registro");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira para a Agência",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "usuario",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "tela",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição amigavel da tela",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "Identificador amigável da tela para uso em URLs e código");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "tela",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição/nome da tela",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome ou descrição completa da tela");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "tela",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<DateTime>(
                name: "vencimento",
                table: "seguro",
                type: "date",
                nullable: true,
                comment: "Data de vencimento (padrão do contrato ou próxima parcela)",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Data de vencimento base do contrato ou da próxima parcela");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_iof",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor de IOF",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor do IOF (Imposto sobre Operações Financeiras) incidente sobre o prêmio");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_base",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor Base Segurado",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor base utilizado para cálculo do seguro (saldo devedor ou valor financiado)");

            migrationBuilder.AlterColumn<ulong>(
                name: "usuario_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: true,
                comment: "Chave estrangeira na tabela usuario",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldNullable: true,
                oldComment: "Chave estrangeira referenciando a tabela usuario responsável pela contratação");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_pagamento",
                table: "seguro",
                type: "enum('À Vista','Parcelado','Único')",
                nullable: false,
                comment: "Identificador do tipo de pagamento (1=à vista, 2=parcelado, 3=Única)",
                oldClrType: typeof(string),
                oldType: "enum('À Vista','Parcelado','Único')",
                oldComment: "Modalidade de pagamento: À Vista, Parcelado ou Único");

            migrationBuilder.AlterColumn<decimal>(
                name: "premio_total",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor do prêmio total do seguro",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor total do prêmio do seguro a ser pago");

            migrationBuilder.AlterColumn<ulong>(
                name: "ponto_atendimento_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira na tabela ponto de atendimento",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela ponto_atendimento onde o seguro foi contratado");

            migrationBuilder.AlterColumn<DateTime>(
                name: "inicio_vigencia",
                table: "seguro",
                type: "date",
                nullable: true,
                comment: "Início de vigência do seguro",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Data de início da vigência do seguro");

            migrationBuilder.AlterColumn<DateTime>(
                name: "fim_vigencia",
                table: "seguro",
                type: "date",
                nullable: true,
                comment: "Fim de vigência do seguro",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Data de término da vigência do seguro");

            migrationBuilder.AlterColumn<decimal>(
                name: "estorno_proporcional",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor de estorno proporcional quando aplicável",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor de estorno proporcional em caso de cancelamento");

            migrationBuilder.AlterColumn<bool>(
                name: "dps",
                table: "seguro",
                type: "tinyint(1)",
                nullable: true,
                comment: "Informação de Exigência de DPS",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldNullable: true,
                oldComment: "Indica se foi exigida Declaração Pessoal de Saúde (true/false)");

            migrationBuilder.AlterColumn<ulong>(
                name: "cooperado_agencia_conta_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira na tabela cooperado_agencia_conta",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela cooperado_agencia_conta");

            migrationBuilder.AlterColumn<string>(
                name: "contrato",
                table: "seguro",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                comment: "Número do contrato do seguro",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldComment: "Número do contrato de crédito vinculado ao seguro");

            migrationBuilder.AlterColumn<int>(
                name: "codigo_grupo",
                table: "seguro",
                type: "int",
                nullable: false,
                comment: "Identificador do grupo (código) do contrato",
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Código identificador do grupo/produto do seguro");

            migrationBuilder.AlterColumn<decimal>(
                name: "capital_segurado",
                table: "seguro",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor do capital segurado",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor total do capital segurado (valor coberto em caso de sinistro)");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<ulong>(
                name: "agencia_seguradora_id",
                table: "seguro",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                comment: "Chave estrangeira na tabela agencia_seguradora");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_maximo",
                table: "seguradora_limite",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor associado à faixa",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor máximo de capital segurado permitido para a faixa");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "seguradora_limite",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela seguradora");

            migrationBuilder.AlterColumn<decimal>(
                name: "limite_dps",
                table: "seguradora_limite",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor limite de exigibilidade para DPS",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor limite de capital segurado que exige Declaração Pessoal de Saúde (DPS)");

            migrationBuilder.AlterColumn<short>(
                name: "idade_inicial",
                table: "seguradora_limite",
                type: "smallint",
                nullable: false,
                comment: "Idade inicial da faixa",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Idade inicial da faixa etária para aplicação da regra");

            migrationBuilder.AlterColumn<short>(
                name: "idade_final",
                table: "seguradora_limite",
                type: "smallint",
                nullable: false,
                comment: "Idade final da faixa",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Idade final da faixa etária para aplicação da regra");

            migrationBuilder.AlterColumn<string>(
                name: "descricao_regra",
                table: "seguradora_limite",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição detalhada da regra para o limite DPS",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição textual detalhada da regra aplicada para o limite e DPS");

            migrationBuilder.AlterColumn<decimal>(
                name: "coeficiente",
                table: "seguradora_limite",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Coeficiente aplicado para cálculos",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Coeficiente multiplicador para cálculo do prêmio");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "seguradora_limite",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "seguradora",
                type: "enum('Ativo','Inativo')",
                nullable: false,
                comment: "Status da seguradora",
                oldClrType: typeof(string),
                oldType: "enum('Ativo','Inativo')",
                oldComment: "Status atual da seguradora: Ativo ou Inativo");

            migrationBuilder.AlterColumn<string>(
                name: "razao_social",
                table: "seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Razão social da seguradora",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Razão social completa da seguradora");

            migrationBuilder.AlterColumn<string>(
                name: "cnpj",
                table: "seguradora",
                type: "char(14)",
                nullable: false,
                comment: "CNPJ da seguradora",
                oldClrType: typeof(string),
                oldType: "char(14)",
                oldComment: "CNPJ da seguradora sem formatação (apenas números)");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "proposta_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela seguradora");

            migrationBuilder.AlterColumn<string>(
                name: "numero_sequencial",
                table: "proposta_seguradora",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "Número sequencial da proposta",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Último número sequencial utilizado para geração de propostas");

            migrationBuilder.AlterColumn<string>(
                name: "descricao_sequencial",
                table: "proposta_seguradora",
                type: "longtext",
                nullable: false,
                defaultValue: "",
                comment: "Descrição sequencial da proposta",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Descrição ou prefixo do formato do número sequencial da proposta");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "proposta_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "ponto_atendimento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome do ponto de atendimento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome completo do ponto de atendimento");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "ponto_atendimento",
                type: "datetime(6)",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data/hora da criação do registro",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data e hora de criação do registro");

            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                table: "ponto_atendimento",
                type: "char(3)",
                nullable: false,
                comment: "Código único do ponto de atendimento",
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldComment: "Código do ponto de atendimento no formato de 3 caracteres, único dentro de uma agência");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "ponto_atendimento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "ponto_atendimento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "slug",
                table: "perfil",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome amigavel do perfil",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldComment: "Identificador amigável do perfil para uso em URLs e código");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "perfil",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome do perfil",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome descritivo do perfil de acesso");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_parcela",
                table: "parcela",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor nominal da parcela",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor nominal atual da parcela a ser cobrado");

            migrationBuilder.AlterColumn<decimal>(
                name: "valor_pago",
                table: "parcela",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor total pago na parcela",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor total efetivamente pago na parcela");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "parcela",
                type: "enum('Em Aberto','Pago','Cancelada')",
                nullable: false,
                comment: "Identificador do status da parcela",
                oldClrType: typeof(string),
                oldType: "enum('Em Aberto','Pago','Cancelada')",
                oldComment: "Status atual da parcela conforme enum status_seguro");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguro_id",
                table: "parcela",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela seguro",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela seguro");

            migrationBuilder.AlterColumn<short>(
                name: "numero_parcela",
                table: "parcela",
                type: "smallint",
                nullable: false,
                comment: "Número sequencial da parcela dentro do seguro",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Número sequencial da parcela dentro do seguro (1, 2, 3...)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "liquidacao",
                table: "parcela",
                type: "datetime",
                nullable: true,
                comment: "Data de liquidação da parcela",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data e hora de liquidação/quitação da parcela");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_ultimo_pagamento",
                table: "parcela",
                type: "datetime",
                nullable: true,
                comment: "Data do último pagamento registrado",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data e hora do último pagamento parcial ou total registrado");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "parcela",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "resposta",
                table: "parametrizacao_resposta",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Valor de resposta do parâmetro",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Valor de resposta ou opção disponível para o campo de parametrização");

            migrationBuilder.AlterColumn<ulong>(
                name: "parametrizacao_id",
                table: "parametrizacao_resposta",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela parametrizacao",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela parametrizacao");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "parametrizacao_resposta",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "lancamento_efetivar",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor do lançamento",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor monetário do lançamento a ser efetivado");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "lancamento_efetivar",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição do lançamento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição detalhada do lançamento a ser efetivado");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_movimentacao",
                table: "lancamento_efetivar",
                type: "datetime",
                nullable: true,
                comment: "Data de movimentação financeira",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data e hora da movimentação financeira no sistema de origem");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_lancamento",
                table: "lancamento_efetivar",
                type: "date",
                nullable: true,
                comment: "Data de lançamento/registro no sistema",
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true,
                oldComment: "Data programada para efetivação do lançamento no sistema");

            migrationBuilder.AlterColumn<ulong>(
                name: "cooperado_id",
                table: "lancamento_efetivar",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela cooperado",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela cooperado");

            migrationBuilder.AlterColumn<string>(
                name: "conta_corrente",
                table: "lancamento_efetivar",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Conta corrente associada ao lançamento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Número da conta corrente do cooperado para débito/crédito do lançamento");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "lancamento_efetivar",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "lancamento_efetivar",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<decimal>(
                name: "valor",
                table: "integracao_senior",
                type: "decimal(10,2)",
                nullable: false,
                comment: "Valor da movimentação",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldComment: "Valor monetário do lançamento a ser integrado");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_lancamento_contabil",
                table: "integracao_senior",
                type: "enum('Seguro Prestamista Contratado', 'Comissão Seguro Prestamista Contratado', 'Cancelamento Seguro Prestamista Parcelado Comissão', 'Cancelamento Seguro Prestamista À Vista Proporcional Comissão', 'Pagamento Seguro Prestamista', 'Recebimento Comissão Seguro Prestamista')",
                nullable: false,
                comment: "Tipo de lançamento contábil",
                oldClrType: typeof(string),
                oldType: "enum('Seguro Prestamista Contratado','Comissão Seguro Prestamista Contratado','Cancelamento Seguro Prestamista Parcelado','Cancelamento Seguro Prestamista Parcelado Comissão','Cancelamento Seguro Prestamista À Vista Proporcional Comissão','Pagamento Seguro Prestamista','Recebimento Comissão Seguro Prestamista')",
                oldComment: "Tipo do lançamento contábil conforme enum tipo_lancamento");

            migrationBuilder.AlterColumn<string>(
                name: "status",
                table: "integracao_senior",
                type: "enum('Enviado', 'Falha')",
                nullable: false,
                comment: "Status integração",
                oldClrType: typeof(string),
                oldType: "enum('Enviado','Falha')",
                oldComment: "Status da integração: Enviado (sucesso) ou Falha (erro no envio)");

            migrationBuilder.AlterColumn<string>(
                name: "descricao",
                table: "integracao_senior",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Descrição da integração",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Descrição detalhada do lançamento para identificação");

            migrationBuilder.AlterColumn<DateTime>(
                name: "data_movimentacao",
                table: "integracao_senior",
                type: "datetime",
                nullable: false,
                comment: "Data e hora da movimentação",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldComment: "Data e hora da movimentação a ser integrada");

            migrationBuilder.AlterColumn<string>(
                name: "conta_contabil_debito",
                table: "integracao_senior",
                type: "varchar(255)",
                nullable: false,
                comment: "Conta contábil de débito",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "Código da conta contábil de débito para o lançamento");

            migrationBuilder.AlterColumn<string>(
                name: "conta_contabil_credito",
                table: "integracao_senior",
                type: "varchar(255)",
                nullable: false,
                comment: "Conta contábil de crédito",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldComment: "Código da conta contábil de crédito para o lançamento");

            migrationBuilder.AlterColumn<string>(
                name: "codigo_pa",
                table: "integracao_senior",
                type: "char(3)",
                nullable: false,
                comment: "Código único do ponto de atendimento",
                oldClrType: typeof(string),
                oldType: "char(3)",
                oldComment: "Código do ponto de atendimento de origem do lançamento");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "integracao_senior",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "integracao_senior",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<short>(
                name: "versao",
                table: "gestao_documento",
                type: "smallint",
                nullable: false,
                comment: "Versão do documento",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Número da versão do documento para controle de alterações");

            migrationBuilder.AlterColumn<string>(
                name: "valor",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Valor de resposta do parâmetro",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Valor padrão ou resposta configurada para o campo");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "gestao_documento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela seguradora");

            migrationBuilder.AlterColumn<int>(
                name: "ordem",
                table: "gestao_documento",
                type: "int",
                nullable: false,
                defaultValue: 0,
                comment: "Ordem de exibição",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0,
                oldComment: "Ordem de exibição do campo no documento");

            migrationBuilder.AlterColumn<string>(
                name: "nome_documento",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome/título do documento",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome ou título do documento a ser gerado");

            migrationBuilder.AlterColumn<string>(
                name: "label",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome amigável do parâmetro",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Rótulo amigável do campo para exibição ao usuário");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "gestao_documento",
                type: "datetime(6)",
                nullable: true,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data/hora de criação",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true,
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data e hora de criação do registro");

            migrationBuilder.AlterColumn<string>(
                name: "campo",
                table: "gestao_documento",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Identificador do parâmetro",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Identificador técnico do campo no documento");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "gestao_documento",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "tipo",
                table: "cooperado",
                type: "enum('Física','Jurídica')",
                nullable: false,
                comment: "Tipo de pessoa",
                oldClrType: typeof(string),
                oldType: "enum('Física','Jurídica')",
                oldComment: "Tipo de pessoa: Física (CPF) ou Jurídica (CNPJ)");

            migrationBuilder.AlterColumn<string>(
                name: "numero_documento",
                table: "cooperado",
                type: "varchar(14)",
                maxLength: 14,
                nullable: false,
                comment: "Documento do cooperado (CPF/CNPJ sem formatação)",
                oldClrType: typeof(string),
                oldType: "varchar(14)",
                oldMaxLength: 14,
                oldComment: "Documento de identificação do cooperado (CPF com 11 dígitos ou CNPJ com 14 dígitos, sem formatação)");

            migrationBuilder.AlterColumn<string>(
                name: "nome_fantasia",
                table: "cooperado",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Nome fantasia (para PJ)",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Nome fantasia do cooperado (aplicável apenas para pessoa jurídica)");

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "cooperado",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome/Razão social do cooperado",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome completo (pessoa física) ou razão social (pessoa jurídica) do cooperado");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "cooperado",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "E-mail de contato",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Endereço de e-mail para contato e comunicações com o cooperado");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "cooperado",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "condicao_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela seguradora");

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentagem_cobertura_perda_renda",
                table: "condicao_seguradora",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Percentual de cobertura para perda de renda",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Percentual de cobertura para sinistro por perda de renda (ex: 0.3000 = 30%)");

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentagem_cobertura_morte",
                table: "condicao_seguradora",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Percentual de cobertura para morte",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Percentual de cobertura para sinistro por morte (ex: 1.0000 = 100%)");

            migrationBuilder.AlterColumn<decimal>(
                name: "porcentagem_cobertura_invalidez",
                table: "condicao_seguradora",
                type: "decimal(5,4)",
                nullable: false,
                comment: "Percentual de cobertura para invalidez",
                oldClrType: typeof(decimal),
                oldType: "decimal(5,4)",
                oldComment: "Percentual de cobertura para sinistro por invalidez (ex: 0.5000 = 50%)");

            migrationBuilder.AlterColumn<bool>(
                name: "periodicidade_30dias",
                table: "condicao_seguradora",
                type: "tinyint(1)",
                nullable: false,
                comment: "Indica se a periodicidade de vencimento é mensal ou a cada 30 dias",
                oldClrType: typeof(bool),
                oldType: "tinyint(1)",
                oldDefaultValue: false,
                oldComment: "Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)");

            migrationBuilder.AlterColumn<short>(
                name: "max_meses_contrato",
                table: "condicao_seguradora",
                type: "smallint",
                nullable: false,
                comment: "Quantidade máxima de meses permitidos para o contrato",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Quantidade máxima de meses permitidos para vigência do contrato");

            migrationBuilder.AlterColumn<short>(
                name: "max_idade",
                table: "condicao_seguradora",
                type: "smallint",
                nullable: false,
                comment: "Idade máxima do proponente",
                oldClrType: typeof(short),
                oldType: "smallint",
                oldComment: "Idade máxima permitida do proponente para contratação do seguro");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "condicao_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<ulong>(
                name: "usuario_id",
                table: "auditoria",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela usuario",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela usuario que realizou a ação");

            migrationBuilder.AlterColumn<string>(
                name: "operacao",
                table: "auditoria",
                type: "enum('Insert','Delete','Update')",
                nullable: false,
                comment: "Tipo da operação",
                oldClrType: typeof(string),
                oldType: "enum('Insert','Delete','Update')",
                oldComment: "Tipo da operação realizada: Insert (inserção), Delete (exclusão) ou Update (atualização)");

            migrationBuilder.AlterColumn<string>(
                name: "modulo",
                table: "auditoria",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Módulo/área do sistema afetado",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome do módulo ou área do sistema onde a operação foi realizada");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "auditoria",
                type: "datetime",
                nullable: true,
                comment: "Data/hora da ação registrada",
                oldClrType: typeof(DateTime),
                oldType: "datetime",
                oldNullable: true,
                oldComment: "Data e hora em que a ação foi registrada");

            migrationBuilder.AlterColumn<string>(
                name: "antes",
                table: "auditoria",
                type: "longtext",
                nullable: false,
                comment: "Payload de dados antes da operação",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Dados do registro antes da alteração em formato JSON ou serializado");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "auditoria",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela agencia onde a ação foi realizada");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "auditoria",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<string>(
                name: "depois",
                table: "auditoria",
                type: "longtext",
                nullable: false,
                comment: "Payload de dados depois da alteração");

            migrationBuilder.AlterColumn<string>(
                name: "tipo_capital",
                table: "apolice_grupo_seguradora",
                type: "enum('Fixo','Variável')",
                nullable: false,
                comment: "Tipo de capital",
                oldClrType: typeof(string),
                oldType: "enum('Fixo','Variável')",
                oldComment: "Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)");

            migrationBuilder.AlterColumn<string>(
                name: "subgrupo",
                table: "apolice_grupo_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Código do subgrupo da apólice",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Código do subgrupo dentro do grupo da apólice");

            migrationBuilder.AlterColumn<string>(
                name: "modalidade_unico",
                table: "apolice_grupo_seguradora",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                comment: "Valor modalidade unico",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true,
                oldComment: "Identificador ou código da modalidade de pagamento único");

            migrationBuilder.AlterColumn<decimal>(
                name: "modalidade_parcelado",
                table: "apolice_grupo_seguradora",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor modalidade parcelado",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor ou taxa para modalidade de pagamento parcelado");

            migrationBuilder.AlterColumn<decimal>(
                name: "modalidade_avista",
                table: "apolice_grupo_seguradora",
                type: "decimal(10,2)",
                nullable: true,
                comment: "Valor modalidade avista",
                oldClrType: typeof(decimal),
                oldType: "decimal(10,2)",
                oldNullable: true,
                oldComment: "Valor ou taxa para modalidade de pagamento à vista");

            migrationBuilder.AlterColumn<string>(
                name: "grupo",
                table: "apolice_grupo_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Código do grupo da apólice",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Código do grupo dentro da apólice");

            migrationBuilder.AlterColumn<string>(
                name: "apolice",
                table: "apolice_grupo_seguradora",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                comment: "Código da apólice",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true,
                oldComment: "Número ou código da apólice contratada");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "apolice_grupo_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddColumn<byte>(
                name: "Ordem",
                table: "apolice_grupo_seguradora",
                type: "tinyint unsigned",
                nullable: false,
                defaultValue: (byte)0,
                comment: "ordem de prioridade dentro da agência");

            migrationBuilder.AddColumn<ulong>(
                name: "agencia_id",
                table: "apolice_grupo_seguradora",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                comment: "Chave estrangeira da tabela agencia");

            migrationBuilder.AddColumn<ulong>(
                name: "seguradora_id",
                table: "apolice_grupo_seguradora",
                type: "bigint unsigned",
                nullable: false,
                defaultValue: 0ul,
                comment: "Chave estrangeira da tabela seguradora");

            migrationBuilder.AlterColumn<ulong>(
                name: "seguradora_id",
                table: "agencia_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela seguradora",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela seguradora");

            migrationBuilder.AlterColumn<ulong>(
                name: "agencia_id",
                table: "agencia_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Chave estrangeira da tabela agencia",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Chave estrangeira referenciando a tabela agencia");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "agencia_seguradora",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "nome",
                table: "agencia",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Nome da agência",
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldComment: "Nome completo da agência");

            migrationBuilder.AlterColumn<DateTime>(
                name: "criado_em",
                table: "agencia",
                type: "datetime(6)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(6)",
                comment: "Data/hora da criação do registro",
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldDefaultValueSql: "CURRENT_TIMESTAMP(6)",
                oldComment: "Data e hora de criação do registro");

            migrationBuilder.AlterColumn<string>(
                name: "codigo",
                table: "agencia",
                type: "char(4)",
                nullable: false,
                comment: "Código único da agência",
                oldClrType: typeof(string),
                oldType: "char(4)",
                oldComment: "Código único da agência no formato de 4 caracteres");

            migrationBuilder.AlterColumn<ulong>(
                name: "id",
                table: "agencia",
                type: "bigint unsigned",
                nullable: false,
                comment: "Identificador do registro na tabela",
                oldClrType: typeof(ulong),
                oldType: "bigint unsigned",
                oldComment: "Identificador único do registro na tabela")
                .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn);

            migrationBuilder.CreateIndex(
                name: "IX_seguro_agencia_seguradora_id",
                table: "seguro",
                column: "agencia_seguradora_id");

            migrationBuilder.CreateIndex(
                name: "ponto_atendimento_index_1",
                table: "ponto_atendimento",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "cooperado_agencia_conta_index_8",
                table: "cooperado_agencia_conta",
                column: "cooperado_id");

            migrationBuilder.CreateIndex(
                name: "IX_contabilizacao_seguradora_seguradora_id",
                table: "contabilizacao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_conta_corrente_seguradora_seguradora_id",
                table: "conta_corrente_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_condicao_seguradora_seguradora_id",
                table: "condicao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_comissao_seguradora_seguradora_id",
                table: "comissao_seguradora",
                column: "seguradora_id");

            migrationBuilder.CreateIndex(
                name: "IX_apolice_grupo_seguradora_agencia_id",
                table: "apolice_grupo_seguradora",
                column: "agencia_id");

            migrationBuilder.CreateIndex(
                name: "IX_apolice_grupo_seguradora_seguradora_id",
                table: "apolice_grupo_seguradora",
                column: "seguradora_id");

            migrationBuilder.AddForeignKey(
                name: "FK_apolice_grupo_seguradora_agencia_agencia_id",
                table: "apolice_grupo_seguradora",
                column: "agencia_id",
                principalTable: "agencia",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_apolice_grupo_seguradora_seguradora_seguradora_id",
                table: "apolice_grupo_seguradora",
                column: "seguradora_id",
                principalTable: "seguradora",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_parametrizacao_resposta_parametrizacao_parametrizacao_id",
                table: "parametrizacao_resposta",
                column: "parametrizacao_id",
                principalTable: "parametrizacao",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_seguro_agencia_seguradora_agencia_seguradora_id",
                table: "seguro",
                column: "agencia_seguradora_id",
                principalTable: "agencia_seguradora",
                principalColumn: "id");
        }
    }
}
