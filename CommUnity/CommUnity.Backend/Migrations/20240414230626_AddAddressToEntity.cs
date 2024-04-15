using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommUnity.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class AddAddressToEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "ResidentialUnits",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Address",
                table: "ResidentialUnits");
        }
    }
}
