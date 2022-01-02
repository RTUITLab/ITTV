using Microsoft.EntityFrameworkCore.Migrations;

namespace KinectTvV2.API.Infrastructure.Data.Migrations
{
    public partial class FileInfoEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileInfoEntity",
                table: "FileInfoEntity");

            migrationBuilder.RenameTable(
                name: "FileInfoEntity",
                newName: "FileInfoEntities");

            migrationBuilder.RenameIndex(
                name: "IX_FileInfoEntity_Inactive",
                table: "FileInfoEntities",
                newName: "IX_FileInfoEntities_Inactive");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileInfoEntities",
                table: "FileInfoEntities",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FileInfoEntities",
                table: "FileInfoEntities");

            migrationBuilder.RenameTable(
                name: "FileInfoEntities",
                newName: "FileInfoEntity");

            migrationBuilder.RenameIndex(
                name: "IX_FileInfoEntities_Inactive",
                table: "FileInfoEntity",
                newName: "IX_FileInfoEntity_Inactive");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FileInfoEntity",
                table: "FileInfoEntity",
                column: "Id");
        }
    }
}
