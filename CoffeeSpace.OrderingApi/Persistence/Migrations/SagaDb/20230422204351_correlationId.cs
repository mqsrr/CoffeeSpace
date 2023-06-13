#nullable disable

using Microsoft.EntityFrameworkCore.Migrations;

namespace CoffeeSpace.OrderingApi.Persistence.Migrations.SagaDb
{
    /// <inheritdoc />
    public partial class correlationId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UpdateOrderStatudCorrelationId",
                table: "OrderStateInstance",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UpdateOrderStatudCorrelationId",
                table: "OrderStateInstance");
        }
    }
}
