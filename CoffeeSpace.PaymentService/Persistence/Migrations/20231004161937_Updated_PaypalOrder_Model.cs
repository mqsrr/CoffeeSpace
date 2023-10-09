using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.PaymentService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Updated_PaypalOrder_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paypal Orders_Orders_OrderId",
                table: "Paypal Orders");

            migrationBuilder.DropIndex(
                name: "IX_Paypal Orders_OrderId",
                table: "Paypal Orders");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "Paypal Orders",
                newName: "ApplicationOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Paypal Orders_Orders_PaypalOrderId",
                table: "Paypal Orders",
                column: "PaypalOrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Paypal Orders_Orders_PaypalOrderId",
                table: "Paypal Orders");

            migrationBuilder.RenameColumn(
                name: "ApplicationOrderId",
                table: "Paypal Orders",
                newName: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Paypal Orders_OrderId",
                table: "Paypal Orders",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Paypal Orders_Orders_OrderId",
                table: "Paypal Orders",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
