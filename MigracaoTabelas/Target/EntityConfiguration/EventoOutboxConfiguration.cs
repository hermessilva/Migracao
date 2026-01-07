using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Seguros.Helpers;


namespace MigracaoTabelas.Target.EntityConfiguration;

public class EventoOutboxConfiguration : BaseEntityConfiguration<EventoOutbox>
{
    public override void Configure(EntityTypeBuilder<EventoOutbox> builder)
    {
        builder.ToTable("evento_outbox", t => t.HasComment("Tabela de eventos para padrão Outbox que armazena um fila de processamento de forma geral."));

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela");

        builder.Property(x => x.IdempotencyKey)
            .HasColumnName("chave_idempotente")
            .HasMaxLength(200)
            .HasComment("Chave de idempotência para evitar processamento duplicado.")
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.Tipo)
                    .HasColumnName("tipo")
                    .HasConversion(v =>
                        v.AsString(),
                        v => EnumHelper.FromString<TipoEvento>(v)
                    )
                    .HasComment("Tipo do evento de negócio."),
                "Processar débito da parcela", "Processar lançamento contábil da parcela"
            )
            .IsRequired();

        builder.Property(x => x.ChaveNegocio)
            .HasColumnName("chave_negocio")
            .HasMaxLength(200)
            .HasComment("Chave de negócio associada como forma de identificação do evento.")
            .IsRequired();

        builder.Property(x => x.Payload)
            .HasColumnName("payload")
            .HasMaxLength(3000)
            .HasComment("Payload do evento (JSON serializado). Armazerna informações do evento.")
            .IsRequired();

        ConfigureEnum(
                builder.Property(x => x.Status)
                    .HasColumnName("status")
                    .HasConversion(v =>
                        v.AsString(),
                        v => EnumHelper.FromString<OutboxStatus>(v)
                    )
                    .HasComment("Status do evento na fila Outbox"),
                "Pendente", "Processando", "Sucesso", "Falha"
            )
            .IsRequired();

        builder.Property(x => x.Tentativas)
            .HasColumnName("tentativas")
            .HasColumnType("tinyint")
            .HasComment("Número de tentativas de processamento.")
            .IsRequired();

        builder.Property(x => x.UltimaAtualizacao)
            .HasColumnName("ultima_atualizacao")
            .HasMaxLength(500)
            .HasComment("Mensagem do último erro ocorrido.");

        builder.Property(x => x.ExternalId)
            .HasColumnName("identificador_externo")
            .HasMaxLength(50)
            .HasComment("Identificador externo associado ao evento.");

        builder.Property(x => x.CriadoEm)
            .HasColumnName("criado_em")
            .HasColumnType("datetime")
            .HasComment("Data e hora de criação do evento.")
            .IsRequired();

        builder.Property(x => x.ProcessadoEm)
            .HasColumnName("processado_em")
            .HasColumnType("datetime")
            .HasComment("Data e hora de processamento do evento.");

        builder.HasIndex(x => x.IdempotencyKey).IsUnique();
    }
}