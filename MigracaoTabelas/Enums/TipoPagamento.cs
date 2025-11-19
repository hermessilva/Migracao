using System.ComponentModel;

using Microsoft.AspNetCore.Mvc;

namespace MigracaoTabelas.Enums
{
    public enum TipoPagamento
    {
        [Description("À Vista")]
        AVista,
        
        [Description("Parcelado")]
        Parcelado,
        
        [Description("Único")]
        Unico
    }
}