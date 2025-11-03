using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Parametrizações do sistema (globais e por agência)
    /// </summary>
    [Table("parametrizacao")]
    public class Parametrizacao
    {
        /// <summary>
        /// Identificador único do conjunto de parametrizações
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Tipo de parametrização: Por Agência, Global
        /// </summary>
        [Column("tipo")]
        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Descrição do item
        /// </summary>
        [Column("descricao")]
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        // Navegações
        public virtual ICollection<ParametrizacaoResposta> Respostas { get; set; } = new List<ParametrizacaoResposta>();
    }
}

