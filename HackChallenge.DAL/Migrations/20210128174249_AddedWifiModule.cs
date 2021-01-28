using Microsoft.EntityFrameworkCore.Migrations;

namespace HackChallenge.DAL.Migrations
{
    public partial class AddedWifiModule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WifiModuleId",
                table: "Wifis",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "WifiModules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModuleMode = table.Column<int>(type: "int", nullable: false),
                    LinuxSystemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WifiModules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WifiModules_LinuxSystems_LinuxSystemId",
                        column: x => x.LinuxSystemId,
                        principalTable: "LinuxSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wifis_WifiModuleId",
                table: "Wifis",
                column: "WifiModuleId");

            migrationBuilder.CreateIndex(
                name: "IX_WifiModules_LinuxSystemId",
                table: "WifiModules",
                column: "LinuxSystemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wifis_WifiModules_WifiModuleId",
                table: "Wifis",
                column: "WifiModuleId",
                principalTable: "WifiModules",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wifis_WifiModules_WifiModuleId",
                table: "Wifis");

            migrationBuilder.DropTable(
                name: "WifiModules");

            migrationBuilder.DropIndex(
                name: "IX_Wifis_WifiModuleId",
                table: "Wifis");

            migrationBuilder.DropColumn(
                name: "WifiModuleId",
                table: "Wifis");
        }
    }
}
