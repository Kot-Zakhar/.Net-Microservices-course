using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CommandsService.Migrations
{
    public partial class uniquenessofplatformexternalid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Platforms_PlatformId",
                table: "Commands");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Platforms_ExternalId",
                table: "Platforms",
                column: "ExternalId");

            migrationBuilder.CreateIndex(
                name: "IX_Platforms_ExternalId",
                table: "Platforms",
                column: "ExternalId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Platforms_PlatformId",
                table: "Commands",
                column: "PlatformId",
                principalTable: "Platforms",
                principalColumn: "ExternalId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Commands_Platforms_PlatformId",
                table: "Commands");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Platforms_ExternalId",
                table: "Platforms");

            migrationBuilder.DropIndex(
                name: "IX_Platforms_ExternalId",
                table: "Platforms");

            migrationBuilder.AddForeignKey(
                name: "FK_Commands_Platforms_PlatformId",
                table: "Commands",
                column: "PlatformId",
                principalTable: "Platforms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
