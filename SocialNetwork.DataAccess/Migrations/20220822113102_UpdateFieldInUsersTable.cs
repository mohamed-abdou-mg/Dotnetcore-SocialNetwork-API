using Microsoft.EntityFrameworkCore.Migrations;

namespace SocialNetwork.DataAccess.Migrations
{
    public partial class UpdateFieldInUsersTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introuction",
                table: "Users",
                newName: "Introduction");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Introduction",
                table: "Users",
                newName: "Introuction");
        }
    }
}
