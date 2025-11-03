using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Tabela de auditoria de eventos do sistema
    /// </summary>
    [Table("auditoria")]
    public class Auditoria
    {
        /// <summary>
        /// Identificador único do log
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Usuário responsável pela ação
        /// </summary>
        [Column("usuario_id")]
        [Required]
        public ulong UsuarioId { get; set; }

        /// <summary>
        /// Agência relacionada ao evento
        /// </summary>
        [Column("agencia_id")]
        [Required]
        public ulong AgenciaId { get; set; }

        /// <summary>
        /// Módulo/área do sistema afetado
        /// </summary>
        [Column("modulo")]
        [Required]
        [StringLength(255)]
        public string Modulo { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de operação: Insert, Delete, Update
        /// </summary>
        [Column("operacao")]
        [Required]
        [StringLength(50)]
        public string Operacao { get; set; } = string.Empty;

        /// <summary>
        /// Payload de dados antes da alteração
        /// </summary>
        [Column("antes")]
        [Required]
        [StringLength(255)]
        public string Antes { get; set; } = string.Empty;

        /// <summary>
        /// Payload de dados depois da alteração
        /// </summary>
        [Column("depois")]
        [Required]
        [StringLength(255)]
        public string Depois { get; set; } = string.Empty;

        /// <summary>
        /// Data/hora da ação registrada
        /// </summary>
        [Column("criado_em")]
        public DateTime? CriadoEm { get; set; }

        // Navegações
        [ForeignKey("UsuarioId")]
        public virtual Usuario Usuario { get; set; } = null!;

        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;
    }
}

