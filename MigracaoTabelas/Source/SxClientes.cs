namespace MigracaoTabelas.Source
{
    public class SxClientes
    {

        /// <summary>
        /// Mapeado de 'CLI_CPFCNPJ'.
        /// Documento (CPF/CNPJ) sem formatação.
        /// (Resultado do COALESCE, nunca será nulo).
        /// </summary>
        public string NumeroDocumento { get; set; } = string.Empty;

        /// <summary>
        /// Mapeado de 'CLI_TIPPES'.
        /// Tipo de pessoa: F (Física) ou J (Jurídica).
        /// </summary>
        public string Tipo { get; set; } = string.Empty;

        /// <summary>
        /// Mapeado de 'cli_nome'.
        /// Nome ou Razão Social.
        /// (Resultado do COALESCE, nunca será nulo).
        /// </summary>
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Mapeado de 'CLI_NFANTA'.
        /// Nome fantasia (pode ser nulo).
        /// </summary>
        public string? NomeFantasia { get; set; }

        /// <summary>
        /// Mapeado de 'CLI_EMAIL'.
        /// E-mail de contato (pode ser nulo).
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// Mapeado de 'AG_CODIGO' (após CAST e COALESCE).
        /// Identificador da agência (nunca será nulo, padrão 0).
        /// </summary>
        public string Agencia { get; set; }

    }
}


