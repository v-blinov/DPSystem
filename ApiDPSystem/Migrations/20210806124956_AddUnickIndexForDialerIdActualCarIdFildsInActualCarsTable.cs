using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class AddUnickIndexForDialerIdActualCarIdFildsInActualCarsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_CarActuals_DealerId_Id",
                table: "CarActuals",
                columns: new[] { "DealerId", "Id" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CarActuals_DealerId_Id",
                table: "CarActuals");
        }
    }
}
