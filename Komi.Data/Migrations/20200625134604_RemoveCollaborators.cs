using Microsoft.EntityFrameworkCore.Migrations;

namespace Komi.Data.Migrations
{
    public partial class RemoveCollaborators : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Groups_Worker_WorkerId",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Groups_WorkerId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Groups");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "WorkerId",
                table: "Groups",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Groups_WorkerId",
                table: "Groups",
                column: "WorkerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Groups_Worker_WorkerId",
                table: "Groups",
                column: "WorkerId",
                principalTable: "Worker",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
