#nullable enable
namespace MigracaoTabelas.Source
{
    public class SxSeguradoras
    {
        /// <summary>Código da Seguradora Prestamista (PST_CODIGO)</summary>
        public string Codigo { get; set; } = string.Empty;

        /// <summary>Nome da Seguradora Prestamista (PST_NOME)</summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>Utiliza em Cheque Especial (PST_CHEQUE)</summary>
        public bool Cheque { get; set; }

        /// <summary>Utiliza em ECR (PST_ECR)</summary>
        public bool Ecr { get; set; }

        /// <summary>Utiliza em Desconto (PST_DESCONTO)</summary>
        public bool Desconto { get; set; }

        /// <summary>Utiliza em Empréstimo (PST_EMPRESTIMO)</summary>
        public bool Emprestimo { get; set; }

        /// <summary>Valor Limite até 30 anos (PST_LIMITE30)</summary>
        public decimal Limite30 { get; set; }

        /// <summary>Coeficiente para até 30 anos (PST_COEF30)</summary>
        public decimal Coef30 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 30 anos (PST_DPS30)</summary>
        public bool Dps30 { get; set; }

        /// <summary>Valor Limite até 35 anos (PST_LIMITE35)</summary>
        public decimal Limite35 { get; set; }

        /// <summary>Coeficiente para até 35 anos (PST_COEF35)</summary>
        public decimal Coef35 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 35 anos (PST_DPS35)</summary>
        public bool Dps35 { get; set; }

        /// <summary>Valor Limite até 40 anos (PST_LIMITE40)</summary>
        public decimal Limite40 { get; set; }

        /// <summary>Coeficiente para até 40 anos (PST_COEF40)</summary>
        public decimal Coef40 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 40 anos (PST_DPS40)</summary>
        public bool Dps40 { get; set; }

        /// <summary>Valor Limite até 45 anos (PST_LIMITE45)</summary>
        public decimal Limite45 { get; set; }

        /// <summary>Coeficiente para até 45 anos (PST_COEF45)</summary>
        public decimal Coef45 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 45 anos (PST_DPS45)</summary>
        public bool Dps45 { get; set; }

        /// <summary>Valor Limite até 50 anos (PST_LIMITE50)</summary>
        public decimal Limite50 { get; set; }

        /// <summary>Coeficiente para até 50 anos (PST_COEF50)</summary>
        public decimal Coef50 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 50 anos (PST_DPS50)</summary>
        public bool Dps50 { get; set; }

        /// <summary>Valor Limite até 55 anos (PST_LIMITE55)</summary>
        public decimal Limite55 { get; set; }

        /// <summary>Coeficiente para até 55 anos (PST_COEF55)</summary>
        public decimal Coef55 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 55 anos (PST_DPS55)</summary>
        public bool Dps55 { get; set; }

        /// <summary>Valor Limite até 60 anos (PST_LIMITE60)</summary>
        public decimal Limite60 { get; set; }

        /// <summary>Coeficiente para até 60 anos (PST_COEF60)</summary>
        public decimal Coef60 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 60 anos (PST_DPS60)</summary>
        public bool Dps60 { get; set; }

        /// <summary>Valor Limite até 65 anos (PST_LIMITE65)</summary>
        public decimal Limite65 { get; set; }

        /// <summary>Coeficiente para até 65 anos (PST_COEF65)</summary>
        public decimal Coef65 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 65 anos (PST_DPS65)</summary>
        public bool Dps65 { get; set; }

        /// <summary>Valor Limite até 70 anos (PST_LIMITE70)</summary>
        public decimal Limite70 { get; set; }

        /// <summary>Coeficiente para até 70 anos (PST_COEF70)</summary>
        public decimal Coef70 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 70 anos (PST_DPS70)</summary>
        public bool Dps70 { get; set; }

        /// <summary>Valor Limite até 75 anos (PST_LIMITE75)</summary>
        public decimal Limite75 { get; set; }

        /// <summary>Coeficiente para até 75 anos (PST_COEF75)</summary>
        public decimal Coef75 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 75 anos (PST_DPS75)</summary>
        public bool Dps75 { get; set; }

        /// <summary>Valor Limite até 80 anos (PST_LIMITE80)</summary>
        public decimal Limite80 { get; set; }

        /// <summary>Coeficiente para até 80 anos (PST_COEF80)</summary>
        public decimal Coef80 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 80 anos (PST_DPS80)</summary>
        public bool Dps80 { get; set; }

        /// <summary>Valor Limite até 85 anos (PST_LIMITE85)</summary>
        public decimal Limite85 { get; set; }

        /// <summary>Coeficiente para até 85 anos (PST_COEF85)</summary>
        public decimal Coef85 { get; set; }

        /// <summary>Exige Declaração Pessoal de Saúde até 85 anos (PST_DPS85)</summary>
        public bool Dps85 { get; set; }

        /// <summary>Número máximo de meses para o contrato (PST_MAXMESES)</summary>
        public int MaxMeses { get; set; }

        /// <summary>Número mínimo de dias para o contrato (PST_MINDIAS)</summary>
        public int MinDias { get; set; }

        /// <summary>Valor Mínimo para obrigar a DPS (PST_VALORDPS)</summary>
        public decimal ValorDps { get; set; }

        /// <summary>Limite máximo de idade (Idade atual + período do contrato) (PST_LIMITEIDADE)</summary>
        public int LimiteIdade { get; set; }

        /// <summary>Flag de quem está editando o registro (EDITANDO)</summary>
        public string? Editando { get; set; }

        /// <summary>Flag para identificar registro ativo ou inativo (PST_ATIVO)</summary>
        public bool Ativo { get; set; }

        /// <summary>Conta Contábil de Credito de SEGP contratado (CONTA_CONTABIL_CREDITO)</summary>
        public string ContaContabilCredito { get; set; } = string.Empty;

        /// <summary>Conta Contábil de Débito de SEGP contratado (CONTA_CONTABIL_DEBITO)</summary>
        public string ContaContabilDebito { get; set; } = string.Empty;

        /// <summary>Conta Contábil de Credito de SEGP COMISSAO contratado (CONTA_CONTABIL_CREDITO_COMISSAO)</summary>
        public string ContaContabilCreditoComissao { get; set; } = string.Empty;

        /// <summary>Conta Contábil de Débito de SEGP COMISSAO contratado (CONTA_CONTABIL_DEBITO_COMISSAO)</summary>
        public string ContaContabilDebitoComissao { get; set; } = string.Empty;

        /// <summary>Porcentagem das cooperativas referente ao Seguro prestamista (PORCENTAGEM_COMISSAO)</summary>
        public decimal PorcentagemComissao { get; set; }
    }
}