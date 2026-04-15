using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraffitiClassificationApi.Migrations
{
    /// <inheritdoc />
    public partial class CaminhoImagem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CaminhoImagem",
                table: "RegistrosPichacao",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaminhoImagem",
                table: "RegistrosPichacao");
        }
    }
}
