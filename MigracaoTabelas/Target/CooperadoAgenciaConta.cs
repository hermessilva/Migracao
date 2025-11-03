using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Junção entre cooperados, agencias e contas
    /// </summary>
    [Table("cooperado_agencia_conta")]
    public class CooperadoAgenciaConta
    {
        /// <summary>
        /// Identificador único de cooperados_agencias_contas
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Identificador do cooperado
        /// </summary>
        [Column("cooperado_id")]
        [Required]
        public ulong CooperadoId { get; set; }

        /// <summary>
        /// Identificador da agencia
        /// </summary>
        [Column("agencia_id")]
        [Required]
        public ulong AgenciaId { get; set; }

        /// <summary>
        /// Código da conta corrente
        /// </summary>
        [Column("conta_corrente")]
        [Required]
        [StringLength(9)]
        public string ContaCorrente { get; set; } = string.Empty;

        // Navegações
        [ForeignKey("CooperadoId")]
        public virtual Cooperado Cooperado { get; set; } = null!;

        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;

        public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
    }
}

