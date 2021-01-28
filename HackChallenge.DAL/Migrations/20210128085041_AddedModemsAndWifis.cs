using Microsoft.EntityFrameworkCore.Migrations;

namespace HackChallenge.DAL.Migrations
{
    public partial class AddedModemsAndWifis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsConnectedTheInternet",
                table: "LinuxSystems",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.CreateTable(
                name: "Wifis",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BSSID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Speed = table.Column<double>(type: "float", nullable: false),
                    QualityOfSignal = table.Column<int>(type: "int", nullable: false),
                    ModemId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wifis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wifis_Modems_ModemId",
                        column: x => x.ModemId,
                        principalTable: "Modems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LinuxSystems_ModemId",
                table: "LinuxSystems",
                column: "ModemId");

            migrationBuilder.CreateIndex(
                name: "IX_Wifis_ModemId",
                table: "Wifis",
                column: "ModemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LinuxSystems_Modems_ModemId",
                table: "LinuxSystems",
                column: "ModemId",
                principalTable: "Modems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LinuxSystems_Modems_ModemId",
                table: "LinuxSystems");

            migrationBuilder.DropTable(
                name: "Wifis");

            migrationBuilder.DropTable(
                name: "Modems");

            migrationBuilder.DropIndex(
                name: "IX_LinuxSystems_ModemId",
                table: "LinuxSystems");

            migrationBuilder.DropColumn(
                name: "IsConnectedTheInternet",
                table: "LinuxSystems");

            migrationBuilder.DropColumn(
                name: "ModemId",
                table: "LinuxSystems");
        }
    }
}
