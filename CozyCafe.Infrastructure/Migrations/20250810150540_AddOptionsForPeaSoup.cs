using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CozyCafe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOptionsForPeaSoup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuItemOptionGroups",
                columns: new[] { "Id", "MenuItemId", "Name" },
                values: new object[] { 1001, 407, "Додаткові добавки" });

            migrationBuilder.InsertData(
                table: "MenuItemOptions",
                columns: new[] { "Id", "ExtraPrice", "Name", "OptionGroupId" },
                values: new object[,]
                {
                    { 1101, 10.00m, "Сметана", 1001 },
                    { 1102, 15.00m, "Круті сухарики", 1001 },
                    { 1103, 5.00m, "Зелень", 1001 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItemOptions",
                keyColumn: "Id",
                keyValue: 1101);

            migrationBuilder.DeleteData(
                table: "MenuItemOptions",
                keyColumn: "Id",
                keyValue: 1102);

            migrationBuilder.DeleteData(
                table: "MenuItemOptions",
                keyColumn: "Id",
                keyValue: 1103);

            migrationBuilder.DeleteData(
                table: "MenuItemOptionGroups",
                keyColumn: "Id",
                keyValue: 1001);
        }
    }
}
