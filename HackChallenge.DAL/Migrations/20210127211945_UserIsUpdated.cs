using Microsoft.EntityFrameworkCore.Migrations;

namespace HackChallenge.DAL.Migrations
{
    public partial class UserIsUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfIncorrectLoginData",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isAuthorized",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfIncorrectLoginData",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "isAuthorized",
                table: "Users");
        }
    }
}
