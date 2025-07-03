using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CozyCafe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddItemsToDesserts : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 107, 1, "Ніжний заварний крем з білковими 'острівцями', що плавають на ванільному соусі.", "/images/deserts/dessert7.jpg", "Île flottante", 155.00m },
                    { 108, 1, "Традиційний французький слойонуватий круасан з ніжним масляним смаком.", "/images/deserts/dessert8.jpg", "Croissant", 90.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 107);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 108);
        }
    }
}
