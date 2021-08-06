using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class AddIndexes_AddIssoldFieldInCarhistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarImages_CarEntities_CarEntityId",
                table: "CarImages");

            migrationBuilder.DropTable(
                name: "CarEntities");

            migrationBuilder.DropTable(
                name: "SoldCarImages");

            migrationBuilder.DropTable(
                name: "SoldCars");

            migrationBuilder.RenameColumn(
                name: "CarEntityId",
                table: "CarImages",
                newName: "CarActualId");

            migrationBuilder.CreateTable(
                name: "CarActuals",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    VinCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DealerId = table.Column<int>(type: "int", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExteriorColorId = table.Column<int>(type: "int", nullable: false),
                    InteriorColorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarActuals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarActuals_Colors_ExteriorColorId",
                        column: x => x.ExteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarActuals_Colors_InteriorColorId",
                        column: x => x.InteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarActuals_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarActuals_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsSold = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Version = table.Column<int>(type: "int", nullable: false),
                    VinCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DealerId = table.Column<int>(type: "int", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExteriorColorId = table.Column<int>(type: "int", nullable: false),
                    InteriorColorId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarHistories_Colors_ExteriorColorId",
                        column: x => x.ExteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarHistories_Colors_InteriorColorId",
                        column: x => x.InteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarHistories_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarHistories_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CarHistoryImages",
                columns: table => new
                {
                    CarHistoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ImageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarHistoryImages", x => new { x.CarHistoryId, x.ImageId });
                    table.ForeignKey(
                        name: "FK_CarHistoryImages_CarHistories_CarHistoryId",
                        column: x => x.CarHistoryId,
                        principalTable: "CarHistories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarHistoryImages_Images_ImageId",
                        column: x => x.ImageId,
                        principalTable: "Images",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarActuals_ConfigurationId",
                table: "CarActuals",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_CarActuals_DealerId",
                table: "CarActuals",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_CarActuals_ExteriorColorId",
                table: "CarActuals",
                column: "ExteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarActuals_InteriorColorId",
                table: "CarActuals",
                column: "InteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistories_ConfigurationId",
                table: "CarHistories",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistories_DealerId",
                table: "CarHistories",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistories_ExteriorColorId",
                table: "CarHistories",
                column: "ExteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistories_InteriorColorId",
                table: "CarHistories",
                column: "InteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarHistories_VinCode_Version",
                table: "CarHistories",
                columns: new[] { "VinCode", "Version" });

            migrationBuilder.CreateIndex(
                name: "IX_CarHistoryImages_ImageId",
                table: "CarHistoryImages",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_CarImages_CarActuals_CarActualId",
                table: "CarImages",
                column: "CarActualId",
                principalTable: "CarActuals",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarImages_CarActuals_CarActualId",
                table: "CarImages");

            migrationBuilder.DropTable(
                name: "CarActuals");

            migrationBuilder.DropTable(
                name: "CarHistoryImages");

            migrationBuilder.DropTable(
                name: "CarHistories");

            migrationBuilder.RenameColumn(
                name: "CarActualId",
                table: "CarImages",
                newName: "CarEntityId");

            migrationBuilder.CreateTable(
                name: "CarEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealerId = table.Column<int>(type: "int", nullable: false),
                    ExteriorColorId = table.Column<int>(type: "int", nullable: false),
                    InteriorColorId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VinCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CarEntities_Colors_ExteriorColorId",
                        column: x => x.ExteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarEntities_Colors_InteriorColorId",
                        column: x => x.InteriorColorId,
                        principalTable: "Colors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CarEntities_Configurations_ConfigurationId",
                        column: x => x.ConfigurationId,
                        principalTable: "Configurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarEntities_Dealers_DealerId",
                        column: x => x.DealerId,
                        principalTable: "Dealers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SoldCars",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DealerId = table.Column<int>(type: "int", nullable: false),
                    ExteriorColorId = table.Column<int>(type: "int", nullable: false),
                    InteriorColorId = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    VinCode = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
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
                name: "IX_CarEntities_ConfigurationId",
                table: "CarEntities",
                column: "ConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_CarEntities_DealerId",
                table: "CarEntities",
                column: "DealerId");

            migrationBuilder.CreateIndex(
                name: "IX_CarEntities_ExteriorColorId",
                table: "CarEntities",
                column: "ExteriorColorId");

            migrationBuilder.CreateIndex(
                name: "IX_CarEntities_InteriorColorId",
                table: "CarEntities",
                column: "InteriorColorId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CarImages_CarEntities_CarEntityId",
                table: "CarImages",
                column: "CarEntityId",
                principalTable: "CarEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
