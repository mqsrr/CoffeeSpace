using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.OrderingApi.Persistence.Migrations.SagaDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderStateInstance",
                columns: table => new
                {
                    CorrelationId = table.Column<Guid>(type: "uuid", nullable: false),
                    CurrentState = table.Column<int>(type: "integer", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: false),
                    BuyerId = table.Column<string>(type: "text", nullable: true),
                    UpdateOrderStatudCorrelationId = table.Column<string>(type: "text", nullable: true),
                    StockValidationSuccess = table.Column<bool>(type: "boolean", nullable: false),
                    PaymentSuccess = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStateInstance", x => x.CorrelationId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderStateInstance");
        }
    }
}
