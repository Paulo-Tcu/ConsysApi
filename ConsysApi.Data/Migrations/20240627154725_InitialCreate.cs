using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ConsysApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PRODUTOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NOME = table.Column<string>(type: "VARCHAR", maxLength: 250, nullable: false),
                    DESCRICAO = table.Column<string>(type: "VARCHAR", maxLength: 500, nullable: false),
                    VALOR = table.Column<decimal>(type: "numeric(14,6)", nullable: false),
                    QUANTIDADE = table.Column<int>(type: "INT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PRODUTOS", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "USUARIOS",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    NOME_ID = table.Column<string>(type: "VARCHAR", maxLength: 25, nullable: false),
                    CRUD = table.Column<string>(type: "VARCHAR", maxLength: 7, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_USUARIOS", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NOME_PRODUTO",
                table: "PRODUTOS",
                column: "NOME");

            migrationBuilder.CreateIndex(
                name: "IX_PK_ID",
                table: "PRODUTOS",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NOME_ID",
                table: "USUARIOS",
                column: "NOME_ID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PK_ID1",
                table: "USUARIOS",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PRODUTOS");

            migrationBuilder.DropTable(
                name: "USUARIOS");
        }
    }
}
