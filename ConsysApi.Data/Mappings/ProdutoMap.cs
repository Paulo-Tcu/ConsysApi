using ConsysApi.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsysApi.Data.Mappings
{
    public class ProdutoMap : IEntityTypeConfiguration<Produtos>
    {
        public void Configure(EntityTypeBuilder<Produtos> builder)
        {
            builder.ToTable("PRODUTOS");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.HasIndex(x => x.Id, "IX_PK_ID")
                .IsUnique();

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasColumnName("NOME")
                .HasColumnType("VARCHAR")
                .HasMaxLength(250);

            builder.HasIndex(x => x.Nome, "IX_NOME_PRODUTO");

            builder.Property(x => x.Descricao)
                .HasColumnName("DESCRICAO")
                .HasColumnType("VARCHAR")
                .HasMaxLength(500)
                .HasDefaultValue(null);

            builder.Property(x => x.Valor)
                .IsRequired()
                .HasColumnName("VALOR")
                .HasColumnType("DECIMAL(14,6)");


            builder.Property(x => x.Quantidade)
                .IsRequired()
                .HasColumnName("QUANTIDADE")
                .HasColumnType("INT");
        }
    }
}
