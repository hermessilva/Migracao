using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Plano de contas contábeis utilizado nas integrações
    /// </summary>
    [Table("contas_contabeis")]
    public class ContaContabil
    {
        /// <summary>
        /// Identificador único da conta contábil
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Código da conta contábil
        /// </summary>
        [Column("conta")]
        [Required]
        [StringLength(50)]
        public string Conta { get; set; } = string.Empty;

        /// <summary>
        /// Descrição da conta contábil
        /// </summary>
        [Column("descricao")]
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        /// <summary>
        /// Conta contábil de crédito
        /// </summary>
        [Column("conta_credito")]
        [Required]
        public bool ContaCredito { get; set; }

        /// <summary>
        /// Conta contábil de débito
        /// </summary>
        [Column("conta_debito")]
        [Required]
        public bool ContaDebito { get; set; }

        /// <summary>
        /// Conta contábil de crédito para comissão
        /// </summary>
        [Column("conta_credito_comissao")]
        [Required]
        public bool ContaCreditoComissao { get; set; }

        /// <summary>
        /// Conta contábil de débito para comissão
        /// </summary>
        [Column("conta_debito_comissao")]
        [Required]
        public bool ContaDebitoComissao { get; set; }

        // Navegações
        public virtual ICollection<ParametroSeguradoraContaContabil> ParametrosContasContabeis { get; set; } = new List<ParametroSeguradoraContaContabil>();
    }
}

