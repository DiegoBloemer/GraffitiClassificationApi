using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraffitiClassificationApi.Migrations
{
    /// <inheritdoc />
    public partial class RenameTablesAndCollunsToSnakeCase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocalizacoesExatas_RegistrosPichacao_RegistroPichacaoId",
                table: "LocalizacoesExatas");

            migrationBuilder.DropForeignKey(
                name: "FK_RegistrosPichacao_Faccoes_FaccaoId",
                table: "RegistrosPichacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_RegistrosPichacao",
                table: "RegistrosPichacao");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LocalizacoesExatas",
                table: "LocalizacoesExatas");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Faccoes",
                table: "Faccoes");

            migrationBuilder.RenameTable(
                name: "RegistrosPichacao",
                newName: "graffitis");

            migrationBuilder.RenameTable(
                name: "LocalizacoesExatas",
                newName: "graffitis_location");

            migrationBuilder.RenameTable(
                name: "Faccoes",
                newName: "gangs");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "graffitis",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "NivelAmeaca",
                table: "graffitis",
                newName: "threat_level");

            migrationBuilder.RenameColumn(
                name: "FaccaoId",
                table: "graffitis",
                newName: "gang_id");

            migrationBuilder.RenameColumn(
                name: "DescricaoVisual",
                table: "graffitis",
                newName: "visual_description");

            migrationBuilder.RenameColumn(
                name: "DataRegistro",
                table: "graffitis",
                newName: "registered_at");

            migrationBuilder.RenameColumn(
                name: "CaminhoImagem",
                table: "graffitis",
                newName: "image_path");

            migrationBuilder.RenameIndex(
                name: "IX_RegistrosPichacao_FaccaoId",
                table: "graffitis",
                newName: "IX_graffitis_gang_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "graffitis_location",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Rua",
                table: "graffitis_location",
                newName: "street");

            migrationBuilder.RenameColumn(
                name: "RegistroPichacaoId",
                table: "graffitis_location",
                newName: "graffiti_id");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "graffitis_location",
                newName: "lon");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "graffitis_location",
                newName: "lat");

            migrationBuilder.RenameColumn(
                name: "Estado",
                table: "graffitis_location",
                newName: "state");

            migrationBuilder.RenameColumn(
                name: "Cidade",
                table: "graffitis_location",
                newName: "city");

            migrationBuilder.RenameColumn(
                name: "Bairro",
                table: "graffitis_location",
                newName: "neighborhood");

            migrationBuilder.RenameIndex(
                name: "IX_LocalizacoesExatas_RegistroPichacaoId",
                table: "graffitis_location",
                newName: "IX_graffitis_location_graffiti_id");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "gangs",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Sigla",
                table: "gangs",
                newName: "acronym");

            migrationBuilder.RenameColumn(
                name: "Origem",
                table: "gangs",
                newName: "origin");

            migrationBuilder.RenameColumn(
                name: "Nome",
                table: "gangs",
                newName: "name");

            migrationBuilder.AddPrimaryKey(
                name: "PK_graffitis",
                table: "graffitis",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_graffitis_location",
                table: "graffitis_location",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_gangs",
                table: "gangs",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_graffitis_gangs_gang_id",
                table: "graffitis",
                column: "gang_id",
                principalTable: "gangs",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_graffitis_location_graffitis_graffiti_id",
                table: "graffitis_location",
                column: "graffiti_id",
                principalTable: "graffitis",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_graffitis_gangs_gang_id",
                table: "graffitis");

            migrationBuilder.DropForeignKey(
                name: "FK_graffitis_location_graffitis_graffiti_id",
                table: "graffitis_location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_graffitis_location",
                table: "graffitis_location");

            migrationBuilder.DropPrimaryKey(
                name: "PK_graffitis",
                table: "graffitis");

            migrationBuilder.DropPrimaryKey(
                name: "PK_gangs",
                table: "gangs");

            migrationBuilder.RenameTable(
                name: "graffitis_location",
                newName: "LocalizacoesExatas");

            migrationBuilder.RenameTable(
                name: "graffitis",
                newName: "RegistrosPichacao");

            migrationBuilder.RenameTable(
                name: "gangs",
                newName: "Faccoes");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "LocalizacoesExatas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "street",
                table: "LocalizacoesExatas",
                newName: "Rua");

            migrationBuilder.RenameColumn(
                name: "state",
                table: "LocalizacoesExatas",
                newName: "Estado");

            migrationBuilder.RenameColumn(
                name: "neighborhood",
                table: "LocalizacoesExatas",
                newName: "Bairro");

            migrationBuilder.RenameColumn(
                name: "lon",
                table: "LocalizacoesExatas",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "lat",
                table: "LocalizacoesExatas",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "graffiti_id",
                table: "LocalizacoesExatas",
                newName: "RegistroPichacaoId");

            migrationBuilder.RenameColumn(
                name: "city",
                table: "LocalizacoesExatas",
                newName: "Cidade");

            migrationBuilder.RenameIndex(
                name: "IX_graffitis_location_graffiti_id",
                table: "LocalizacoesExatas",
                newName: "IX_LocalizacoesExatas_RegistroPichacaoId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "RegistrosPichacao",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "visual_description",
                table: "RegistrosPichacao",
                newName: "DescricaoVisual");

            migrationBuilder.RenameColumn(
                name: "threat_level",
                table: "RegistrosPichacao",
                newName: "NivelAmeaca");

            migrationBuilder.RenameColumn(
                name: "registered_at",
                table: "RegistrosPichacao",
                newName: "DataRegistro");

            migrationBuilder.RenameColumn(
                name: "image_path",
                table: "RegistrosPichacao",
                newName: "CaminhoImagem");

            migrationBuilder.RenameColumn(
                name: "gang_id",
                table: "RegistrosPichacao",
                newName: "FaccaoId");

            migrationBuilder.RenameIndex(
                name: "IX_graffitis_gang_id",
                table: "RegistrosPichacao",
                newName: "IX_RegistrosPichacao_FaccaoId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Faccoes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "origin",
                table: "Faccoes",
                newName: "Origem");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Faccoes",
                newName: "Nome");

            migrationBuilder.RenameColumn(
                name: "acronym",
                table: "Faccoes",
                newName: "Sigla");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LocalizacoesExatas",
                table: "LocalizacoesExatas",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RegistrosPichacao",
                table: "RegistrosPichacao",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Faccoes",
                table: "Faccoes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocalizacoesExatas_RegistrosPichacao_RegistroPichacaoId",
                table: "LocalizacoesExatas",
                column: "RegistroPichacaoId",
                principalTable: "RegistrosPichacao",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RegistrosPichacao_Faccoes_FaccaoId",
                table: "RegistrosPichacao",
                column: "FaccaoId",
                principalTable: "Faccoes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
