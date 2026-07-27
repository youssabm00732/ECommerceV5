using Ecommerce.Domain.Entities.Proudacts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data.Configurations
{
    public class ProudactConfigurations : IEntityTypeConfiguration<Proudact>
    {
        public void Configure(EntityTypeBuilder<Proudact> builder)
        {
            builder.Property(x => x.Name).IsRequired().HasMaxLength(50);
            builder.Property(x => x.PictureUrl).IsRequired().HasMaxLength(200);
            builder.Property(x => x.Description).IsRequired().HasMaxLength(500);

            builder.Property(x => x.Price).HasColumnType("decimal(10,2)");
        }
    }
}
