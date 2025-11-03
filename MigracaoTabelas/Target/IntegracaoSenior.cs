using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Fila/controle de integrações financeiras com sistema Sênior
    /// </summary>
    [Table("integracao_senior")]
    public class IntegracaoSenior
    {
        /// <summary>
        /// Identificador único do registro de integração
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
        /// Tipo de status de integração: Enviado, Erro
        /// </summary>
        [Column("tipo")]
        [Required]
        [StringLength(50)]
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Data de movimentação a integrar
        /// </summary>
        [Column("data_movimentacao")]
        [Required]
        public DateTime DataMovimentacao { get; set; }

        /// <summary>
        /// Valor a integrar
        /// </summary>
        [Column("valor", TypeName = "decimal(10,2)")]
        [Required]
        public decimal Valor { get; set; }

        /// <summary>
        /// Identificador do lançamento associado
        /// </summary>
        [Column("lancamento")]
        [Required]
        public int Lancamento { get; set; }

        /// <summary>
        /// Descrição do item de integração
        /// </summary>
        [Column("descricao")]
        [Required]
        [StringLength(255)]
        public string Descricao { get; set; } = string.Empty;

        // Navegações
        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;
    }
}
