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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.Property(o => o.SubTotal).HasColumnType("decimal(10,2)");
            builder.Property(o => o.BuyerEmail).IsRequired().HasMaxLength(250);
            builder.Property(o => o.Status).HasConversion<string>().HasMaxLength(50); // HasConversion -> de 34an hwa enum fa by defult byrg3 int 0 , 1 , 2 .. fa al Client m4 hyfhm da fa b7wl al kym de ll string bta3ha (pending = 0)
            builder.OwnsOne(o => o.ShippingAddress); // hykon m4 table bl goz2 mn table al Order .. kol order lazm ykon leh address 
        }
    }
}
