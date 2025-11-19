using MigracaoTabelas.Enums;
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
                Status = StatusSeguro.CanceladoPeloCooperado.AsString();
                break;
            case 2:
                Status = StatusSeguro.CanceladoPelaCooperativa.AsString();
                break;
            case 3:
                Status = StatusSeguro.Sinistro.AsString();
                break;
            case 4:
                Status = StatusSeguro.RecusadoPelaSeguradora.AsString();
                break;
            case 5:
                Status = StatusSeguro.CanceladoPorAditivo.AsString();
                break;
            case 6:
                Status = StatusSeguro.LiquidacaoAntecipada.AsString();
                break;
            default:
                Status = StatusSeguro.Ativo.AsString();
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
                TipoPagamento = Enums.TipoPagamento.AVista.AsString(); // (SEG_MODALIDADE: Modalidade -> tipo_pagamento: Identificador do tipo de pagamento)
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
    public ulong AgenciaSeguradoraId { get; set; }
    public ulong CooperadoAgenciaContaId { get; set; }
    public ulong PontoAtendimentoId { get; set; }
    public ulong? UsuarioId { get; set; }
    public string Status { get; set; }
    public string Contrato { get; set; } = null!;
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }
    public uint CodigoGrupo { get; set; }
    public ushort QuantidadeParcelas { get; set; }
    public DateTime? Vencimento { get; set; }
    public decimal CapitalSegurado { get; set; }
    public decimal PremioTotal { get; set; }
    public string TipoPagamento { get; set; }
    public decimal EstornoProporcional { get; set; }
    public decimal? ValorBase { get; set; }
    public bool? Dps { get; set; }
    public decimal? ValorIof { get; set; }

    public virtual AgenciaSeguradora AgenciasSeguradoras { get; set; } = null!;
    public virtual CooperadoAgenciaConta CooperadosAgenciasContas { get; set; } = null!;
    public virtual PontoAtendimento PontosAtendimentos { get; set; } = null!;
    public virtual Usuario Usuarios { get; set; }
    public virtual ICollection<Parcela> Parcelas { get; set; } = new List<Parcela>();
    public virtual ICollection<SeguroCancelamento> SegurosCancelamentos { get; set; } = new List<SeguroCancelamento>();
}