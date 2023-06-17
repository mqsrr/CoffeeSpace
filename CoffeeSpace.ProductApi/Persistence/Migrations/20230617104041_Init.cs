using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.ProductApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Title = table.Column<string>(type: "character varying(64)", unicode: false, maxLength: 64, nullable: false),
                    Description = table.Column<string>(type: "character varying(64)", unicode: false, maxLength: 64, nullable: false),
                    UnitPrice = table.Column<float>(type: "real", unicode: false, nullable: false),
                    Discount = table.Column<float>(type: "real", unicode: false, nullable: false),
                    Quantity = table.Column<int>(type: "integer", unicode: false, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_Id",
                table: "Products",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
