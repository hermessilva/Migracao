using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Source
{

    /// <summary>
    /// Parcelas do Seguro Prestamista
    /// </summary>
    [Table("ep_segparcela")]
    public class SxEpSegParcela
    {
        /// <summary>
        /// Conta corrente do cooperado
        /// </summary>
        [Column("cco_conta")]
        [Required]
        [StringLength(9)]
        public string CcoConta { get; set; } = string.Empty;

        /// <summary>
        /// Numero de Contrato
        /// </summary>
        [Column("seg_contrato")]
        [Required]
        [StringLength(10)]
        public string SegContrato { get; set; } = string.Empty;

        /// <summary>
        /// Sequencial em caso de Contrato de aditivo
        /// </summary>
        [Column("con_seq")]
        [Required]
        public short ConSeq { get; set; } = 0;

        /// <summary>
        /// Numero da Parcela
        /// </summary>
        [Column("seg_parcela")]
        [Required]
        public int SegParcela { get; set; }

        /// <summary>
        /// Valor da Parcela
        /// </summary>
        [Column("seg_valor", TypeName = "decimal(12,2)")]
        [Required]
        public decimal SegValor { get; set; }

        /// <summary>
        /// Data de Vencimento
        /// </summary>
        [Column("seg_vcto")]
        [Required]
        public DateTime SegVcto { get; set; }

        /// <summary>
        /// Data de Pagamento
        /// </summary>
        [Column("seg_pgto")]
        public DateTime? SegPgto { get; set; }

        /// <summary>
        /// Data de Cancelamento
        /// </summary>
        [Column("seg_cancelado")]
        public DateTime? SegCancelado { get; set; }

        /// <summary>
        /// Se foi Liberado: 0 - Na mesa, 1 - Liberado
        /// </summary>
        [Column("seg_liberado")]
        [Required]
        public bool SegLiberado { get; set; } = false;

        [Key]
        [Column("sql_rowid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SqlRowId { get; set; }

    }
}

