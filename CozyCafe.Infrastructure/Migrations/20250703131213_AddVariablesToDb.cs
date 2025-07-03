using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CozyCafe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddVariablesToDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "Id", "CategoryId", "Description", "ImageUrl", "Name", "Price" },
                values: new object[,]
                {
                    { 101, 1, "Класичний французький перевернутий яблучний пиріг із карамеллю.", "/images/deserts/dessert1.jpg", "Tarte Tatin", 150.00m },
                    { 102, 1, "Листковий торт із кремом патисьєр та ніжною текстурою.", "/images/deserts/dessert2.jpg", "Mille-feuille", 180.00m },
                    { 103, 1, "Ніжне мигдалеве печиво з кремовою начинкою різних смаків.", "/images/deserts/dessert3.jpg", "Macaron", 40.00m },
                    { 104, 1, "Шоколадний торт з рідкою гарячою начинкою всередині.", "/images/deserts/dessert4.jpg", "Fondant au chocolat", 200.00m },
                    { 105, 1, "Запечений крем з карамелізованою хрусткою скоринкою.", "/images/deserts/dessert5.jpg", "Crème brûlée", 170.00m },
                    { 106, 1, "Французька запіканка з вишнями, ніжна і ароматна.", "/images/deserts/dessert6.jpg", "Clafoutis", 160.00m },
                    { 201, 2, "Традиційний французький рибний суп з прянощами та морепродуктами.", "/images/main-dish/main1.jpg", "Bouillabaisse", 320.00m },
                    { 202, 2, "Філе форелі, обсмажене з вершковим соусом та лимоном.", "/images/main-dish/main2.jpg", "Truite Meunière", 280.00m },
                    { 203, 2, "Солень зі смаженим лимонним соусом, класика французької кухні.", "/images/main-dish/main3.jpg", "Sole Meunière", 350.00m },
                    { 204, 2, "Філе лосося, запечене у пергаменті з овочами та травами.", "/images/main-dish/main4.jpg", "Salmon en Papillote", 340.00m },
                    { 205, 2, "Курка, тушкована в червоному вині з грибами та цибулею.", "/images/main-dish/main5.jpg", "Coq au Vin", 300.00m },
                    { 206, 2, "Тушкована яловичина з вином, овочами та ароматними травами.", "/images/main-dish/main6.jpg", "Boeuf Bourguignon", 380.00m },
                    { 207, 2, "Качка з апельсиновим соусом — класичне поєднання солодкого та солоного.", "/images/main-dish/main7.jpg", "Duck à l'Orange", 390.00m },
                    { 208, 2, "Запечена овочева рататуй з ніжним ягням.", "/images/main-dish/main8.jpg", "Ratatouille with Lamb", 370.00m },
                    { 301, 4, "Свіжий лимонад з м’ятою та льодом.", "/images/drinks/drink1.jpg", "Citron Pressé", 60.00m },
                    { 302, 4, "Газований апельсиновий напій — класика Франції.", "/images/drinks/drink2.jpg", "Orangina", 50.00m },
                    { 303, 4, "Фруктовий сидр з яблук, солодкий та освіжаючий.", "/images/drinks/drink3.jpg", "Pommeau", 70.00m },
                    { 304, 4, "Французька кава з гарячим молоком.", "/images/drinks/drink4.jpg", "Café au lait", 45.00m },
                    { 305, 4, "Ігристе вино з регіону Шампань, класика святкових моментів.", "/images/drinks/drink5.jpg", "Champagne", 250.00m },
                    { 306, 4, "Коктейль із шампанського та чорносмородинового лікеру.", "/images/drinks/drink6.jpg", "Kir Royale", 280.00m },
                    { 307, 4, "Анісовий алкогольний напій, популярний у південній Франції.", "/images/drinks/drink7.jpg", "Pastis", 180.00m },
                    { 308, 4, "Витриманий французький бренді з багатим ароматом.", "/images/drinks/drink8.jpg", "Cognac", 300.00m },
                    { 401, 3, "Традиційний французький салат з тунцем, яйцями та оливками.", "/images/appetizers/appetizer1.jpg", "Salade Niçoise", 150.00m },
                    { 402, 3, "Салат з беконом, яйцем пашот та гірчичною заправкою.", "/images/appetizers/appetizer2.jpg", "Salade Lyonnaise", 160.00m },
                    { 403, 3, "Салат із теплою козячою бринзою на тості.", "/images/appetizers/appetizer3.jpg", "Salade de Chèvre Chaud", 170.00m },
                    { 404, 3, "Простий морквяний салат з лимонною заправкою.", "/images/appetizers/appetizer4.jpg", "Salade de Carottes Râpées", 120.00m },
                    { 405, 3, "Французький цибульний суп з грінками та сиром.", "/images/appetizers/appetizer5.jpg", "Soupe à l'oignon", 140.00m },
                    { 406, 3, "Легкий курячий бульйон з ароматними травами.", "/images/appetizers/appetizer6.jpg", "Bouillon Blanc", 130.00m },
                    { 407, 3, "Зелений гороховий крем-суп.", "/images/appetizers/appetizer7.jpg", "Potage Saint-Germain", 150.00m },
                    { 408, 3, "Гарбузовий крем-суп з ніжним смаком.", "/images/appetizers/appetizer8.jpg", "Velouté de Potiron", 160.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 101);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 102);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 103);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 104);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 105);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 106);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 201);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 202);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 203);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 204);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 205);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 206);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 207);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 208);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 301);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 302);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 303);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 304);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 305);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 306);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 307);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 308);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 401);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 402);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 403);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 404);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 405);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 406);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 407);

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "Id",
                keyValue: 408);
        }
    }
}
