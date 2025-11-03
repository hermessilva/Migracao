using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Catálogo de ações que podem ser executadas nas telas
    /// </summary>
    [Table("acao")]
    public class Acao
    {
        /// <summary>
        /// Identificador único da ação
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Descrição da ação (ex.: Visualizar, Editar, Excluir)
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

