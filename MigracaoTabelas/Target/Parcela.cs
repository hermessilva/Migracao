using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target;

public class Parcela
{
    public void Assign(SxEpSegParcela source)
    {
        // Campos de PK 'id' são Identity, não são mapeados diretamente da source.

        // Mapeamentos com correspondência direta:
        this.Status = (byte)(source.SegLiberado ? 1 : 0); // SegLiberado -> Status (0=aberta, 1=quitada)
        this.NumeroParcela = (ushort)source.SegParcela; // SegParcela -> NumeroParcela
        this.ValorParcela = source.SegValor; // SegValor -> ValorParcela
        this.Vencimento = source.SegVcto; // SegVcto -> Vencimento
        this.Liquidacao = source.SegPgto; // SegPgto -> Liquidacao
        this.DataUltimoPagamento = source.SegPgto; // SegPgto -> DataUltimoPagamento

        if (source.SegPgto.HasValue)
            this.ValorPago = source.SegValor; // Se houver pagamento, atribuir o valor pago igual ao valor da parcela
        else
            this.ValorPago = 0.00m; // Sem pagamento registrado

        // Campos da Source sem correspondência direta no Target:
        // source.CcoConta; // Usado para buscar SeguroId
        // source.SegContrato; // Usado para buscar SeguroId
        // source.ConSeq; // Usado para buscar SeguroId
        // source.SegCancelado; // Sem correspondência no Target
    }
    
    public ulong Id { get; set; }
    public ulong SeguroId { get; set; }
    public byte Status { get; set; }
    public ushort NumeroParcela { get; set; }
    public decimal ValorParcela { get; set; }
    public decimal ValorPago { get; set; }
    public DateTime Vencimento { get; set; }
    public DateTime? Liquidacao { get; set; }
    public DateTime? DataUltimoPagamento { get; set; }

    public virtual Seguro Seguro { get; set; } = null!;
}