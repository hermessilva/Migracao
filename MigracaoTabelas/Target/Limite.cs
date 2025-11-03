using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Faixas e coeficientes utilizados em regras de cálculo
    /// </summary>
    [Table("limite")]
    public class Limite
    {
        /// <summary>
        /// Identificador único do limite/faixa
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Idade inicial da faixa
        /// </summary>
        [Column("idade_inicial")]
        [Required]
        public short IdadeInicial { get; set; }

        /// <summary>
        /// Idade final da faixa
        /// </summary>
        [Column("idade_final")]
        [Required]
        public short IdadeFinal { get; set; }

        /// <summary>
        /// Valor associado à faixa
        /// </summary>
        [Column("valor", TypeName = "decimal(10,2)")]
        [Required]
        public decimal Valor { get; set; }

        /// <summary>
        /// Coeficiente aplicado para cálculos
        /// </summary>
        [Column("coeficiente", TypeName = "decimal(5,4)")]
        [Required]
        public decimal Coeficiente { get; set; }

        // Navegações
        public virtual ICollection<ParametroSeguradoraLimite> ParametrosLimites { get; set; } = new List<ParametroSeguradoraLimite>();
    }
}

