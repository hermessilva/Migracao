CREATE TABLE `agencia` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `codigo` char(4) UNIQUE NOT NULL COMMENT 'Código único da agência',
  `nome` varchar(255) UNIQUE NOT NULL COMMENT 'Nome da agência',
  `criado_em` datetime NOT NULL DEFAULT (now()) COMMENT 'Data/hora de criação do registro'
);

CREATE TABLE `ponto_atendimento` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela agencia',
  `codigo` char(3) NOT NULL COMMENT 'Código do ponto de atendimento dentro de uma agência',
  `nome` varchar(255) UNIQUE NOT NULL COMMENT 'Nome do ponto de atendimento',
  `criado_em` datetime DEFAULT (now()) COMMENT 'Data/hora de criação do registro'
);

CREATE TABLE `perfil` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `nome` varchar(255) UNIQUE NOT NULL COMMENT 'Nome do perfil'
);

CREATE TABLE `usuario` (
  `id` BIGINT PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_id` bigint NOT NULL COMMENT 'Chave estrangeira para a agência',
  `ponto_atendimento_id` BIGINT NOT NULL COMMENT 'Chave estrangeira para o ponto de atendimento vinculado',
  `perfil_id` BIGINT COMMENT 'Chave estrangeira para a tabela perfil opcional para o perfil principal do usuário',
  `login` varchar(255) NOT NULL COMMENT 'Login de acesso do usuário',
  `nome` varchar(255) NOT NULL COMMENT 'Nome completo do usuário',
  `email` varchar(255) NOT NULL COMMENT 'E-mail do usuário',
  `status` enum NOT NULL COMMENT 'ENUM(''Ativo'', ''Inativo'') — Indica o status do perfil',
  `criado_em` datetime DEFAULT (now()) COMMENT 'Data/hora de criação do registro'
);

CREATE TABLE `tela` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição/nome da tela'
);

CREATE TABLE `acao` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição da ação (ex.: Visualizar, Editar, Excluir)'
);

CREATE TABLE `tela_acao` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `tela_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela tela',
  `acao_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela acao',
  PRIMARY KEY (`tela_id`, `acao_id`)
);

CREATE TABLE `tela_acao_perfil` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `tela_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela tela',
  `acao_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela acao',
  `perfil_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela perfil',
  PRIMARY KEY (`tela_id`, `acao_id`, `perfil_id`)
);

CREATE TABLE `auditoria` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `usuario_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela usuario',
  `agencia_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela agencia',
  `modulo` varchar(255) NOT NULL COMMENT 'Módulo/área do sistema afetado',
  `operacao` enum NOT NULL COMMENT 'ENUM(''Insert'', ''Delete'', ''Update'') — Tipo da operação',
  `antes` varchar(255) NOT NULL COMMENT 'Payload de dados antes da alteração',
  `depois` varchar(255) NOT NULL COMMENT 'Payload de dados depois da alteração',
  `criado_em` datetime COMMENT 'Data/hora da ação registrada'
);

CREATE TABLE `seguradora` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `nome` varchar(255) NOT NULL COMMENT 'Nome fantasia da seguradora',
  `cnpj` char(14) UNIQUE NOT NULL COMMENT 'CNPJ da seguradora',
  `razao_social` varchar(255) NOT NULL COMMENT 'Razão social da seguradora',
  `cep` char(8) COMMENT 'CEP do endereço',
  `rua` varchar(255) COMMENT 'Logradouro/endereço',
  `complemento` varchar(255) COMMENT 'Complemento do endereço',
  `numero` varchar(10) COMMENT 'Número do endereço',
  `bairro` varchar(255) COMMENT 'Bairro',
  `cidade` varchar(255) COMMENT 'Cidade',
  `uf` char(2) COMMENT 'UF',
  `telefone` varchar(20) COMMENT 'Telefone de contato',
  `email` varchar(255) COMMENT 'E-mail de contato'
);

CREATE TABLE `parametro_seguradora` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `seguradora_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela seguradora',
  `max_meses_contrato` smallint NOT NULL COMMENT 'Quantidade máxima de meses permitidos para o contrato',
  `valor_minimo_dps` decimal(10,2) NOT NULL COMMENT 'Valor mínimo aceito para DPS',
  `min_dias_contratar` smallint NOT NULL COMMENT 'Dias mínimos para contratação do seguro',
  `max_idade` smallint NOT NULL COMMENT 'Idade máxima do proponente',
  `tipo_operacao` int NOT NULL COMMENT 'Identificador do tipo de operação',
  `capital` decimal(10,2) NOT NULL COMMENT 'Capital segurado padrão',
  `porcentagem_cobertura_morte` decimal(5,4) NOT NULL COMMENT 'Percentual de cobertura para morte',
  `porcentagem_cobertura_invalidez` decimal(5,4) NOT NULL COMMENT 'Percentual de cobertura para invalidez',
  `apolice_unico` varchar(255) NOT NULL COMMENT 'Código da apólice do tipo único',
  `apolice_avista` varchar(255) NOT NULL COMMENT 'Código da apólice para pagamento à vista',
  `apolice_parcelado` varchar(255) NOT NULL COMMENT 'Código da apólice para pagamento parcelado',
  `porcentagem_comissao_corretora` decimal(5,4) NOT NULL COMMENT 'Percentual de comissão da corretora',
  `porcentagem_comissao_cooperativa` decimal(5,4) NOT NULL COMMENT 'Percentual de comissão da cooperativa'
);

CREATE TABLE `limite` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `idade_inicial` smallint NOT NULL COMMENT 'Idade inicial da faixa',
  `idade_final` smallint NOT NULL COMMENT 'Idade final da faixa',
  `valor` decimal(10,2) NOT NULL COMMENT 'Valor associado à faixa',
  `coeficiente` decimal(5,4) NOT NULL COMMENT 'Coeficiente aplicado para cálculos'
);

CREATE TABLE `parametro_seguradora_limite` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `parametro_id` bigint NOT NULL COMMENT 'Chave estrangeira para parâmetros de seguradora',
  `limite_id` bigint NOT NULL COMMENT 'Chave estrangeira para faixas de limites',
  PRIMARY KEY (`parametro_id`, `limite_id`)
);

CREATE TABLE `contas_contabeis` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `conta` varchar(50) NOT NULL COMMENT 'Código da conta contábil',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil',
  `conta_credito` bool NOT NULL COMMENT 'Conta contábil de crédito',
  `conta_debito` bool NOT NULL COMMENT 'Conta contábil de débito',
  `conta_credito_comissao` bool NOT NULL COMMENT 'Conta contábil de crédito para comissão',
  `conta_debito_comissao` bool NOT NULL COMMENT 'Conta contábil de débito para comissão'
);

CREATE TABLE `parametro_seguradora_conta_contabil` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `parametro_seguradora_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela parametro_seguradora',
  `conta_contabil_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela conta_contabil',
  PRIMARY KEY (`parametro_seguradora_id`, `conta_contabil_id`)
);

CREATE TABLE `grupo_seguradora` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela agencia',
  `avista` decimal(10,2) COMMENT 'Campo genérico para configuração de modalidade à vista',
  `parcelado` decimal(10,2) COMMENT 'Campo genérico para configuração de modalidade parcelado'
);

CREATE TABLE `gestao_documento` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `seguradora_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela seguradora',
  `nome_documento` varchar(255) NOT NULL COMMENT 'Nome/título do documento',
  `versao` smallint NOT NULL COMMENT 'Versão do documento',
  `label` varchar(255) NOT NULL COMMENT 'Nome amigável do parâmetro',
  `campo` varchar(255) NOT NULL COMMENT 'Identificador do parâmetro',
  `valor` varchar(255) NOT NULL COMMENT 'Valor de resposta do parâmetro',
  `ordem` int NOT NULL DEFAULT 0 COMMENT 'Ordem de exibição',
  `criado_em` datetime DEFAULT (now()) COMMENT 'Data/hora de criação'
);

CREATE TABLE `priorizacao` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela agencia',
  `seguradora_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela seguradora',
  `ordem` tinyint NOT NULL COMMENT 'ordem de prioridade dentro da agência'
);

CREATE TABLE `cooperado` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `numero_documento` varchar(14) NOT NULL COMMENT 'Documento do cooperado (CPF/CNPJ sem formatação)',
  `tipo` enum NOT NULL COMMENT 'ENUM(''Física'', ''Jurídica'') — Tipo de pessoa',
  `nome` varchar(255) NOT NULL COMMENT 'Nome/Razão social do cooperado',
  `nome_fantasia` varchar(255) COMMENT 'Nome fantasia (para PJ)',
  `email` varchar(255) COMMENT 'E-mail de contato'
);

CREATE TABLE `cooperado_agencia_conta` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `cooperado_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela cooperado',
  `agencia_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela agencia',
  `conta_corrente` char(9) NOT NULL COMMENT 'Código da conta corrente'
);

CREATE TABLE `seguro` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_seguradora_id` bigint NOT NULL COMMENT 'Chave estrangeira na tabela agencia_seguradora',
  `cooperado_agencia_conta_id` bigint NOT NULL COMMENT 'Chave estrangeira na tabela cooperado_agencia_conta',
  `ponto_atendimento_id` bigint NOT NULL COMMENT 'Chave estrangeira na tabela ponto de atendimento',
  `usuario_id` bigint COMMENT 'Chave estrangeira na tabela usuario',
  `status` tinyint NOT NULL COMMENT 'Identificador do status (ex.: 1=aberto, 2=quitado, 3=cancelado)',
  `contrato` varchar(10) NOT NULL COMMENT 'Número do contrato do seguro',
  `inicio_vigencia` date COMMENT 'Início de vigência do seguro',
  `fim_vigencia` date COMMENT 'Fim de vigência do seguro',
  `codigo_grupo` int NOT NULL COMMENT 'Identificador do grupo (código) do contrato',
  `quantidade_parcelas` smallint NOT NULL COMMENT 'Quantidade total de parcelas do seguro',
  `vencimento` date COMMENT 'Data de vencimento (padrão do contrato ou próxima parcela)',
  `data_pagamento` datetime COMMENT 'Data de pagamento (quitado)',
  `capital_segurado` decimal(10,2) NOT NULL COMMENT 'Valor do capital segurado',
  `premio_total` decimal(10,2) NOT NULL COMMENT 'Valor do prêmio total do seguro',
  `tipo_pagamento` tinyint NOT NULL COMMENT 'Identificador do tipo de pagamento (ex.: 1=à vista, 2=parcelado)',
  `estorno_proporcional` decimal(10,2) NOT NULL COMMENT 'Valor de estorno proporcional quando aplicável',
  `valor_base` decimal(10,2) COMMENT 'Valor Base Segurado',
  `dps` tinyint(1) COMMENT 'Informação de Exigência de DPS',
  `valor_iof` decimal(10,2) COMMENT 'Valor de IOF'
);

CREATE TABLE `seguro_seguradora` (
  `id` bigint AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `seguro_id` bigint AUTO_INCREMENT COMMENT 'Chave estrangeira na tabela seguro',
  `seguradora_id` bigint AUTO_INCREMENT COMMENT 'Chave estrangeira na tabela seguradora',
  PRIMARY KEY (`id`, `seguro_id`, `seguradora_id`)
);

CREATE TABLE `agencia_seguradora` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_id` int NOT NULL COMMENT 'Identificadora da agencia',
  `seguradora_id` int NOT NULL COMMENT 'Identificador da seguradora'
);

CREATE TABLE `parcelas` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `seguro_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela seguro',
  `status` tinyint NOT NULL COMMENT 'Identificador do status da parcela (ex.: 1=aberta, 2=quitada, 3=cancelada)',
  `numero_parcela` smallint NOT NULL COMMENT 'Número sequencial da parcela dentro do seguro',
  `valor_parcela` decimal(10,2) NOT NULL COMMENT 'Valor nominal da parcela',
  `valor_pago` decimal(10,2) NOT NULL COMMENT 'Valor total pago na parcela',
  `vencimento` date NOT NULL COMMENT 'Data de vencimento da parcela',
  `liquidacao` datetime COMMENT 'Data de liquidação da parcela',
  `data_ultimo_pagamento` datetime COMMENT 'Data do último pagamento registrado'
);

CREATE TABLE `parametrizacao` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `tipo` enum NOT NULL COMMENT 'ENUM(''Por Agência'', ''Global'') — Tipo de parâmetro',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição do item'
);

CREATE TABLE `parametrizacao_resposta` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `parametrizacao_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela parametrizacao',
  `agencia_id` bigint COMMENT 'Chave estrangeira da tabela agencia',
  `resposta` varchar(255) NOT NULL COMMENT 'Valor de resposta do parâmetro'
);

CREATE TABLE `lancamento_efetivar` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela agencia',
  `cooperado_id` bigint NOT NULL COMMENT 'Chave estrangeira da tabela cooperado',
  `conta_corrente` varchar(255) NOT NULL COMMENT 'Conta corrente associada ao lançamento',
  `data_movimentacao` datetime COMMENT 'Data de movimentação financeira',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição do lançamento',
  `valor` decimal(10,2) NOT NULL COMMENT 'Valor do lançamento',
  `data_lancamento` date COMMENT 'Data de lançamento/registro no sistema'
);

CREATE TABLE `integracao_senior` (
  `id` bigint PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador do registro na tabela',
  `agencia_id` int NOT NULL COMMENT 'Chave estrangeira da tabela agência',
  `tipo` enum NOT NULL COMMENT 'ENUM(''Enviado'', ''Erro'') — Status integração',
  `data_movimentacao` datetime NOT NULL COMMENT 'Data de movimentação a integrar',
  `valor` decimal(10,2) NOT NULL COMMENT 'Valor a integrar',
  `lancamento` int NOT NULL COMMENT 'Identificador do lançamento associado',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição do item de integração'
);

CREATE UNIQUE INDEX `ponto_atendimento_index_0` ON `ponto_atendimento` (`agencia_id`, `codigo`);

CREATE INDEX `ponto_atendimento_index_1` ON `ponto_atendimento` (`agencia_id`);

CREATE INDEX `tela_acao_index_2` ON `tela_acao` (`tela_id`);

CREATE INDEX `tela_acao_index_3` ON `tela_acao` (`acao_id`);

CREATE INDEX `tela_acao_perfil_index_4` ON `tela_acao_perfil` (`tela_id`);

CREATE INDEX `tela_acao_perfil_index_5` ON `tela_acao_perfil` (`acao_id`);

CREATE INDEX `tela_acao_perfil_index_6` ON `tela_acao_perfil` (`perfil_id`);

CREATE INDEX `parametro_seguradora_limite_index_7` ON `parametro_seguradora_limite` (`parametro_id`);

CREATE INDEX `parametro_seguradora_limite_index_8` ON `parametro_seguradora_limite` (`limite_id`);

CREATE INDEX `parametro_seguradora_conta_contabil_index_9` ON `parametro_seguradora_conta_contabil` (`parametro_seguradora_id`);

CREATE INDEX `parametro_seguradora_conta_contabil_index_10` ON `parametro_seguradora_conta_contabil` (`conta_contabil_id`);

CREATE UNIQUE INDEX `cooperado_agencia_conta_index_11` ON `cooperado_agencia_conta` (`cooperado_id`, `agencia_id`, `conta_corrente`);

CREATE INDEX `cooperado_agencia_conta_index_12` ON `cooperado_agencia_conta` (`cooperado_id`);

CREATE INDEX `cooperado_agencia_conta_index_13` ON `cooperado_agencia_conta` (`agencia_id`);

CREATE INDEX `cooperado_agencia_conta_index_14` ON `cooperado_agencia_conta` (`conta_corrente`);

ALTER TABLE `agencia` COMMENT = 'Armazena informações das Agências';

ALTER TABLE `ponto_atendimento` COMMENT = 'Armazena informações de Pontos de atendimento de cada Agência';

ALTER TABLE `perfil` COMMENT = 'Tabela de perfis que agrupam permissões de acesso';

ALTER TABLE `usuario` COMMENT = 'Tabela de usuários do sistema';

ALTER TABLE `tela` COMMENT = 'Catálogo de telas (módulos/páginas) do sistema';

ALTER TABLE `acao` COMMENT = 'Catálogo de ações que podem ser executadas nas telas';

ALTER TABLE `tela_acao` COMMENT = 'Junção N:N definindo quais ações pertencem a cada tela';

ALTER TABLE `tela_acao_perfil` COMMENT = 'Permissões: quais perfis podem executar determinadas ações em determinadas telas';

ALTER TABLE `auditoria` COMMENT = 'Tabela de auditoria de eventos do sistema';

ALTER TABLE `seguradora` COMMENT = 'Armazena os dados da seguradora';

ALTER TABLE `parametro_seguradora` COMMENT = 'Parâmetros financeiros/operacionais aplicados por seguradora';

ALTER TABLE `limite` COMMENT = 'Faixas e coeficientes utilizados em regras de cálculo';

ALTER TABLE `parametro_seguradora_limite` COMMENT = 'Junção N:N ligando parâmetros de seguradora às faixas de limites';

ALTER TABLE `contas_contabeis` COMMENT = 'Plano de contas contábeis utilizado nas integrações';

ALTER TABLE `parametro_seguradora_conta_contabil` COMMENT = 'Junção entre parâmetros de seguradora e contas contábeis';

ALTER TABLE `grupo_seguradora` COMMENT = 'Grupos/configurações agregadas por seguradora e agência';

ALTER TABLE `gestao_documento` COMMENT = 'Gestão de documentos';

ALTER TABLE `priorizacao` COMMENT = 'Ordem de priorização de seguradoras por agência';

ALTER TABLE `cooperado` COMMENT = 'Cadastro de cooperados vinculados a uma agência';

ALTER TABLE `cooperado_agencia_conta` COMMENT = 'Junção entre cooperados, agencias e contas';

ALTER TABLE `seguro` COMMENT = 'Contratos de seguros e seus metadados financeiros e relacionamentos';

ALTER TABLE `agencia_seguradora` COMMENT = 'Vinculo para relacionar agencias com seguradoras';

ALTER TABLE `parcelas` COMMENT = 'Parcelas financeiras vinculadas a um seguro';

ALTER TABLE `parametrizacao` COMMENT = 'Parametrizações de campos para preechimento';

ALTER TABLE `parametrizacao_resposta` COMMENT = 'Resposta dos campos de parametrização';

ALTER TABLE `lancamento_efetivar` COMMENT = 'Lançamentos financeiros que serão/são efetivados';

ALTER TABLE `integracao_senior` COMMENT = 'Fila/controle de integrações financeiras com sistema Sênior';

ALTER TABLE `ponto_atendimento` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `usuario` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `usuario` ADD FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`);

ALTER TABLE `usuario` ADD FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`);

ALTER TABLE `tela_acao` ADD FOREIGN KEY (`tela_id`) REFERENCES `tela` (`id`);

ALTER TABLE `tela_acao` ADD FOREIGN KEY (`acao_id`) REFERENCES `acao` (`id`);

ALTER TABLE `tela_acao_perfil` ADD FOREIGN KEY (`tela_id`) REFERENCES `tela` (`id`);

ALTER TABLE `tela_acao_perfil` ADD FOREIGN KEY (`acao_id`) REFERENCES `acao` (`id`);

ALTER TABLE `tela_acao_perfil` ADD FOREIGN KEY (`perfil_id`) REFERENCES `perfil` (`id`);

ALTER TABLE `auditoria` ADD FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`);

ALTER TABLE `auditoria` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `parametro_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `parametro_seguradora_limite` ADD FOREIGN KEY (`parametro_id`) REFERENCES `parametro_seguradora` (`id`);

ALTER TABLE `parametro_seguradora_limite` ADD FOREIGN KEY (`limite_id`) REFERENCES `limite` (`id`);

ALTER TABLE `parametro_seguradora_conta_contabil` ADD FOREIGN KEY (`parametro_seguradora_id`) REFERENCES `parametro_seguradora` (`id`);

ALTER TABLE `parametro_seguradora_conta_contabil` ADD FOREIGN KEY (`conta_contabil_id`) REFERENCES `contas_contabeis` (`id`);

ALTER TABLE `grupo_seguradora` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `gestao_documento` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `priorizacao` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `priorizacao` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `cooperado_agencia_conta` ADD FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`);

ALTER TABLE `cooperado_agencia_conta` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`agencia_seguradora_id`) REFERENCES `agencia_seguradora` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`cooperado_agencia_conta_id`) REFERENCES `cooperado_agencia_conta` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`);

ALTER TABLE `agencia_seguradora` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `agencia_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `parcelas` ADD FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`);

ALTER TABLE `lancamento_efetivar` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `lancamento_efetivar` ADD FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`);

ALTER TABLE `integracao_senior` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);
