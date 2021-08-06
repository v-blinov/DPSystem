using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class AddNessessaryFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarEntity_Colors_ExteriorColorId",
                table: "CarEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEntity_Colors_InteriorColorId",
                table: "CarEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEntity_Configurations_ConfigurationId",
                table: "CarEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEntity_Dealers_DealerId",
                table: "CarEntity");

            migrationBuilder.DropForeignKey(
                name: "FK_CarImage_CarEntity_CarEntityId",
                table: "CarImage");

            migrationBuilder.DropForeignKey(
                name: "FK_CarImage_Images_ImageId",
                table: "CarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarImage",
                table: "CarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarEntity",
                table: "CarEntity");

            migrationBuilder.RenameTable(
                name: "CarImage",
                newName: "CarImages");

            migrationBuilder.RenameTable(
                name: "CarEntity",
                newName: "CarEntities");

            migrationBuilder.RenameIndex(
                name: "IX_CarImage_ImageId",
                table: "CarImages",
                newName: "IX_CarImages_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntity_InteriorColorId",
                table: "CarEntities",
                newName: "IX_CarEntities_InteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntity_ExteriorColorId",
                table: "CarEntities",
                newName: "IX_CarEntities_ExteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntity_DealerId",
                table: "CarEntities",
                newName: "IX_CarEntities_DealerId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntity_ConfigurationId",
                table: "CarEntities",
                newName: "IX_CarEntities_ConfigurationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarImages",
                table: "CarImages",
                columns: new[] { "CarEntityId", "ImageId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarEntities",
                table: "CarEntities",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntities_Colors_ExteriorColorId",
                table: "CarEntities",
                column: "ExteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntities_Colors_InteriorColorId",
                table: "CarEntities",
                column: "InteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntities_Configurations_ConfigurationId",
                table: "CarEntities",
                column: "ConfigurationId",
                principalTable: "Configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntities_Dealers_DealerId",
                table: "CarEntities",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarImages_CarEntities_CarEntityId",
                table: "CarImages",
                column: "CarEntityId",
                principalTable: "CarEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarImages_Images_ImageId",
                table: "CarImages",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CarEntities_Colors_ExteriorColorId",
                table: "CarEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEntities_Colors_InteriorColorId",
                table: "CarEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEntities_Configurations_ConfigurationId",
                table: "CarEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_CarEntities_Dealers_DealerId",
                table: "CarEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_CarImages_CarEntities_CarEntityId",
                table: "CarImages");

            migrationBuilder.DropForeignKey(
                name: "FK_CarImages_Images_ImageId",
                table: "CarImages");

            migrationBuilder.DropTable(
                name: "SoldCarImages");

            migrationBuilder.DropTable(
                name: "SoldCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarImages",
                table: "CarImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarEntities",
                table: "CarEntities");

            migrationBuilder.RenameTable(
                name: "CarImages",
                newName: "CarImage");

            migrationBuilder.RenameTable(
                name: "CarEntities",
                newName: "CarEntity");

            migrationBuilder.RenameIndex(
                name: "IX_CarImages_ImageId",
                table: "CarImage",
                newName: "IX_CarImage_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntities_InteriorColorId",
                table: "CarEntity",
                newName: "IX_CarEntity_InteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntities_ExteriorColorId",
                table: "CarEntity",
                newName: "IX_CarEntity_ExteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntities_DealerId",
                table: "CarEntity",
                newName: "IX_CarEntity_DealerId");

            migrationBuilder.RenameIndex(
                name: "IX_CarEntities_ConfigurationId",
                table: "CarEntity",
                newName: "IX_CarEntity_ConfigurationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarImage",
                table: "CarImage",
                columns: new[] { "CarEntityId", "ImageId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarEntity",
                table: "CarEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntity_Colors_ExteriorColorId",
                table: "CarEntity",
                column: "ExteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntity_Colors_InteriorColorId",
                table: "CarEntity",
                column: "InteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntity_Configurations_ConfigurationId",
                table: "CarEntity",
                column: "ConfigurationId",
                principalTable: "Configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarEntity_Dealers_DealerId",
                table: "CarEntity",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarImage_CarEntity_CarEntityId",
                table: "CarImage",
                column: "CarEntityId",
                principalTable: "CarEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CarImage_Images_ImageId",
                table: "CarImage",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
