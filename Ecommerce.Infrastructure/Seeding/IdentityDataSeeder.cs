using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Identity;
using Ecommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Seeding
{
    public class IdentityDataSeeder : IDataSeeder
    {

        #region DI 
        private readonly StoreIdentityDbContext dbContext;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ILogger<IdentityDataSeeder> logger;
        public IdentityDataSeeder(StoreIdentityDbContext dbContext , UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> roleManager , ILogger<IdentityDataSeeder> logger)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
        }
        #endregion
        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                // 1 - Check on Pending Migration 
                var PendingMigration = await dbContext.Database.GetPendingMigrationsAsync(ct);
                // lw feh any Pending Migration = a3mlha migrate fel Databse 
                if (PendingMigration.Any())
                    await dbContext.Database.MigrateAsync(ct);
                // 2 - b3d ma 3mlt applay ll Pending migration = a3ml Seed 
                // 4of al roleManger table lw mfho4 any item = a3mlo item 
                if (!await roleManager.Roles.AnyAsync(ct))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                    await roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
                }
                // f 7alt lw m3nde4 wla User fel Database = a3mlo 3la al 22l one user ykon Admin
                if (!await userManager.Users.AnyAsync(ct))
                {
                    var Admin = new ApplicationUser()
                    {
                        DisplayName = "Mohamed Ahmed",
                        Email = "Mohamed@gmail.com",
                        UserName = "Mohamed",
                        PhoneNumber = "01234567891"
                    };
                    // create user 
                    var Result = await userManager.CreateAsync(Admin, "P@ssw0rd");
                    // lw f 7alt an al user tmam success adelo rol -> Admin 
                    if (Result.Succeeded)
                        await userManager.AddToRoleAsync(Admin, "Admin");
                    else
                        logger.LogWarning("The User Did Not Create");
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex,"Can Not Seed The Data");
                throw;
            }
          

        }
    }
}
