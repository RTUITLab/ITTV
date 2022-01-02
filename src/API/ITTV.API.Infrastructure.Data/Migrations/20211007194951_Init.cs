using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace KinectTvV2.API.Infrastructure.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FileInfoEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    BaseName = table.Column<string>(type: "text", nullable: true),
                    Created = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Updated = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    Inactive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FileInfoEntity", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BASE64NAME_FILE",
                table: "FileInfoEntity",
                column: "BaseName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FileInfoEntity_Inactive",
                table: "FileInfoEntity",
                column: "Inactive");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FileInfoEntity");
        }
    }
}
