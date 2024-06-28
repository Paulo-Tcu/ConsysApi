using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConsysApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUsuarios : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NOME_ID",
                table: "USUARIOS",
                type: "VARCHAR(25)",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "CRUD",
                table: "USUARIOS",
                type: "VARCHAR(7)",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR",
                oldMaxLength: 7);

            migrationBuilder.AddColumn<string>(
                name: "SENHA",
                table: "USUARIOS",
                type: "VARCHAR(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SENHA",
                table: "USUARIOS");

            migrationBuilder.AlterColumn<string>(
                name: "NOME_ID",
                table: "USUARIOS",
                type: "VARCHAR",
                maxLength: 25,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(25)",
                oldMaxLength: 25);

            migrationBuilder.AlterColumn<string>(
                name: "CRUD",
                table: "USUARIOS",
                type: "VARCHAR",
                maxLength: 7,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "VARCHAR(7)",
                oldMaxLength: 7);
        }
    }
}
