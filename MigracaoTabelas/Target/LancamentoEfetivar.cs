using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Lançamentos financeiros que serão/são efetivados
    /// </summary>
    [Table("lancamento_efetivar")]
    public class LancamentoEfetivar
    {
        /// <summary>
        /// Identificador único do lançamento a efetivar
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK da agência do lançamento
        /// </summary>
        [Column("agencia_id")]
        [Required]
        public ulong AgenciaId { get; set; }

        /// <summary>
        /// Conta corrente associada ao lançamento
        /// </summary>
        [Column("conta_corrente")]
        [Required]
        [StringLength(255)]
        public string ContaCorrente { get; set; } = string.Empty;

        /// <summary>
        /// FK do cooperado relacionado
        /// </summary>
        [Column("cooperado_id")]
        [Required]
        public ulong CooperadoId { get; set; }

        /// <summary>
        /// Data de movimentação financeira
        /// </summary>
        [Column("data_movimentacao")]
        public DateTime? DataMovimentacao { get; set; }

        /// <summary>
        /// Descrição do lançamento
        /// </summary>
        [Column("descricao")]
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Valor do lançamento
        /// </summary>
        [Column("valor", TypeName = "decimal(10,2)")]
        [Required]
        public decimal Valor { get; set; }

        /// <summary>
        /// Data de lançamento/registro no sistema
        /// </summary>
        [Column("data_lancamento")]
        public DateTime? DataLancamento { get; set; }

        // Navegações
        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;

        [ForeignKey("CooperadoId")]
        public virtual Cooperado Cooperado { get; set; } = null!;
    }
}

