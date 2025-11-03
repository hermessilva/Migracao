using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Ordem de priorização de seguradoras por agência
    /// </summary>
    [Table("priorizacao")]
    public class Priorizacao
    {
        /// <summary>
        /// Identificador único da priorização
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK da agência para a qual a ordem se aplica
        /// </summary>
        [Column("agencia_id")]
        [Required]
        public ulong AgenciaId { get; set; }

        /// <summary>
        /// FK da seguradora priorizada
        /// </summary>
        [Column("seguradora_id")]
        [Required]
        public ulong SeguradoraId { get; set; }

        /// <summary>
        /// Posição/ordem de prioridade dentro da agência
        /// </summary>
        [Column("ordem")]
        [Required]
        public byte Ordem { get; set; }

        // Navegações
        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;

        [ForeignKey("SeguradoraId")]
        public virtual Seguradora Seguradora { get; set; } = null!;
    }
}

