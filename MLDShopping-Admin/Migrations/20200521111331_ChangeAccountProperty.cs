using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDShopping_Admin.Migrations
{
    public partial class ChangeAccountProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "UserImageUrl",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImageUrl",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
