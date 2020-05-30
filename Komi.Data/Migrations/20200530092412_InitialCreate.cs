using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Komi.Data.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GuildId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GuildId);
                });

            migrationBuilder.CreateTable(
                name: "GroupSetting",
                columns: table => new
                {
                    GroupSettingId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSetting", x => x.GroupSettingId);
                    table.ForeignKey(
                        name: "FK_GroupSetting_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModerationData",
                columns: table => new
                {
                    ModerationDataId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationData", x => x.ModerationDataId);
                    table.ForeignKey(
                        name: "FK_ModerationData_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModerationSetting",
                columns: table => new
                {
                    ModerationSettingId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    KickAt = table.Column<int>(nullable: true),
                    BanAt = table.Column<int>(nullable: true),
                    GroupId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationSetting", x => x.ModerationSettingId);
                    table.ForeignKey(
                        name: "FK_ModerationSetting_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Prefix",
                columns: table => new
                {
                    PrefixId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Text = table.Column<string>(nullable: false),
                    GroupSettingId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prefix", x => x.PrefixId);
                    table.ForeignKey(
                        name: "FK_Prefix_GroupSetting_GroupSettingId",
                        column: x => x.GroupSettingId,
                        principalTable: "GroupSetting",
                        principalColumn: "GroupSettingId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarningData",
                columns: table => new
                {
                    WarningId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserId = table.Column<ulong>(nullable: false),
                    ModId = table.Column<ulong>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    GroupId = table.Column<ulong>(nullable: false),
                    ModerationDataId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningData", x => x.WarningId);
                    table.ForeignKey(
                        name: "FK_WarningData_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WarningData_ModerationData_ModerationDataId",
                        column: x => x.ModerationDataId,
                        principalTable: "ModerationData",
                        principalColumn: "ModerationDataId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupSetting_GroupId",
                table: "GroupSetting",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModerationData_GroupId",
                table: "ModerationData",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ModerationSetting_GroupId",
                table: "ModerationSetting",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prefix_GroupSettingId",
                table: "Prefix",
                column: "GroupSettingId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningData_GroupId",
                table: "WarningData",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningData_ModerationDataId",
                table: "WarningData",
                column: "ModerationDataId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ModerationSetting");

            migrationBuilder.DropTable(
                name: "Prefix");

            migrationBuilder.DropTable(
                name: "WarningData");

            migrationBuilder.DropTable(
                name: "GroupSetting");

            migrationBuilder.DropTable(
                name: "ModerationData");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
