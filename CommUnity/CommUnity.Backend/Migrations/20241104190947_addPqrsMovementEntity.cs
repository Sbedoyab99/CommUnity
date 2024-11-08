using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommUnity.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class addPqrsMovementEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PqrsMovement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observation = table.Column<string>(type: "nvarchar(3000)", maxLength: 3000, nullable: false),
                    PqrsState = table.Column<int>(type: "int", nullable: false),
                    PqrsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PqrsMovement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PqrsMovement_Pqrss_PqrsId",
                        column: x => x.PqrsId,
                        principalTable: "Pqrss",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PqrsMovement_PqrsId",
                table: "PqrsMovement",
                column: "PqrsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PqrsMovement");
        }
    }
}
