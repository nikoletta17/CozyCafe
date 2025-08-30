using CozyCafe.Models.Domain.Admin;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CozyCafe.Infrastructure.Data
{
    public class SeedData
    {
        public static void Seed(ModelBuilder builder)
        {
            
            var desserts = new List<MenuItem>
{
            new MenuItem
            {
                Id = 101,
                Name = "Tarte Tatin",
                Description = "Класичний французький перевернутий яблучний пиріг із карамеллю.",
                Price = 150.00m,
                ImageUrl = "/images/deserts/dessert1.jpg",
                CategoryId = 1 // Десерти
            },
            new MenuItem
            {
                Id = 102,
                Name = "Mille-feuille",
                Description = "Листковий торт із кремом патисьєр та ніжною текстурою.",
                Price = 180.00m,
                ImageUrl = "/images/deserts/dessert2.jpg",
                CategoryId = 1
            },
            new MenuItem
            {
                Id = 103,
                Name = "Macaron",
                Description = "Ніжне мигдалеве печиво з кремовою начинкою різних смаків.",
                Price = 40.00m,
                ImageUrl = "/images/deserts/dessert3.jpg",
                CategoryId = 1
            },
            new MenuItem
            {
                Id = 104,
                Name = "Fondant au chocolat",
                Description = "Шоколадний торт з рідкою гарячою начинкою всередині.",
                Price = 200.00m,
                ImageUrl = "/images/deserts/dessert4.jpg",
                CategoryId = 1
            },
            new MenuItem
            {
                Id = 105,
                Name = "Crème brûlée",
                Description = "Запечений крем з карамелізованою хрусткою скоринкою.",
                Price = 170.00m,
                ImageUrl = "/images/deserts/dessert5.jpg",
                CategoryId = 1
            },
            new MenuItem
            {
                Id = 106,
                Name = "Clafoutis",
                Description = "Французька запіканка з вишнями, ніжна і ароматна.",
                Price = 160.00m,
                ImageUrl = "/images/deserts/dessert6.jpg",
                CategoryId = 1
            },

                new MenuItem
            {
                Id = 107,
                Name = "Île flottante",
                Description = "Ніжний заварний крем з білковими 'острівцями', що плавають на ванільному соусі.",
                Price = 155.00m,
                ImageUrl = "/images/deserts/dessert7.jpg",
                CategoryId = 1
            },

                   new MenuItem
                {
                    Id = 108,
                    Name = "Croissant",
                    Description = "Традиційний французький слойонуватий круасан з ніжним масляним смаком.",
                    Price = 90.00m,
                    ImageUrl = "/images/deserts/dessert8.jpg",
                    CategoryId = 1
                }
        };

            var mainDishes = new List<MenuItem>
        {
            new MenuItem
            {
                Id = 201,
                Name = "Bouillabaisse",
                Description = "Традиційний французький рибний суп з прянощами та морепродуктами.",
                Price = 320.00m,
                ImageUrl = "/images/main-dish/main1.jpg",
                CategoryId = 2 // Основні страви
            },
            new MenuItem
            {
                Id = 202,
                Name = "Truite Meunière",
                Description = "Філе форелі, обсмажене з вершковим соусом та лимоном.",
                Price = 280.00m,
                ImageUrl = "/images/main-dish/main2.jpg",
                CategoryId = 2
            },
            new MenuItem
            {
                Id = 203,
                Name = "Sole Meunière",
                Description = "Солень зі смаженим лимонним соусом, класика французької кухні.",
                Price = 350.00m,
                ImageUrl = "/images/main-dish/main3.jpg",
                CategoryId = 2
            },
            new MenuItem
            {
                Id = 204,
                Name = "Salmon en Papillote",
                Description = "Філе лосося, запечене у пергаменті з овочами та травами.",
                Price = 340.00m,
                ImageUrl = "/images/main-dish/main4.jpg",
                CategoryId = 2
            },
            new MenuItem
            {
                Id = 205,
                Name = "Coq au Vin",
                Description = "Курка, тушкована в червоному вині з грибами та цибулею.",
                Price = 300.00m,
                ImageUrl = "/images/main-dish/main5.jpg",
                CategoryId = 2
            },
            new MenuItem
            {
                Id = 206,
                Name = "Boeuf Bourguignon",
                Description = "Тушкована яловичина з вином, овочами та ароматними травами.",
                Price = 380.00m,
                ImageUrl = "/images/main-dish/main6.jpg",
                CategoryId = 2
            },
            new MenuItem
            {
                Id = 207,
                Name = "Duck à l'Orange",
                Description = "Качка з апельсиновим соусом — класичне поєднання солодкого та солоного.",
                Price = 390.00m,
                ImageUrl = "/images/main-dish/main7.jpg",
                CategoryId = 2
            },
            new MenuItem
            {
                Id = 208,
                Name = "Ratatouille with Lamb",
                Description = "Запечена овочева рататуй з ніжним ягням.",
                Price = 370.00m,
                ImageUrl = "/images/main-dish/main8.jpg",
                CategoryId = 2
            }
        };

            var drinks = new List<MenuItem>
        {
            new MenuItem
            {
                Id = 301,
                Name = "Citron Pressé",
                Description = "Свіжий лимонад з м’ятою та льодом.",
                Price = 60.00m,
                ImageUrl = "/images/drinks/drink1.jpg",
                CategoryId = 4 // Напої
            },
            new MenuItem
            {
                Id = 302,
                Name = "Orangina",
                Description = "Газований апельсиновий напій — класика Франції.",
                Price = 50.00m,
                ImageUrl = "/images/drinks/drink2.jpg",
                CategoryId = 4
            },
            new MenuItem
            {
                Id = 303,
                Name = "Pommeau",
                Description = "Фруктовий сидр з яблук, солодкий та освіжаючий.",
                Price = 70.00m,
                ImageUrl = "/images/drinks/drink3.jpg",
                CategoryId = 4
            },
            new MenuItem
            {
                Id = 304,
                Name = "Café au lait",
                Description = "Французька кава з гарячим молоком.",
                Price = 45.00m,
                ImageUrl = "/images/drinks/drink4.jpg",
                CategoryId = 4
            },
            new MenuItem
            {
                Id = 305,
                Name = "Champagne",
                Description = "Ігристе вино з регіону Шампань, класика святкових моментів.",
                Price = 250.00m,
                ImageUrl = "/images/drinks/drink5.jpg",
                CategoryId = 4
            },
            new MenuItem
            {
                Id = 306,
                Name = "Kir Royale",
                Description = "Коктейль із шампанського та чорносмородинового лікеру.",
                Price = 280.00m,
                ImageUrl = "/images/drinks/drink6.jpg",
                CategoryId = 4
            },
            new MenuItem
            {
                Id = 307,
                Name = "Pastis",
                Description = "Анісовий алкогольний напій, популярний у південній Франції.",
                Price = 180.00m,
                ImageUrl = "/images/drinks/drink7.jpg",
                CategoryId = 4
            },
            new MenuItem
            {
                Id = 308,
                Name = "Cognac",
                Description = "Витриманий французький бренді з багатим ароматом.",
                Price = 300.00m,
                ImageUrl = "/images/drinks/drink8.jpg",
                CategoryId = 4
            }
        };

            var appetizers = new List<MenuItem>
        {
            new MenuItem
            {
                Id = 401,
                Name = "Salade Niçoise",
                Description = "Традиційний французький салат з тунцем, яйцями та оливками.",
                Price = 150.00m,
                ImageUrl = "/images/appetizers/appetizer1.jpg",
                CategoryId = 3 // Закуски
            },
            new MenuItem
            {
                Id = 402,
                Name = "Salade Lyonnaise",
                Description = "Салат з беконом, яйцем пашот та гірчичною заправкою.",
                Price = 160.00m,
                ImageUrl = "/images/appetizers/appetizer2.jpg",
                CategoryId = 3
            },
            new MenuItem
            {
                Id = 403,
                Name = "Salade de Chèvre Chaud",
                Description = "Салат із теплою козячою бринзою на тості.",
                Price = 170.00m,
                ImageUrl = "/images/appetizers/appetizer3.jpg",
                CategoryId = 3
            },
            new MenuItem
            {
                Id = 404,
                Name = "Salade de Carottes Râpées",
                Description = "Простий морквяний салат з лимонною заправкою.",
                Price = 120.00m,
                ImageUrl = "/images/appetizers/appetizer4.jpg",
                CategoryId = 3
            },
            new MenuItem
            {
                Id = 405,
                Name = "Soupe à l'oignon",
                Description = "Французький цибульний суп з грінками та сиром.",
                Price = 140.00m,
                ImageUrl = "/images/appetizers/appetizer5.jpg",
                CategoryId = 3
            },
            new MenuItem
            {
                Id = 406,
                Name = "Bouillon Blanc",
                Description = "Легкий курячий бульйон з ароматними травами.",
                Price = 130.00m,
                ImageUrl = "/images/appetizers/appetizer6.jpg",
                CategoryId = 3
            },
            new MenuItem
            {
                Id = 407,
                Name = "Potage Saint-Germain",
                Description = "Зелений гороховий крем-суп.",
                Price = 150.00m,
                ImageUrl = "/images/appetizers/appetizer7.jpg",
                CategoryId = 3
            },
            new MenuItem
            {
                Id = 408,
                Name = "Velouté de Potiron",
                Description = "Гарбузовий крем-суп з ніжним смаком.",
                Price = 160.00m,
                ImageUrl = "/images/appetizers/appetizer8.jpg",
                CategoryId = 3
            }
        };
            builder.Entity<MenuItem>().HasData(desserts.ToArray());
            builder.Entity<MenuItem>().HasData(mainDishes.ToArray());
            builder.Entity<MenuItem>().HasData(appetizers.ToArray());
            builder.Entity<MenuItem>().HasData(drinks.ToArray());

            // Група опцій
            builder.Entity<MenuItemOptionGroup>().HasData(
             new MenuItemOptionGroup
             {
                 Id = 1001,               
                 Name = "Додаткові добавки",
                 MenuItemId = 407          
             }
         );

            // Опції
            builder.Entity<MenuItemOption>().HasData(
              new MenuItemOption
              {
                  Id = 1101,
                  Name = "Сметана",
                  ExtraPrice = 10.00m,
                  OptionGroupId = 1001
              },
              new MenuItemOption
              {
                  Id = 1102,
                  Name = "Круті сухарики",
                  ExtraPrice = 15.00m,
                  OptionGroupId = 1001
              },
              new MenuItemOption
              {
                  Id = 1103,
                  Name = "Зелень",
                  ExtraPrice = 5.00m,
                  OptionGroupId = 1001
              }
          );

            builder.Entity<MenuItemOptionGroup>().HasData(
            new MenuItemOptionGroup
            {
                Id = 1,
                Name = "Соуси",
                MenuItemId = 406 // Bouillon Blanc
            },
            new MenuItemOptionGroup
            {
                Id = 2,
                Name = "Додатки",
                MenuItemId = 108 // Croissant
            }
        );

            builder.Entity<MenuItemOption>().HasData(
                new MenuItemOption { Id = 1, Name = "Без соусу", OptionGroupId = 1 },
                new MenuItemOption { Id = 2, Name = "Гострий соус", ExtraPrice = 5.00m, OptionGroupId = 1 },
                new MenuItemOption { Id = 3, Name = "Сир", ExtraPrice = 10.00m, OptionGroupId = 2 },
                new MenuItemOption { Id = 4, Name = "Мигдаль", ExtraPrice = 7.50m, OptionGroupId = 2 }
            );

        }
    
    }
}
