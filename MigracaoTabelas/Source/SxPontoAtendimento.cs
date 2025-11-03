using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Source
{
    /// <summary>
    /// Projeção keyless da consulta que retorna PA_CODIGO e CCO_CONTA
    /// </summary>
    public class SxPontoAtendimento
    {
        [Column("CODIGO")]
        public string Codigo { get; set; }

        [Column("AGENCIA")]
        public string Agencia { get; set; }

        [Column("NOME")]
        public string Nome { get; set; }
    }
}


