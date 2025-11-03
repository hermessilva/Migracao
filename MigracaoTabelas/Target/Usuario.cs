using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MigracaoTabelas.Target
{
    /// <summary>
    /// Tabela de usuários do sistema com vínculos à agência, ponto de atendimento e perfil (opcional)
    /// </summary>
    [Table("usuario")]
    public class Usuario
    {
        /// <summary>
        /// Identificador único do usuário
        /// </summary>
        [Key]
        [Column("id")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public ulong Id { get; set; }

        /// <summary>
        /// Login de acesso do usuário
        /// </summary>
        [Column("login")]
        [Required]
        [StringLength(255)]
        public string Login { get; set; } = string.Empty;

        /// <summary>
        /// Nome completo do usuário
        /// </summary>
        [Column("nome")]
        [Required]
        [StringLength(255)]
        public string Nome { get; set; } = string.Empty;

        /// <summary>
        /// E-mail do usuário
        /// </summary>
        [Column("email")]
        [Required]
        [StringLength(255)]
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Status do usuário: Ativo, Inativo
        /// </summary>
        [Column("status")]
        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Ativo";

        /// <summary>
        /// Data/hora de criação do registro
        /// </summary>
        [Column("criado_em")]
        public DateTime? CriadoEm { get; set; }

        /// <summary>
        /// FK para a agência vinculada
        /// </summary>
        [Column("agencia_id")]
        [Required]
        public ulong AgenciaId { get; set; }

        /// <summary>
        /// FK para o ponto de atendimento vinculado
        /// </summary>
        [Column("ponto_atendimento_id")]
        [Required]
        public ulong PontoAtendimentoId { get; set; }

        /// <summary>
        /// FK opcional para o perfil principal do usuário
        /// </summary>
        [Column("perfil_id")]
        public ulong? PerfilId { get; set; }

        // Navegações
        [ForeignKey("AgenciaId")]
        public virtual Agencia Agencia { get; set; } = null!;

        [ForeignKey("PontoAtendimentoId")]
        public virtual PontoAtendimento PontoAtendimento { get; set; } = null!;

        [ForeignKey("PerfilId")]
        public virtual Perfil? Perfil { get; set; }

        public virtual ICollection<Auditoria> Auditorias { get; set; } = new List<Auditoria>();
    }
}

