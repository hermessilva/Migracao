using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Source
{
    /// <summary>
    /// Projeção keyless da consulta que retorna PA_CODIGO e CCO_CONTA
    /// </summary>
    public class SxAgencia
    {
        [Column("CODIGO")]
        public string Codigo { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }

        [Column("SIGLA")]
        public string Sigla { get; set; }
    }
}


