using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HalceraAPI.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class ProductContextAD : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DiscountAmount",
                table: "Prices",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompositionType",
                table: "Compositions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountAmount",
                table: "Prices");

            migrationBuilder.DropColumn(
                name: "CompositionType",
                table: "Compositions");
        }
    }
}
