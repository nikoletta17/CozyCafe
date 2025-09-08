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
        #region DbSets
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<CartItem> CartItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MenuItemOptionGroup> MenuItemOptionGroups { get; set; }
        public DbSet<MenuItemOption> MenuItemOptions { get; set; }
        public DbSet<OrderItemOption> OrderItemOptions { get; set; }
        public DbSet<CartItemOption> CartItemOptions { get; set; }

        #endregion DbSets
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            #region Entity Configurations

            ConfigureCategory(builder);
            ConfigureCart(builder);
            ConfigureReview(builder);
            ConfigureOrder(builder);
            ConfigureMenuItem(builder);
            ConfigureCartItem(builder);
            ConfigureCartItemOptions(builder);
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

            #endregion Entity Configurations

            #region Seed Data 
            SeedData.Seed(builder);
            #endregion Seed Data 

            builder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Десерти" },
                new Category { Id = 2, Name = "Основні страви" },
                new Category { Id = 3, Name = "Закуски" },
                new Category { Id = 4, Name = "Напої" }
            );
        }

        //Add relationships between entities
        #region Configure Relationships
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

        private void ConfigureCartItemOptions(ModelBuilder builder)
        {
            builder.Entity<CartItemOption>()
                .HasOne(cio => cio.CartItem)
                .WithMany(ci => ci.SelectedOptions)
                .HasForeignKey(cio => cio.CartItemId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<CartItemOption>()
                .HasOne(cio => cio.MenuItemOption)
                .WithMany() 
                .HasForeignKey(cio => cio.MenuItemOptionId)
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

        #endregion Configure Relationships
    }
}
