using Microsoft.EntityFrameworkCore.Migrations;

namespace HackChallenge.DAL.Migrations
{
    public partial class AddedCorrectColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CountOfCorrectUserName",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountOfCorrectUserName",
                table: "Users");
        }
    }
}
