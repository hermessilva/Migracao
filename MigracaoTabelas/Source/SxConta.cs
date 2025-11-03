using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Source
{
    /// <summary>
    /// Projeção keyless da consulta que retorna PA_CODIGO e CCO_CONTA
    /// </summary>
    public class SxContas
    {
        [Column("CPFCNPJ")]
        public string CPFCNPJ { get; set; }

        [Column("CONTA")]
        public string Codigo { get; set; }

        public string PaCodigo { get; set; }
    }
}


