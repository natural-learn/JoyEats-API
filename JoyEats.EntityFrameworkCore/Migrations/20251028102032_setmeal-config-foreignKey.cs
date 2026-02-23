using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyTakeOut.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class setmealconfigforeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Setmeal_CategoryId",
                table: "Setmeal",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Setmeal_Category_CategoryId",
                table: "Setmeal");

            migrationBuilder.DropIndex(
                name: "IX_Setmeal_CategoryId",
                table: "Setmeal");
        }
    }
}
