using System.Collections.Generic;
using System.Reflection.Emit;
using Core.CustomerRegistration.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Store.Persistence
{
    public sealed class CustomerRegistrationDbContext(DbContextOptions<CustomerRegistrationDbContext> options) : DbContext(options)
    {
        public DbSet<CustomerEntity> Customers => Set<CustomerEntity>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerEntity>(b =>
            {
                b.ToTable("Customers");
                b.HasKey(p => p.Id);
                b.Property(p => p.Name).IsRequired().HasMaxLength(200);
                b.Property(p => p.Email).IsRequired().HasMaxLength(200);
                b.Property(p => p.Occupation).IsRequired().HasMaxLength(200);
                b.Property(p => p.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
        }
    }
}