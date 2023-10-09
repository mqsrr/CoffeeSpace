using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.PaymentService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Removed_Required_Constraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PurchaseUnits_InvoiceId",
                table: "PurchaseUnits");

            migrationBuilder.RenameColumn(
                name: "InvoiceId",
                table: "PurchaseUnits",
                newName: "CustomId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PurchaseUnits",
                type: "text",
                nullable: false,
                defaultValueSql: "gen_random_uuid()",
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateTime",
                table: "Orders",
                type: "text",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false);

            migrationBuilder.AlterColumn<string>(
                name: "ExpirationTime",
                table: "Orders",
                type: "text",
                unicode: false,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CustomId",
                table: "PurchaseUnits",
                newName: "InvoiceId");

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "PurchaseUnits",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldDefaultValueSql: "gen_random_uuid()");

            migrationBuilder.AlterColumn<string>(
                name: "UpdateTime",
                table: "Orders",
                type: "text",
                unicode: false,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ExpirationTime",
                table: "Orders",
                type: "text",
                unicode: false,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldUnicode: false,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_InvoiceId",
                table: "PurchaseUnits",
                column: "InvoiceId",
                unique: true);
        }
    }
}
