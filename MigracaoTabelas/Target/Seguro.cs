using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Contratos de seguros e seus metadados financeiros e relacionamentos
    /// </summary>
    [Table("seguro")]
    public class Seguro
    {
        public void Assign(SxEpSegPrestamista source)
        {
            // Campos de PK 'id' são Identity, não são mapeados diretamente da source.

            // (SEG_CANCTIPO: Tipo de Cancelamento -> status: Identificador do status)
            // Regra aplicada: > 0 consideramos cancelado (3), senão aberto (1)
            this.Status = (byte)((source.SegCancTipo.HasValue && source.SegCancTipo.Value > 0) ? 3 : 1);

            // Mapeamentos com correspondência direta
            this.Contrato = source.SegContrato ?? string.Empty;                         // (SEG_CONTRATO: Contrato -> contrato: Número do contrato do seguro)
            this.InicioVigencia = source.SegInicio;                                     // (SEG_INICIO: Início do Contrato -> inicio_vigencia: Início de vigência do seguro)
            this.FimVigencia = source.SegFim;                                           // (SEG_FIM: Final do Contrato -> fim_vigencia: Fim de vigência do seguro)

            // Quantidade de parcelas: preferir valor informado (SegMeses); fallback para quantidade de parcelas de navegação
            this.QuantidadeParcelas = (short)(source.SegMeses ?? 0);                             // (SEG_MESES: Nº de Meses do Seguro -> quantidade_parcelas: Quantidade total de parcelas)


            // Vencimento: manter como fim de vigência, até haver regra específica
            this.Vencimento = source.SegFim;                                            // (SEG_FIM: Final do Contrato -> vencimento: Data de vencimento) [assumido]

            // Capital segurado: usar base segurada como principal, com fallback para valor do contrato
            this.CapitalSegurado = (source.SegBase ?? source.SegVrContrato) ?? 0.00m;   // (SEG_BASE: Valor Base Segurado / SEG_VRCONTRATO: Valor do Contrato -> capital_segurado: Valor do capital segurado)

            // Prêmio total e tipo de pagamento
            this.PremioTotal = source.SegPremio ?? 0.00m;                               // (SEG_PREMIO: Valor do Seguro -> premio_total: Valor do prêmio total do seguro)
            this.TipoPagamento = (byte)(source.SegModalidade ?? 0);                             // (SEG_MODALIDADE: Modalidade -> tipo_pagamento: Identificador do tipo de pagamento)

            // Campos que precisam de lógica de negócio para buscar FKs

            // Sem correspondência direta no momento (deixar explícito para avaliação de negócio):
            // this.CodigoGrupo = 0;
            // this.EstornoProporcional = 0.00m;

            // Campos da Source sem correspondência direta no Target (mantidos apenas para contexto de migração):
            // source.SegNome;
            // source.SegNasc;
            // source.SegTipoConta;
            // source.SegCancelamento;
            // source.SegCancMotivo;
            // source.SegIof;
            // source.SegDps;
            // source.SegEfetivacao;
            // source.ConSeq;
            // source.ControleUnimed;
            // source.SqlDeleted;
        }

        /// <summary>
        /// Identificador único do seguro
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Identificador do status (ex.: 1=aberto, 2=quitado, 3=cancelado)
        /// </summary>
        [Column("status")]
        [Required]
        public byte Status { get; set; } = 0;

        /// <summary>
        /// FK do ponto de atendimento
        /// </summary>
        [Column("ponto_atendimento_id")]
        [Required]
        public ulong PontoAtendimentoId { get; set; }

        /// <summary>
        /// Conta corrente relacionada ao seguro
        /// </summary>
        [Column("cooperado_agencia_conta_id")]
        [Required]
        public ulong CooperadoAgenciaContaId { get; set; }

        /// <summary>
        /// Número do contrato do seguro
        /// </summary>
        [Column("contrato")]
        [Required]
        [StringLength(10)]
        public string Contrato { get; set; } = string.Empty;

        /// <summary>
        /// Início de vigência do seguro
        /// </summary>
        [Column("inicio_vigencia")]
        public DateTime? InicioVigencia { get; set; }

        /// <summary>
        /// Fim de vigência do seguro
        /// </summary>
        [Column("fim_vigencia")]
        public DateTime? FimVigencia { get; set; }

        /// <summary>
        /// Fim de vigência do seguro
        /// </summary>
        [Column("valor_iof", TypeName = "decimal(10,2)")]
        public decimal? ValorIof { get; set; } = 0.00m;

        /// <summary>
        /// FK do vínculo entre agencia e seguradora
        /// </summary>
        [Column("agencia_seguradora_id")]
        [Required]
        public ulong AgenciaSeguradoraId { get; set; }

        /// <summary>
        /// Identificador do grupo (código)
        /// </summary>
        [Column("codigo_grupo")]
        [Required]
        public int CodigoGrupo { get; set; } = 0;

        /// <summary>
        /// Quantidade total de parcelas do seguro
        /// </summary>
        [Column("quantidade_parcelas")]
        [Required]
        public short QuantidadeParcelas { get; set; } = 0;

        /// <summary>
        /// Data de vencimento (padrão do contrato ou próxima parcela)
        /// </summary>
        [Column("vencimento")]
        public DateTime? Vencimento { get; set; }


        /// Valor do capital segurado
        /// </summary>
        [Column("capital_segurado", TypeName = "decimal(10,2)")]
        [Required]
        public decimal CapitalSegurado { get; set; } = 0.00m;

        /// <summary>
        /// Valor do prêmio total do seguro
        /// </summary>
        [Column("premio_total", TypeName = "decimal(10,2)")]
        [Required]
        public decimal PremioTotal { get; set; } = 0.00m;

        /// <summary>
        /// Identificador do tipo de pagamento (ex.: 1=à vista, 2=parcelado)
        /// </summary>
        [Column("tipo_pagamento")]
        [Required]
        public byte TipoPagamento { get; set; } = 0;

        /// <summary>
        /// Valor de estorno proporcional quando aplicável
        /// </summary>
        [Column("estorno_proporcional", TypeName = "decimal(10,2)")]
        [Required]
        public decimal EstornoProporcional { get; set; } = 0.00m;

        /// <summary>
        /// Valor Base Segurado
        /// </summary>
        [Column("valor_base", TypeName = "decimal(10,2)")]
        public decimal? ValorBase { get; set; }

        /// <summary>
        /// Informação de Exigência de DPS
        /// </summary>
        [Column("dps")]
        public bool? Dps { get; set; }

        /// <summary>
        /// FK do usuário responsável/criador
        /// </summary>
        [Column("usuario_id")]
        public ulong? UsuarioId { get; set; }

        // Navegações
        [ForeignKey("PontoAtendimentoId")]
        public virtual PontoAtendimento PontoAtendimento { get; set; } = null!;

        [ForeignKey("CooperadoAgenciaContaId")]
        public virtual CooperadoAgenciaConta ContaCorrente { get; set; } = null!;

        [ForeignKey("AgenciaSeguradoraId")]
        public virtual AgenciaSeguradora AgenciaSeguradora { get; set; } = null!;

        [ForeignKey("UsuarioId")]
        public virtual Usuario? Usuario { get; set; }

        public virtual ICollection<Parcela> Parcelas { get; set; } = new List<Parcela>();
    }
}

