using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TP1_Web4.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateNullableAuthorId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evenements_AspNetUsers_AuteurId",
                table: "Evenements");

            migrationBuilder.AlterColumn<string>(
                name: "AuteurId",
                table: "Evenements",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Evenements_AspNetUsers_AuteurId",
                table: "Evenements",
                column: "AuteurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evenements_AspNetUsers_AuteurId",
                table: "Evenements");

            migrationBuilder.AlterColumn<string>(
                name: "AuteurId",
                table: "Evenements",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Evenements_AspNetUsers_AuteurId",
                table: "Evenements",
                column: "AuteurId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
