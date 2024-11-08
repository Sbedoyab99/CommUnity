using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommUnity.BackEnd.Migrations
{
    /// <inheritdoc />
    public partial class pqrsMovements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PqrsMovement_Pqrss_PqrsId",
                table: "PqrsMovement");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PqrsMovement",
                table: "PqrsMovement");

            migrationBuilder.RenameTable(
                name: "PqrsMovement",
                newName: "PqrsMovements");

            migrationBuilder.RenameIndex(
                name: "IX_PqrsMovement_PqrsId",
                table: "PqrsMovements",
                newName: "IX_PqrsMovements_PqrsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PqrsMovements",
                table: "PqrsMovements",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_PqrsMovements_Id",
                table: "PqrsMovements",
                column: "Id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_PqrsMovements_Pqrss_PqrsId",
                table: "PqrsMovements",
                column: "PqrsId",
                principalTable: "Pqrss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PqrsMovements_Pqrss_PqrsId",
                table: "PqrsMovements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PqrsMovements",
                table: "PqrsMovements");

            migrationBuilder.DropIndex(
                name: "IX_PqrsMovements_Id",
                table: "PqrsMovements");

            migrationBuilder.RenameTable(
                name: "PqrsMovements",
                newName: "PqrsMovement");

            migrationBuilder.RenameIndex(
                name: "IX_PqrsMovements_PqrsId",
                table: "PqrsMovement",
                newName: "IX_PqrsMovement_PqrsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PqrsMovement",
                table: "PqrsMovement",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PqrsMovement_Pqrss_PqrsId",
                table: "PqrsMovement",
                column: "PqrsId",
                principalTable: "Pqrss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
