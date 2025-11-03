using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Junção entre parâmetros de seguradora e contas contábeis
    /// </summary>
    [Table("parametro_seguradora_conta_contabil")]
    public class ParametroSeguradoraContaContabil
    {
        /// <summary>
        /// Identificador único
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK para parâmetros de seguradora
        /// </summary>
        [Column("parametro_seguradora_id")]
        [Required]
        public ulong ParametroSeguradoraId { get; set; }

        /// <summary>
        /// Identificador/código da conta contábil associada
        /// </summary>
        [Column("conta_contabil_id")]
        [Required]
        public ulong ContaContabilId { get; set; }

        // Navegações
        [ForeignKey("ParametroSeguradoraId")]
        public virtual ParametroSeguradora ParametroSeguradora { get; set; } = null!;

        [ForeignKey("ContaContabilId")]
        public virtual ContaContabil ContaContabil { get; set; } = null!;
    }
}

