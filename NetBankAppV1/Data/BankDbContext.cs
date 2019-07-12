using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetBankAppV1.Models;

namespace NetBankAppV1.Data
{
    public class BankDbContext : IdentityDbContext
    {
        public BankDbContext(DbContextOptions<BankDbContext> options)
            : base(options)
        {
        }


        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        //public DbSet<Customer> Customers { get; set; }
        //public DbSet<ApplicationDbContext>
        public DbSet<Account> Accounts { get; set; }
        public DbSet<CheckingAccount> CheckingAccounts { get; set; }
        public DbSet<BusinessAccount> BusinessAccounts { get; set; }
        public DbSet<Loan> Loan { get; set; }
        public DbSet<TermDeposit> TermDeposits { get; set; }
        public DbSet<Transaction> Transcations { get; set; }
        public DbSet<Customer> customers { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<CheckingAccount>();
            builder.Entity<BusinessAccount>();
            builder.Entity<Loan>();
            builder.Entity<TermDeposit>();
            builder.Entity<User>()
                .HasOne(u => u.customer)
                .WithOne(c => c.CustomerUser)
                .HasForeignKey<Customer>(b => b.UserRef);
            builder.Entity<Account>()
                .HasMany(c => c.Transcations)
                .WithOne(e => e.account)
                .IsRequired();
            builder.Entity<Transaction>()
                .HasOne(c => c.account)
                .WithMany(e => e.Transcations);
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}
