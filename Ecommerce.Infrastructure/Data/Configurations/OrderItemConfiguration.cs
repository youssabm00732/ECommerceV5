using Ecommerce.Domain.Entities.Orders;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(o => o.Price).HasColumnType("decimal(10,2)");
            builder.OwnsOne(o => o.Proudact); // de m3naha an  public ProudactItemOrdered Proudact { get; set; }.. ProudactItemOrdered m4 hyb2a table bl hyb2a goz2 mn table aly hwa OrderItem .. 3mltha b  OwnsOne() 
        }
    }
}
