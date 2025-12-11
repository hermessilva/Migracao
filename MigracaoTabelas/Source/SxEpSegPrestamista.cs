using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Source
{
    /// <summary>
    /// Seguro Prestamista
    /// </summary>
    [Table("ep_segprestamista")]
    public class SxEpSegPrestamista
    {
        /// <summary>
        /// Código da Seguradora
        /// </summary>
        [Column("PST_CODIGO")]
        [StringLength(4)]
        public string? PstCodigo { get; set; } // char(4) DEFAULT NULL -> string?

        /// <summary>
        /// Código da Conta Corrente
        /// </summary>
        [Column("CCO_CONTA")]
        [StringLength(9)]
        public string? CcoConta { get; set; } // char(9) DEFAULT NULL -> string?

        /// <summary>
        /// CPF do Cliente ou Diretor
        /// </summary>
        [Column("SEG_CPF")]
        [StringLength(11)]
        public string? SegCpf { get; set; } // char(11) DEFAULT NULL -> string?

        /// <summary>
        /// Nome do Cliente ou Diretor
        /// </summary>
        [Column("SEG_NOME")]
        [StringLength(50)]
        public string? SegNome { get; set; } // char(50) DEFAULT NULL -> string?

        /// <summary>
        /// Nascimento do Cliente ou Diretor
        /// </summary>
        [Column("SEG_NASC")]
        public DateTime? SegNasc { get; set; } // date DEFAULT NULL -> DateTime?

        /// <summary>
        /// Contrato
        /// </summary>
        [Column("SEG_CONTRATO")]
        [StringLength(10)]
        public string? SegContrato { get; set; } // char(10) DEFAULT NULL -> string?

        /// <summary>
        /// Modalidade: 1-Ch.Especial  2-ECR   3-Desconto  4-Empréstimo
        /// </summary>
        [Column("SEG_MODALIDADE")]
        // É NOT NULL (int) pois tem DEFAULT e o DDL não diz NULL
        public int? SegModalidade { get; set; } = 4; // int DEFAULT '4' -> int

        /// <summary>
        /// Tipo de Conta:  F-Física   J-Jurídica
        /// </summary>
        [Column("SEG_TIPOCONTA")]
        [StringLength(1)]
        public string? SegTipoConta { get; set; } // char(1) DEFAULT NULL -> string?

        /// <summary>
        /// Início do Contrato
        /// </summary>
        [Column("SEG_INICIO")]
        public DateTime? SegInicio { get; set; } // date DEFAULT NULL -> DateTime?

        /// <summary>
        /// Final do Contrato
        /// </summary>
        [Column("SEG_FIM")]
        public DateTime? SegFim { get; set; } // date DEFAULT NULL -> DateTime?

        /// <summary>
        /// Número de Meses do Seguro (pode ser diferente das parcelas)
        /// </summary>
        [Column("SEG_MESES")]
        public int? SegMeses { get; set; } // int DEFAULT NULL -> int?

        /// <summary>
        /// Data do Cancelamento do Seguro ou Liquidação do Contrato
        /// </summary>
        [Column("SEG_CANCELAMENTO")]
        public DateTime? SegCancelamento { get; set; } // date DEFAULT NULL -> DateTime?

        /// <summary>
        /// Tipo de Cancelamento:   0-Não cancelado    1-Cancelamento solicitado pelo cooperado    2-Cancelamento solicitado pela Agência    3-Sinistro    4-Rejeitado pela Seguradora
        /// </summary>
        [Column("SEG_CANCTIPO")]
        // É NOT NULL (int) pois tem DEFAULT e o DDL não diz NULL
        public int? SegCancTipo { get; set; } = 0; // int DEFAULT '0' -> int

        /// <summary>
        /// Descrição do motivo de cancelamento
        /// </summary>
        [Column("SEG_CANCMOTIVO")]
        public byte[]? SegCancMotivo { get; set; } // mediumblob COMMENT -> byte[]?

        /// <summary>
        /// Valor do Contrato
        /// </summary>
        [Column("SEG_VRCONTRATO", TypeName = "decimal(12,2)")]
        public decimal? SegVrContrato { get; set; } = 0.00m; // decimal NOT NULL (por ter DEFAULT) -> decimal

        /// <summary>
        /// Saldo Atualizado do Contrato
        /// </summary>
        [Column("Saldo", TypeName = "decimal(12,2)")]
        public decimal Saldo { get; set; } 

        /// <summary>
        /// Valor Base Segurado
        /// </summary>
        [Column("SEG_BASE", TypeName = "decimal(12,2)")]
        public decimal? SegBase { get; set; } = 0.00m; // decimal NOT NULL -> decimal

        /// <summary>
        /// Valor do Seguro
        /// </summary>
        [Column("SEG_PREMIO", TypeName = "decimal(12,2)")]
        public decimal? SegPremio { get; set; } = 0.00m; // decimal NOT NULL -> decimal

        /// <summary>
        /// Valor de IOF
        /// </summary>
        [Column("SEG_IOF", TypeName = "decimal(12,2)")]
        public decimal? SegIof { get; set; } = 0.00m; // decimal NOT NULL -> decimal

        /// <summary>
        /// Informação de Exigência de DPS
        /// </summary>
        [Column("SEG_DPS")]
        // bool (não-anulável) é o mapeamento mais provável para tinyint(1) NOT NULL
        public bool? SegDps { get; set; } = false; // tinyint(1) DEFAULT '0' -> bool

        /// <summary>
        /// Data de Efetivação do Contrato
        /// </summary>
        [Column("SEG_EFETIVACAO")]
        public DateTime? SegEfetivacao { get; set; } // date DEFAULT NULL -> DateTime?

        /// <summary>
        /// Sequencial para um mesmmo contrato, no caso de um aditivo de renegociação
        /// </summary>
        [Column("CON_SEQ")]
        public short ConSeq { get; set; } = 0; // smallint NOT NULL -> short

        /// <summary>
        /// Número de controle - UNIMED
        /// </summary>
        [Column("controle_unimed")]
        public int? ControleUnimed { get; set; } // int DEFAULT NULL -> int?

        /// <summary>
        /// Chave primária técnica (auto incremento)
        /// </summary>
        [Key]
        [Column("sql_rowid")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long SqlRowId { get; set; } // bigint NOT NULL AUTO_INCREMENT -> long

        /// <summary>
        /// Flag de registro excluído logicamente (F/T)
        /// </summary>
        [Column("sql_deleted")]
        [Required]
        [StringLength(1)]
        public string SqlDeleted { get; set; } = "F"; // enum NOT NULL -> string

    }
}