using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommUnity.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class pqrs2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Pqrss",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResidentialUnitId",
                table: "Pqrss",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Pqrss_ResidentialUnitId",
                table: "Pqrss",
                column: "ResidentialUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_Pqrss_ResidentialUnits_ResidentialUnitId",
                table: "Pqrss",
                column: "ResidentialUnitId",
                principalTable: "ResidentialUnits",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Pqrss_ResidentialUnits_ResidentialUnitId",
                table: "Pqrss");

            migrationBuilder.DropIndex(
                name: "IX_Pqrss_ResidentialUnitId",
                table: "Pqrss");

            migrationBuilder.DropColumn(
                name: "ResidentialUnitId",
                table: "Pqrss");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Pqrss",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);
        }
    }
}
