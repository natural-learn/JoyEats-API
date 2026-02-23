using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SkyTakeOut.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class updateordersrelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OrdersId",
                table: "OrderDetail",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrdersId",
                table: "OrderDetail",
                column: "OrdersId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Orders_OrdersId",
                table: "OrderDetail",
                column: "OrdersId",
                principalTable: "Orders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Orders_OrdersId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_OrdersId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "OrdersId",
                table: "OrderDetail");
        }
    }
}
