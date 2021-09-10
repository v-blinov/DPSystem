using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class AddIsActualField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActual",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActual",
                table: "Cars");
        }
    }
}
