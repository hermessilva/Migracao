using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Permissões: quais perfis podem executar determinadas ações em determinadas telas
    /// </summary>
    [Table("tela_acao_perfil")]
    public class TelaAcaoPerfil
    {
        /// <summary>
        /// Identificador único
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK da tela
        /// </summary>
        [Column("tela_id")]
        [Required]
        public ulong TelaId { get; set; }

        /// <summary>
        /// FK da ação
        /// </summary>
        [Column("acao_id")]
        [Required]
        public ulong AcaoId { get; set; }

        /// <summary>
        /// FK do perfil
        /// </summary>
        [Column("perfil_id")]
        [Required]
        public ulong PerfilId { get; set; }

        // Navegações
        [ForeignKey("TelaId")]
        public virtual Tela Tela { get; set; } = null!;

        [ForeignKey("AcaoId")]
        public virtual Acao Acao { get; set; } = null!;

        [ForeignKey("PerfilId")]
        public virtual Perfil Perfil { get; set; } = null!;
    }
}

