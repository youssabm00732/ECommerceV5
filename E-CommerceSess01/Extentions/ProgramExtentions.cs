using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Seeding;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSess01.Extentions
{
    public static class ProgramExtentions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app) // this WebApplication app -> leh ? 
        {
            // 1 - Create Scope 
            var scope = app.Services.CreateScope();

            // 2 - Create Object of DbContext 
            //var DbContext = scope.ServiceProvider.GetRequiredService<StoreDbContext>();

            // 3 - Create object from ILogger 
            //var CatalogLogger = scope.ServiceProvider.GetRequiredService<ILogger<CatalogDataSeeder>>();

            // 4 - hat al Data aly m3molha pending .. lma 3mlt Add-migrations "" .. w lsa m3mltlha4 Update Database 
            //var Pending = await DbContext.Database.GetPendingMigrationsAsync();
            //if (Pending.Count() > 0)
            //    await DbContext.Database.MigrateAsync(); // 3mlt ll data de migrate 

            // step 4 hwdiha ll seed class in infrastrycture layer (session03)

            // 5 - after migrate => seed data in DbContext 
            //CatalogDataSeeder catalogDataSeeder = new CatalogDataSeeder(DbContext, CatalogLogger);

            var seeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Catalog");

            await /*CatalogDataSeeder*/ seeder.SeedAsync();

            // identity database 
            var Identityseeder = scope.ServiceProvider.GetRequiredKeyedService<IDataSeeder>("Identity");

            await  Identityseeder.SeedAsync();

        }
    }
}
