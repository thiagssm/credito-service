using CreditoService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CreditoService.Infrastructure.Persistence.Configurations;

public sealed class CreditoConfiguration : IEntityTypeConfiguration<Credito>
{
    public void Configure(EntityTypeBuilder<Credito> builder)
    {
        builder.ToTable("credito");
        builder.HasKey(credito => credito.Id);

        builder.Property(credito => credito.Id).HasColumnName("id").ValueGeneratedOnAdd();
        builder.Property(credito => credito.NumeroCredito).HasColumnName("numero_credito").HasMaxLength(50).IsRequired();
        builder.Property(credito => credito.NumeroNfse).HasColumnName("numero_nfse").HasMaxLength(50).IsRequired();
        builder.Property(credito => credito.DataConstituicao).HasColumnName("data_constituicao").IsRequired();
        builder.Property(credito => credito.ValorIssqn).HasColumnName("valor_issqn").HasPrecision(15, 2).IsRequired();
        builder.Property(credito => credito.TipoCredito).HasColumnName("tipo_credito").HasMaxLength(50).IsRequired();
        builder.Property(credito => credito.SimplesNacional).HasColumnName("simples_nacional").IsRequired();
        builder.Property(credito => credito.Aliquota).HasColumnName("aliquota").HasPrecision(5, 2).IsRequired();
        builder.Property(credito => credito.ValorFaturado).HasColumnName("valor_faturado").HasPrecision(15, 2).IsRequired();
        builder.Property(credito => credito.ValorDeducao).HasColumnName("valor_deducao").HasPrecision(15, 2).IsRequired();
        builder.Property(credito => credito.BaseCalculo).HasColumnName("base_calculo").HasPrecision(15, 2).IsRequired();

        builder.HasIndex(credito => credito.NumeroCredito).IsUnique();
        builder.HasIndex(credito => credito.NumeroNfse);
    }
}
