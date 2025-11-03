using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Gestão de documentos
    /// </summary>
    [Table("gestao_documento")]
    public class GestaoDocumento
    {
        /// <summary>
        /// Identificador único do documento gerenciado
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK da seguradora proprietária do documento
        /// </summary>
        [Column("seguradora_id")]
        [Required]
        public ulong SeguradoraId { get; set; }

        /// <summary>
        /// Nome/título do documento
        /// </summary>
        [Column("nome_documento")]
        [Required]
        [StringLength(255)]
        public string NomeDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Versão do documento
        /// </summary>
        [Column("versao")]
        [Required]
        public short Versao { get; set; }

        /// <summary>
        /// Nome amigável do parâmetro
        /// </summary>
        [Column("label")]
        [Required]
        [StringLength(255)]
        public string Label { get; set; } = string.Empty;

        /// <summary>
        /// Identificador do parâmetro
        /// </summary>
        [Column("campo")]
        [Required]
        [StringLength(255)]
        public string Campo { get; set; } = string.Empty;

        /// <summary>
        /// Valor de resposta do parâmetro
        /// </summary>
        [Column("valor")]
        [Required]
        [StringLength(255)]
        public string Valor { get; set; } = string.Empty;

        /// <summary>
        /// Ordem de exibição
        /// </summary>
        [Column("ordem")]
        [Required]
        public int Ordem { get; set; } = 0;

        /// <summary>
        /// Data/hora de criação
        /// </summary>
        [Column("criado_em")]
        public DateTime? CriadoEm { get; set; }

        // Navegações
        [ForeignKey("SeguradoraId")]
        public virtual Seguradora Seguradora { get; set; } = null!;
    }
}

