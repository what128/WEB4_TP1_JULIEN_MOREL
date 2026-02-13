using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1_Web4.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeEvenementAuteurStringToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auteur",
                table: "Evenements");

            migrationBuilder.AddColumn<string>(
                name: "AuteurId",
                table: "Evenements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Evenements_AuteurId",
                table: "Evenements",
                column: "AuteurId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evenements_AspNetUsers_AuteurId",
                table: "Evenements",
                column: "AuteurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evenements_AspNetUsers_AuteurId",
                table: "Evenements");

            migrationBuilder.DropIndex(
                name: "IX_Evenements_AuteurId",
                table: "Evenements");

            migrationBuilder.DropColumn(
                name: "AuteurId",
                table: "Evenements");

            migrationBuilder.AddColumn<string>(
                name: "Auteur",
                table: "Evenements",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
