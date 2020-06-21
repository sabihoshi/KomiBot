using Microsoft.EntityFrameworkCore.Migrations;

namespace Komi.Data.Migrations
{
    public partial class GroupMemberFix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Member_Id",
                table: "GroupMembers");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.AlterColumn<long>(
                name: "GroupMemberId",
                table: "GroupMembers",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Users_Id",
                table: "GroupMembers",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Users_Id",
                table: "GroupMembers");

            migrationBuilder.AlterColumn<int>(
                name: "GroupMemberId",
                table: "GroupMembers",
                type: "integer",
                nullable: false,
                oldClrType: typeof(long));

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    Id = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Member_Users_Id",
                        column: x => x.Id,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Member_Id",
                table: "GroupMembers",
                column: "Id",
                principalTable: "Member",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
