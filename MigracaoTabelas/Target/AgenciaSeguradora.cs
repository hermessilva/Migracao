using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Vinculo para relacionar agencias com seguradoras
    /// </summary>
    [Table("agencia_seguradora")]
    public class AgenciaSeguradora
    {
        /// <summary>
        /// Identificador único do agencias_seguradoras
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Identificadora da agencia
        /// </summary>
        [Column("agencia_id")]
        [Required]
        public ulong AgenciaId { get; set; }

        /// <summary>
        /// Identificador da seguradora
        /// </summary>
        [Column("seguradora_id")]
        [Required]
        public ulong SeguradoraId { get; set; }

        // Navegações
        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;

        [ForeignKey("SeguradoraId")]
        public virtual Seguradora Seguradora { get; set; } = null!;

        public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
    }
}

