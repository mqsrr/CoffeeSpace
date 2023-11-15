using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.OrderingApi.Persistence.Migrations.SagaDb
{
    /// <inheritdoc />
    public partial class ChangeStatusTypeToText : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateOrderStatudCorrelationId",
                table: "OrderStateInstance");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "OrderStateInstance",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdateOrderStatusCorrelationId",
                table: "OrderStateInstance",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStateInstance_CorrelationId",
                table: "OrderStateInstance",
                column: "CorrelationId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderStateInstance_CorrelationId",
                table: "OrderStateInstance");

            migrationBuilder.DropColumn(
                name: "UpdateOrderStatusCorrelationId",
                table: "OrderStateInstance");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "OrderStateInstance",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "UpdateOrderStatudCorrelationId",
                table: "OrderStateInstance",
                type: "text",
                nullable: true);
        }
    }
}
