using System.Collections.Generic;
using Customs.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace Customs.DAL
{
    public class CustomsContext : DbContext
    {
        public CustomsContext(DbContextOptions<CustomsContext> options)
            : base(options)
        {
        }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Storage> Storages { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<EmployeeStorage> EmployeeStorages { get; set; }

        public DbSet<Duty> Duties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<EmployeeStorage>()
                .HasKey(es => new {es.EmployeeId, es.StorageId});
            builder.Entity<EmployeeStorage>()
                .HasOne(es => es.Employee)
                .WithMany(es => es.EmployeeStorages)
                .HasForeignKey(es => es.EmployeeId);
            builder.Entity<EmployeeStorage>()
                .HasOne(es => es.Storage)
                .WithMany(es => es.EmployeeStorages)
                .HasForeignKey(es => es.StorageId);

            builder.Entity<Duty>()
                .HasOne(es => es.Storage)
                .WithMany(es => es.Duties)
                .HasForeignKey(es => es.StorageId)
                .OnDelete(DeleteBehavior.NoAction);
            builder.Entity<Product>()
                .HasOne(es => es.Duty)
                .WithOne(es => es.Product)
                .HasForeignKey<Duty>(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Duty>()
                .HasOne(es => es.Employee)
                .WithMany(es => es.Duties)
                .HasForeignKey(x => x.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Product>()
                .HasOne(es => es.Storage)
                .WithMany(es => es.Products)
                .HasForeignKey(x => x.StorageId)
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        public void DetachEntities<TEntity>(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                DetachEntity(entity);
            }
        }

        private void DetachEntity<TEntity>(TEntity entity)
        {
            Entry(entity).State = EntityState.Detached;
        }
    }
}