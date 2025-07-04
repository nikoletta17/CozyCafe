using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CozyCafe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedOptionsData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuItemOptionGroups",
                columns: new[] { "Id", "MenuItemId", "Name" },
                values: new object[,]
                {
                    { 1, 406, "Соуси" },
                    { 2, 108, "Додатки" }
                });

            migrationBuilder.InsertData(
                table: "MenuItemOptions",
                columns: new[] { "Id", "ExtraPrice", "Name", "OptionGroupId" },
                values: new object[,]
                {
                    { 1, null, "Без соусу", 1 },
                    { 2, 5.00m, "Гострий соус", 1 },
                    { 3, 10.00m, "Сир", 2 },
                    { 4, 7.50m, "Мигдаль", 2 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItemOptions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItemOptions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "MenuItemOptions",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "MenuItemOptions",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "MenuItemOptionGroups",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "MenuItemOptionGroups",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
