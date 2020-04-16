using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MLDShopping_Admin.Entities
{
    public class CMSShoppingContext : DbContext
    {
        public CMSShoppingContext(DbContextOptions<CMSShoppingContext> options): base(options)
        {
        }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<AccountPermission> AccountPermissions { get; set; }
        public DbSet<Permission> Permissions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().ToTable("Account");
            modelBuilder.Entity<AccountPermission>().ToTable("AccountPermission");
            modelBuilder.Entity<Permission>().ToTable("Permission");
        }
    }
}
