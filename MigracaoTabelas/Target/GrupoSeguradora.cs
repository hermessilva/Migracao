using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Grupos/configurações agregadas por seguradora e agência
    /// </summary>
    [Table("grupo_seguradora")]
    public class GrupoSeguradora
    {
        /// <summary>
        /// Identificador único do grupo da seguradora
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK da agência
        /// </summary>
        [Column("agencia_id")]
        [Required]
        public ulong AgenciaId { get; set; }

        /// <summary>
        /// Campo genérico para configuração de modalidade à vista
        /// </summary>
        [Column("avista", TypeName = "decimal(10,2)")]
        public decimal? Avista { get; set; }

        /// <summary>
        /// Campo genérico para configuração de modalidade parcelado
        /// </summary>
        [Column("parcelado", TypeName = "decimal(10,2)")]
        public decimal? Parcelado { get; set; }

        // Navegações
        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;
    }
}

