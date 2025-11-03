
START TRANSACTION;

CREATE TABLE `acao` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `descricao` varchar(255) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `agencia` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `codigo` varchar(4) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `criado_em` datetime(6) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `contas_contabeis` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `conta` varchar(50) NOT NULL,
    `descricao` varchar(255) NOT NULL,
    `conta_credito` tinyint(1) NOT NULL,
    `conta_debito` tinyint(1) NOT NULL,
    `conta_credito_comissao` tinyint(1) NOT NULL,
    `conta_debito_comissao` tinyint(1) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `cooperado` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `numero_documento` varchar(14) NOT NULL,
    `tipo` varchar(1) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `nome_fantasia` varchar(255) NULL,
    `email` varchar(255) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `limite` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `idade_inicial` smallint NOT NULL,
    `idade_final` smallint NOT NULL,
    `valor` decimal(10,2) NOT NULL,
    `coeficiente` decimal(5,4) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `perfil` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `nome` varchar(255) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `seguradora` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `nome` varchar(255) NOT NULL,
    `cnpj` varchar(14) NOT NULL,
    `razao_social` varchar(255) NOT NULL,
    `cep` varchar(8) NULL,
    `rua` varchar(255) NULL,
    `complemento` varchar(255) NULL,
    `numero` varchar(10) NULL,
    `bairro` varchar(255) NULL,
    `cidade` varchar(255) NULL,
    `uf` varchar(2) NULL,
    `telefone` varchar(20) NULL,
    `email` varchar(255) NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `tela` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `descricao` varchar(255) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE TABLE `grupo_seguradora` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint NOT NULL,
    `avista` decimal(10,2) NULL,
    `parcelado` decimal(10,2) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_grupo_seguradora_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `integracao_senior` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint NOT NULL,
    `tipo` varchar(50) NOT NULL,
    `data_movimentacao` datetime(6) NOT NULL,
    `valor` decimal(10,2) NOT NULL,
    `lancamento` int NOT NULL,
    `descricao` varchar(255) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_integracao_senior_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `parametrizacao` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `tipo` varchar(50) NOT NULL,
    `descricao` varchar(255) NOT NULL,
    `AgenciasId` bigint NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parametrizacao_agencia_AgenciasId` FOREIGN KEY (`AgenciasId`) REFERENCES `agencia` (`id`)
);

CREATE TABLE `ponto_atendimento` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint NOT NULL,
    `codigo` varchar(3) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `criado_em` datetime(6) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_ponto_atendimento_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `cooperado_agencia_conta` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `cooperado_id` bigint NOT NULL,
    `agencia_id` bigint NOT NULL,
    `conta_corrente` varchar(9) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_cooperado_agencia_conta_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_cooperado_agencia_conta_cooperado_cooperado_id` FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `lancamento_efetivar` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint NOT NULL,
    `conta_corrente` varchar(255) NOT NULL,
    `cooperado_id` bigint NOT NULL,
    `data_movimentacao` datetime(6) NULL,
    `descricao` varchar(255) NOT NULL,
    `valor` decimal(10,2) NOT NULL,
    `data_lancamento` datetime(6) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_lancamento_efetivar_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_lancamento_efetivar_cooperado_cooperado_id` FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `agencia_seguradora` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint NOT NULL,
    `seguradora_id` bigint NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_agencia_seguradora_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_agencia_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `gestao_documento` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint NOT NULL,
    `nome_documento` varchar(255) NOT NULL,
    `versao` smallint NOT NULL,
    `label` varchar(255) NOT NULL,
    `campo` varchar(255) NOT NULL,
    `valor` varchar(255) NOT NULL,
    `ordem` int NOT NULL,
    `criado_em` datetime(6) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_gestao_documento_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `parametro_seguradora` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `seguradora_id` bigint NOT NULL,
    `max_meses_contrato` smallint NOT NULL,
    `valor_minimo_dps` decimal(10,2) NOT NULL,
    `min_dias_contratar` smallint NOT NULL,
    `max_idade` smallint NOT NULL,
    `tipo_operacao` int NOT NULL,
    `capital` decimal(10,2) NOT NULL,
    `porcentagem_cobertura_morte` decimal(5,4) NOT NULL,
    `porcentagem_cobertura_invalidez` decimal(5,4) NOT NULL,
    `apolice_unico` varchar(255) NOT NULL,
    `apolice_avista` varchar(255) NOT NULL,
    `apolice_parcelado` varchar(255) NOT NULL,
    `porcentagem_comissao_corretora` decimal(5,4) NOT NULL,
    `porcentagem_comissao_cooperativa` decimal(5,4) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parametro_seguradora_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `priorizacao` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `agencia_id` bigint NOT NULL,
    `seguradora_id` bigint NOT NULL,
    `ordem` tinyint unsigned NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_priorizacao_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_priorizacao_seguradora_seguradora_id` FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `tela_acao` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `tela_id` bigint NOT NULL,
    `acao_id` bigint NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_tela_acao_acao_acao_id` FOREIGN KEY (`acao_id`) REFERENCES `acao` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_tela_acao_tela_tela_id` FOREIGN KEY (`tela_id`) REFERENCES `tela` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `tela_acao_perfil` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `tela_id` bigint NOT NULL,
    `acao_id` bigint NOT NULL,
    `perfil_id` bigint NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_tela_acao_perfil_acao_acao_id` FOREIGN KEY (`acao_id`) REFERENCES `acao` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_tela_acao_perfil_perfil_perfil_id` FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_tela_acao_perfil_tela_tela_id` FOREIGN KEY (`tela_id`) REFERENCES `tela` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `parametrizacao_resposta` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `parametrizacao_id` bigint NOT NULL,
    `agencia_id` bigint NULL,
    `resposta` varchar(255) NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parametrizacao_resposta_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE SET NULL,
    CONSTRAINT `FK_parametrizacao_resposta_parametrizacao_parametrizacao_id` FOREIGN KEY (`parametrizacao_id`) REFERENCES `parametrizacao` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `usuario` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `login` varchar(255) NOT NULL,
    `nome` varchar(255) NOT NULL,
    `email` varchar(255) NOT NULL,
    `status` varchar(50) NOT NULL,
    `criado_em` datetime(6) NULL,
    `agencia_id` bigint NOT NULL,
    `ponto_atendimento_id` bigint NOT NULL,
    `perfil_id` bigint NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_usuario_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_usuario_perfil_perfil_id` FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`) ON DELETE SET NULL,
    CONSTRAINT `FK_usuario_ponto_atendimento_ponto_atendimento_id` FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `parametro_seguradora_conta_contabil` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `parametro_seguradora_id` bigint NOT NULL,
    `conta_contabil_id` bigint NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parametro_seguradora_conta_contabil_contas_contabeis_conta_c~` FOREIGN KEY (`conta_contabil_id`) REFERENCES `contas_contabeis` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_parametro_seguradora_conta_contabil_parametro_seguradora_par~` FOREIGN KEY (`parametro_seguradora_id`) REFERENCES `parametro_seguradora` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `parametro_seguradora_limite` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `parametro_id` bigint NOT NULL,
    `limite_id` bigint NOT NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parametro_seguradora_limite_limite_limite_id` FOREIGN KEY (`limite_id`) REFERENCES `limite` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_parametro_seguradora_limite_parametro_seguradora_parametro_id` FOREIGN KEY (`parametro_id`) REFERENCES `parametro_seguradora` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `auditoria` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `usuario_id` bigint NOT NULL,
    `agencia_id` bigint NOT NULL,
    `modulo` varchar(255) NOT NULL,
    `operacao` varchar(50) NOT NULL,
    `antes` varchar(255) NOT NULL,
    `depois` varchar(255) NOT NULL,
    `criado_em` datetime(6) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_auditoria_agencia_agencia_id` FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_auditoria_usuario_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`) ON DELETE RESTRICT
);

CREATE TABLE `seguro` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `status` tinyint unsigned NOT NULL,
    `ponto_atendimento_id` bigint NOT NULL,
    `cooperado_id` bigint NULL,
    `cooperado_agencia_conta_id` bigint NOT NULL,
    `contrato` varchar(10) NOT NULL,
    `inicio_vigencia` datetime(6) NULL,
    `fim_vigencia` datetime(6) NULL,
    `valor_iof` decimal(10,2) NULL,
    `agencia_seguradora_id` bigint NOT NULL,
    `codigo_grupo` int NOT NULL,
    `quantidade_parcelas` smallint NOT NULL,
    `vencimento` datetime(6) NULL,
    `data_pagamento` datetime(6) NULL,
    `capital_segurado` decimal(10,2) NOT NULL,
    `premio_total` decimal(10,2) NOT NULL,
    `tipo_pagamento` tinyint unsigned NOT NULL,
    `estorno_proporcional` decimal(10,2) NOT NULL,
    `valor_base` decimal(10,2) NULL,
    `dps` tinyint(1) NULL,
    `usuario_id` bigint NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_seguro_agencia_seguradora_agencia_seguradora_id` FOREIGN KEY (`agencia_seguradora_id`) REFERENCES `agencia_seguradora` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_seguro_cooperado_agencia_conta_cooperado_agencia_conta_id` FOREIGN KEY (`cooperado_agencia_conta_id`) REFERENCES `cooperado_agencia_conta` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_seguro_cooperado_cooperado_id` FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`),
    CONSTRAINT `FK_seguro_ponto_atendimento_ponto_atendimento_id` FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_seguro_usuario_usuario_id` FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`) ON DELETE SET NULL
);

CREATE TABLE `parcelas` (
    `id` bigint NOT NULL AUTO_INCREMENT,
    `seguro_id` bigint NOT NULL,
    `status` tinyint unsigned NOT NULL,
    `numero_parcela` smallint NOT NULL,
    `valor_parcela` decimal(10,2) NOT NULL,
    `valor_pago` decimal(10,2) NOT NULL,
    `vencimento` datetime(6) NOT NULL,
    `liquidacao` datetime(6) NULL,
    `data_ultimo_pagamento` datetime(6) NULL,
    PRIMARY KEY (`id`),
    CONSTRAINT `FK_parcelas_seguro_seguro_id` FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`) ON DELETE RESTRICT
);

CREATE UNIQUE INDEX `IX_agencia_codigo` ON `agencia` (`codigo`);

CREATE UNIQUE INDEX `IX_agencia_nome` ON `agencia` (`nome`);

CREATE INDEX `IX_agencia_seguradora_agencia_id` ON `agencia_seguradora` (`agencia_id`);

CREATE INDEX `IX_agencia_seguradora_seguradora_id` ON `agencia_seguradora` (`seguradora_id`);

CREATE INDEX `IX_auditoria_agencia_id` ON `auditoria` (`agencia_id`);

CREATE INDEX `IX_auditoria_usuario_id` ON `auditoria` (`usuario_id`);

CREATE INDEX `IX_cooperado_agencia_conta_agencia_id` ON `cooperado_agencia_conta` (`agencia_id`);

CREATE UNIQUE INDEX `IX_cooperado_agencia_conta_cooperado_id_agencia_id_conta_corren~` ON `cooperado_agencia_conta` (`cooperado_id`, `agencia_id`, `conta_corrente`);

CREATE INDEX `IX_gestao_documento_seguradora_id` ON `gestao_documento` (`seguradora_id`);

CREATE INDEX `IX_grupo_seguradora_agencia_id` ON `grupo_seguradora` (`agencia_id`);

CREATE INDEX `IX_integracao_senior_agencia_id` ON `integracao_senior` (`agencia_id`);

CREATE INDEX `IX_lancamento_efetivar_agencia_id` ON `lancamento_efetivar` (`agencia_id`);

CREATE INDEX `IX_lancamento_efetivar_cooperado_id` ON `lancamento_efetivar` (`cooperado_id`);

CREATE INDEX `IX_parametrizacao_AgenciasId` ON `parametrizacao` (`AgenciasId`);

CREATE INDEX `IX_parametrizacao_resposta_agencia_id` ON `parametrizacao_resposta` (`agencia_id`);

CREATE INDEX `IX_parametrizacao_resposta_parametrizacao_id` ON `parametrizacao_resposta` (`parametrizacao_id`);

CREATE INDEX `IX_parametro_seguradora_seguradora_id` ON `parametro_seguradora` (`seguradora_id`);

CREATE INDEX `IX_parametro_seguradora_conta_contabil_conta_contabil_id` ON `parametro_seguradora_conta_contabil` (`conta_contabil_id`);

CREATE UNIQUE INDEX `IX_parametro_seguradora_conta_contabil_parametro_seguradora_id_~` ON `parametro_seguradora_conta_contabil` (`parametro_seguradora_id`, `conta_contabil_id`);

CREATE INDEX `IX_parametro_seguradora_limite_limite_id` ON `parametro_seguradora_limite` (`limite_id`);

CREATE UNIQUE INDEX `IX_parametro_seguradora_limite_parametro_id_limite_id` ON `parametro_seguradora_limite` (`parametro_id`, `limite_id`);

CREATE INDEX `IX_parcelas_seguro_id` ON `parcelas` (`seguro_id`);

CREATE UNIQUE INDEX `IX_perfil_nome` ON `perfil` (`nome`);

CREATE UNIQUE INDEX `IX_ponto_atendimento_agencia_id_codigo` ON `ponto_atendimento` (`agencia_id`, `codigo`);

CREATE INDEX `IX_priorizacao_agencia_id` ON `priorizacao` (`agencia_id`);

CREATE INDEX `IX_priorizacao_seguradora_id` ON `priorizacao` (`seguradora_id`);

CREATE UNIQUE INDEX `IX_seguradora_cnpj` ON `seguradora` (`cnpj`);

CREATE INDEX `IX_seguro_agencia_seguradora_id` ON `seguro` (`agencia_seguradora_id`);

CREATE INDEX `IX_seguro_cooperado_agencia_conta_id` ON `seguro` (`cooperado_agencia_conta_id`);

CREATE INDEX `IX_seguro_cooperado_id` ON `seguro` (`cooperado_id`);

CREATE INDEX `IX_seguro_ponto_atendimento_id` ON `seguro` (`ponto_atendimento_id`);

CREATE INDEX `IX_seguro_usuario_id` ON `seguro` (`usuario_id`);

CREATE INDEX `IX_tela_acao_acao_id` ON `tela_acao` (`acao_id`);

CREATE UNIQUE INDEX `IX_tela_acao_tela_id_acao_id` ON `tela_acao` (`tela_id`, `acao_id`);

CREATE INDEX `IX_tela_acao_perfil_acao_id` ON `tela_acao_perfil` (`acao_id`);

CREATE INDEX `IX_tela_acao_perfil_perfil_id` ON `tela_acao_perfil` (`perfil_id`);

CREATE UNIQUE INDEX `IX_tela_acao_perfil_tela_id_acao_id_perfil_id` ON `tela_acao_perfil` (`tela_id`, `acao_id`, `perfil_id`);

CREATE INDEX `IX_usuario_agencia_id` ON `usuario` (`agencia_id`);

CREATE INDEX `IX_usuario_perfil_id` ON `usuario` (`perfil_id`);

CREATE INDEX `IX_usuario_ponto_atendimento_id` ON `usuario` (`ponto_atendimento_id`);


COMMIT;

