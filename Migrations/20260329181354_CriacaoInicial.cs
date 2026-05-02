using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace GraffitiClassificationApi.Migrations
{
    /// <inheritdoc />
    public partial class CriacaoInicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Faccoes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nome = table.Column<string>(type: "text", nullable: false),
                    Sigla = table.Column<string>(type: "text", nullable: false),
                    Origem = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faccoes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RegistrosPichacao",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    DataRegistro = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DescricaoVisual = table.Column<string>(type: "text", nullable: false),
                    NivelAmeaca = table.Column<string>(type: "text", nullable: false),
                    FaccaoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RegistrosPichacao", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RegistrosPichacao_Faccoes_FaccaoId",
                        column: x => x.FaccaoId,
                        principalTable: "Faccoes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "LocalizacoesExatas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Rua = table.Column<string>(type: "text", nullable: false),
                    Bairro = table.Column<string>(type: "text", nullable: false),
                    Cidade = table.Column<string>(type: "text", nullable: false),
                    Estado = table.Column<string>(type: "text", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    RegistroPichacaoId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalizacoesExatas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocalizacoesExatas_RegistrosPichacao_RegistroPichacaoId",
                        column: x => x.RegistroPichacaoId,
                        principalTable: "RegistrosPichacao",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocalizacoesExatas_RegistroPichacaoId",
                table: "LocalizacoesExatas",
                column: "RegistroPichacaoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RegistrosPichacao_FaccaoId",
                table: "RegistrosPichacao",
                column: "FaccaoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalizacoesExatas");

            migrationBuilder.DropTable(
                name: "RegistrosPichacao");

            migrationBuilder.DropTable(
                name: "Faccoes");
        }
    }
}
