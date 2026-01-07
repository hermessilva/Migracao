CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);

START TRANSACTION;
CREATE TABLE `acao` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `descricao` varchar(255) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `agencia` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `codigo` char(4) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `criado_em` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`id`)
);

CREATE TABLE `auditoria` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `chave` bigint unsigned NULL,
    `usuario_id` bigint unsigned NULL,
    `agencia_id` bigint unsigned NULL,
    `modulo` varchar(255) NULL,
    `rota` varchar(255) NULL,
    `tabela` varchar(255) NOT NULL,
    `operacao` enum('Inserção','Atualização','Deleção','Login','Refresh Token') NOT NULL,
    `dados_anteriores` varchar(8000) NOT NULL,
    `criado_em` datetime NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `cooperado` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `numero_documento` varchar(14) NOT NULL,
    `tipo` enum('Física','Jurídica') NOT NULL,
    `nome` varchar(255) NOT NULL,
    `nome_fantasia` varchar(255) NULL,
    `email` varchar(255) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `evento_outbox` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `chave_idempotente` varchar(200) NOT NULL,
    `tipo` enum('Processar débito da parcela','Processar lançamento contábil da parcela') NOT NULL,
    `chave_negocio` varchar(200) NOT NULL,
    `payload` varchar(3000) NOT NULL,
    `status` enum('Pendente','Processando','Sucesso','Falha') NOT NULL,
    `tentativas` tinyint NOT NULL,
    `ultima_atualizacao` varchar(500) NULL,
    `identificador_externo` varchar(50) NULL,
    `criado_em` datetime NOT NULL,
    `processado_em` datetime NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `parametrizacao` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `descricao` varchar(255) NOT NULL,
    `identificador` varchar(50) NOT NULL,
    `valor` varchar(255) NOT NULL,
    `tipo` varchar(50) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `perfil` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `nome` varchar(255) NOT NULL,
    `slug` varchar(50) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `nome` varchar(255) NOT NULL,
    `cnpj` char(14) NOT NULL,
    `razao_social` varchar(255) NOT NULL,
    `status` enum('Ativo','Inativo') NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `seguro_parametro` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `tipo_capital` enum('Fixo','Variável') NOT NULL,
    `periodicidade_30dias` tinyint(1) NOT NULL DEFAULT FALSE,
    `coeficiente` decimal(8,7) NOT NULL,
    `porcentual_iof` decimal(5,4) NOT NULL,
    `porcentagem_comissao_corretora` decimal(5,4) NOT NULL,
    `porcentagem_comissao_cooperativa` decimal(5,4) NOT NULL,
    `porcentagem_cobertura_morte` decimal(5,4) NOT NULL DEFAULT 0.0,
    `capital_morte` decimal(18,2) NOT NULL DEFAULT 0.0,
    `premio_morte` decimal(18,2) NOT NULL DEFAULT 0.0,
    `porcentagem_cobertura_invalidez` decimal(5,4) NOT NULL DEFAULT 0.0,
    `capital_invalidez` decimal(18,2) NOT NULL DEFAULT 0.0,
    `premio_invalidez` decimal(18,2) NOT NULL DEFAULT 0.0,
    PRIMARY KEY (`id`)
);

CREATE TABLE `tela` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `descricao` varchar(255) NOT NULL,
    `slug` varchar(50) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `integracao_senior` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint unsigned NOT NULL,
    `status` enum('Enviado','Falha') NOT NULL,
    `tipo_lancamento_contabil` enum('Seguro Prestamista Contratado','Comissão Seguro Prestamista Contratado','Cancelamento Seguro Prestamista Parcelado Comissão','Cancelamento Seguro Prestamista À Vista Proporcional Comissão','Pagamento Seguro Prestamista','Recebimento Comissão Seguro Prestamista','Recebimento Premio Seguro Prestamista Parcelado','Recebimento Comissão Seguro Prestamista Parcelado') NOT NULL,
    `codigo_pa` char(3) NOT NULL,
    `conta_contabil_debito` varchar(255) NOT NULL,
    `conta_contabil_credito` varchar(255) NOT NULL,
    `data_movimentacao` datetime NOT NULL,
    `valor` decimal(10,2) NOT NULL,
    `descricao` varchar(255) NOT NULL,
    `numero_lancamento` varchar(50) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_integracao_senior_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`)
);

CREATE TABLE `ponto_atendimento` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint unsigned NOT NULL,
    `codigo` char(3) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `criado_em` datetime(6) NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_ponto_atendimento_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`)
);

CREATE TABLE `cooperado_agencia_conta` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `cooperado_id` bigint unsigned NOT NULL,
    `agencia_id` bigint unsigned NOT NULL,
    `conta_corrente` char(9) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_cooperado_agencia_conta_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`),
    CONSTRAINT `FK_cooperado_agencia_conta_cooperado_cooperado_id` FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`)
);

CREATE TABLE `lancamento_efetivar` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint unsigned NOT NULL,
    `cooperado_id` bigint unsigned NOT NULL,
    `conta_corrente` varchar(255) NOT NULL,
    `data_movimentacao` datetime NULL,
    `descricao` varchar(255) NOT NULL,
    `valor` decimal(10,2) NOT NULL,
    `data_lancamento` date NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_lancamento_efetivar_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`),
    CONSTRAINT `FK_lancamento_efetivar_cooperado_cooperado_id` FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`)
);

CREATE TABLE `apolice_grupo_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint unsigned NOT NULL,
    `seguradora_id` bigint unsigned NOT NULL,
    `ordem` int NOT NULL,
    `apolice` varchar(255) NULL,
    `grupo` varchar(255) NULL,
    `subgrupo` varchar(255) NULL,
    `tipo_capital` enum('Fixo','Variável') NOT NULL,
    `modalidade_unico` varchar(50) NULL,
    `modalidade_avista` decimal(10,2) NULL,
    `modalidade_parcelado` decimal(10,2) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_apolice_grupo_seguradora_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`),
    CONSTRAINT `FK_apolice_grupo_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `comissao_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `porcentagem_comissao_corretora` decimal(5,4) NOT NULL,
    `porcentagem_comissao_cooperativa` decimal(5,4) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_comissao_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `condicao_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `max_meses_contrato` smallint NOT NULL,
    `max_idade` smallint NOT NULL,
    `porcentagem_cobertura_morte` decimal(5,4) NOT NULL,
    `porcentagem_cobertura_invalidez` decimal(5,4) NOT NULL,
    `porcentagem_cobertura_perda_renda` decimal(5,4) NOT NULL,
    `periodicidade_30dias` tinyint(1) NOT NULL DEFAULT FALSE,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_condicao_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `conta_corrente_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `conta_corrente_prestamista` varchar(50) NOT NULL,
    `descricao_conta_corrente_prestamista` varchar(255) NOT NULL,
    `conta_cancelamento_prestamista` varchar(50) NOT NULL,
    `descricao_conta_cancelamento_prestamista` varchar(255) NOT NULL,
    `conta_estorno_prestamista` varchar(50) NOT NULL,
    `descricao_conta_estorno_prestamista` varchar(255) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_conta_corrente_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `contabilizacao_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `credito_premio_contratacao` varchar(50) NOT NULL,
    `descricao_credito_premio_contratacao` varchar(255) NOT NULL,
    `debito_premio_contratacao` varchar(50) NOT NULL,
    `descricao_debito_premio_contratacao` varchar(255) NOT NULL,
    `credito_comissao_contratacao` varchar(50) NOT NULL,
    `descricao_credito_comissao_contratacao` varchar(255) NOT NULL,
    `debito_comissao_contratacao` varchar(50) NOT NULL,
    `descricao_debito_comissao_contratacao` varchar(255) NOT NULL,
    `credito_cancelamento_comissao_parc_tot` varchar(50) NOT NULL,
    `descricao_credito_cancelamento_comissao_parc_tot` varchar(255) NOT NULL,
    `debito_cancelamento_comissao_parc_tot` varchar(50) NOT NULL,
    `descricao_debito_cancelamento_comissao_parc_tot` varchar(255) NOT NULL,
    `credito_cancelamento_comissao_avista` varchar(50) NOT NULL,
    `descricao_credito_cancelamento_comissao_avista` varchar(255) NOT NULL,
    `debito_cancelamento_comissao_avista` varchar(50) NOT NULL,
    `descricao_debito_cancelamento_comissao_avista` varchar(255) NOT NULL,
    `credito_valor_pago` varchar(50) NOT NULL,
    `descricao_credito_valor_pago` varchar(255) NOT NULL,
    `debito_valor_pago` varchar(50) NOT NULL,
    `descricao_debito_valor_pago` varchar(255) NOT NULL,
    `credito_comissao_valor_pago` varchar(50) NOT NULL,
    `descricao_comissao_credito_valor_pago` varchar(255) NOT NULL,
    `debito_comissao_valor_pago` varchar(50) NOT NULL,
    `descricao_comissao_debito_valor_pago` varchar(255) NOT NULL,
    `debito_premio_parcela` varchar(50) NOT NULL,
    `descricao_debito_premio_parcela` varchar(255) NOT NULL,
    `credito_premio_parcela` varchar(50) NOT NULL,
    `descricao_credito_premio_parcela` varchar(255) NOT NULL,
    `debito_comissao_parcela` varchar(50) NOT NULL,
    `descricao_debito_comissao_parcela` varchar(255) NOT NULL,
    `credito_comissao_parcela` varchar(50) NOT NULL,
    `descricao_credito_comissao_parcela` varchar(255) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_contabilizacao_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `gestao_documento` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `tipo` enum('Termo de Adesão','DPS') NOT NULL,
    `validade` date NOT NULL,
    `status` enum('Ativo','Inativo') NOT NULL,
    `modelo` mediumblob NOT NULL,
    `extensao` varchar(15) NOT NULL,
    `criado_em` datetime(6) NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_gestao_documento_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `proposta_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `descricao_sequencial` varchar(255) NULL,
    `numero_sequencial` varchar(255) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_proposta_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `seguradora_limite` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `idade_inicial` smallint NOT NULL,
    `idade_final` smallint NOT NULL,
    `valor_maximo` decimal(10,2) NOT NULL,
    `coeficiente` decimal(8,7) NOT NULL,
    `limite_dps` decimal(10,2) NOT NULL,
    `descricao_regra` varchar(255) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_seguradora_limite_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `tela_acao` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `tela_id` bigint unsigned NOT NULL,
    `acao_id` bigint unsigned NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `AK_tela_acao_tela_id_acao_id` UNIQUE (`tela_id`, `acao_id`),
    CONSTRAINT `FK_tela_acao_acao_acao_id` FOREIGN KEY (`acao_id`) REFERENCES `acao` (`id`),
    CONSTRAINT `FK_tela_acao_tela_tela_id` FOREIGN KEY (`tela_id`) REFERENCES `tela` (`id`)
);

CREATE TABLE `tela_acao_perfil` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `tela_id` bigint unsigned NOT NULL,
    `acao_id` bigint unsigned NOT NULL,
    `perfil_id` bigint unsigned NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `AK_tela_acao_perfil_tela_id_acao_id_perfil_id` UNIQUE (`tela_id`, `acao_id`, `perfil_id`),
    CONSTRAINT `FK_tela_acao_perfil_acao_acao_id` FOREIGN KEY (`acao_id`) REFERENCES `acao` (`id`),
    CONSTRAINT `FK_tela_acao_perfil_perfil_perfil_id` FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`),
    CONSTRAINT `FK_tela_acao_perfil_tela_tela_id` FOREIGN KEY (`tela_id`) REFERENCES `tela` (`id`)
);

CREATE TABLE `usuario` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `ponto_atendimento_id` bigint unsigned NOT NULL,
    `perfil_id` bigint unsigned NULL,
    `login` varchar(255) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `email` varchar(255) NOT NULL,
    `status` enum('Ativo','Inativo') NOT NULL,
    `criado_em` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_usuario_perfil_perfil_id` FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_usuario_ponto_atendimento_ponto_atendimento_id` FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `seguro` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `cooperado_agencia_conta_id` bigint unsigned NOT NULL,
    `ponto_atendimento_id` bigint unsigned NOT NULL,
    `apolice_grupo_seguradora_id` bigint unsigned NOT NULL,
    `seguro_parametro_id` bigint unsigned NOT NULL,
    `usuario_id` bigint unsigned NULL,
    `contrato_sequencia` varchar(2) NULL DEFAULT '00',
    `status` enum('Pendente','Ativo','Recusado','Expirado','Cancelado') NOT NULL,
    `motivo` enum('Em analise na seguradora','Aguardando faturamento','Aguardando documentação','Pagamento à vista','Pagamento parcelado','Inadimplente','Regular','Recusado pela seguradora','Expiração da vigência do seguro','Aditivo','Cancelamento por prejuízo','Renegociação','Sinistro','Solicitado pela cooperativa','Solicitado pelo cooperado','Liquidação antecipada') NOT NULL,
    `contrato` varchar(10) NOT NULL,
    `numero_contrato_emprestimo` varchar(20) NULL,
    `inicio_vigencia` date NULL,
    `fim_vigencia` date NULL,
    `codigo_grupo` int NOT NULL,
    `quantidade_parcelas` smallint NOT NULL,
    `vencimento` date NULL,
    `capital_segurado` decimal(10,2) NOT NULL,
    `premio_total` decimal(10,2) NOT NULL,
    `tipo_pagamento` enum('À Vista','Parcelado','Único') NOT NULL,
    `estorno_proporcional` decimal(10,2) NOT NULL,
    `valor_base` decimal(10,2) NULL,
    `declaracao_pessoal_saude` tinyint(1) NULL,
    `valor_iof` decimal(10,2) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_seguro_apolice_grupo_seguradora_apolice_grupo_seguradora_id` FOREIGN KEY (`apolice_grupo_seguradora_id`) REFERENCES `apolice_grupo_seguradora` (`id`),
    CONSTRAINT `FK_seguro_cooperado_agencia_conta_cooperado_agencia_conta_id` FOREIGN KEY (`cooperado_agencia_conta_id`) REFERENCES `cooperado_agencia_conta` (`id`),
    CONSTRAINT `FK_seguro_ponto_atendimento_ponto_atendimento_id` FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`),
    CONSTRAINT `FK_seguro_seguro_parametro_seguro_parametro_id` FOREIGN KEY (`seguro_parametro_id`) REFERENCES `seguro_parametro` (`id`),
    CONSTRAINT `FK_seguro_usuario_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`)
);

CREATE TABLE `parcela` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguro_id` bigint unsigned NOT NULL,
    `status` enum('Pendente','Em Aberto','Pago','Cancelada') NOT NULL,
    `numero_parcela` smallint NOT NULL,
    `valor_parcela` decimal(10,2) NOT NULL,
    `valor_original` decimal(10,2) NOT NULL,
    `valor_pago` decimal(10,2) NOT NULL,
    `vencimento` date NOT NULL,
    `liquidacao` datetime NULL,
    `data_ultimo_pagamento` datetime NULL,
    `comissao_corretora` decimal(10,2) NOT NULL,
    `comissao_cooperativa` decimal(10,2) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parcela_seguro_seguro_id` FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`)
);

CREATE TABLE `seguro_cancelamento` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguro_id` bigint unsigned NOT NULL,
    `data` date NOT NULL,
    `criado_em` datetime NOT NULL,
    `motivo` enum('Aditivo','Cancelamento por prejuízo','Renegociaçao','Sinistro','Solicitado pela cooperativa','Solicitado pelo cooperado','Liquidação Antecipada') NOT NULL,
    `valor_restituir` decimal(10,2) NOT NULL,
    `valor_comissao` decimal(10,2) NOT NULL,
    `dias_utilizados` int NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_seguro_cancelamento_seguro_seguro_id` FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`)
);

CREATE UNIQUE INDEX `ux_acao_descricao` ON `acao` (`descricao`);

CREATE UNIQUE INDEX `IX_agencia_codigo` ON `agencia` (`codigo`);

CREATE UNIQUE INDEX `IX_agencia_nome` ON `agencia` (`nome`);

CREATE INDEX `IX_apolice_grupo_seguradora_seguradora_id` ON `apolice_grupo_seguradora` (`seguradora_id`);

CREATE INDEX `IX_ApoliceGrupoSeguradora_Agencia_Seguradora_TipoCapital` ON `apolice_grupo_seguradora` (`agencia_id`, `seguradora_id`, `tipo_capital`);

CREATE INDEX `IX_ApoliceGrupoSeguradora_Ordem` ON `apolice_grupo_seguradora` (`ordem`);

CREATE UNIQUE INDEX `comissao_seguradora_index_5` ON `comissao_seguradora` (`seguradora_id`);

CREATE UNIQUE INDEX `condicao_seguradora_index_1` ON `condicao_seguradora` (`seguradora_id`);

CREATE UNIQUE INDEX `conta_corrente_seguradora_index_4` ON `conta_corrente_seguradora` (`seguradora_id`);

CREATE UNIQUE INDEX `contabilizacao_seguradora_index_3` ON `contabilizacao_seguradora` (`seguradora_id`);

CREATE INDEX `idx_conta_corrente` ON `cooperado_agencia_conta` (`conta_corrente`);

CREATE UNIQUE INDEX `idx_cooperado_id_agencia_id_conta_corrente` ON `cooperado_agencia_conta` (`cooperado_id`, `agencia_id`, `conta_corrente`);

CREATE INDEX `IX_cooperado_agencia_conta_agencia_id` ON `cooperado_agencia_conta` (`agencia_id`);

CREATE UNIQUE INDEX `IX_evento_outbox_chave_idempotente` ON `evento_outbox` (`chave_idempotente`);

CREATE INDEX `IX_gestao_documento_seguradora_id` ON `gestao_documento` (`seguradora_id`);

CREATE INDEX `IX_integracao_senior_agencia_id` ON `integracao_senior` (`agencia_id`);

CREATE INDEX `IX_lancamento_efetivar_agencia_id` ON `lancamento_efetivar` (`agencia_id`);

CREATE INDEX `IX_lancamento_efetivar_cooperado_id` ON `lancamento_efetivar` (`cooperado_id`);

CREATE INDEX `IX_parcela_seguro_id` ON `parcela` (`seguro_id`);

CREATE UNIQUE INDEX `IX_perfil_nome` ON `perfil` (`nome`);

CREATE UNIQUE INDEX `IX_perfil_slug` ON `perfil` (`slug`);

CREATE UNIQUE INDEX `idx_agencia_id_codigo` ON `ponto_atendimento` (`agencia_id`, `codigo`);

CREATE UNIQUE INDEX `IX_ponto_atendimento_nome` ON `ponto_atendimento` (`nome`);

CREATE INDEX `IX_proposta_seguradora_seguradora_id` ON `proposta_seguradora` (`seguradora_id`);

CREATE UNIQUE INDEX `IX_seguradora_cnpj` ON `seguradora` (`cnpj`);

CREATE INDEX `IX_seguradora_limite_seguradora_id` ON `seguradora_limite` (`seguradora_id`);

CREATE UNIQUE INDEX `idx_seguro_parametro_id` ON `seguro` (`seguro_parametro_id`);

CREATE INDEX `IX_seguro_apolice_grupo_seguradora_id` ON `seguro` (`apolice_grupo_seguradora_id`);

CREATE INDEX `IX_seguro_cooperado_agencia_conta_id` ON `seguro` (`cooperado_agencia_conta_id`);

CREATE INDEX `IX_seguro_ponto_atendimento_id` ON `seguro` (`ponto_atendimento_id`);

CREATE INDEX `IX_seguro_usuario_id` ON `seguro` (`usuario_id`);

CREATE INDEX `IX_seguro_cancelamento_seguro_id` ON `seguro_cancelamento` (`seguro_id`);

CREATE UNIQUE INDEX `IX_tela_slug` ON `tela` (`slug`);

CREATE INDEX `tela_acao_index_2` ON `tela_acao` (`tela_id`);

CREATE INDEX `tela_acao_index_3` ON `tela_acao` (`acao_id`);

CREATE INDEX `tela_acao_perfil_index_4` ON `tela_acao_perfil` (`tela_id`);

CREATE INDEX `tela_acao_perfil_index_5` ON `tela_acao_perfil` (`acao_id`);

CREATE INDEX `tela_acao_perfil_index_6` ON `tela_acao_perfil` (`perfil_id`);

CREATE INDEX `IX_usuario_perfil_id` ON `usuario` (`perfil_id`);

CREATE INDEX `IX_usuario_ponto_atendimento_id` ON `usuario` (`ponto_atendimento_id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260106211356_Init', '9.0.10');

COMMIT;

