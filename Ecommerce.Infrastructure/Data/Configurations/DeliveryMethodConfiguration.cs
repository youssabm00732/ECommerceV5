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
    public class DeliveryMethodConfiguration : IEntityTypeConfiguration<DelivaryMethod>
    {
        public void Configure(EntityTypeBuilder<DelivaryMethod> builder)
        {
            builder.Property(d => d.Cost).HasColumnType("decimal(10,2)");
            builder.Property(d => d.ShortName).HasColumnType("varchar").HasMaxLength(50);
            builder.Property(d => d.Cost).HasColumnType("varchar").HasMaxLength(100);
            builder.Property(d => d.Cost).HasColumnType("varchar").HasMaxLength(50);
          
        }
    }
}
