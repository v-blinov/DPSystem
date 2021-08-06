using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class DeleteExtraFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SoldCarImage");

            migrationBuilder.DropTable(
                name: "SoldCar");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SoldCar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealerId = table.Column<int>(type: "int", nullable: false),
                    ExteriorColorId = table.Column<int>(type: "int", nullable: false),
                    InteriorColorId = table.Column<int>(type: "int", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VinCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldCar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SoldCar_Colors_ExteriorColorId",
                        column: x => x.ExteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldCar_Colors_InteriorColorId",
                        column: x => x.InteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SoldCar_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldCar_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldCarImage",
                columns: table => new
                {
                    SoldCarId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SoldCarImage", x => new { x.SoldCarId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_SoldCarImage_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SoldCarImage_SoldCar_SoldCarId",
                        column: x => x.SoldCarId,
                        principalTable: "SoldCar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SoldCar_ConfigurationId",
                table: "SoldCar",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCar_DealerId",
                table: "SoldCar",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCar_ExteriorColorId",
                table: "SoldCar",
                column: "ExteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCar_InteriorColorId",
                table: "SoldCar",
                column: "InteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_SoldCarImage_ImageId",
                table: "SoldCarImage",
                column: "ImageId");
        }
    }
}
