using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoffeeSpace.ProductApi.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Increased_MaxSizeOf_Description : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(184)",
                unicode: false,
                maxLength: 184,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(64)",
                oldUnicode: false,
                oldMaxLength: 64);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "character varying(64)",
                unicode: false,
                maxLength: 64,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(184)",
                oldUnicode: false,
                oldMaxLength: 184);
        }
    }
}
