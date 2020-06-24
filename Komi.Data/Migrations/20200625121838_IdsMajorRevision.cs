using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Komi.Data.Migrations
{
    public partial class IdsMajorRevision : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Users_Id",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSetting_Groups_GroupId",
                table: "GroupSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Works_WorkId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Groups_GroupId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_Job_JobId",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Groups_GroupId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Series_SeriesId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "ModerationSetting");

            migrationBuilder.DropTable(
                name: "WarningData");

            migrationBuilder.DropTable(
                name: "ModerationData");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkTypeSetting",
                table: "WorkTypeSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Works",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_GroupId",
                table: "Works");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Series",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_GroupId",
                table: "Series");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prefix",
                table: "Prefix");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Job",
                table: "Job");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_GroupId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "WorkTypeSettingId",
                table: "WorkTypeSetting");

            migrationBuilder.DropColumn(
                name: "WorkId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "WorkerId",
                table: "Worker");

            migrationBuilder.DropColumn(
                name: "SeriesId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "PrefixId",
                table: "Prefix");

            migrationBuilder.DropColumn(
                name: "Text",
                table: "Prefix");

            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "GroupMemberId",
                table: "GroupMembers");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "WorkTypeSetting",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<double>(
                name: "Volume",
                table: "Works",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.Sql(@"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

                ALTER TABLE ""Works"" ALTER COLUMN ""SeriesId"" TYPE uuid USING (uuid_generate_v4());
                ALTER TABLE ""Works"" ALTER COLUMN ""SeriesId"" DROP NOT NULL;
                ALTER TABLE ""Works"" ALTER COLUMN ""SeriesId"" DROP DEFAULT;");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "SeriesId",
            //    table: "Works",
            //    nullable: true,
            //    oldClrType: typeof(long),
            //    oldType: "bigint",
            //    oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Chapter",
                table: "Works",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Works",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "GroupGuildId",
                table: "Works",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

                ALTER TABLE ""Worker"" ALTER COLUMN ""JobId"" TYPE uuid USING (uuid_generate_v4());
                ALTER TABLE ""Worker"" ALTER COLUMN ""JobId"" DROP NOT NULL;
                ALTER TABLE ""Worker"" ALTER COLUMN ""JobId"" DROP DEFAULT;");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "JobId",
            //    table: "Worker",
            //    nullable: false,
            //    oldClrType: typeof(decimal),
            //    oldType: "numeric(20,0)");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Series",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "GroupGuildId",
                table: "Series",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Prefix",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Value",
                table: "Prefix",
                nullable: false,
                defaultValue: "");

            migrationBuilder.Sql(@"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

                ALTER TABLE ""Job"" ALTER COLUMN ""WorkId"" TYPE uuid USING (uuid_generate_v4());
                ALTER TABLE ""Job"" ALTER COLUMN ""WorkId"" DROP NOT NULL;
                ALTER TABLE ""Job"" ALTER COLUMN ""WorkId"" DROP DEFAULT;");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "WorkId",
            //    table: "Job",
            //    nullable: true,
            //    oldClrType: typeof(decimal),
            //    oldType: "numeric(20,0)",
            //    oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "Job",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<decimal>(
                name: "TrackingChannel",
                table: "GroupSetting",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(20,0)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BanAt",
                table: "GroupSetting",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "KickAt",
                table: "GroupSetting",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GuildId",
                table: "Groups",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.Sql(@"
                CREATE EXTENSION IF NOT EXISTS ""uuid-ossp"";

                ALTER TABLE ""GroupMembers"" DROP COLUMN ""Id"";

                ALTER TABLE ""GroupMembers""
	                add ""Id"" uuid NOT NULL;

                ALTER TABLE ""GroupMembers""
	                add constraint PK_GroupMembers
		                primary key (""Id"");");

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "Id",
            //    table: "GroupMembers",
            //    nullable: false,
            //    oldClrType: typeof(decimal),
            //    oldType: "numeric(20,0)");

            migrationBuilder.AddColumn<decimal>(
                name: "GroupGuildId",
                table: "GroupMembers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "UserId",
                table: "GroupMembers",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkTypeSetting",
                table: "WorkTypeSetting",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Works",
                table: "Works",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Series",
                table: "Series",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prefix",
                table: "Prefix",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Job",
                table: "Job",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "GuildId");

            migrationBuilder.CreateTable(
                name: "Warning",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<decimal>(nullable: false),
                    ModId = table.Column<decimal>(nullable: false),
                    Count = table.Column<int>(nullable: false),
                    Reason = table.Column<string>(nullable: true),
                    Date = table.Column<DateTimeOffset>(nullable: false),
                    GroupGuildId = table.Column<decimal>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warning", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Warning_Groups_GroupGuildId",
                        column: x => x.GroupGuildId,
                        principalTable: "Groups",
                        principalColumn: "GuildId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Warning_Users_ModId",
                        column: x => x.ModId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Warning_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_GroupGuildId",
                table: "Works",
                column: "GroupGuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_GroupGuildId",
                table: "Series",
                column: "GroupGuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupGuildId",
                table: "GroupMembers",
                column: "GroupGuildId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_UserId",
                table: "GroupMembers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Warning_GroupGuildId",
                table: "Warning",
                column: "GroupGuildId");

            migrationBuilder.CreateIndex(
                name: "IX_Warning_ModId",
                table: "Warning",
                column: "ModId");

            migrationBuilder.CreateIndex(
                name: "IX_Warning_UserId",
                table: "Warning",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupGuildId",
                table: "GroupMembers",
                column: "GroupGuildId",
                principalTable: "Groups",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Users_UserId",
                table: "GroupMembers",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSetting_Groups_GroupId",
                table: "GroupSetting",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Works_WorkId",
                table: "Job",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Groups_GroupGuildId",
                table: "Series",
                column: "GroupGuildId",
                principalTable: "Groups",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_Users_Id",
                table: "Worker",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_Job_JobId",
                table: "Worker",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Groups_GroupGuildId",
                table: "Works",
                column: "GroupGuildId",
                principalTable: "Groups",
                principalColumn: "GuildId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Series_SeriesId",
                table: "Works",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Groups_GroupGuildId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupMembers_Users_UserId",
                table: "GroupMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_GroupSetting_Groups_GroupId",
                table: "GroupSetting");

            migrationBuilder.DropForeignKey(
                name: "FK_Job_Works_WorkId",
                table: "Job");

            migrationBuilder.DropForeignKey(
                name: "FK_Series_Groups_GroupGuildId",
                table: "Series");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_Users_Id",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Worker_Job_JobId",
                table: "Worker");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Groups_GroupGuildId",
                table: "Works");

            migrationBuilder.DropForeignKey(
                name: "FK_Works_Series_SeriesId",
                table: "Works");

            migrationBuilder.DropTable(
                name: "Warning");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkTypeSetting",
                table: "WorkTypeSetting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Works",
                table: "Works");

            migrationBuilder.DropIndex(
                name: "IX_Works_GroupGuildId",
                table: "Works");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Series",
                table: "Series");

            migrationBuilder.DropIndex(
                name: "IX_Series_GroupGuildId",
                table: "Series");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Prefix",
                table: "Prefix");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Job",
                table: "Job");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Groups",
                table: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_GroupGuildId",
                table: "GroupMembers");

            migrationBuilder.DropIndex(
                name: "IX_GroupMembers_UserId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "WorkTypeSetting");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "GroupGuildId",
                table: "Works");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "GroupGuildId",
                table: "Series");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Prefix");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "Prefix");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Job");

            migrationBuilder.DropColumn(
                name: "BanAt",
                table: "GroupSetting");

            migrationBuilder.DropColumn(
                name: "KickAt",
                table: "GroupSetting");

            migrationBuilder.DropColumn(
                name: "GuildId",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "GroupGuildId",
                table: "GroupMembers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GroupMembers");

            migrationBuilder.AddColumn<int>(
                name: "WorkTypeSettingId",
                table: "WorkTypeSetting",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Volume",
                table: "Works",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "SeriesId",
                table: "Works",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Chapter",
                table: "Works",
                type: "integer",
                nullable: true,
                oldClrType: typeof(double));

            migrationBuilder.AddColumn<decimal>(
                name: "WorkId",
                table: "Works",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "GroupId",
                table: "Works",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "JobId",
                table: "Worker",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<decimal>(
                name: "WorkerId",
                table: "Worker",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "SeriesId",
                table: "Series",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<decimal>(
                name: "GroupId",
                table: "Series",
                type: "numeric(20,0)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PrefixId",
                table: "Prefix",
                type: "integer",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "Text",
                table: "Prefix",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "WorkId",
                table: "Job",
                type: "numeric(20,0)",
                nullable: true,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "JobId",
                table: "Job",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "TrackingChannel",
                table: "GroupSetting",
                type: "numeric(20,0)",
                nullable: true,
                oldClrType: typeof(decimal));

            migrationBuilder.AddColumn<decimal>(
                name: "GroupId",
                table: "Groups",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<decimal>(
                name: "Id",
                table: "GroupMembers",
                type: "numeric(20,0)",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddColumn<decimal>(
                name: "GroupId",
                table: "GroupMembers",
                type: "numeric(20,0)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "GroupMemberId",
                table: "GroupMembers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkTypeSetting",
                table: "WorkTypeSetting",
                column: "WorkTypeSettingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Works",
                table: "Works",
                column: "WorkId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Series",
                table: "Series",
                column: "SeriesId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Prefix",
                table: "Prefix",
                column: "PrefixId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Job",
                table: "Job",
                column: "JobId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Groups",
                table: "Groups",
                column: "GroupId");

            migrationBuilder.CreateTable(
                name: "ModerationData",
                columns: table => new
                {
                    ModerationDataId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    GroupId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationData", x => x.ModerationDataId);
                    table.ForeignKey(
                        name: "FK_ModerationData_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ModerationSetting",
                columns: table => new
                {
                    GroupId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    BanAt = table.Column<int>(type: "integer", nullable: true),
                    KickAt = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ModerationSetting", x => x.GroupId);
                    table.ForeignKey(
                        name: "FK_ModerationSetting_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "GroupId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WarningData",
                columns: table => new
                {
                    WarningDataId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Count = table.Column<int>(type: "integer", nullable: false),
                    Date = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ModId = table.Column<decimal>(type: "numeric(20,0)", nullable: false),
                    ModerationDataId = table.Column<int>(type: "integer", nullable: true),
                    Reason = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<decimal>(type: "numeric(20,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarningData", x => x.WarningDataId);
                    table.ForeignKey(
                        name: "FK_WarningData_ModerationData_ModerationDataId",
                        column: x => x.ModerationDataId,
                        principalTable: "ModerationData",
                        principalColumn: "ModerationDataId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Works_GroupId",
                table: "Works",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Series_GroupId",
                table: "Series",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_GroupMembers_GroupId",
                table: "GroupMembers",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_ModerationData_GroupId",
                table: "ModerationData",
                column: "GroupId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WarningData_ModerationDataId",
                table: "WarningData",
                column: "ModerationDataId");

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Groups_GroupId",
                table: "GroupMembers",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupMembers_Users_Id",
                table: "GroupMembers",
                column: "Id",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GroupSetting_Groups_GroupId",
                table: "GroupSetting",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Job_Works_WorkId",
                table: "Job",
                column: "WorkId",
                principalTable: "Works",
                principalColumn: "WorkId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Series_Groups_GroupId",
                table: "Series",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Worker_Job_JobId",
                table: "Worker",
                column: "JobId",
                principalTable: "Job",
                principalColumn: "JobId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Groups_GroupId",
                table: "Works",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "GroupId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Works_Series_SeriesId",
                table: "Works",
                column: "SeriesId",
                principalTable: "Series",
                principalColumn: "SeriesId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
