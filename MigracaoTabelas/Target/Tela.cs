using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Catálogo de telas (módulos/páginas) do sistema
    /// </summary>
    [Table("tela")]
    public class Tela
    {
        /// <summary>
        /// Identificador único da tela
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Descrição/nome da tela
        /// </summary>
        [Column("descricao")]
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        // Navegações
        public virtual ICollection<TelaAcao> TelasAcoes { get; set; } = new List<TelaAcao>();
        public virtual ICollection<TelaAcaoPerfil> TelasAcoesPerfis { get; set; } = new List<TelaAcaoPerfil>();
    }
}

