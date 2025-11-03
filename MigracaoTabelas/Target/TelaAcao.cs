using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Junção N:N definindo quais ações pertencem a cada tela
    /// </summary>
    [Table("tela_acao")]
    public class TelaAcao
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

        // Navegações
        [ForeignKey("TelaId")]
        public virtual Tela Tela { get; set; } = null!;

        [ForeignKey("AcaoId")]
        public virtual Acao Acao { get; set; } = null!;
    }
}

