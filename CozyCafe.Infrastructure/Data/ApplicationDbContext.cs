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

            builder.Entity<MenuItem>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            });

            builder.Entity<MenuItemOption>(entity =>
            {
                entity.Property(e => e.ExtraPrice).HasColumnType("decimal(10,2)");
            });

            builder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Total).HasColumnType("decimal(10,2)");
            });

            builder.Entity<OrderItem>(entity =>
            {
                entity.Property(e => e.Price).HasColumnType("decimal(10,2)");
            });


            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Десерти" },
                new Category { Id = 2, Name = "Основні страви" },
                new Category { Id = 3, Name = "Закуски" },
                new Category { Id = 4, Name = "Напої" }
            );


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
