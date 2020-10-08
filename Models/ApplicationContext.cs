using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Stock.Models;

namespace Stock.Models
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<Stocks> Stocks { get; set; }
        public DbSet<ProductInfo> ProductInfos { get; set; }
        public DbSet<SalesInvoice> SalesInvoices { get; set; }
        public DbSet<SalesInfo> SalesInfos { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string adminRoleName = "admin";
            string userRoleName = "user";
            string driverRoleName = "driver";
            string storeKeeperRoleName = "storeKeeper";
            string loaderRoleName = "loader";

            string adminEmail = "admin@mail.com";
            string adminPassword = "123456";

            // добавляем роли
            var adminRole = new Role { Id = 1, Name = adminRoleName };
            var userRole = new Role() { Id = 2, Name = userRoleName };
            var driverRole = new Role { Id = 3, Name = driverRoleName };
            var storeKeeperRole = new Role { Id = 4, Name = storeKeeperRoleName };
            var loaderRole = new Role { Id = 5, Name = loaderRoleName };

            modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole, driverRole, storeKeeperRole, loaderRole });

            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = adminRole.Id };
            modelBuilder.Entity<User>().HasData(new User[] { adminUser });

            modelBuilder.Entity<Product>();
            modelBuilder.Entity<PurchaseInvoice>();
            modelBuilder.Entity<Stocks>();
            modelBuilder.Entity<SalesInvoice>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
