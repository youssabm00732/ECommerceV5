using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Proudacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class StoreDbContext:DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options):base(options)
        {
            
        }
        #region DbSet<>
        public DbSet<Proudact> proudacts { get; set; } 
        public DbSet<ProudactBrand> proudactBrands { get; set; } 
        public DbSet<ProudactTypes> proudactTypes { get; set; }
        public DbSet<Order> orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<DelivaryMethod> delivaryMethods { get; set; }
        #endregion

        #region OnModelCreating()
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(StoreDbContext).Assembly);
        }
        #endregion
    }
}
