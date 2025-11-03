using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Junção N:N ligando parâmetros de seguradora às faixas de limites
    /// </summary>
    [Table("parametro_seguradora_limite")]
    public class ParametroSeguradoraLimite
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
        [Column("parametro_id")]
        [Required]
        public ulong ParametroId { get; set; }

        /// <summary>
        /// FK para faixas de limites
        /// </summary>
        [Column("limite_id")]
        [Required]
        public ulong LimiteId { get; set; }

        // Navegações
        [ForeignKey("ParametroId")]
        public virtual ParametroSeguradora Parametro { get; set; } = null!;

        [ForeignKey("LimiteId")]
        public virtual Limite Limite { get; set; } = null!;
    }
}

