using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.OrderingApi.Persistence.Migrations.OrderingDb
{
    /// <inheritdoc />
    public partial class Remove_PaymentInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Payments_PaymentInfoId",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PaymentInfoId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Buyers_Id",
                table: "Buyers");

            migrationBuilder.DropColumn(
                name: "PaymentInfoId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Status",
                table: "Orders",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Buyers",
                type: "character varying(64)",
                unicode: false,
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Buyers",
                type: "character varying(64)",
                unicode: false,
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldUnicode: false,
                oldMaxLength: 50);

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_Id",
                table: "Buyers",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Buyers_Id",
                table: "Buyers");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "Orders",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "PaymentInfoId",
                table: "Orders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Buyers",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldUnicode: false,
                oldMaxLength: 64);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Buyers",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldUnicode: false,
                oldMaxLength: 64);

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    CardType = table.Column<int>(type: "integer", unicode: false, maxLength: 50, nullable: false),
                    ExpirationMonth = table.Column<int>(type: "integer", unicode: false, nullable: false),
                    ExpirationYear = table.Column<int>(type: "integer", unicode: false, nullable: false),
                    SecurityNumber = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentInfoId",
                table: "Orders",
                column: "PaymentInfoId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_Id",
                table: "Buyers",
                column: "Id",
                unique: true,
                descending: new bool[0]);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Payments_PaymentInfoId",
                table: "Orders",
                column: "PaymentInfoId",
                principalTable: "Payments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
