using Microsoft.EntityFrameworkCore.Migrations;

namespace Komi.Data.Migrations
{
    public partial class EnumerableEnums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_Groups_GroupId",
                table: "GroupMember");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMember_Member_UserId",
                table: "GroupMember");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMember",
                table: "GroupMember");

            migrationBuilder.DropColumn(
                name: "IsEnabled",
                table: "WorkTypeSetting");

            migrationBuilder.RenameTable(
                name: "GroupMember",
                newName: "GroupMembers");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMember_UserId",
                table: "GroupMembers",
                newName: "IX_GroupMembers_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMember_GroupId",
                table: "GroupMembers",
                newName: "IX_GroupMembers_GroupId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers",
                column: "GroupMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Member_UserId",
                table: "GroupMembers",
                column: "UserId",
                principalTable: "Member",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Member_UserId",
                table: "GroupMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GroupMembers",
                table: "GroupMembers");

            migrationBuilder.RenameTable(
                name: "GroupMembers",
                newName: "GroupMember");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMembers_UserId",
                table: "GroupMember",
                newName: "IX_GroupMember_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_GroupMembers_GroupId",
                table: "GroupMember",
                newName: "IX_GroupMember_GroupId");

            migrationBuilder.AddColumn<bool>(
                name: "IsEnabled",
                table: "WorkTypeSetting",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_GroupMember",
                table: "GroupMember",
                column: "GroupMemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_Groups_GroupId",
                table: "GroupMember",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_Member_UserId",
                table: "GroupMember",
                column: "UserId",
                principalTable: "Member",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
