using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class AddDriveFieldInCarTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Drive",
                table: "Cars",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Drive",
                table: "Cars");
        }
    }
}
