INSERT INTO parametrizacao
(id, identificador, descricao, valor, tipo)
VALUES(1, 'TempoInatividadePortal', 'Definir o tempo de inatividade no Portal para desconectar automaticamente e retornar a tela de login.', '23:50:00', 'TimeSpan');
INSERT INTO parametrizacao
(id, identificador, descricao, valor, tipo)
VALUES(2, 'IOFSeguroPrestamista', 'IOF Seguro Prestamista (%)', '0.0038', 'Decimal');
INSERT INTO parametrizacao
(id, identificador, descricao, valor, tipo)
VALUES(3, 'IRRF_PERCENTUAL', 'Percentual de IRRF aplicado sobre comissões acima do limite', '1.5', 'Decimal');
INSERT INTO parametrizacao
(id, identificador, descricao, valor, tipo)
VALUES(4, 'IRRF_LIMITE_VALOR', 'Valor limite de comissão para aplicação de IRRF', '666.67', 'Decimal');
INSERT INTO parametrizacao
(id, identificador, descricao, valor, tipo)
VALUES(6, 'CodigoLancamentoCashOut', 'Código de lançamento para CashOut - Débito de contratação/parcelas', '1546', 'String');
INSERT INTO parametrizacao
(id, identificador, descricao, valor, tipo)
VALUES(7, 'CodigoLancamentoCashInRecusa', 'Código de lançamento para CashIn - Cancelamento por recusa da seguradora', '2546', 'String');
INSERT INTO parametrizacao
(id, identificador, descricao, valor, tipo)
VALUES(8, 'CodigoLancamentoCashInCancelamentoProporcional', 'Código de lançamento para CashIn - Cancelamentos proporcionais', '2558', 'String');

INSERT INTO acao
(id, descricao)
VALUES(5, 'Ativar');
INSERT INTO acao
(id, descricao)
VALUES(4, 'Buscar');
INSERT INTO acao
(id, descricao)
VALUES(1, 'Criar');
INSERT INTO acao
(id, descricao)
VALUES(3, 'Deletar');
INSERT INTO acao
(id, descricao)
VALUES(2, 'Editar');

INSERT INTO perfil
(id, nome, slug)
VALUES(1, 'Central', 'central');
INSERT INTO perfil
(id, nome, slug)
VALUES(2, 'Cooperativa (Singular)', 'cooperativa_singular');
INSERT INTO perfil
(id, nome, slug)
VALUES(6, 'Corretora (Administrador)', 'corretora_administrador');
INSERT INTO perfil
(id, nome, slug)
VALUES(32, 'Contabilidade', 'contabilidade');


INSERT INTO tela
(id, slug, descricao)
VALUES(1, 'usuario', 'Usuarios');
INSERT INTO tela
(id, slug, descricao)
VALUES(2, 'perfil_permissoes', 'Permissões');
INSERT INTO tela
(id, slug, descricao)
VALUES(3, 'logs', 'Logs');
INSERT INTO tela
(id, slug, descricao)
VALUES(4, 'seguradora', 'Seguradoras');
INSERT INTO tela
(id, slug, descricao)
VALUES(5, 'priorizacao', 'Priorização');
INSERT INTO tela
(id, slug, descricao)
VALUES(6, 'documentos', 'Respositório de Modelos');
INSERT INTO tela
(id, slug, descricao)
VALUES(7, 'seguros', 'Consulta Seguros');
INSERT INTO tela
(id, slug, descricao)
VALUES(8, 'faturamento', 'Faturamento');
INSERT INTO tela
(id, slug, descricao)
VALUES(11, 'integracao_senior', 'Integração Sênior');
INSERT INTO tela
(id, slug, descricao)
VALUES(12, 'relatorios', 'Relatórios');
INSERT INTO tela
(id, slug, descricao)
VALUES(13, 'parametrizacao', 'Parametrização');
INSERT INTO tela
(id, slug, descricao)
VALUES(14, 'cotacoes', 'Cotações');
INSERT INTO tela
(id, slug, descricao)
VALUES(15, 'comissoes', 'Comissões');


INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(1, 1, 1);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(2, 1, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(4, 1, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(12, 1, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(5, 2, 1);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(7, 2, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(11, 2, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(13, 2, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(67, 3, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(59, 4, 1);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(60, 4, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(62, 4, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(63, 4, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(55, 5, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(57, 5, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(58, 5, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(49, 6, 1);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(50, 6, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(52, 6, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(53, 6, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(45, 7, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(46, 7, 3);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(47, 7, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(39, 8, 1);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(40, 8, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(41, 8, 3);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(42, 8, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(43, 8, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(24, 11, 1);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(25, 11, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(26, 11, 3);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(27, 11, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(28, 11, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(19, 12, 1);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(20, 12, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(21, 12, 3);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(22, 12, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(23, 12, 5);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(15, 13, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(17, 13, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(70, 14, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(69, 14, 4);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(72, 15, 2);
INSERT INTO tela_acao
(id, tela_id, acao_id)
VALUES(71, 15, 4);


INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(321, 1, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(206, 1, 1, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(45, 1, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(322, 1, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(207, 1, 2, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(47, 1, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(323, 1, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(60, 1, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(324, 1, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(208, 1, 4, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(61, 1, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(325, 1, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(62, 1, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(260, 2, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(5, 2, 1, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(46, 2, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(261, 2, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(63, 2, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(262, 2, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(64, 2, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(263, 2, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(9, 2, 4, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(65, 2, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(264, 2, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(66, 2, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(247, 3, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(337, 3, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(248, 3, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(249, 3, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(184, 4, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(72, 4, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(185, 4, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(73, 4, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(186, 4, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(74, 4, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(187, 4, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(75, 4, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(188, 4, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(76, 4, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(269, 5, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(117, 5, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(270, 5, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(118, 5, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(271, 5, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(119, 5, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(272, 5, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(120, 5, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(273, 5, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(121, 5, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(314, 6, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(112, 6, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(316, 6, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(113, 6, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(315, 6, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(114, 6, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(313, 6, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(115, 6, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(312, 6, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(116, 6, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(107, 7, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(341, 7, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(108, 7, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(342, 7, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(109, 7, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(343, 7, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(110, 7, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(344, 7, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(111, 7, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(356, 8, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(102, 8, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(358, 8, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(103, 8, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(357, 8, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(104, 8, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(355, 8, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(105, 8, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(354, 8, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(106, 8, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(250, 11, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(255, 11, 1, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(87, 11, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(251, 11, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(256, 11, 2, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(88, 11, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(252, 11, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(257, 11, 3, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(89, 11, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(253, 11, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(258, 11, 4, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(90, 11, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(254, 11, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(259, 11, 5, 2);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(91, 11, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(274, 12, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(82, 12, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(275, 12, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(83, 12, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(276, 12, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(84, 12, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(277, 12, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(85, 12, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(278, 12, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(86, 12, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(167, 13, 1, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(77, 13, 1, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(168, 13, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(78, 13, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(169, 13, 3, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(79, 13, 3, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(170, 13, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(80, 13, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(171, 13, 5, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(81, 13, 5, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(346, 14, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(352, 14, 2, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(347, 14, 4, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(353, 14, 4, 6);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(332, 15, 2, 1);
INSERT INTO tela_acao_perfil
(id, tela_id, acao_id, perfil_id)
VALUES(333, 15, 4, 1);

