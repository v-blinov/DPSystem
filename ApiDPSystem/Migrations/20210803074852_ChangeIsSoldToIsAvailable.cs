using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class ChangeIsSoldToIsAvailable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSold",
                table: "CarEntities");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "CarEntities",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "CarEntities");

            migrationBuilder.AddColumn<bool>(
                name: "IsSold",
                table: "CarEntities",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
