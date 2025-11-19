using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MigracaoTabelas.Target.EntityConfiguration;

public sealed class SeguroCancelamentoConfiguration : IEntityTypeConfiguration<SeguroCancelamento>
{
    public void Configure(EntityTypeBuilder<SeguroCancelamento> pBuilder)
    {
        pBuilder.ToTable("seguro_cancelamento", pT => pT.HasComment("Registro de cancelamento de seguro"));

        pBuilder.HasKey(pX => pX.Id);
        pBuilder.Property(pX => pX.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd()
            .HasComment("Identificador do registro na tabela")
            .IsRequired();

        pBuilder.Property(pX => pX.SeguroId)
            .HasColumnName("seguro_id")
            .HasComment("Chave estrangeira da tabela seguro")
            .IsRequired();

        pBuilder.Property(pX => pX.Data)
            .HasColumnName("data")
            .HasColumnType("date")
            .HasComment("Data do cancelamento")
            .IsRequired();

        pBuilder.Property(pX => pX.CriadoEm)
            .HasColumnName("criado_em")
            .HasColumnType("datetime")
            .HasComment("Data/hora de criação do registro")
            .IsRequired();

        pBuilder.Property(pX => pX.Motivo)
            .HasColumnName("motivo")
            .HasColumnType("enum('Expiração da Vigência do Seguro','Cancelado pelo Cooperado','Cancelado pela Cooperativa','Sinistro','Recusado pela Seguradora','Cancelamento por Prejuízo','Liquidação Antecipada','Cancelado por Renegociação','Cancelado por Aditivo')")
            .HasComment("Motivo do cancelamento")
            .IsRequired();

        pBuilder.Property(pX => pX.ValorRestituir)
            .HasColumnName("valor_restituir")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor que foi restituido ao segurado")
            .IsRequired();

        pBuilder.Property(pX => pX.ValorComissao)
            .HasColumnName("valor_comissao")
            .HasColumnType("decimal(10,2)")
            .HasComment("Valor que foi laçando de abatimento de comissão")
            .IsRequired();

        pBuilder.Property(pX => pX.DiasUtilizados)
            .HasColumnName("dias_utilizados")
            .HasColumnType("int")
            .HasComment("Quantidade de dias que foi utilizado o seguro")
            .IsRequired();

        pBuilder.HasOne(pX => pX.Seguros)
            .WithMany(pX => pX.SegurosCancelamentos)
            .HasForeignKey(pX => pX.SeguroId)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
