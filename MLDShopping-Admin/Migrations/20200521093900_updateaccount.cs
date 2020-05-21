using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDShopping_Admin.Migrations
{
    public partial class updateaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_AccountPermissions_AccountPermissionId",
                table: "AccountPermissions");

            migrationBuilder.AddColumn<string>(
                name: "UserImage",
                table: "Accounts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserImage",
                table: "Accounts");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AccountPermissions_AccountPermissionId",
                table: "AccountPermissions",
                column: "AccountPermissionId");
        }
    }
}
