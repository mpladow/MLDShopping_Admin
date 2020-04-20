using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MLDShopping_Admin.Migrations
{
    public partial class updatePrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPermission_Accounts_AccountId",
                table: "AccountPermission");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountPermission_Permissions_PermissionId",
                table: "AccountPermission");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountPermission",
                table: "AccountPermission");

            migrationBuilder.RenameTable(
                name: "AccountPermission",
                newName: "AccountPermissions");

            migrationBuilder.RenameIndex(
                name: "IX_AccountPermission_PermissionId",
                table: "AccountPermissions",
                newName: "IX_AccountPermissions_PermissionId");

            migrationBuilder.AddColumn<int>(
                name: "AccountPermissionId",
                table: "AccountPermissions",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AccountPermissions_AccountPermissionId",
                table: "AccountPermissions",
                column: "AccountPermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountPermissions",
                table: "AccountPermissions",
                columns: new[] { "AccountId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPermissions_Accounts_AccountId",
                table: "AccountPermissions",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPermissions_Permissions_PermissionId",
                table: "AccountPermissions",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountPermissions_Accounts_AccountId",
                table: "AccountPermissions");

            migrationBuilder.DropForeignKey(
                name: "FK_AccountPermissions_Permissions_PermissionId",
                table: "AccountPermissions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AccountPermissions_AccountPermissionId",
                table: "AccountPermissions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AccountPermissions",
                table: "AccountPermissions");

            migrationBuilder.DropColumn(
                name: "AccountPermissionId",
                table: "AccountPermissions");

            migrationBuilder.RenameTable(
                name: "AccountPermissions",
                newName: "AccountPermission");

            migrationBuilder.RenameIndex(
                name: "IX_AccountPermissions_PermissionId",
                table: "AccountPermission",
                newName: "IX_AccountPermission_PermissionId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_AccountPermission",
                table: "AccountPermission",
                columns: new[] { "AccountId", "PermissionId" });

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPermission_Accounts_AccountId",
                table: "AccountPermission",
                column: "AccountId",
                principalTable: "Accounts",
                principalColumn: "AccountId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountPermission_Permissions_PermissionId",
                table: "AccountPermission",
                column: "PermissionId",
                principalTable: "Permissions",
                principalColumn: "PermissionId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
