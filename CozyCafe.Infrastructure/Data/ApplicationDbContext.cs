using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using CozyCafe.Models.Domain.Admin;
using CozyCafe.Models.Domain.Common;
using CozyCafe.Models.Domain.ForUser;
using CozyCafe.Models.DTO.Admin;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CozyCafe.Infrastructure.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        //Add DbSet
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<MenuItemOptionGroup> MenuItemOptionGroups { get; set; }
        public DbSet<MenuItemOption> MenuItemOptions { get; set; }
        public DbSet<OrderItemOption> OrderItemOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            ConfigureCategory(builder);
            ConfigureCart(builder);
            ConfigureReview(builder);
            ConfigureOrder(builder);
            ConfigureMenuItem(builder);
            ConfigureCartItem(builder);
            ConfigureOrderItem(builder);
            ConfigureMenuItemOptionGroup(builder);
            ConfigureMenuItemOption(builder);
            ConfigureOrderItemOption(builder);

            builder.Entity<Category>().HasData(
    new Category { Id = 1, Name = "Десерти" },
    new Category { Id = 2, Name = "Основні страви" },
    new Category { Id = 3, Name = "Закуски" },
    new Category { Id = 4, Name = "Напої" }
);


            var desserts = new List<MenuItemDto>
            {
                new MenuItemDto
                {
                    Id = 1,
                    Name = "Tarte Tatin",
                    Description = "Класичний французький перевернутий яблучний пиріг із карамеллю.",
                    Price = 150.00m,
                    ImageUrl = "/images/deserts/dessert1.jpg",
                    CategoryName = "Десерти"
                },
                new MenuItemDto
                {
                    Id = 2,
                    Name = "Mille-feuille",
                    Description = "Листковий торт із кремом патисьєр та ніжною текстурою.",
                    Price = 180.00m,
                    ImageUrl = "/images/deserts/dessert2.jpg",
                    CategoryName = "Десерти"
                },
                new MenuItemDto
                {
                    Id = 3,
                    Name = "Macaron",
                    Description = "Ніжне мигдалеве печиво з кремовою начинкою різних смаків.",
                    Price = 40.00m,
                    ImageUrl = "/images/deserts/dessert3.jpg",
                    CategoryName = "Десерти"
                },
                new MenuItemDto
                {
                    Id = 4,
                    Name = "Fondant au chocolat",
                    Description = "Шоколадний торт з рідкою гарячою начинкою всередині.",
                    Price = 200.00m,
                    ImageUrl = "/images/deserts/dessert4.jpg",
                    CategoryName = "Десерти"
                },
                new MenuItemDto
                {
                    Id = 5,
                    Name = "Crème brûlée",
                    Description = "Запечений крем з карамелізованою хрусткою скоринкою.",
                    Price = 170.00m,
                    ImageUrl = "/images/deserts/dessert5.jpg",
                    CategoryName = "Десерти"
                },
                new MenuItemDto
                {
                    Id = 6,
                    Name = "Clafoutis",
                    Description = "Французька запіканка з вишнями, ніжна і ароматна.",
                    Price = 160.00m,
                    ImageUrl = "/images/deserts/dessert6.jpg",
                    CategoryName = "Десерти"
                }
            };

                var mainDishes = new List<MenuItemDto>
            {
                // Рибні
                new MenuItemDto
                {
                    Id = 101,
                    Name = "Bouillabaisse",
                    Description = "Традиційний французький рибний суп з прянощами та морепродуктами.",
                    Price = 320.00m,
                    ImageUrl = "/images/main-dish/main1.jpg",
                    CategoryName = "Основні страви"
                },
                new MenuItemDto
                {
                    Id = 102,
                    Name = "Truite Meunière",
                    Description = "Філе форелі, обсмажене з вершковим соусом та лимоном.",
                    Price = 280.00m,
                    ImageUrl = "/images/main-dish/main2.jpg",
                    CategoryName = "Основні страви"
                },
                new MenuItemDto
                {
                    Id = 103,
                    Name = "Sole Meunière",
                    Description = "Солень зі смаженим лимонним соусом, класика французької кухні.",
                    Price = 350.00m,
                    ImageUrl = "/images/main-dish/main3.jpg",
                    CategoryName = "Основні страви"
                },
                new MenuItemDto
                {
                    Id = 104,
                    Name = "Salmon en Papillote",
                    Description = "Філе лосося, запечене у пергаменті з овочами та травами.",
                    Price = 340.00m,
                    ImageUrl = "/images/main-dish/main4.jpg",
                    CategoryName = "Основні страви"
                },

                // М’ясні
                new MenuItemDto
                {
                    Id = 105,
                    Name = "Coq au Vin",
                    Description = "Курка, тушкована в червоному вині з грибами та цибулею.",
                    Price = 300.00m,
                    ImageUrl = "/images/main-dish/main5.jpg",
                    CategoryName = "Основні страви"
                },
                new MenuItemDto
                {
                    Id = 106,
                    Name = "Boeuf Bourguignon",
                    Description = "Тушкована яловичина з вином, овочами та ароматними травами.",
                    Price = 380.00m,
                    ImageUrl = "/images/main-dish/main6.jpg",
                    CategoryName = "Основні страви"
                },
                new MenuItemDto
                {
                    Id = 107,
                    Name = "Duck à l'Orange",
                    Description = "Качка з апельсиновим соусом — класичне поєднання солодкого та солоного.",
                    Price = 390.00m,
                    ImageUrl = "/images/main-dish/main7.jpg",
                    CategoryName = "Основні страви"
                },
                new MenuItemDto
                {
                    Id = 108,
                    Name = "Ratatouille with Lamb",
                    Description = "Запечена овочева рататуй з ніжним ягням.",
                    Price = 370.00m,
                    ImageUrl = "/images/main-dish/main8.jpg",
                    CategoryName = "Основні страви"
                }
            };

            var drinks = new List<MenuItemDto>
{
    // Безалкогольні
    new MenuItemDto
    {
        Id = 201,
        Name = "Citron Pressé",
        Description = "Свіжий лимонад з м’ятою та льодом.",
        Price = 60.00m,
        ImageUrl = "/images/drinks/drink1.jpg",
        CategoryName = "Напої"
    },
    new MenuItemDto
    {
        Id = 202,
        Name = "Orangina",
        Description = "Газований апельсиновий напій — класика Франції.",
        Price = 50.00m,
        ImageUrl = "/images/drinks/drink2.jpg",
        CategoryName = "Напої"
    },
    new MenuItemDto
    {
        Id = 203,
        Name = "Pommeau",
        Description = "Фруктовий сидр з яблук, солодкий та освіжаючий.",
        Price = 70.00m,
        ImageUrl = "/images/drinks/drink3.jpg",
        CategoryName = "Напої"
    },
    new MenuItemDto
    {
        Id = 204,
        Name = "Café au lait",
        Description = "Французька кава з гарячим молоком.",
        Price = 45.00m,
        ImageUrl = "/images/drinks/drink4.jpg",
        CategoryName = "Напої"
    },

    // Алкогольні
    new MenuItemDto
    {
        Id = 205,
        Name = "Champagne",
        Description = "Ігристе вино з регіону Шампань, класика святкових моментів.",
        Price = 250.00m,
        ImageUrl = "/images/drinks/drink5.jpg",
        CategoryName = "Напої"
    },
    new MenuItemDto
    {
        Id = 206,
        Name = "Kir Royale",
        Description = "Коктейль із шампанського та чорносмородинового лікеру.",
        Price = 280.00m,
        ImageUrl = "/images/drinks/drink6.jpg",
        CategoryName = "Напої"
    },
    new MenuItemDto
    {
        Id = 207,
        Name = "Pastis",
        Description = "Анісовий алкогольний напій, популярний у південній Франції.",
        Price = 180.00m,
        ImageUrl = "/images/drinks/drink7.jpg",
        CategoryName = "Напої"
    },
    new MenuItemDto
    {
        Id = 208,
        Name = "Cognac",
        Description = "Витриманий французький бренді з багатим ароматом.",
        Price = 300.00m,
        ImageUrl = "/images/drinks/drink8.jpg",
        CategoryName = "Напої"
    }
};

            var appetizers = new List<MenuItemDto>
{
    // Салати
    new MenuItemDto
    {
        Id = 301,
        Name = "Salade Niçoise",
        Description = "Традиційний французький салат з тунцем, яйцями та оливками.",
        Price = 150.00m,
        ImageUrl = "/images/appetizers/appetizer1.jpg",
        CategoryName = "Закуски"
    },
    new MenuItemDto
    {
        Id = 302,
        Name = "Salade Lyonnaise",
        Description = "Салат з беконом, яйцем пашот та гірчичною заправкою.",
        Price = 160.00m,
        ImageUrl = "/images/appetizers/appetizer2.jpg",
        CategoryName = "Закуски"
    },
    new MenuItemDto
    {
        Id = 303,
        Name = "Salade de Chèvre Chaud",
        Description = "Салат із теплою козячою бринзою на тості.",
        Price = 170.00m,
        ImageUrl = "/images/appetizers/appetizer3.jpg",
        CategoryName = "Закуски"
    },
    new MenuItemDto
    {
        Id = 304,
        Name = "Salade de Carottes Râpées",
        Description = "Простий морквяний салат з лимонною заправкою.",
        Price = 120.00m,
        ImageUrl = "/images/appetizers/appetizer4.jpg",
        CategoryName = "Закуски"
    },

    // Супи
    new MenuItemDto
    {
        Id = 305,
        Name = "Soupe à l'oignon",
        Description = "Французький цибульний суп з грінками та сиром.",
        Price = 140.00m,
        ImageUrl = "/images/appetizers/appetizer5.jpg",
        CategoryName = "Закуски"
    },
    new MenuItemDto
    {
        Id = 306,
        Name = "Bouillon Blanc",
        Description = "Легкий курячий бульйон з ароматними травами.",
        Price = 130.00m,
        ImageUrl = "/images/appetizers/appetizer6.jpg",
        CategoryName = "Закуски"
    },
    new MenuItemDto
    {
        Id = 307,
        Name = "Potage Saint-Germain",
        Description = "Зелений гороховий крем-суп.",
        Price = 150.00m,
        ImageUrl = "/images/appetizers/appetizer7.jpg",
        CategoryName = "Закуски"
    },
    new MenuItemDto
    {
        Id = 308,
        Name = "Velouté de Potiron",
        Description = "Гарбузовий крем-суп з ніжним смаком.",
        Price = 160.00m,
        ImageUrl = "/images/appetizers/appetizer8.jpg",
        CategoryName = "Закуски"
    }
};



        }

        //Add relationships between entities
        private void ConfigureCategory(ModelBuilder builder)
        {
            builder.Entity<Category>()
                .HasOne(c => c.ParentCategory)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Category>()
            .HasIndex(c => c.Name)
            .IsUnique();
        }

        private void ConfigureCart(ModelBuilder builder)
        {
            builder.Entity<Cart>()
                .HasOne(c => c.User)
                .WithOne(u => u.Cart)
                .HasForeignKey<Cart>(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureReview(ModelBuilder builder)
        {
            builder.Entity<Review>()
                .HasOne(r => r.User)
                .WithMany(u => u.Reviews)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Review>()
                .HasOne(r => r.MenuItem)
                .WithMany(m => m.Reviews)
                .HasForeignKey(r => r.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureOrder(ModelBuilder builder)
        {
            builder.Entity<Order>()
               .HasOne(o => o.Discount)
               .WithMany(d => d.Orders)
               .HasForeignKey(o => o.DiscountId)
               .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>(); // Щоб зберігати enum як string у БД
        }

        private void ConfigureMenuItem(ModelBuilder builder)
        {
            builder.Entity<MenuItem>()
                .HasOne(m => m.Category)
                .WithMany(c => c.MenuItems)
                .HasForeignKey(m => m.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureCartItem(ModelBuilder builder)
        {
            builder.Entity<CartItem>()
                .HasOne(ci => ci.Cart)
                .WithMany(c => c.Items)
                .HasForeignKey(ci => ci.CartId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItem>()
                .HasOne(ci => ci.MenuItem)
                .WithMany(m => m.CartItems)
                .HasForeignKey(ci => ci.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureOrderItem(ModelBuilder builder)
        {
            builder.Entity<OrderItem>()
                .HasOne(oi => oi.Order)
                .WithMany(o => o.Items)
                .HasForeignKey(oi => oi.OrderId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItem>()
                .HasOne(oi => oi.MenuItem)
                .WithMany(m => m.OrderItems)
                .HasForeignKey(oi => oi.MenuItemId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        private void ConfigureMenuItemOptionGroup(ModelBuilder builder)
        {
            builder.Entity<MenuItemOptionGroup>()
                .HasOne(g => g.MenuItem)
                .WithMany(m => m.OptionGroups)
                .HasForeignKey(g => g.MenuItemId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureMenuItemOption(ModelBuilder builder)
        {
            builder.Entity<MenuItemOption>()
                .HasOne(o => o.OptionGroup)
                .WithMany(g => g.Options)
                .HasForeignKey(o => o.OptionGroupId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureOrderItemOption(ModelBuilder builder)
        {
            builder.Entity<OrderItemOption>()
                .HasOne(oio => oio.OrderItem)
                .WithMany(oi => oi.SelectedOptions)
                .HasForeignKey(oio => oio.OrderItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<OrderItemOption>()
                .HasOne(oio => oio.MenuItemOption)
                .WithMany(opt => opt.OrderItemOptions)
                .HasForeignKey(oio => oio.MenuItemOptionId)
                .OnDelete(DeleteBehavior.Cascade);
        }

    }
}
