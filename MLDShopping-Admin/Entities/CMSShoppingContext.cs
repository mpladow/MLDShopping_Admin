using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountPermission>().HasKey(sc => new { sc.AccountId, sc.PermissionId });

//            modelBuilder.Entity<AccountPermission>()
//.HasOne(pt => pt.Account)
//.WithMany(p => p.AccountPermissions)
//.HasForeignKey(pt => pt.AccountId);

//            modelBuilder.Entity<AccountPermission>()
//                .HasOne(pt => pt.Permission)
//                .WithMany(t => t.AccountPermissions)
//                .HasForeignKey(pt => pt.PermissionId);

        }
    }
}
