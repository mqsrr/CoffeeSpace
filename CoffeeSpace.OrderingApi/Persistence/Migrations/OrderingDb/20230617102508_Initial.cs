using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.OrderingApi.Persistence.Migrations.OrderingDb
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Street = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    City = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Country = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Buyers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    Name = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    CardNumber = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    SecurityNumber = table.Column<string>(type: "text", unicode: false, nullable: false),
                    ExpirationMonth = table.Column<int>(type: "integer", unicode: false, nullable: false),
                    ExpirationYear = table.Column<int>(type: "integer", unicode: false, nullable: false),
                    CardType = table.Column<int>(type: "integer", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    BuyerId = table.Column<string>(type: "text", nullable: false),
                    AddressId = table.Column<string>(type: "text", nullable: false),
                    PaymentInfoId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_AddressId",
                        column: x => x.AddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Buyers_BuyerId",
                        column: x => x.BuyerId,
                        principalTable: "Buyers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Orders_Payments_PaymentInfoId",
                        column: x => x.PaymentInfoId,
                        principalTable: "Payments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", unicode: false, nullable: false),
                    Title = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "character varying(50)", unicode: false, maxLength: 50, nullable: false),
                    UnitPrice = table.Column<float>(type: "real", precision: 2, nullable: false),
                    Discount = table.Column<float>(type: "real", nullable: false),
                    OrderId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buyers_Id",
                table: "Buyers",
                column: "Id",
                unique: true,
                descending: new bool[0]);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_Id",
                table: "OrderItems",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_AddressId",
                table: "Orders",
                column: "AddressId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_Id",
                table: "Orders",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PaymentInfoId",
                table: "Orders",
                column: "PaymentInfoId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Buyers");

            migrationBuilder.DropTable(
                name: "Payments");
        }
    }
}
