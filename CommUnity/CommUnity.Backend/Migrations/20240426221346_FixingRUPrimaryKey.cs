using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommUnity.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class FixingRUPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResidentialUnits_Name",
                table: "ResidentialUnits");

            migrationBuilder.CreateIndex(
                name: "IX_ResidentialUnits_Name_CityId",
                table: "ResidentialUnits",
                columns: new[] { "Name", "CityId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ResidentialUnits_Name_CityId",
                table: "ResidentialUnits");

            migrationBuilder.CreateIndex(
                name: "IX_ResidentialUnits_Name",
                table: "ResidentialUnits",
                column: "Name",
                unique: true);
        }
    }
}
