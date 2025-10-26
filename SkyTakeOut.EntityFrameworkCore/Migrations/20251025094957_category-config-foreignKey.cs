using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyTakeOut.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class categoryconfigforeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Dish_CategoryId",
                table: "Dish",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dish_Category_CategoryId",
                table: "Dish");

            migrationBuilder.DropIndex(
                name: "IX_Dish_CategoryId",
                table: "Dish");
        }
    }
}
