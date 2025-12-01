ALTER TABLE `apolice_grupo_seguradora` DROP CONSTRAINT `FK_apolice_grupo_seguradora_agencia_agencia_id`;

ALTER TABLE `apolice_grupo_seguradora` DROP CONSTRAINT `FK_apolice_grupo_seguradora_seguradora_seguradora_id`;

ALTER TABLE `parametrizacao_resposta` DROP CONSTRAINT `FK_parametrizacao_resposta_parametrizacao_parametrizacao_id`;

ALTER TABLE `seguro` DROP CONSTRAINT `FK_seguro_agencia_seguradora_agencia_seguradora_id`;

DROP INDEX IX_seguro_agencia_seguradora_id ON seguro;

DROP INDEX ponto_atendimento_index_1 ON ponto_atendimento;

DROP INDEX cooperado_agencia_conta_index_8 ON cooperado_agencia_conta;

DROP INDEX IX_contabilizacao_seguradora_seguradora_id ON contabilizacao_seguradora;

DROP INDEX IX_conta_corrente_seguradora_seguradora_id ON conta_corrente_seguradora;

DROP INDEX IX_condicao_seguradora_seguradora_id ON condicao_seguradora;

DROP INDEX IX_comissao_seguradora_seguradora_id ON comissao_seguradora;

DROP INDEX IX_apolice_grupo_seguradora_agencia_id ON apolice_grupo_seguradora;

DROP INDEX IX_apolice_grupo_seguradora_seguradora_id ON apolice_grupo_seguradora;

ALTER TABLE `seguro` DROP COLUMN `agencia_seguradora_id`;

ALTER TABLE `auditoria` DROP COLUMN `depois`;

ALTER TABLE `apolice_grupo_seguradora` DROP COLUMN `Ordem`;

ALTER TABLE `apolice_grupo_seguradora` DROP COLUMN `agencia_id`;

ALTER TABLE `apolice_grupo_seguradora` DROP COLUMN `seguradora_id`;

ALTER TABLE `cooperado_agencia_conta` RENAME INDEX `cooperado_agencia_conta_index_9` TO `IX_cooperado_agencia_conta_agencia_id`;

ALTER TABLE `cooperado_agencia_conta` RENAME INDEX `cooperado_agencia_conta_index_7` TO `cooperado_agencia_conta_index_6`;

ALTER TABLE `cooperado_agencia_conta` RENAME INDEX `cooperado_agencia_conta_index_10` TO `cooperado_agencia_conta_index_7`;

ALTER TABLE `usuario` MODIFY `status` enum('Ativo','Inativo') NOT NULL;

ALTER TABLE `usuario` MODIFY `ponto_atendimento_id` bigint unsigned NOT NULL;

ALTER TABLE `usuario` MODIFY `perfil_id` bigint unsigned NULL;

ALTER TABLE `usuario` MODIFY `nome` varchar(255) NOT NULL;

ALTER TABLE `usuario` MODIFY `login` varchar(255) NOT NULL;

ALTER TABLE `usuario` MODIFY `email` varchar(255) NOT NULL;

ALTER TABLE `usuario` MODIFY `criado_em` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6);

ALTER TABLE `usuario` MODIFY `agencia_id` bigint unsigned NOT NULL;

ALTER TABLE `usuario` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `tela` MODIFY `slug` varchar(50) NOT NULL;

ALTER TABLE `tela` MODIFY `descricao` varchar(255) NOT NULL;

ALTER TABLE `tela` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `seguro` MODIFY `vencimento` date NULL;

ALTER TABLE `seguro` MODIFY `valor_iof` decimal(10,2) NULL;

ALTER TABLE `seguro` MODIFY `valor_base` decimal(10,2) NULL;

ALTER TABLE `seguro` MODIFY `usuario_id` bigint unsigned NULL;

ALTER TABLE `seguro` MODIFY `tipo_pagamento` enum('À Vista','Parcelado','Único') NOT NULL;

ALTER TABLE `seguro` MODIFY `premio_total` decimal(10,2) NOT NULL;

ALTER TABLE `seguro` MODIFY `ponto_atendimento_id` bigint unsigned NOT NULL;

ALTER TABLE `seguro` MODIFY `inicio_vigencia` date NULL;

ALTER TABLE `seguro` MODIFY `fim_vigencia` date NULL;

ALTER TABLE `seguro` MODIFY `estorno_proporcional` decimal(10,2) NOT NULL;

ALTER TABLE `seguro` MODIFY `dps` tinyint(1) NULL;

ALTER TABLE `seguro` MODIFY `cooperado_agencia_conta_id` bigint unsigned NOT NULL;

ALTER TABLE `seguro` MODIFY `contrato` varchar(10) NOT NULL;

ALTER TABLE `seguro` MODIFY `codigo_grupo` int NOT NULL;

ALTER TABLE `seguro` MODIFY `capital_segurado` decimal(10,2) NOT NULL;

ALTER TABLE `seguro` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `seguro` ADD `apolice_grupo_seguradora_id` bigint unsigned NOT NULL DEFAULT 0;

ALTER TABLE `seguro` ADD `seguro_parametro_id` bigint unsigned NOT NULL DEFAULT 0;

ALTER TABLE `seguradora_limite` MODIFY `valor_maximo` decimal(10,2) NOT NULL;

ALTER TABLE `seguradora_limite` MODIFY `seguradora_id` bigint unsigned NOT NULL;

ALTER TABLE `seguradora_limite` MODIFY `limite_dps` decimal(10,2) NOT NULL;

ALTER TABLE `seguradora_limite` MODIFY `idade_inicial` smallint NOT NULL;

ALTER TABLE `seguradora_limite` MODIFY `idade_final` smallint NOT NULL;

ALTER TABLE `seguradora_limite` MODIFY `descricao_regra` varchar(255) NOT NULL;

ALTER TABLE `seguradora_limite` MODIFY `coeficiente` decimal(5,4) NOT NULL;

ALTER TABLE `seguradora_limite` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `seguradora` MODIFY `status` enum('Ativo','Inativo') NOT NULL;

ALTER TABLE `seguradora` MODIFY `razao_social` varchar(255) NOT NULL;

ALTER TABLE `seguradora` MODIFY `cnpj` char(14) NOT NULL;

ALTER TABLE `seguradora` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `proposta_seguradora` MODIFY `seguradora_id` bigint unsigned NOT NULL;

ALTER TABLE `proposta_seguradora` MODIFY `numero_sequencial` varchar(255) NULL;

ALTER TABLE `proposta_seguradora` MODIFY `descricao_sequencial` varchar(255) NULL;

ALTER TABLE `proposta_seguradora` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `ponto_atendimento` MODIFY `nome` varchar(255) NOT NULL;

ALTER TABLE `ponto_atendimento` MODIFY `criado_em` datetime(6) NULL DEFAULT CURRENT_TIMESTAMP(6);

ALTER TABLE `ponto_atendimento` MODIFY `codigo` char(3) NOT NULL;

ALTER TABLE `ponto_atendimento` MODIFY `agencia_id` bigint unsigned NOT NULL;

ALTER TABLE `ponto_atendimento` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `perfil` MODIFY `slug` varchar(50) NOT NULL;

ALTER TABLE `perfil` MODIFY `nome` varchar(255) NOT NULL;

ALTER TABLE `parcela` MODIFY `valor_parcela` decimal(10,2) NOT NULL;

ALTER TABLE `parcela` MODIFY `valor_pago` decimal(10,2) NOT NULL;

ALTER TABLE `parcela` MODIFY `status` enum('Em Aberto','Pago','Cancelada') NOT NULL;

ALTER TABLE `parcela` MODIFY `seguro_id` bigint unsigned NOT NULL;

ALTER TABLE `parcela` MODIFY `numero_parcela` smallint NOT NULL;

ALTER TABLE `parcela` MODIFY `liquidacao` datetime NULL;

ALTER TABLE `parcela` MODIFY `data_ultimo_pagamento` datetime NULL;

ALTER TABLE `parcela` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `parcela` ADD `valor_original` decimal(10,2) NOT NULL DEFAULT 0.0;

ALTER TABLE `parametrizacao_resposta` MODIFY `resposta` varchar(255) NOT NULL;

ALTER TABLE `parametrizacao_resposta` MODIFY `parametrizacao_id` bigint unsigned NOT NULL;

ALTER TABLE `parametrizacao_resposta` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `lancamento_efetivar` MODIFY `valor` decimal(10,2) NOT NULL;

ALTER TABLE `lancamento_efetivar` MODIFY `descricao` varchar(255) NOT NULL;

ALTER TABLE `lancamento_efetivar` MODIFY `data_movimentacao` datetime NULL;

ALTER TABLE `lancamento_efetivar` MODIFY `data_lancamento` date NULL;

ALTER TABLE `lancamento_efetivar` MODIFY `cooperado_id` bigint unsigned NOT NULL;

ALTER TABLE `lancamento_efetivar` MODIFY `conta_corrente` varchar(255) NOT NULL;

ALTER TABLE `lancamento_efetivar` MODIFY `agencia_id` bigint unsigned NOT NULL;

ALTER TABLE `lancamento_efetivar` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `integracao_senior` MODIFY `valor` decimal(10,2) NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `tipo_lancamento_contabil` enum('Seguro Prestamista Contratado','Comissão Seguro Prestamista Contratado','Cancelamento Seguro Prestamista Parcelado','Cancelamento Seguro Prestamista Parcelado Comissão','Cancelamento Seguro Prestamista À Vista Proporcional Comissão','Pagamento Seguro Prestamista','Recebimento Comissão Seguro Prestamista') NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `status` enum('Enviado','Falha') NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `descricao` varchar(255) NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `data_movimentacao` datetime NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `conta_contabil_debito` varchar(255) NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `conta_contabil_credito` varchar(255) NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `codigo_pa` char(3) NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `agencia_id` bigint unsigned NOT NULL;

ALTER TABLE `integracao_senior` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `integracao_senior` ADD `numero_lancamento` int NOT NULL DEFAULT 0;

ALTER TABLE `gestao_documento` MODIFY `versao` smallint NOT NULL;

ALTER TABLE `gestao_documento` MODIFY `valor` varchar(255) NOT NULL;

ALTER TABLE `gestao_documento` MODIFY `seguradora_id` bigint unsigned NOT NULL;

ALTER TABLE `gestao_documento` MODIFY `ordem` int NOT NULL DEFAULT 0;

ALTER TABLE `gestao_documento` MODIFY `nome_documento` varchar(255) NOT NULL;

ALTER TABLE `gestao_documento` MODIFY `label` varchar(255) NOT NULL;

ALTER TABLE `gestao_documento` MODIFY `criado_em` datetime(6) NULL DEFAULT CURRENT_TIMESTAMP(6);

ALTER TABLE `gestao_documento` MODIFY `campo` varchar(255) NOT NULL;

ALTER TABLE `gestao_documento` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `cooperado` MODIFY `tipo` enum('Física','Jurídica') NOT NULL;

ALTER TABLE `cooperado` MODIFY `numero_documento` varchar(14) NOT NULL;

ALTER TABLE `cooperado` MODIFY `nome_fantasia` varchar(255) NULL;

ALTER TABLE `cooperado` MODIFY `nome` varchar(255) NOT NULL;

ALTER TABLE `cooperado` MODIFY `email` varchar(255) NULL;

ALTER TABLE `cooperado` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `condicao_seguradora` MODIFY `seguradora_id` bigint unsigned NOT NULL;

ALTER TABLE `condicao_seguradora` MODIFY `porcentagem_cobertura_perda_renda` decimal(5,4) NOT NULL;

ALTER TABLE `condicao_seguradora` MODIFY `porcentagem_cobertura_morte` decimal(5,4) NOT NULL;

ALTER TABLE `condicao_seguradora` MODIFY `porcentagem_cobertura_invalidez` decimal(5,4) NOT NULL;

ALTER TABLE `condicao_seguradora` MODIFY `periodicidade_30dias` tinyint(1) NOT NULL DEFAULT FALSE;

ALTER TABLE `condicao_seguradora` MODIFY `max_meses_contrato` smallint NOT NULL;

ALTER TABLE `condicao_seguradora` MODIFY `max_idade` smallint NOT NULL;

ALTER TABLE `condicao_seguradora` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `auditoria` MODIFY `usuario_id` bigint unsigned NOT NULL;

ALTER TABLE `auditoria` MODIFY `operacao` enum('Insert','Delete','Update') NOT NULL;

ALTER TABLE `auditoria` MODIFY `modulo` varchar(255) NOT NULL;

ALTER TABLE `auditoria` MODIFY `criado_em` datetime NULL;

ALTER TABLE `auditoria` MODIFY `antes` varchar(255) NOT NULL;

ALTER TABLE `auditoria` MODIFY `agencia_id` bigint unsigned NOT NULL;

ALTER TABLE `auditoria` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `tipo_capital` enum('Fixo','Variável') NOT NULL;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `subgrupo` varchar(255) NULL;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `modalidade_unico` varchar(50) NULL;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `modalidade_parcelado` decimal(10,2) NULL;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `modalidade_avista` decimal(10,2) NULL;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `grupo` varchar(255) NULL;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `apolice` varchar(255) NULL;

ALTER TABLE `apolice_grupo_seguradora` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `apolice_grupo_seguradora` ADD `agencia_seguradora_id` bigint unsigned NOT NULL DEFAULT 0;

ALTER TABLE `agencia_seguradora` MODIFY `seguradora_id` bigint unsigned NOT NULL;

ALTER TABLE `agencia_seguradora` MODIFY `agencia_id` bigint unsigned NOT NULL;

ALTER TABLE `agencia_seguradora` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

ALTER TABLE `agencia_seguradora` ADD `ordem` tinyint NOT NULL DEFAULT 0;

ALTER TABLE `agencia` MODIFY `nome` varchar(255) NOT NULL;

ALTER TABLE `agencia` MODIFY `criado_em` datetime(6) NOT NULL DEFAULT CURRENT_TIMESTAMP(6);

ALTER TABLE `agencia` MODIFY `codigo` char(4) NOT NULL;

ALTER TABLE `agencia` MODIFY `id` bigint unsigned NOT NULL AUTO_INCREMENT;

CREATE TABLE `seguro_parametro` (
    `id` bigint unsigned NOT NULL AUTO_INCREMENT,
    `tipo_capital` enum('Fixo','Variável') NOT NULL,
    `periodicidade_30dias` tinyint(1) NOT NULL DEFAULT FALSE,
    `coeficiente` decimal(5,4) NOT NULL,
    PRIMARY KEY (`id`)
);

CREATE UNIQUE INDEX `IX_tela_slug` ON `tela` (`slug`);

CREATE INDEX `IX_seguro_apolice_grupo_seguradora_id` ON `seguro` (`apolice_grupo_seguradora_id`);

CREATE UNIQUE INDEX `seguro_index_8` ON `seguro` (`seguro_parametro_id`);

CREATE UNIQUE INDEX `IX_perfil_slug` ON `perfil` (`slug`);

CREATE UNIQUE INDEX `contabilizacao_seguradora_index_3` ON `contabilizacao_seguradora` (`seguradora_id`);

CREATE UNIQUE INDEX `conta_corrente_seguradora_index_4` ON `conta_corrente_seguradora` (`seguradora_id`);

CREATE UNIQUE INDEX `condicao_seguradora_index_1` ON `condicao_seguradora` (`seguradora_id`);

CREATE UNIQUE INDEX `comissao_seguradora_index_5` ON `comissao_seguradora` (`seguradora_id`);

CREATE INDEX `IX_apolice_grupo_seguradora_agencia_seguradora_id` ON `apolice_grupo_seguradora` (`agencia_seguradora_id`);

ALTER TABLE `apolice_grupo_seguradora` ADD CONSTRAINT `FK_apolice_grupo_seguradora_agencia_seguradora_agencia_segurado~` FOREIGN KEY (`agencia_seguradora_id`) REFERENCES `agencia_seguradora` (`id`);

ALTER TABLE `parametrizacao_resposta` ADD CONSTRAINT `FK_parametrizacao_resposta_parametrizacao_parametrizacao_id` FOREIGN KEY (`parametrizacao_id`) REFERENCES `parametrizacao` (`id`);

ALTER TABLE `seguro` ADD CONSTRAINT `FK_seguro_apolice_grupo_seguradora_apolice_grupo_seguradora_id` FOREIGN KEY (`apolice_grupo_seguradora_id`) REFERENCES `apolice_grupo_seguradora` (`id`);

ALTER TABLE `seguro` ADD CONSTRAINT `FK_seguro_seguro_parametro_seguro_parametro_id` FOREIGN KEY (`seguro_parametro_id`) REFERENCES `seguro_parametro` (`id`);

