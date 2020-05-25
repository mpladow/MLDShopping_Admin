using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MLDShopping_Admin.Entities.Models;

namespace MLDShopping_Admin.Entities
{
    public class CMSShoppingContext : DbContext
    {
        public CMSShoppingContext(DbContextOptions<CMSShoppingContext> options) : base(options)
        {
        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountPermission> AccountPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<MediaUrl> MediaUrls { get; set; }
        public DbSet<Product> Products { get; set; }
        //public DbSet<Product> SubCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountPermission>().HasKey(sc => new { sc.AccountId, sc.PermissionId });
            modelBuilder.Entity<AccountPermission>()
                .HasOne(pt => pt.Account)
                .WithMany(p => p.AccountPermissions)
                .HasForeignKey(pt => pt.AccountId);
            modelBuilder.Entity<AccountPermission>()
                .HasOne(pt => pt.Permission)
                .WithMany(p => p.AccountPermissions)
                .HasForeignKey(pt => pt.PermissionId);
            modelBuilder.Entity<Product>()
                .HasMany(c => c.MediaUrls)
                .WithOne(e => e.Product);

            //modelBuilder.Entity<ProductCategory>().HasKey(sc => new { sc.ProductId, sc.CategoryId });
            //modelBuilder.Entity<ProductCategory>()
            //    .HasOne(pt => pt.Product)
            //    .WithMany(t => t.ProductCategory)
            //    .HasForeignKey(pt => pt.ProductId);
            //modelBuilder.Entity<ProductCategory>()
            //    .HasOne(pt => pt.Category)
            //    .WithMany(p => p.ProductCategory)
            //    .HasForeignKey(pt => pt.CategoryId);
        }
    }
}
