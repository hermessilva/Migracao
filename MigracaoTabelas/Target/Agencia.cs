using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Cadastro de agências
    /// </summary>
    [Table("agencia")]
    public class Agencia
    {
        /// <summary>
        /// Identificador único da agência
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Código único da agência
        /// </summary>
        [Column("codigo")]
        [Required]
        [StringLength(4)]
        public string Codigo { get; set; } = string.Empty;

        /// <summary>
        /// Nome da agência (único)
        /// </summary>
        [Column("nome")]
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// Data/hora de criação do registro
        /// </summary>
        [Column("criado_em")]
        public DateTime? CriadoEm { get; set; }

        // Navegações
        public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
        public virtual ICollection<PontoAtendimento> PontosAtendimento { get; set; } = new List<PontoAtendimento>();
        public virtual ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
        public virtual ICollection<GrupoSeguradora> GruposSeguradora { get; set; } = new List<GrupoSeguradora>();
        public virtual ICollection<Priorizacao> Priorizacoes { get; set; } = new List<Priorizacao>();
        public virtual ICollection<CooperadoAgenciaConta> CooperadosAgenciasContas { get; set; } = new List<CooperadoAgenciaConta>();
        public virtual ICollection<LancamentoEfetivar> LancamentosEfetivar { get; set; } = new List<LancamentoEfetivar>();
        public virtual ICollection<IntegracaoSenior> IntegracaoSenior { get; set; } = new List<IntegracaoSenior>();
        public virtual ICollection<ParametrizacaoResposta> ParametrizacaoRespostas { get; set; } = new List<ParametrizacaoResposta>();
    }
}

