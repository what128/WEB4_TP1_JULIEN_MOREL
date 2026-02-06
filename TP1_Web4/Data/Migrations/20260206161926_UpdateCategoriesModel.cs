using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1_Web4.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoriesModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Archive",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Archive",
                table: "Categories");
        }
    }
}
