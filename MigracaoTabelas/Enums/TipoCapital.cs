using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

namespace MigracaoTabelas.Enums
{
    public enum TipoCapital
    {                           
        [Description("Fixo")]
        Fixo,
        [Description("Variável")] 
        Variavel
    }
}