using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommUnity.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class CreateEventEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    ResidentialUnitId = table.Column<int>(type: "int", nullable: true),
                    ApartmentId = table.Column<int>(type: "int", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CommonZoneId = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true),
                    Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MailArrival_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VisitorEntry_Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Plate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_Apartments_ApartmentId",
                        column: x => x.ApartmentId,
                        principalTable: "Apartments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_CommonZones_CommonZoneId",
                        column: x => x.CommonZoneId,
                        principalTable: "CommonZones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Events_ResidentialUnits_ResidentialUnitId",
                        column: x => x.ResidentialUnitId,
                        principalTable: "ResidentialUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ApartmentId",
                table: "Events",
                column: "ApartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_CommonZoneId",
                table: "Events",
                column: "CommonZoneId");

            migrationBuilder.CreateIndex(
                name: "IX_Events_ResidentialUnitId",
                table: "Events",
                column: "ResidentialUnitId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");
        }
    }
}
