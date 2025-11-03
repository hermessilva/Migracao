using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Cadastro de seguradoras
    /// </summary>
    [Table("seguradora")]
    public class Seguradora
    {
        /// <summary>
        /// Identificador único da seguradora
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Nome fantasia da seguradora
        /// </summary>
        [Column("nome")]
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// CNPJ da seguradora (somente dígitos)
        /// </summary>
        [Column("cnpj")]
        [Required]
        [StringLength(14)]
        public string Cnpj { get; set; } = string.Empty;

        /// <summary>
        /// Razão social da seguradora
        /// </summary>
        [Column("razao_social")]
        [Required]
        [StringLength(255)]
        public string RazaoSocial { get; set; } = string.Empty;

        /// <summary>
        /// status de contato
        /// </summary>
        [Column("status")]
        [StringLength(10)]
        public string Status { get; set; } = "Ativo";
        /// <summary>
        /// Codigo de contato
        /// </summary>
        [Column("codigo")]
        [StringLength(4)]
        public string Codigo { get; set; }

        // Navegações
        public virtual ICollection<ParametroSeguradora> ParametrosSeguradora { get; set; } = new List<ParametroSeguradora>();
        public virtual ICollection<GestaoDocumento> GestaoDocumentos { get; set; } = new List<GestaoDocumento>();
        public virtual ICollection<Priorizacao> Priorizacoes { get; set; } = new List<Priorizacao>();
    }
}

