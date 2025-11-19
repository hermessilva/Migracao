namespace MigracaoTabelas.Target
{
    public class SeguroCancelamento
    {
        public ulong Id { get; set; }
        public ulong SeguroId { get; set; }
        public DateOnly Data { get; set; }
        public DateTime CriadoEm { get; set; }
        public string Motivo { get; set; } = null!;
        public decimal ValorRestituir { get; set; }
        public decimal ValorComissao { get; set; }
        public int DiasUtilizados { get; set; }

        public virtual Seguro Seguros { get; set; } = null!;
    }
}
