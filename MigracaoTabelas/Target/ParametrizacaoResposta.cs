using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Resposta dos campos de parametrização
    /// </summary>
    [Table("parametrizacao_resposta")]
    public class ParametrizacaoResposta
    {
        /// <summary>
        /// Identificador único
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK da parametrização
        /// </summary>
        [Column("parametrizacao_id")]
        [Required]
        public ulong ParametrizacaoId { get; set; }

        /// <summary>
        /// FK opcional da agência (para parametrizações específicas)
        /// </summary>
        [Column("agencia_id")]
        public ulong? AgenciaId { get; set; }

        /// <summary>
        /// Valor de resposta do parâmetro
        /// </summary>
        [Column("resposta")]
        [Required]
        [StringLength(255)]
        public string Resposta { get; set; } = string.Empty;

        // Navegações
        [ForeignKey("ParametrizacaoId")]
        public virtual Parametrizacao Parametrizacao { get; set; } = null!;

        [ForeignKey("AgenciaId")]
        public virtual Agencia? Agencia { get; set; }
    }
}

