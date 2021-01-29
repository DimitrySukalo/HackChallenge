using Microsoft.EntityFrameworkCore.Migrations;

namespace HackChallenge.DAL.Migrations
{
    public partial class ChangedWifi : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Channel",
                table: "Wifis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Cipher",
                table: "Wifis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "EncryptionType",
                table: "Wifis",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Channel",
                table: "Wifis");

            migrationBuilder.DropColumn(
                name: "Cipher",
                table: "Wifis");

            migrationBuilder.DropColumn(
                name: "EncryptionType",
                table: "Wifis");
        }
    }
}
