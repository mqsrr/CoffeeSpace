using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace CoffeeSpace.PaymentService.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CheckoutPaymentIntent = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    CreateTime = table.Column<string>(type: "text", unicode: false, nullable: false),
                    ExpirationTime = table.Column<string>(type: "text", unicode: false, nullable: true),
                    Status = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    UpdateTime = table.Column<string>(type: "text", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Paypal Orders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicationOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    PaypalOrderId = table.Column<string>(type: "text", nullable: false),
                    BuyerId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paypal Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Paypal Orders_Orders_PaypalOrderId",
                        column: x => x.PaypalOrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseUnits",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    CustomId = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: true),
                    SoftDescriptor = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseUnits", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseUnits_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Items",
                columns: table => new
                {
                    PurchaseUnitId = table.Column<string>(type: "text", nullable: false),
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Category = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(250)", unicode: false, maxLength: 250, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Quantity = table.Column<string>(type: "text", unicode: false, nullable: false),
                    UnitAmount = table.Column<string>(type: "text", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Items", x => new { x.PurchaseUnitId, x.Id });
                    table.ForeignKey(
                        name: "FK_Items_PurchaseUnits_PurchaseUnitId",
                        column: x => x.PurchaseUnitId,
                        principalTable: "PurchaseUnits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Paypal Orders_Id",
                table: "Paypal Orders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Paypal Orders_PaypalOrderId",
                table: "Paypal Orders",
                column: "PaypalOrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_Id",
                table: "PurchaseUnits",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseUnits_OrderId",
                table: "PurchaseUnits",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Items");

            migrationBuilder.DropTable(
                name: "Paypal Orders");

            migrationBuilder.DropTable(
                name: "PurchaseUnits");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
