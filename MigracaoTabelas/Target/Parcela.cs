using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using MigracaoTabelas.Source;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Parcelas financeiras vinculadas a um seguro
    /// </summary>
    [Table("parcela")]
    public class Parcela
    {
        /// <summary>
        /// Método para atribuir valores de uma entidade Source para a Target.
        /// </summary>
        /// <param name="source">Entidade de origem (SxEpSegParcela)</param>
        public void Assign(SxEpSegParcela source)
        {
            // Campos de PK 'id' são Identity, não são mapeados diretamente da source.

            // Mapeamentos com correspondência direta:
            this.Status = (byte)(source.SegLiberado ? 1 : 0); // SegLiberado -> Status (0=aberta, 1=quitada)
            this.NumeroParcela = (short)source.SegParcela; // SegParcela -> NumeroParcela
            this.ValorParcela = source.SegValor; // SegValor -> ValorParcela
            this.Vencimento = source.SegVcto; // SegVcto -> Vencimento
            this.Liquidacao = source.SegPgto; // SegPgto -> Liquidacao
            this.DataUltimoPagamento = source.SegPgto; // SegPgto -> DataUltimoPagamento

            if (source.SegPgto.HasValue)
                this.ValorPago = source.SegValor; // Se houver pagamento, atribuir o valor pago igual ao valor da parcela
            else
                this.ValorPago = 0.00m; // Sem pagamento registrado

            // Campos da Source sem correspondência direta no Target:
            // source.CcoConta; // Usado para buscar SeguroId
            // source.SegContrato; // Usado para buscar SeguroId
            // source.ConSeq; // Usado para buscar SeguroId
            // source.SegCancelado; // Sem correspondência no Target
        }
        /// <summary>
        /// Identificador único da parcela
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// FK do seguro ao qual a parcela pertence
        /// </summary>
        [Column("seguro_id")]
        [Required]
        public ulong SeguroId { get; set; }

        /// <summary>
        /// Identificador do status da parcela (ex.: 1=aberta, 2=quitada, 3=cancelada)
        /// </summary>
        [Column("status")]
        [Required]
        public byte Status { get; set; } = 0;

        /// <summary>
        /// Número sequencial da parcela dentro do seguro
        /// </summary>
        [Column("numero_parcela")]
        [Required]
        public short NumeroParcela { get; set; } = 0;

        /// <summary>
        /// Valor nominal da parcela
        /// </summary>
        [Column("valor_parcela", TypeName = "decimal(10,2)")]
        [Required]
        public decimal ValorParcela { get; set; } = 0.00m;

        /// <summary>
        /// Valor total pago na parcela
        /// </summary>
        [Column("valor_pago", TypeName = "decimal(10,2)")]
        [Required]
        public decimal ValorPago { get; set; } = 0.00m;

        /// <summary>
        /// Data de vencimento da parcela
        /// </summary>
        [Column("vencimento")]
        [Required]
        public DateTime Vencimento { get; set; }

        /// <summary>
        /// Data de liquidação da parcela
        /// </summary>
        [Column("liquidacao")]
        public DateTime? Liquidacao { get; set; }

        /// <summary>
        /// Data do último pagamento registrado
        /// </summary>
        [Column("data_ultimo_pagamento")]
        public DateTime? DataUltimoPagamento { get; set; }

        // Navegações
        [ForeignKey("SeguroId")]
        public virtual Seguro Seguro { get; set; } = null!;
    }
}

