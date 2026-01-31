using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1_Web4.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddUpdateEvenementCategorieModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Evenements_CategorieId",
                table: "Evenements",
                column: "CategorieId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evenements_Categories_CategorieId",
                table: "Evenements",
                column: "CategorieId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evenements_Categories_CategorieId",
                table: "Evenements");

            migrationBuilder.DropIndex(
                name: "IX_Evenements_CategorieId",
                table: "Evenements");
        }
    }
}
