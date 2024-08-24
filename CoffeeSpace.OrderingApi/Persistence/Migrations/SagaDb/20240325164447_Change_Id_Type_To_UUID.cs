using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.OrderingApi.Persistence.Migrations.SagaDb
{
    /// <inheritdoc />
    public partial class Change_Id_Type_To_UUID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "UpdateOrderStatusCorrelationId",
                table: "OrderStateInstance",
                defaultValue: null,
                oldDefaultValue: "''::text");           
            
            migrationBuilder.AlterColumn<Guid>(
                name: "BuyerId",
                table: "OrderStateInstance",
                defaultValue: null,
                oldDefaultValue: "''::text");
            
            migrationBuilder.AlterColumn<Guid>(
                name: "UpdateOrderStatusCorrelationId",
                table: "OrderStateInstance",
                type: "uuid using \"UpdateOrderStatusCorrelationId\"::uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<Guid>(
                name: "OrderId",
                table: "OrderStateInstance",
                type: "uuid using \"OrderId\"::uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false);

            migrationBuilder.AlterColumn<Guid>(
                name: "BuyerId",
                table: "OrderStateInstance",
                type: "uuid using \"BuyerId\"::uuid",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UpdateOrderStatusCorrelationId",
                table: "OrderStateInstance",
                type: "text",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "OrderId",
                table: "OrderStateInstance",
                type: "text",
                unicode: false,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "OrderStateInstance",
                type: "text",
                unicode: false,
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid");
        }
    }
}
