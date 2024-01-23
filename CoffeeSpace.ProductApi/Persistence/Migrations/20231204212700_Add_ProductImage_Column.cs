using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.ProductApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Add_ProductImage_Column : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Discount",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(200)",
                unicode: false,
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(184)",
                oldUnicode: false,
                oldMaxLength: 184);

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Products",
                type: "bytea",
                unicode: false,
                nullable: false,
                defaultValue: new byte[0]);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(184)",
                unicode: false,
                maxLength: 184,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldUnicode: false,
                oldMaxLength: 200);

            migrationBuilder.AddColumn<float>(
                name: "Discount",
                table: "Products",
                type: "real",
                unicode: false,
                nullable: false,
                defaultValue: 0f);
        }
    }
}
