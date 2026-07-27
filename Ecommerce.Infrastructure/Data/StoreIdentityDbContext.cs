using Ecommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Data
{
    public class StoreIdentityDbContext(DbContextOptions<StoreIdentityDbContext> options):IdentityDbContext<ApplicationUser>(options)
    {
        // kda ana 3mlt Dbset ll Tables bto3i aly gwa al Identiity Packages 
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // make Config by Fluent Api on ENTITES
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<Address>().ToTable("Adresses");

        }
    }
}
