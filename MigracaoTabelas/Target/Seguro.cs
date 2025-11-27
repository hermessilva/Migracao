using System.ComponentModel;

using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target;

public class Seguro
{
    public void Assign(SxEpSegPrestamista source)
    {
        // Campos de PK 'id' são Identity, não são mapeados diretamente da source.

        // (SEG_CANCTIPO: Tipo de Cancelamento -> status: Identificador do status)
        // Regra aplicada: > 0 consideramos cancelado (3), senão aberto (1)

        switch (source.SegCancTipo.Value)
        {
            case 1:
                Status = StatusSeguro.CanceladoPeloCooperado;
                break;
            case 2:
                Status = StatusSeguro.CanceladoPelaCooperativa;
                break;
            case 3:
                Status = StatusSeguro.Sinistro;
                break;
            case 4:
                Status = StatusSeguro.RecusadoPelaSeguradora;
                break;
            case 5:
                Status = StatusSeguro.CanceladoPorAditivo;
                break;
            case 6:
                Status = StatusSeguro.LiquidacaoAntecipada;
                break;
            default:
                Status = StatusSeguro.Ativo;
                break;
        }

        // Mapeamentos com correspondência direta
        Contrato = source.SegContrato ?? string.Empty;                         // (SEG_CONTRATO: Contrato -> contrato: Número do contrato do seguro)
        InicioVigencia = source.SegInicio;                                     // (SEG_INICIO: Início do Contrato -> inicio_vigencia: Início de vigência do seguro)
        FimVigencia = source.SegFim;                                           // (SEG_FIM: Final do Contrato -> fim_vigencia: Fim de vigência do seguro)

        // Quantidade de parcelas: preferir valor informado (SegMeses); fallback para quantidade de parcelas de navegação
        QuantidadeParcelas = (ushort)(source.SegMeses ?? 0);                             // (SEG_MESES: Nº de Meses do Seguro -> quantidade_parcelas: Quantidade total de parcelas)


        // Vencimento: manter como fim de vigência, até haver regra específica
        Vencimento = source.SegFim;                                            // (SEG_FIM: Final do Contrato -> vencimento: Data de vencimento) [assumido]

        // Capital segurado: usar base segurada como principal, com fallback para valor do contrato
        CapitalSegurado = (source.SegBase ?? source.SegVrContrato) ?? 0.00m;   // (SEG_BASE: Valor Base Segurado / SEG_VRCONTRATO: Valor do Contrato -> capital_segurado: Valor do capital segurado)

        // Prêmio total e tipo de pagamento
        PremioTotal = source.SegPremio ?? 0.00m;                               // (SEG_PREMIO: Valor do Seguro -> premio_total: Valor do prêmio total do seguro)
        switch (source.SegModalidade)
        {
            default:
                TipoPagamento = TipoPagamentoSeguro.AVista; // (SEG_MODALIDADE: Modalidade -> tipo_pagamento: Identificador do tipo de pagamento)
                break;
        }

        // Campos que precisam de lógica de negócio para buscar FKs

        // Sem correspondência direta no momento (deixar explícito para avaliação de negócio):
        // this.CodigoGrupo = 0;
        // this.EstornoProporcional = 0.00m;

        // Campos da Source sem correspondência direta no Target (mantidos apenas para contexto de migração):
        // source.SegNome;
        // source.SegNasc;
        // source.SegTipoConta;
        // source.SegCancelamento;
        // source.SegCancMotivo;
        // source.SegIof;
        // source.SegDps;
        // source.SegEfetivacao;
        // source.ConSeq;
        // source.ControleUnimed;
        // source.SqlDeleted;
    }
    public ulong Id { get; set; }
    public ulong ApoliceGrupoSeguradoraId { get; set; }
    public ulong CooperadoAgenciaContaId { get; set; }
    public ulong PontoAtendimentoId { get; set; }
    public ulong? UsuarioId { get; set; }
    public StatusSeguro Status { get; set; }
    public string Contrato { get; set; }
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }
    public uint CodigoGrupo { get; set; }
    public ushort QuantidadeParcelas { get; set; }
    public DateTime? Vencimento { get; set; }
    public decimal CapitalSegurado { get; set; }
    public decimal PremioTotal { get; set; }
    public TipoPagamentoSeguro TipoPagamento { get; set; }
    public decimal EstornoProporcional { get; set; }
    public decimal? ValorBase { get; set; }
    public bool? Dps { get; set; }
    public decimal? ValorIof { get; set; }
    public TipoCapitalSeguro TipoCapital { get; set; }
    public bool Periodicidade30Dias { get; set; }

    public virtual ApoliceGrupoSeguradora ApolicesGruposSeguradoras { get; set; }
    public virtual CooperadoAgenciaConta CooperadosAgenciasContas { get; set; }
    public virtual PontoAtendimento PontosAtendimentos { get; set; }
    public virtual Usuario Usuarios { get; set; }
    public virtual ICollection<Parcela> Parcelas { get; set; } = new List<Parcela>();
    public virtual ICollection<SeguroCancelamento> SegurosCancelamentos { get; set; } = new List<SeguroCancelamento>();

    public void AlterarStatus(StatusSeguro requestStatus)
    {
        Status = requestStatus;
    }

    public static bool PermiteAlteracaoManual(StatusSeguro status)
    {
        return status == StatusSeguro.Ativo
            || status == StatusSeguro.EmAnalisePelaSeguradora
            || status == StatusSeguro.PendenteDeDocumentacao
            || status == StatusSeguro.RecusadoPelaSeguradora
            || status == StatusSeguro.Sinistro;
    }
}

public enum TipoPagamentoSeguro
{
    [Description("À Vista")]
    AVista = 1,

    [Description("Parcelado")]
    Parcelado = 2,

    [Description("Único")]
    Unico = 3
}

public enum TipoCapitalSeguro
{
    [Description("Fixo")]
    Fixo = 1,

    [Description("Variável")]
    Variavel = 2
}

public enum StatusSeguro
{
    [Description("Ativo")]
    Ativo = 1,

    [Description("Em análise pela Seguradora")]
    EmAnalisePelaSeguradora = 2,

    [Description("Pendente de Documentação")]
    PendenteDeDocumentacao = 3,

    [Description("Expiração da Vigência do Seguro")]
    ExpiracaoDaVigenciaDoSeguro = 4,

    [Description("Cancelado pelo Cooperado")]
    CanceladoPeloCooperado = 5,

    [Description("Cancelado pela Cooperativa")]
    CanceladoPelaCooperativa = 6,

    [Description("Sinistro")]
    Sinistro = 7,

    [Description("Recusado pela Seguradora")]
    RecusadoPelaSeguradora = 8,

    [Description("Cancelamento por Prejuízo")]
    CancelamentoPorPrejuizo = 9,

    [Description("Liquidação Antecipada")]
    LiquidacaoAntecipada = 10,

    [Description("Cancelado por Renegociação")]
    CanceladoPorRenegociacao = 11,

    [Description("Cancelado por Aditivo")]
    CanceladoPorAditivo = 12
}