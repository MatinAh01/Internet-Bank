using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace InternetBank.DataLayer.Context
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>()
                .Property(u => u.CustomUserId)
                .ValueGeneratedOnAdd();

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.CustomUserId)
                .IsUnique();

            builder.Entity<Account>()
                .HasIndex(a => new { a.UserId, a.CardNumber, a.ExpireDate, a.CVV2 })
                .HasDatabaseName("IX_Account_UserId_CardNumber_ExpireDate_CVV2");
        }
    }

}