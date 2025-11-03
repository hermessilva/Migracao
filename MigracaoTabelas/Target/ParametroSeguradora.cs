using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Parâmetros financeiros/operacionais aplicados por seguradora
    /// </summary>
    [Table("parametro_seguradora")]
    public class ParametroSeguradora
    {
        /// <summary>
        /// Identificador único do parâmetro de seguradora
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK para seguradoras (parâmetros por seguradora)
        /// </summary>
        [Column("seguradora_id")]
        [Required]
        public ulong SeguradoraId { get; set; }

        /// <summary>
        /// Quantidade máxima de meses permitidos para o contrato
        /// </summary>
        [Column("max_meses_contrato")]
        [Required]
        public short MaxMesesContrato { get; set; }

        /// <summary>
        /// Valor mínimo aceito para DPS
        /// </summary>
        [Column("valor_minimo_dps", TypeName = "decimal(10,2)")]
        [Required]
        public decimal ValorMinimoDps { get; set; }

        /// <summary>
        /// Dias mínimos para contratação do seguro
        /// </summary>
        [Column("min_dias_contratar")]
        [Required]
        public short MinDiasContratar { get; set; }

        /// <summary>
        /// Idade máxima do proponente
        /// </summary>
        [Column("max_idade")]
        [Required]
        public short MaxIdade { get; set; }

        /// <summary>
        /// Identificador do tipo de operação (tabela auxiliar)
        /// </summary>
        [Column("tipo_operacao")]
        [Required]
        public int TipoOperacao { get; set; }

        /// <summary>
        /// Capital segurado padrão
        /// </summary>
        [Column("capital", TypeName = "decimal(10,2)")]
        [Required]
        public decimal Capital { get; set; }

        /// <summary>
        /// Percentual de cobertura para morte
        /// </summary>
        [Column("porcentagem_cobertura_morte", TypeName = "decimal(5,4)")]
        [Required]
        public decimal PorcentagemCoberturaMorte { get; set; }

        /// <summary>
        /// Percentual de cobertura para invalidez
        /// </summary>
        [Column("porcentagem_cobertura_invalidez", TypeName = "decimal(5,4)")]
        [Required]
        public decimal PorcentagemCoberturaInvalidez { get; set; }

        /// <summary>
        /// Código da apólice do tipo único
        /// </summary>
        [Column("apolice_unico")]
        [Required]
        [StringLength(255)]
        public string ApoliceUnico { get; set; } = string.Empty;

        /// <summary>
        /// Código da apólice para pagamento à vista
        /// </summary>
        [Column("apolice_avista")]
        [Required]
        [StringLength(255)]
        public string ApoliceAvista { get; set; } = string.Empty;

        /// <summary>
        /// Código da apólice para pagamento parcelado
        /// </summary>
        [Column("apolice_parcelado")]
        [Required]
        [StringLength(255)]
        public string ApoliceParcelado { get; set; } = string.Empty;

        /// <summary>
        /// Percentual de comissão da corretora
        /// </summary>
        [Column("porcentagem_comissao_corretora", TypeName = "decimal(5,4)")]
        [Required]
        public decimal PorcentagemComissaoCorretora { get; set; }

        /// <summary>
        /// Percentual de comissão da cooperativa
        /// </summary>
        [Column("porcentagem_comissao_cooperativa", TypeName = "decimal(5,4)")]
        [Required]
        public decimal PorcentagemComissaoCooperativa { get; set; }

        // Navegações
        [ForeignKey("SeguradoraId")]
        public virtual Seguradora Seguradora { get; set; } = null!;

        public virtual ICollection<ParametroSeguradoraLimite> ParametrosLimites { get; set; } = new List<ParametroSeguradoraLimite>();
        public virtual ICollection<ParametroSeguradoraContaContabil> ParametrosContasContabeis { get; set; } = new List<ParametroSeguradoraContaContabil>();
    }
}

