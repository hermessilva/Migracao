using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Cadastro de cooperados vinculados a uma agência
    /// </summary>
    [Table("cooperado")]
    public class Cooperado
    {
        /// <summary>
        /// Identificador único do cooperado
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Documento do cooperado (CPF/CNPJ sem formatação)
        /// </summary>
        [Column("numero_documento")]
        [Required]
        [StringLength(14)]
        public string NumeroDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Tipo de pessoa: F (Física) ou J (Jurídica)
        /// </summary>
        [Column("tipo")]
        [Required]
        [StringLength(1)]
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Nome/Razão social do cooperado
        /// </summary>
        [Column("nome")]
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Nome fantasia (para PJ)
        /// </summary>
        [Column("nome_fantasia")]
        [StringLength(255)]
        public string? NomeFantasia { get; set; }

        /// <summary>
        /// E-mail de contato
        /// </summary>
        [Column("email")]
        [StringLength(255)]
        public string? Email { get; set; }

        // Navegações
        public virtual ICollection<CooperadoAgenciaConta> CooperadosAgenciasContas { get; set; } = new List<CooperadoAgenciaConta>();
        public virtual ICollection<LancamentoEfetivar> LancamentosEfetivar { get; set; } = new List<LancamentoEfetivar>();
    }
}

