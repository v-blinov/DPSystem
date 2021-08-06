using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiDPSystem.Migrations
{
    public partial class UsingTwoTablesWithOneModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCarImages_Images_ImageId",
                table: "SoldCarImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCarImages_SoldCars_SoldCarId",
                table: "SoldCarImages");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCars_Colors_ExteriorColorId",
                table: "SoldCars");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCars_Colors_InteriorColorId",
                table: "SoldCars");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCars_Configurations_ConfigurationId",
                table: "SoldCars");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCars_Dealers_DealerId",
                table: "SoldCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldCars",
                table: "SoldCars");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldCarImages",
                table: "SoldCarImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarImages",
                table: "CarImages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarEntities",
                table: "CarEntities");

            migrationBuilder.RenameTable(
                name: "SoldCars",
                newName: "SoldCar");

            migrationBuilder.RenameTable(
                name: "SoldCarImages",
                newName: "SoldCarImage");

            migrationBuilder.RenameTable(
                name: "CarImages",
                newName: "CarImage");

            migrationBuilder.RenameTable(
                name: "CarEntities",
                newName: "CarEntity");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCars_InteriorColorId",
                table: "SoldCar",
                newName: "IX_SoldCar_InteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCars_ExteriorColorId",
                table: "SoldCar",
                newName: "IX_SoldCar_ExteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCars_DealerId",
                table: "SoldCar",
                newName: "IX_SoldCar_DealerId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCars_ConfigurationId",
                table: "SoldCar",
                newName: "IX_SoldCar_ConfigurationId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCarImages_ImageId",
                table: "SoldCarImage",
                newName: "IX_SoldCarImage_ImageId");

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
                name: "PK_SoldCar",
                table: "SoldCar",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldCarImage",
                table: "SoldCarImage",
                columns: new[] { "SoldCarId", "ImageId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCar_Colors_ExteriorColorId",
                table: "SoldCar",
                column: "ExteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCar_Colors_InteriorColorId",
                table: "SoldCar",
                column: "InteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCar_Configurations_ConfigurationId",
                table: "SoldCar",
                column: "ConfigurationId",
                principalTable: "Configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCar_Dealers_DealerId",
                table: "SoldCar",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCarImage_Images_ImageId",
                table: "SoldCarImage",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCarImage_SoldCar_SoldCarId",
                table: "SoldCarImage",
                column: "SoldCarId",
                principalTable: "SoldCar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCar_Colors_ExteriorColorId",
                table: "SoldCar");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCar_Colors_InteriorColorId",
                table: "SoldCar");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCar_Configurations_ConfigurationId",
                table: "SoldCar");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCar_Dealers_DealerId",
                table: "SoldCar");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCarImage_Images_ImageId",
                table: "SoldCarImage");

            migrationBuilder.DropForeignKey(
                name: "FK_SoldCarImage_SoldCar_SoldCarId",
                table: "SoldCarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldCarImage",
                table: "SoldCarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SoldCar",
                table: "SoldCar");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarImage",
                table: "CarImage");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CarEntity",
                table: "CarEntity");

            migrationBuilder.RenameTable(
                name: "SoldCarImage",
                newName: "SoldCarImages");

            migrationBuilder.RenameTable(
                name: "SoldCar",
                newName: "SoldCars");

            migrationBuilder.RenameTable(
                name: "CarImage",
                newName: "CarImages");

            migrationBuilder.RenameTable(
                name: "CarEntity",
                newName: "CarEntities");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCarImage_ImageId",
                table: "SoldCarImages",
                newName: "IX_SoldCarImages_ImageId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCar_InteriorColorId",
                table: "SoldCars",
                newName: "IX_SoldCars_InteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCar_ExteriorColorId",
                table: "SoldCars",
                newName: "IX_SoldCars_ExteriorColorId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCar_DealerId",
                table: "SoldCars",
                newName: "IX_SoldCars_DealerId");

            migrationBuilder.RenameIndex(
                name: "IX_SoldCar_ConfigurationId",
                table: "SoldCars",
                newName: "IX_SoldCars_ConfigurationId");

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
                name: "PK_SoldCarImages",
                table: "SoldCarImages",
                columns: new[] { "SoldCarId", "ImageId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_SoldCars",
                table: "SoldCars",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarImages",
                table: "CarImages",
                columns: new[] { "CarEntityId", "ImageId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_CarEntities",
                table: "CarEntities",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCarImages_Images_ImageId",
                table: "SoldCarImages",
                column: "ImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCarImages_SoldCars_SoldCarId",
                table: "SoldCarImages",
                column: "SoldCarId",
                principalTable: "SoldCars",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCars_Colors_ExteriorColorId",
                table: "SoldCars",
                column: "ExteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCars_Colors_InteriorColorId",
                table: "SoldCars",
                column: "InteriorColorId",
                principalTable: "Colors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCars_Configurations_ConfigurationId",
                table: "SoldCars",
                column: "ConfigurationId",
                principalTable: "Configurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SoldCars_Dealers_DealerId",
                table: "SoldCars",
                column: "DealerId",
                principalTable: "Dealers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
