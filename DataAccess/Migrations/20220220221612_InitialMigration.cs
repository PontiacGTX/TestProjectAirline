using Microsoft.EntityFrameworkCore.Migrations;

namespace DataAccess.Migrations
{
    public partial class InitialMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Journeys",
                columns: table => new
                {
                    IdJourney = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Journeys", x => x.IdJourney);
                });

            migrationBuilder.CreateTable(
                name: "Transports",
                columns: table => new
                {
                    IdTransport = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightCarrier = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FlightNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transports", x => x.IdTransport);
                });

            migrationBuilder.CreateTable(
                name: "Flights",
                columns: table => new
                {
                    IdFlight = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Origin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Destination = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false),
                    IdTransport = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flights", x => x.IdFlight);
                    table.ForeignKey(
                        name: "FK_Flights_Transports_IdTransport",
                        column: x => x.IdTransport,
                        principalTable: "Transports",
                        principalColumn: "IdTransport",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "JourneyFlights",
                columns: table => new
                {
                    IdJourney = table.Column<int>(type: "int", nullable: false),
                    IdFlight = table.Column<int>(type: "int", nullable: false),
                    Index = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyFlights", x => new { x.IdFlight, x.IdJourney });
                    table.ForeignKey(
                        name: "FK_JourneyFlights_Flights_IdFlight",
                        column: x => x.IdFlight,
                        principalTable: "Flights",
                        principalColumn: "IdFlight",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_JourneyFlights_Journeys_IdJourney",
                        column: x => x.IdJourney,
                        principalTable: "Journeys",
                        principalColumn: "IdJourney",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flights_IdTransport",
                table: "Flights",
                column: "IdTransport");

            migrationBuilder.CreateIndex(
                name: "IX_JourneyFlights_IdJourney",
                table: "JourneyFlights",
                column: "IdJourney");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "JourneyFlights");

            migrationBuilder.DropTable(
                name: "Flights");

            migrationBuilder.DropTable(
                name: "Journeys");

            migrationBuilder.DropTable(
                name: "Transports");
        }
    }
}
