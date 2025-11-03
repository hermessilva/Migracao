using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Tabela de perfis que agrupam permissões de acesso
    /// </summary>
    [Table("perfil")]
    public class Perfil
    {
        /// <summary>
        /// Identificador único do perfil
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Nome do perfil (único)
        /// </summary>
        [Column("nome")]
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        // Navegações
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<TelaAcaoPerfil> TelasAcoesPerfis { get; set; } = new List<TelaAcaoPerfil>();
    }
}

