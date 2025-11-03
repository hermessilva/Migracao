using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Pontos de atendimento vinculados a cada agência
    /// </summary>
    [Table("ponto_atendimento")]
    public class PontoAtendimento
    {
        /// <summary>
        /// Identificador único do ponto de atendimento
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
        /// Código do ponto de atendimento dentro da agência
        /// </summary>
        [Column("codigo")]
        [Required]
        [StringLength(3)]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Nome do ponto de atendimento (único)
        /// </summary>
        [Column("nome")]
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Data/hora de criação do registro
        /// </summary>
        [Column("criado_em")]
        public DateTime? CriadoEm { get; set; } = DateTime.Now;

        // Navegações
        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;

        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<Seguro> Seguros { get; set; } = new List<Seguro>();
    }
}

