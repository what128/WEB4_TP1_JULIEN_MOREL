using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1_Web4.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressAndCityForUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Addresse",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Addresse",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "AspNetUsers");
        }
    }
}
