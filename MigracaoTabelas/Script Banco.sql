
CREATE TABLE `agencia` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `codigo` char(4) UNIQUE NOT NULL COMMENT 'Código único da agência no formato de 4 caracteres',
  `nome` varchar(255) UNIQUE NOT NULL COMMENT 'Nome completo da agência',
  `criado_em` datetime NOT NULL DEFAULT (now()) COMMENT 'Data e hora de criação do registro'
);

CREATE TABLE `ponto_atendimento` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `agencia_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia',
  `codigo` char(3) NOT NULL COMMENT 'Código do ponto de atendimento no formato de 3 caracteres, único dentro de uma agência',
  `nome` varchar(255) UNIQUE NOT NULL COMMENT 'Nome completo do ponto de atendimento',
  `criado_em` datetime DEFAULT (now()) COMMENT 'Data e hora de criação do registro'
);

CREATE TABLE `perfil` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `nome` varchar(255) UNIQUE NOT NULL COMMENT 'Nome descritivo do perfil de acesso',
  `slug` varchar(50) UNIQUE NOT NULL COMMENT 'Identificador amigável do perfil para uso em URLs e código'
);

CREATE TABLE `usuario` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `agencia_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia',
  `ponto_atendimento_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela ponto_atendimento',
  `perfil_id` bigint unsigned COMMENT 'Chave estrangeira opcional referenciando a tabela perfil para o perfil principal do usuário',
  `login` varchar(255) NOT NULL COMMENT 'Login de acesso do usuário ao sistema',
  `nome` varchar(255) NOT NULL COMMENT 'Nome completo do usuário',
  `email` varchar(255) NOT NULL COMMENT 'Endereço de e-mail do usuário para contato e notificações',
  `status` enum('Ativo','Inativo') NOT NULL COMMENT 'Indica o status do perfil',
  `criado_em` datetime DEFAULT (now()) COMMENT 'Data e hora de criação do registro'
);

CREATE TABLE `tela` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `slug` varchar(50) UNIQUE NOT NULL COMMENT 'Identificador amigável da tela para uso em URLs e código',
  `descricao` varchar(255) NOT NULL COMMENT 'Nome ou descrição completa da tela'
);

CREATE TABLE `acao` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição da ação disponível (ex.: Visualizar, Editar, Excluir, Aprovar)'
);

CREATE TABLE `tela_acao` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `tela_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela tela',
  `acao_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela acao'
);

CREATE TABLE `tela_acao_perfil` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `tela_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela tela',
  `acao_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela acao',
  `perfil_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela perfil'
);

CREATE TABLE `auditoria` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `usuario_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela usuario que realizou a ação',
  `agencia_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia onde a ação foi realizada',
  `modulo` varchar(255) NOT NULL COMMENT 'Nome do módulo ou área do sistema onde a operação foi realizada',
  `operacao` enum('Insert','Delete','Update') NOT NULL COMMENT 'Tipo da operação realizada: Insert (inserção), Delete (exclusão) ou Update (atualização)',
  `antes` text NOT NULL COMMENT 'Dados do registro antes da alteração em formato JSON ou serializado',
  `criado_em` datetime COMMENT 'Data e hora em que a ação foi registrada'
);

CREATE TABLE `seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `nome` varchar(255) NOT NULL COMMENT 'Nome fantasia da seguradora',
  `cnpj` char(14) UNIQUE NOT NULL COMMENT 'CNPJ da seguradora sem formatação (apenas números)',
  `razao_social` varchar(255) NOT NULL COMMENT 'Razão social completa da seguradora',
  `status` enum('Ativo','Inativo') NOT NULL COMMENT 'Status atual da seguradora: Ativo ou Inativo'
);

CREATE TABLE `condicao_seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `max_meses_contrato` smallint NOT NULL COMMENT 'Quantidade máxima de meses permitidos para vigência do contrato',
  `max_idade` smallint NOT NULL COMMENT 'Idade máxima permitida do proponente para contratação do seguro',
  `porcentagem_cobertura_morte` decimal(5,4) NOT NULL COMMENT 'Percentual de cobertura para sinistro por morte (ex: 1.0000 = 100%)',
  `porcentagem_cobertura_invalidez` decimal(5,4) NOT NULL COMMENT 'Percentual de cobertura para sinistro por invalidez (ex: 0.5000 = 50%)',
  `porcentagem_cobertura_perda_renda` decimal(5,4) NOT NULL COMMENT 'Percentual de cobertura para sinistro por perda de renda (ex: 0.3000 = 30%)',
  `periodicidade_30dias` tinyint(1) NOT NULL DEFAULT (false) COMMENT 'Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)'
);

CREATE TABLE `seguradora_limite` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `idade_inicial` smallint NOT NULL COMMENT 'Idade inicial da faixa etária para aplicação da regra',
  `idade_final` smallint NOT NULL COMMENT 'Idade final da faixa etária para aplicação da regra',
  `valor_maximo` decimal(10,2) NOT NULL COMMENT 'Valor máximo de capital segurado permitido para a faixa',
  `coeficiente` decimal(8,7) NOT NULL COMMENT 'Coeficiente multiplicador para cálculo do prêmio',
  `limite_dps` decimal(10,2) NOT NULL COMMENT 'Valor limite de capital segurado que exige Declaração Pessoal de Saúde (DPS)',
  `descricao_regra` varchar(255) NOT NULL COMMENT 'Descrição textual detalhada da regra aplicada para o limite e DPS'
);

CREATE TABLE `apolice_grupo_seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `agencia_seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia_seguradora',
  `apolice` varchar(255) COMMENT 'Número ou código da apólice contratada',
  `grupo` varchar(255) COMMENT 'Código do grupo dentro da apólice',
  `subgrupo` varchar(255) COMMENT 'Código do subgrupo dentro do grupo da apólice',
  `tipo_capital` enum('Fixo','Variável') NOT NULL COMMENT 'Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)',
  `modalidade_unico` varchar(50) COMMENT 'Identificador ou código da modalidade de pagamento único',
  `modalidade_avista` decimal(10,2) COMMENT 'Valor ou taxa para modalidade de pagamento à vista',
  `modalidade_parcelado` decimal(10,2) COMMENT 'Valor ou taxa para modalidade de pagamento parcelado'
);

CREATE TABLE `proposta_seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `descricao_sequencial` varchar(255) COMMENT 'Descrição ou prefixo do formato do número sequencial da proposta',
  `numero_sequencial` varchar(255) COMMENT 'Último número sequencial utilizado para geração de propostas'
);

CREATE TABLE `contabilizacao_seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `credito_premio_contratacao` varchar(50) NOT NULL COMMENT 'Código da conta contábil de crédito para lançamento do prêmio na contratação',
  `descricao_credito_premio_contratacao` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de crédito do prêmio na contratação',
  `debito_premio_contratacao` varchar(50) NOT NULL COMMENT 'Código da conta contábil de débito para lançamento do prêmio na contratação',
  `descricao_debito_premio_contratacao` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de débito do prêmio na contratação',
  `credito_comissao_contratacao` varchar(50) NOT NULL COMMENT 'Código da conta contábil de crédito para lançamento da comissão na contratação',
  `descricao_credito_comissao_contratacao` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de crédito da comissão na contratação',
  `debito_comissao_contratacao` varchar(50) NOT NULL COMMENT 'Código da conta contábil de débito para lançamento da comissão na contratação',
  `descricao_debito_comissao_contratacao` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de débito da comissão na contratação',
  `credito_cancelamento_comissao_parc_tot` varchar(50) NOT NULL COMMENT 'Código da conta contábil de crédito para cancelamento de comissão parcial ou total',
  `descricao_credito_cancelamento_comissao_parc_tot` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de crédito para cancelamento de comissão parcial ou total',
  `debito_cancelamento_comissao_parc_tot` varchar(50) NOT NULL COMMENT 'Código da conta contábil de débito para cancelamento de comissão parcial ou total',
  `descricao_debito_cancelamento_comissao_parc_tot` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de débito para cancelamento de comissão parcial ou total',
  `credito_cancelamento_comissao_avista` varchar(50) NOT NULL COMMENT 'Código da conta contábil de crédito para cancelamento de comissão de seguro à vista',
  `descricao_credito_cancelamento_comissao_avista` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de crédito para cancelamento de comissão de seguro à vista',
  `debito_cancelamento_comissao_avista` varchar(50) NOT NULL COMMENT 'Código da conta contábil de débito para cancelamento de comissão de seguro à vista',
  `descricao_debito_cancelamento_comissao_avista` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de débito para cancelamento de comissão de seguro à vista',
  `credito_valor_pago` varchar(50) NOT NULL COMMENT 'Código da conta contábil de crédito para registro de valor pago',
  `descricao_credito_valor_pago` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de crédito para registro de valor pago',
  `debito_valor_pago` varchar(50) NOT NULL COMMENT 'Código da conta contábil de débito para registro de valor pago',
  `descricao_debito_valor_pago` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de débito para registro de valor pago',
  `credito_comissao_valor_pago` varchar(50) NOT NULL COMMENT 'Código da conta contábil de crédito para comissão sobre valor pago',
  `descricao_comissao_credito_valor_pago` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de crédito para comissão sobre valor pago',
  `debito_comissao_valor_pago` varchar(50) NOT NULL COMMENT 'Código da conta contábil de débito para comissão sobre valor pago',
  `descricao_comissao_debito_valor_pago` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de débito para comissão sobre valor pago',
  `debito_premio_parcela` varchar(50) NOT NULL COMMENT 'Código da conta contábil debito premio parcela',
  `descricao_debito_premio_parcela` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil de débito para comissão sobre valor pago',
  `credito_premio_parcela` varchar(50) NOT NULL COMMENT 'Código da conta contábil credito premio parcela',
  `descricao_credito_premio_parcela` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil credito premio parcela',
  `debito_comissao_parcela` varchar(50) NOT NULL COMMENT 'Código da conta contábil debito comissao parcela',
  `descricao_debito_comissao_parcela` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil debito comissao parcela',
  `credito_comissao_parcela` varchar(50) NOT NULL COMMENT 'Código da conta contábil credito comissao parcela',
  `descricao_credito_comissao_parcela` varchar(255) NOT NULL COMMENT 'Descrição da conta contábil credito comissao parcela'
);

CREATE TABLE `conta_corrente_seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `conta_corrente_prestamista` varchar(50) NOT NULL COMMENT 'Número da conta corrente para operações de seguro prestamista',
  `descricao_conta_corrente_prestamista` varchar(255) NOT NULL COMMENT 'Descrição da conta corrente para operações de seguro prestamista',
  `conta_cancelamento_prestamista` varchar(50) NOT NULL COMMENT 'Número da conta para lançamentos de cancelamento de seguro prestamista',
  `descricao_conta_cancelamento_prestamista` varchar(255) NOT NULL COMMENT 'Descrição da conta para lançamentos de cancelamento de seguro prestamista',
  `conta_estorno_prestamista` varchar(50) NOT NULL COMMENT 'Número da conta para lançamentos de estorno de seguro prestamista',
  `descricao_conta_estorno_prestamista` varchar(255) NOT NULL COMMENT 'Descrição da conta para lançamentos de estorno de seguro prestamista'
);

CREATE TABLE `comissao_seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `porcentagem_comissao_corretora` decimal(5,4) NOT NULL COMMENT 'Percentual de comissão destinado à corretora (ex: 0.1500 = 15%)',
  `porcentagem_comissao_cooperativa` decimal(5,4) NOT NULL COMMENT 'Percentual de comissão destinado à cooperativa (ex: 0.0500 = 5%)'
);

CREATE TABLE `gestao_documento` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `nome_documento` varchar(255) NOT NULL COMMENT 'Nome ou título do documento a ser gerado',
  `versao` smallint NOT NULL COMMENT 'Número da versão do documento para controle de alterações',
  `label` varchar(255) NOT NULL COMMENT 'Rótulo amigável do campo para exibição ao usuário',
  `campo` varchar(255) NOT NULL COMMENT 'Identificador técnico do campo no documento',
  `valor` varchar(255) NOT NULL COMMENT 'Valor padrão ou resposta configurada para o campo',
  `ordem` int NOT NULL DEFAULT 0 COMMENT 'Ordem de exibição do campo no documento',
  `criado_em` datetime DEFAULT (now()) COMMENT 'Data e hora de criação do registro'
);

CREATE TABLE `cooperado` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `numero_documento` varchar(14) NOT NULL COMMENT 'Documento de identificação do cooperado (CPF com 11 dígitos ou CNPJ com 14 dígitos, sem formatação)',
  `tipo` enum('Física','Jurídica') NOT NULL COMMENT 'Tipo de pessoa: Física (CPF) ou Jurídica (CNPJ)',
  `nome` varchar(255) NOT NULL COMMENT 'Nome completo (pessoa física) ou razão social (pessoa jurídica) do cooperado',
  `nome_fantasia` varchar(255) COMMENT 'Nome fantasia do cooperado (aplicável apenas para pessoa jurídica)',
  `email` varchar(255) COMMENT 'Endereço de e-mail para contato e comunicações com o cooperado'
);

CREATE TABLE `cooperado_agencia_conta` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `cooperado_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela cooperado',
  `agencia_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia',
  `conta_corrente` char(9) NOT NULL COMMENT 'Número da conta corrente do cooperado na agência (9 caracteres)'
);

CREATE TABLE `seguro` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `usuario_id` bigint unsigned COMMENT 'Chave estrangeira referenciando a tabela usuario responsável pela contratação',
  `ponto_atendimento_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela ponto_atendimento onde o seguro foi contratado',
  `cooperado_agencia_conta_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela cooperado_agencia_conta',
  `apolice_grupo_seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela apolice_grupo_seguradora indicando a apolice contratada',
  `seguro_parametro_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguro_parametro com os parâmetros de cálculo',
  `status` ENUM ('Em análise pela Seguradora', 'Pendente de documentação', 'Ativo', 'Expiração da vigência do seguro', 'Cancelado pelo cooperado', 'Cancelado pela cooperativa', 'Sinistro', 'Recusado pela seguradora', 'Cancelamento por prejuízo', 'Liquidação antecipada', 'Cancelado por renegociação', 'Cancelado por aditivo') NOT NULL COMMENT 'Status atual do seguro conforme enum status_seguro',
  `contrato` varchar(10) NOT NULL COMMENT 'Número do contrato de crédito vinculado ao seguro',
  `inicio_vigencia` date COMMENT 'Data de início da vigência do seguro',
  `fim_vigencia` date COMMENT 'Data de término da vigência do seguro',
  `codigo_grupo` int NOT NULL COMMENT 'Código identificador do grupo/produto do seguro',
  `quantidade_parcelas` smallint NOT NULL COMMENT 'Quantidade total de parcelas do seguro',
  `vencimento` date COMMENT 'Data de vencimento base do contrato ou da próxima parcela',
  `capital_segurado` decimal(10,2) NOT NULL COMMENT 'Valor total do capital segurado (valor coberto em caso de sinistro)',
  `premio_total` decimal(10,2) NOT NULL COMMENT 'Valor total do prêmio do seguro a ser pago',
  `tipo_pagamento` ENUM ('À Vista', 'Parcelado', 'Único') NOT NULL COMMENT 'Modalidade de pagamento: À Vista, Parcelado ou Único',
  `estorno_proporcional` decimal(10,2) NOT NULL COMMENT 'Valor de estorno proporcional em caso de cancelamento',
  `valor_base` decimal(10,2) COMMENT 'Valor base utilizado para cálculo do seguro (saldo devedor ou valor financiado)',
  `dps` tinyint(1) COMMENT 'Indica se foi exigida Declaração Pessoal de Saúde (true/false)',
  `valor_iof` decimal(10,2) COMMENT 'Valor do IOF (Imposto sobre Operações Financeiras) incidente sobre o prêmio'
);

CREATE TABLE `seguro_parametro` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `tipo_capital` enum('Fixo','Variável') NOT NULL COMMENT 'Tipo de capital segurado: Fixo (valor constante) ou Variável (acompanha saldo devedor)',
  `periodicidade_30dias` tinyint(1) NOT NULL DEFAULT (false) COMMENT 'Indica se a periodicidade de vencimento é a cada 30 dias (true) ou mensal no mesmo dia (false)',
  `coeficiente` decimal(8,7) NOT NULL COMMENT 'Coeficiente multiplicador utilizado para cálculo do prêmio e estornos',
  `IOF` decimal(5,4) NOT NULL COMMENT 'Porcentual de IOF cobrado no seguro',
  `porcentagem_comissao_corretora` decimal(5,4) NOT NULL COMMENT 'Percentual de comissão destinado à corretora (ex: 0.1500 = 15%)',
  `porcentagem_comissao_cooperativa` decimal(5,4) NOT NULL COMMENT 'Percentual de comissão destinado à cooperativa (ex: 0.0500 = 5%)'
);

CREATE TABLE `agencia_seguradora` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `agencia_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia',
  `seguradora_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguradora',
  `ordem` tinyint NOT NULL COMMENT 'Ordem de prioridade da seguradora dentro da agência (menor = maior prioridade)'
);

CREATE TABLE `parcela` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguro_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguro',
  `status` enum('Em Aberto','Pago','Cancelada') NOT NULL COMMENT 'Status atual da parcela conforme enum status_seguro',
  `numero_parcela` smallint NOT NULL COMMENT 'Número sequencial da parcela dentro do seguro (1, 2, 3...)',
  `valor_parcela` decimal(10,2) NOT NULL COMMENT 'Valor nominal atual da parcela a ser cobrado',
  `valor_original` decimal(10,2) NOT NULL COMMENT 'Valor original da parcela calculado na contratação',
  `valor_pago` decimal(10,2) NOT NULL COMMENT 'Valor total efetivamente pago na parcela',
  `vencimento` date NOT NULL COMMENT 'Data de vencimento da parcela',
  `liquidacao` datetime COMMENT 'Data e hora de liquidação/quitação da parcela',
  `data_ultimo_pagamento` datetime COMMENT 'Data e hora do último pagamento parcial ou total registrado'
);

CREATE TABLE `parametrizacao` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição ou nome do campo de parametrização configurável'
);

CREATE TABLE `parametrizacao_resposta` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `parametrizacao_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela parametrizacao',
  `resposta` varchar(255) NOT NULL COMMENT 'Valor de resposta ou opção disponível para o campo de parametrização'
);

CREATE TABLE `lancamento_efetivar` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `agencia_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia',
  `cooperado_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela cooperado',
  `conta_corrente` varchar(255) NOT NULL COMMENT 'Número da conta corrente do cooperado para débito/crédito do lançamento',
  `data_movimentacao` datetime COMMENT 'Data e hora da movimentação financeira no sistema de origem',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição detalhada do lançamento a ser efetivado',
  `valor` decimal(10,2) NOT NULL COMMENT 'Valor monetário do lançamento a ser efetivado',
  `data_lancamento` date COMMENT 'Data programada para efetivação do lançamento no sistema'
);

CREATE TABLE `integracao_senior` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `agencia_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela agencia',
  `conta_contabil_credito` varchar(255) NOT NULL COMMENT 'Código da conta contábil de crédito para o lançamento',
  `conta_contabil_debito` varchar(255) NOT NULL COMMENT 'Código da conta contábil de débito para o lançamento',
  `status` enum('Enviado','Falha') NOT NULL COMMENT 'Status da integração: Enviado (sucesso) ou Falha (erro no envio)',
  `data_movimentacao` datetime NOT NULL COMMENT 'Data e hora da movimentação a ser integrada',
  `valor` decimal(10,2) NOT NULL COMMENT 'Valor monetário do lançamento a ser integrado',
  `descricao` varchar(255) NOT NULL COMMENT 'Descrição detalhada do lançamento para identificação',
  `tipo_lancamento_contabil` ENUM ('Seguro Prestamista Contratado', 'Comissão Seguro Prestamista Contratado', 'Cancelamento Seguro Prestamista Parcelado', 'Cancelamento Seguro Prestamista Parcelado Comissão', 'Cancelamento Seguro Prestamista À Vista Proporcional Comissão', 'Pagamento Seguro Prestamista', 'Recebimento Comissão Seguro Prestamista', 'Recebimento Premio Seguro Prestamista Parcelado', 'Recebimento Comissão Seguro Prestamista Parcelado') NOT NULL COMMENT 'Tipo do lançamento contábil conforme enum tipo_lancamento',
  `codigo_pa` char(3) NOT NULL COMMENT 'Código do ponto de atendimento de origem do lançamento'
);

CREATE TABLE `seguro_cancelamento` (
  `id` bigint unsigned PRIMARY KEY AUTO_INCREMENT COMMENT 'Identificador único do registro na tabela',
  `seguro_id` bigint unsigned NOT NULL COMMENT 'Chave estrangeira referenciando a tabela seguro',
  `data` date NOT NULL DEFAULT (now()) COMMENT 'Data efetiva do cancelamento do seguro',
  `criado_em` datetime NOT NULL DEFAULT (now()) COMMENT 'Data e hora de criação do registro de cancelamento',
  `motivo` ENUM ('Em análise pela Seguradora', 'Pendente de documentação', 'Ativo', 'Expiração da vigência do seguro', 'Cancelado pelo cooperado', 'Cancelado pela cooperativa', 'Sinistro', 'Recusado pela seguradora', 'Cancelamento por prejuízo', 'Liquidação antecipada', 'Cancelado por renegociação', 'Cancelado por aditivo') NOT NULL COMMENT 'Motivo do cancelamento conforme enum status_seg_cancel',
  `valor_restituir` decimal(10,2) NOT NULL COMMENT 'Valor do prêmio a ser restituído ao cooperado',
  `valor_comissao` decimal(10,2) NOT NULL COMMENT 'Valor da comissão a ser estornada devido ao cancelamento',
  `dias_utilizados` int NOT NULL COMMENT 'Quantidade de dias de vigência utilizados até o cancelamento'
);

CREATE UNIQUE INDEX `ponto_atendimento_index_0` ON `ponto_atendimento` (`agencia_id`, `codigo`);

CREATE UNIQUE INDEX `cooperado_agencia_conta_index_5` ON `cooperado_agencia_conta` (`cooperado_id`, `agencia_id`, `conta_corrente`);

CREATE INDEX `cooperado_agencia_conta_index_6` ON `cooperado_agencia_conta` (`conta_corrente`);

CREATE UNIQUE INDEX `seguro_index_7` ON `seguro` (`seguro_parametro_id`);

ALTER TABLE `agencia` COMMENT = 'Armazena informações cadastrais das agências da cooperativa';

ALTER TABLE `ponto_atendimento` COMMENT = 'Armazena informações dos pontos de atendimento (PAs) vinculados a cada agência';

ALTER TABLE `perfil` COMMENT = 'Tabela de perfis que agrupam permissões de acesso ao sistema';

ALTER TABLE `usuario` COMMENT = 'Tabela de usuários do sistema com suas credenciais e vínculos organizacionais';

ALTER TABLE `tela` COMMENT = 'Catálogo de telas (módulos/páginas) disponíveis no sistema';

ALTER TABLE `acao` COMMENT = 'Catálogo de ações que podem ser executadas nas telas do sistema';

ALTER TABLE `tela_acao` COMMENT = 'Tabela de junção N:N que define quais ações estão disponíveis em cada tela';

ALTER TABLE `tela_acao_perfil` COMMENT = 'Tabela de permissões que define quais perfis podem executar determinadas ações em telas específicas';

ALTER TABLE `auditoria` COMMENT = 'Tabela de auditoria para rastreamento de todas as operações realizadas no sistema';

ALTER TABLE `seguradora` COMMENT = 'Armazena os dados cadastrais das seguradoras parceiras';

ALTER TABLE `condicao_seguradora` COMMENT = 'Parâmetros de condições operacionais e financeiras aplicados por seguradora';

ALTER TABLE `seguradora_limite` COMMENT = 'Define faixas etárias, coeficientes e limites de DPS por seguradora para cálculo de prêmios';

ALTER TABLE `apolice_grupo_seguradora` COMMENT = 'Configurações de apólices e grupos por vínculo agência-seguradora';

ALTER TABLE `proposta_seguradora` COMMENT = 'Controle de numeração sequencial de propostas por seguradora';

ALTER TABLE `contabilizacao_seguradora` COMMENT = 'Configuração de contas contábeis por seguradora para lançamentos de prêmios, comissões e cancelamentos';

ALTER TABLE `conta_corrente_seguradora` COMMENT = 'Configuração de contas correntes por seguradora para operações de seguro prestamista';

ALTER TABLE `comissao_seguradora` COMMENT = 'Configuração de percentuais de comissão por seguradora para corretora e cooperativa';

ALTER TABLE `gestao_documento` COMMENT = 'Gestão de templates e campos de documentos por seguradora para geração automática';

ALTER TABLE `cooperado` COMMENT = 'Cadastro de cooperados (clientes) da cooperativa que podem contratar seguros';

ALTER TABLE `cooperado_agencia_conta` COMMENT = 'Tabela de junção que vincula cooperados às suas contas correntes em cada agência';

ALTER TABLE `seguro` COMMENT = 'Contratos de seguros prestamista com informações de vigência, valores e relacionamentos';

ALTER TABLE `seguro_parametro` COMMENT = 'Parâmetros de contratação do seguro utilizados para cálculos de parcelas, prêmios e cancelamentos';

ALTER TABLE `agencia_seguradora` COMMENT = 'Tabela de vínculo que relaciona agências com seguradoras autorizadas e define prioridade';

ALTER TABLE `parcela` COMMENT = 'Parcelas financeiras de prêmio vinculadas a um contrato de seguro';

ALTER TABLE `parametrizacao` COMMENT = 'Catálogo de campos de parametrização do sistema para configurações dinâmicas';

ALTER TABLE `parametrizacao_resposta` COMMENT = 'Opções de resposta disponíveis para cada campo de parametrização';

ALTER TABLE `lancamento_efetivar` COMMENT = 'Fila de lançamentos financeiros pendentes de efetivação nas contas dos cooperados';

ALTER TABLE `integracao_senior` COMMENT = 'Fila de controle de integrações contábeis com o sistema Sênior (ERP)';

ALTER TABLE `seguro_cancelamento` COMMENT = 'Registro de cancelamentos de seguros com cálculo de restituição e estorno de comissão';

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

ALTER TABLE `condicao_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `seguradora_limite` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `apolice_grupo_seguradora` ADD FOREIGN KEY (`agencia_seguradora_id`) REFERENCES `agencia_seguradora` (`id`);

ALTER TABLE `proposta_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `contabilizacao_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `conta_corrente_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `comissao_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `gestao_documento` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `cooperado_agencia_conta` ADD FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`);

ALTER TABLE `cooperado_agencia_conta` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`apolice_grupo_seguradora_id`) REFERENCES `apolice_grupo_seguradora` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`cooperado_agencia_conta_id`) REFERENCES `cooperado_agencia_conta` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`ponto_atendimento_id`) REFERENCES `ponto_atendimento` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`usuario_id`) REFERENCES `usuario` (`id`);

ALTER TABLE `seguro` ADD FOREIGN KEY (`seguro_parametro_id`) REFERENCES `seguro_parametro` (`id`);

ALTER TABLE `agencia_seguradora` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `agencia_seguradora` ADD FOREIGN KEY (`seguradora_id`) REFERENCES `seguradora` (`id`);

ALTER TABLE `parcela` ADD FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`);

ALTER TABLE `parametrizacao_resposta` ADD FOREIGN KEY (`parametrizacao_id`) REFERENCES `parametrizacao` (`id`);

ALTER TABLE `lancamento_efetivar` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `lancamento_efetivar` ADD FOREIGN KEY (`cooperado_id`) REFERENCES `cooperado` (`id`);

ALTER TABLE `integracao_senior` ADD FOREIGN KEY (`agencia_id`) REFERENCES `agencia` (`id`);

ALTER TABLE `seguro_cancelamento` ADD FOREIGN KEY (`seguro_id`) REFERENCES `seguro` (`id`);
