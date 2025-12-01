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

CREATE TABLE `cooperado` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `numero_documento` varchar(14) NOT NULL,
    `tipo` enum('Física','Jurídica') NOT NULL,
    `nome` varchar(255) NOT NULL,
    `nome_fantasia` varchar(255) NULL,
    `email` varchar(255) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `parametrizacao` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `descricao` varchar(255) NOT NULL,
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
    `iof` decimal(5,4) NOT NULL,
    `porcentagem_comissao_corretora` decimal(5,4) NOT NULL,
    `porcentagem_comissao_cooperativa` decimal(5,4) NOT NULL,
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
    `tipo_lancamento_contabil` enum('Seguro Prestamista Contratado', 'Comissão Seguro Prestamista Contratado', 'Cancelamento Seguro Prestamista Parcelado Comissão', 'Cancelamento Seguro Prestamista À Vista Proporcional Comissão', 'Pagamento Seguro Prestamista', 'Recebimento Comissão Seguro Prestamista', 'Recebimento Premio Seguro Prestamista Parcelado', 'Recebimento Comissão Seguro Prestamista Parcelado') NOT NULL,
    `codigo_pa` char(3) NOT NULL,
    `conta_contabil_debito` varchar(255) NOT NULL,
    `conta_contabil_credito` varchar(255) NOT NULL,
    `data_movimentacao` datetime NOT NULL,
    `valor` decimal(10,2) NOT NULL,
    `descricao` varchar(255) NOT NULL,
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

CREATE TABLE `parametrizacao_resposta` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `parametrizacao_id` bigint unsigned NOT NULL,
    `resposta` varchar(255) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parametrizacao_resposta_parametrizacao_parametrizacao_id` FOREIGN KEY (`parametrizacao_id`) REFERENCES `parametrizacao` (`id`)
);

CREATE TABLE `agencia_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint unsigned NOT NULL,
    `seguradora_id` bigint unsigned NOT NULL,
    `ordem` tinyint NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_agencia_seguradora_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`),
    CONSTRAINT `FK_agencia_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
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
    `DescricaoCreditoComissaoParcela` longtext NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_contabilizacao_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`)
);

CREATE TABLE `gestao_documento` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint unsigned NOT NULL,
    `nome_documento` varchar(255) NOT NULL,
    `versao` smallint NOT NULL,
    `label` varchar(255) NOT NULL,
    `campo` varchar(255) NOT NULL,
    `valor` varchar(255) NOT NULL,
    `ordem` int NOT NULL DEFAULT 0,
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

CREATE TABLE `usuario` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint unsigned NOT NULL,
    `ponto_atendimento_id` bigint unsigned NOT NULL,
    `perfil_id` bigint unsigned NULL,
    `login` varchar(255) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `email` varchar(255) NOT NULL,
    `status` enum('Ativo','Inativo') NOT NULL,
    `criado_em` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6),
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_usuario_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_usuario_perfil_perfil_id` FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_usuario_ponto_atendimento_ponto_atendimento_id` FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `apolice_grupo_seguradora` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `agencia_seguradora_id` bigint unsigned NOT NULL,
    `apolice` varchar(255) NULL,
    `grupo` varchar(255) NULL,
    `subgrupo` varchar(255) NULL,
    `tipo_capital` enum('Fixo','Variável') NOT NULL,
    `modalidade_unico` varchar(50) NULL,
    `modalidade_avista` decimal(10,2) NULL,
    `modalidade_parcelado` decimal(10,2) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_apolice_grupo_seguradora_agencia_seguradora_agencia_segurado~` FOREIGN KEY (`agencia_seguradora_id`) REFERENCES `agencia_seguradora` (`id`)
);

CREATE TABLE `tela_acao_perfil` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `tela_id` bigint unsigned NOT NULL,
    `acao_id` bigint unsigned NOT NULL,
    `perfil_id` bigint unsigned NOT NULL,
    `TelaAcaoId` bigint unsigned NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `AK_tela_acao_perfil_tela_id_acao_id_perfil_id` UNIQUE (`tela_id`, `acao_id`, `perfil_id`),
    CONSTRAINT `FK_tela_acao_perfil_acao_acao_id` FOREIGN KEY (`acao_id`) REFERENCES `acao` (`id`),
    CONSTRAINT `FK_tela_acao_perfil_perfil_perfil_id` FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`),
    CONSTRAINT `FK_tela_acao_perfil_tela_acao_TelaAcaoId` FOREIGN KEY (`TelaAcaoId`) REFERENCES `tela_acao` (`id`),
    CONSTRAINT `FK_tela_acao_perfil_tela_tela_id` FOREIGN KEY (`tela_id`) REFERENCES `tela` (`id`)
);

CREATE TABLE `auditoria` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `usuario_id` bigint unsigned NOT NULL,
    `agencia_id` bigint unsigned NOT NULL,
    `modulo` varchar(255) NOT NULL,
    `operacao` enum('Insert','Delete','Update') NOT NULL,
    `antes` longtext NOT NULL,
    `criado_em` datetime NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_auditoria_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`),
    CONSTRAINT `FK_auditoria_usuario_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`)
);

CREATE TABLE `seguro` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `cooperado_agencia_conta_id` bigint unsigned NOT NULL,
    `ponto_atendimento_id` bigint unsigned NOT NULL,
    `apolice_grupo_seguradora_id` bigint unsigned NOT NULL,
    `seguro_parametro_id` bigint unsigned NOT NULL,
    `usuario_id` bigint unsigned NULL,
    `status` enum('Em análise pela Seguradora','Pendente de Documentação','Ativo','Expiração da Vigência do Seguro','Cancelado pelo Cooperado','Cancelado pela Cooperativa','Sinistro','Recusado pela Seguradora','Cancelamento por Prejuízo','Liquidação Antecipada','Cancelado por Renegociação','Cancelado por Aditivo') NOT NULL,
    `contrato` varchar(10) NOT NULL,
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
    `dps` tinyint(1) NULL,
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
    `status` enum('Em Aberto','Pago','Cancelada') NOT NULL,
    `numero_parcela` smallint NOT NULL,
    `valor_parcela` decimal(10,2) NOT NULL,
    `valor_original` decimal(10,2) NOT NULL,
    `valor_pago` decimal(10,2) NOT NULL,
    `vencimento` date NOT NULL,
    `liquidacao` datetime NULL,
    `data_ultimo_pagamento` datetime NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parcela_seguro_seguro_id` FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`)
);

CREATE TABLE `seguro_cancelamento` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `seguro_id` bigint unsigned NOT NULL,
    `data` date NOT NULL,
    `criado_em` datetime NOT NULL,
    `motivo` enum('Expiração da Vigência do Seguro','Cancelado pelo Cooperado','Cancelado pela Cooperativa','Sinistro','Recusado pela Seguradora','Cancelamento por Prejuízo','Liquidação Antecipada','Cancelado por Renegociação','Cancelado por Aditivo') NOT NULL,
    `valor_restituir` decimal(10,2) NOT NULL,
    `valor_comissao` decimal(10,2) NOT NULL,
    `dias_utilizados` int NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_seguro_cancelamento_seguro_seguro_id` FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`)
);

CREATE UNIQUE INDEX `IX_agencia_codigo` ON `agencia` (`codigo`);

CREATE UNIQUE INDEX `IX_agencia_nome` ON `agencia` (`nome`);

CREATE UNIQUE INDEX `cooperado_agencia_conta_index_6` ON `cooperado_agencia_conta` (`cooperado_id`, `agencia_id`, `conta_corrente`);

CREATE INDEX `cooperado_agencia_conta_index_7` ON `cooperado_agencia_conta` (`conta_corrente`);

CREATE UNIQUE INDEX `IX_perfil_nome` ON `perfil` (`nome`);

CREATE UNIQUE INDEX `IX_perfil_slug` ON `perfil` (`slug`);

CREATE UNIQUE INDEX `IX_ponto_atendimento_nome` ON `ponto_atendimento` (`nome`);

CREATE UNIQUE INDEX `ponto_atendimento_index_0` ON `ponto_atendimento` (`agencia_id`, `codigo`);

CREATE UNIQUE INDEX `IX_seguradora_cnpj` ON `seguradora` (`cnpj`);

CREATE UNIQUE INDEX `IX_tela_slug` ON `tela` (`slug`);


