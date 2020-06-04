﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Komi.Data.Migrations
{
    public partial class TrackingModelsInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    UserId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.UserId);
                    table.ForeignKey(
                        name: "FK_Member_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupMember",
                columns: table => new
                {
                    GroupMemberId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<ulong>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMember", x => x.GroupMemberId);
                    table.ForeignKey(
                        name: "FK_GroupMember_Member_UserId",
                        column: x => x.UserId,
                        principalTable: "Member",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GroupSetting",
                columns: table => new
                {
                    GroupSettingId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TrackingChannel = table.Column<ulong>(nullable: false),
                    GroupId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupSetting", x => x.GroupSettingId);
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
                name: "WorkTypeSetting",
                columns: table => new
                {
                    WorkTypeSettingId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    WorkType = table.Column<int>(nullable: false),
                    IsEnabled = table.Column<bool>(nullable: false),
                    GroupSettingId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkTypeSetting", x => x.WorkTypeSettingId);
                    table.ForeignKey(
                        name: "FK_WorkTypeSetting_GroupSetting_GroupSettingId",
                        column: x => x.GroupSettingId,
                        principalTable: "GroupSetting",
                        principalColumn: "GroupSettingId",
                        onDelete: ReferentialAction.Restrict);
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
                });

            migrationBuilder.CreateTable(
                name: "Series",
                columns: table => new
                {
                    SeriesId = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GroupId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Series", x => x.SeriesId);
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
                        name: "FK_WarningData_ModerationData_ModerationDataId",
                        column: x => x.ModerationDataId,
                        principalTable: "ModerationData",
                        principalColumn: "ModerationDataId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Works",
                columns: table => new
                {
                    WorkId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Volume = table.Column<int>(nullable: true),
                    Chapter = table.Column<int>(nullable: true),
                    GroupId = table.Column<ulong>(nullable: false),
                    OverridenStatus = table.Column<int>(nullable: true),
                    SeriesId = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Works", x => x.WorkId);
                    table.ForeignKey(
                        name: "FK_Works_Series_SeriesId",
                        column: x => x.SeriesId,
                        principalTable: "Series",
                        principalColumn: "SeriesId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Job",
                columns: table => new
                {
                    JobId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Type = table.Column<int>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    WorkId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Job", x => x.JobId);
                    table.ForeignKey(
                        name: "FK_Job_Works_WorkId",
                        column: x => x.WorkId,
                        principalTable: "Works",
                        principalColumn: "WorkId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Worker",
                columns: table => new
                {
                    WorkerId = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(nullable: false),
                    Started = table.Column<DateTimeOffset>(nullable: false),
                    Finished = table.Column<DateTimeOffset>(nullable: false),
                    JobId = table.Column<ulong>(nullable: false),
                    UserId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Worker", x => x.WorkerId);
                    table.ForeignKey(
                        name: "FK_Worker_Job_JobId",
                        column: x => x.JobId,
                        principalTable: "Job",
                        principalColumn: "JobId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    GroupId = table.Column<ulong>(nullable: false),
                    WorkerId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_Groups_Worker_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "Worker",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_GroupId",
                table: "GroupMember",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMember_UserId",
                table: "GroupMember",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Groups_WorkerId",
                table: "Groups",
                column: "WorkerId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupSetting_GroupId",
                table: "GroupSetting",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Job_WorkId",
                table: "Job",
                column: "WorkId");

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
                name: "IX_Series_GroupId",
                table: "Series",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningData_GroupId",
                table: "WarningData",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_WarningData_ModerationDataId",
                table: "WarningData",
                column: "ModerationDataId");

            migrationBuilder.CreateIndex(
                name: "IX_Worker_JobId",
                table: "Worker",
                column: "JobId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_GroupId",
                table: "Works",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Works_SeriesId",
                table: "Works",
                column: "SeriesId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkTypeSetting_GroupSettingId",
                table: "WorkTypeSetting",
                column: "GroupSettingId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMember_Groups_GroupId",
                table: "GroupMember",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSetting_Groups_GroupId",
                table: "GroupSetting",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationData_Groups_GroupId",
                table: "ModerationData",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ModerationSetting_Groups_GroupId",
                table: "ModerationSetting",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Groups_GroupId",
                table: "Series",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WarningData_Groups_GroupId",
                table: "WarningData",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Groups_GroupId",
                table: "Works",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Series_Groups_GroupId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Groups_GroupId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "GroupMember");

            migrationBuilder.DropTable(
                name: "ModerationSetting");

            migrationBuilder.DropTable(
                name: "Prefix");

            migrationBuilder.DropTable(
                name: "WarningData");

            migrationBuilder.DropTable(
                name: "WorkTypeSetting");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "ModerationData");

            migrationBuilder.DropTable(
                name: "GroupSetting");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Worker");

            migrationBuilder.DropTable(
                name: "Job");

            migrationBuilder.DropTable(
                name: "Works");

            migrationBuilder.DropTable(
                name: "Series");
        }
    }
}
