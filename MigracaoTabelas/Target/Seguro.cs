using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target;

public class Seguro
{
    public void Assign(SxEpSegPrestamista source)
    {
        // Campos de PK 'id' são Identity, não são mapeados diretamente da source.

        // (SEG_CANCTIPO: Tipo de Cancelamento -> status: Identificador do status)
        // Regra aplicada: > 0 consideramos cancelado (3), senão aberto (1)
        this.Status = (byte)((source.SegCancTipo.HasValue && source.SegCancTipo.Value > 0) ? 3 : 1);

        // Mapeamentos com correspondência direta
        this.Contrato = source.SegContrato ?? string.Empty;                         // (SEG_CONTRATO: Contrato -> contrato: Número do contrato do seguro)
        this.InicioVigencia = source.SegInicio;                                     // (SEG_INICIO: Início do Contrato -> inicio_vigencia: Início de vigência do seguro)
        this.FimVigencia = source.SegFim;                                           // (SEG_FIM: Final do Contrato -> fim_vigencia: Fim de vigência do seguro)

        // Quantidade de parcelas: preferir valor informado (SegMeses); fallback para quantidade de parcelas de navegação
        this.QuantidadeParcelas = (ushort)(source.SegMeses ?? 0);                             // (SEG_MESES: Nº de Meses do Seguro -> quantidade_parcelas: Quantidade total de parcelas)


        // Vencimento: manter como fim de vigência, até haver regra específica
        this.Vencimento = source.SegFim;                                            // (SEG_FIM: Final do Contrato -> vencimento: Data de vencimento) [assumido]

        // Capital segurado: usar base segurada como principal, com fallback para valor do contrato
        this.CapitalSegurado = (source.SegBase ?? source.SegVrContrato) ?? 0.00m;   // (SEG_BASE: Valor Base Segurado / SEG_VRCONTRATO: Valor do Contrato -> capital_segurado: Valor do capital segurado)

        // Prêmio total e tipo de pagamento
        this.PremioTotal = source.SegPremio ?? 0.00m;                               // (SEG_PREMIO: Valor do Seguro -> premio_total: Valor do prêmio total do seguro)
        this.TipoPagamento = (byte)(source.SegModalidade ?? 0);                             // (SEG_MODALIDADE: Modalidade -> tipo_pagamento: Identificador do tipo de pagamento)

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
    public byte Status { get; set; }
    public string Contrato { get; set; } = null!;
    public DateTime? InicioVigencia { get; set; }
    public DateTime? FimVigencia { get; set; }
    public uint CodigoGrupo { get; set; }
    public ushort QuantidadeParcelas { get; set; }
    public DateTime? Vencimento { get; set; }
    public decimal CapitalSegurado { get; set; }
    public decimal PremioTotal { get; set; }
    public byte TipoPagamento { get; set; }
    public decimal EstornoProporcional { get; set; }
    public decimal? ValorBase { get; set; }
    public bool? Dps { get; set; }
    public decimal? ValorIof { get; set; }

    public virtual CooperadoAgenciaConta CooperadosAgenciasContas { get; set; } = null!;
    public virtual PontoAtendimento PontosAtendimentos { get; set; } = null!;
    public virtual Usuario Usuarios { get; set; }
    public virtual ICollection<Parcela> Parcelas { get; set; } = new List<Parcela>();
}