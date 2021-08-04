using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class AddSoldCarTableAndDependency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoldCars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VinCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    DealerId = table.Column<int>(type: "int", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExteriorColorId = table.Column<int>(type: "int", nullable: false),
                    InteriorColorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldCars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldCars_Colors_ExteriorColorId",
                        column: x => x.ExteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldCars_Colors_InteriorColorId",
                        column: x => x.InteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoldCars_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldCars_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldCarImages",
                columns: table => new
                {
                    SoldCarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldCarImages", x => new { x.SoldCarId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_SoldCarImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldCarImages_SoldCars_SoldCarId",
                        column: x => x.SoldCarId,
                        principalTable: "SoldCars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldCarImages_ImageId",
                table: "SoldCarImages",
                column: "ImageId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCars_ConfigurationId",
                table: "SoldCars",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCars_DealerId",
                table: "SoldCars",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCars_ExteriorColorId",
                table: "SoldCars",
                column: "ExteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCars_InteriorColorId",
                table: "SoldCars",
                column: "InteriorColorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldCarImages");

            migrationBuilder.DropTable(
                name: "SoldCars");
        }
    }
}
