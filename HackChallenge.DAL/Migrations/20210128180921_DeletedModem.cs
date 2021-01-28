using Microsoft.EntityFrameworkCore.Migrations;

namespace HackChallenge.DAL.Migrations
{
    public partial class DeletedModem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinuxSystems_Modems_ModemId",
                table: "LinuxSystems");

            migrationBuilder.DropForeignKey(
                name: "FK_WifiModules_LinuxSystems_LinuxSystemId",
                table: "WifiModules");

            migrationBuilder.DropForeignKey(
                name: "FK_Wifis_Modems_ModemId",
                table: "Wifis");

            migrationBuilder.DropTable(
                name: "Modems");

            migrationBuilder.DropIndex(
                name: "IX_Wifis_ModemId",
                table: "Wifis");

            migrationBuilder.DropIndex(
                name: "IX_WifiModules_LinuxSystemId",
                table: "WifiModules");

            migrationBuilder.DropIndex(
                name: "IX_LinuxSystems_ModemId",
                table: "LinuxSystems");

            migrationBuilder.DropColumn(
                name: "ModemId",
                table: "Wifis");

            migrationBuilder.DropColumn(
                name: "LinuxSystemId",
                table: "WifiModules");

            migrationBuilder.DropColumn(
                name: "ModemId",
                table: "LinuxSystems");

            migrationBuilder.AddColumn<int>(
                name: "WifiModuleId",
                table: "LinuxSystems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_LinuxSystems_WifiModuleId",
                table: "LinuxSystems",
                column: "WifiModuleId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LinuxSystems_WifiModules_WifiModuleId",
                table: "LinuxSystems",
                column: "WifiModuleId",
                principalTable: "WifiModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinuxSystems_WifiModules_WifiModuleId",
                table: "LinuxSystems");

            migrationBuilder.DropIndex(
                name: "IX_LinuxSystems_WifiModuleId",
                table: "LinuxSystems");

            migrationBuilder.DropColumn(
                name: "WifiModuleId",
                table: "LinuxSystems");

            migrationBuilder.AddColumn<int>(
                name: "ModemId",
                table: "Wifis",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LinuxSystemId",
                table: "WifiModules",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ModemId",
                table: "LinuxSystems",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Modems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IPAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Login = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Modems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wifis_ModemId",
                table: "Wifis",
                column: "ModemId");

            migrationBuilder.CreateIndex(
                name: "IX_WifiModules_LinuxSystemId",
                table: "WifiModules",
                column: "LinuxSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_LinuxSystems_ModemId",
                table: "LinuxSystems",
                column: "ModemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinuxSystems_Modems_ModemId",
                table: "LinuxSystems",
                column: "ModemId",
                principalTable: "Modems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WifiModules_LinuxSystems_LinuxSystemId",
                table: "WifiModules",
                column: "LinuxSystemId",
                principalTable: "LinuxSystems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Wifis_Modems_ModemId",
                table: "Wifis",
                column: "ModemId",
                principalTable: "Modems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
