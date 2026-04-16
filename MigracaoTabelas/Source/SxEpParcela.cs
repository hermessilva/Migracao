#nullable enable
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Source
{
    [Table("ep_parcela")]
    public class SxEpParcela
    {
        [Key]
        [Column("sql_rowid")]
        public long SqlRowid
        {
            get; set;
        }

        [Column("CCO_CONTA")]
        [StringLength(9)]
        public string CcoConta
        {
            get; set;
        }

        [Column("CON_NDOC")]
        [StringLength(10)]
        public string ConNdoc
        {
            get; set;
        }

        [Column("CON_SEQ")]
        public ushort ConSeq
        {
            get; set;
        } // smallint(2) unsigned

        [Column("EMP_PARCELA")]
        public int? EmpParcela
        {
            get; set;
        }

        [Column("EMP_VALOR")]
        public decimal? EmpValor
        {
            get; set;
        }

        [Column("PERCENTUAL_AMORTIZACAO")]
        public decimal PercentualAmortizacao
        {
            get; set;
        }

        [Column("EMP_IOF")]
        public decimal? EmpIof
        {
            get; set;
        }

        [Column("EMP_SLDDEV")]
        public decimal? EmpSldDev
        {
            get; set;
        }

        [Column("EMP_VLRINI")]
        public decimal? EmpVlrIni
        {
            get; set;
        }

        [Column("EMP_VLRJUR")]
        public decimal? EmpVlrJur
        {
            get; set;
        }

        [Column("EMP_VLRJURPOS")]
        public decimal EmpVlrJurPos
        {
            get; set;
        }

        [Column("EMP_VLRJURPRE")]
        public decimal EmpVlrJurPre
        {
            get; set;
        }

        [Column("EMP_VLRSEG")]
        public decimal? EmpVlrSeg
        {
            get; set;
        }

        [Column("EMP_PAGSEG")]
        public DateTime? EmpPagSeg
        {
            get; set;
        }

        [Column("EMP_PAGPOS")]
        public DateTime? EmpPagPos
        {
            get; set;
        }

        [Column("EMP_CALCULO")]
        public DateTime? EmpCalculo
        {
            get; set;
        }

        [Column("EMP_VCTO")]
        public DateTime? EmpVcto
        {
            get; set;
        }

        [Column("EMP_PGTO")]
        public DateTime? EmpPgto
        {
            get; set;
        }

        [Column("EMP_CRELI")]
        public DateTime? EmpCreli
        {
            get; set;
        }

        [Column("sql_deleted")]
        public string SqlDeleted
        {
            get; set;
        } // Mapeado como string para tratar o ENUM('F', 'T')

    }
}