using ConsysApi.Data.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConsysApi.Data.Mappings
{
    public class UsuariosMap : IEntityTypeConfiguration<Usuarios>
    {
        public void Configure(EntityTypeBuilder<Usuarios> builder)
        {
            builder.ToTable("USUARIOS");
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => x.Id, "IX_PK_ID")
                .IsUnique();

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseIdentityColumn();

            builder.Property(x => x.NomeId)
                .IsRequired()
                .HasColumnName("NOME_ID")
                .HasColumnType("VARCHAR(25)")
                .HasMaxLength(25);

            builder.HasIndex(x => x.NomeId, "IX_NOME_ID")
                .IsUnique();

            builder.Property(x => x.Crud)
                .IsRequired()
                .HasColumnName("CRUD")
                .HasColumnType("VARCHAR(7)")
                .HasMaxLength(7);

            builder.Property(x => x.Senha)
                .IsRequired()
                .HasColumnName("SENHA")
                .HasColumnType("VARCHAR(500)")
                .HasMaxLength(500);
        }
    }
}
