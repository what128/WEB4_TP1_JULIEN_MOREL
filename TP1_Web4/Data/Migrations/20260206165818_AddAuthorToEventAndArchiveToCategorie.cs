using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1_Web4.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAuthorToEventAndArchiveToCategorie : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auteur",
                table: "Evenements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auteur",
                table: "Evenements");
        }
    }
}
