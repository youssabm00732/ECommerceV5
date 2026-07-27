using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Proudacts;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Seeding
{
    public class CatalogDataSeeder : IDataSeeder
    {

        #region Dependancy Injection 
        private readonly StoreDbContext dbContext;
        private readonly ILogger<CatalogDataSeeder> logger;
        public CatalogDataSeeder(StoreDbContext dbContext , ILogger<CatalogDataSeeder> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        #endregion

        public async Task SeedAsync(CancellationToken ct = default)
        {
            try
            {
                // pending migrations -> migrate it 
                var Pending = await dbContext.Database.GetPendingMigrationsAsync();
                if (Pending.Count() > 0)
                    await dbContext.Database.MigrateAsync();

                // Build Path to Reach for Josn Folder  
                var SeedPath = Path.Combine(AppContext.BaseDirectory, "DataSeed");
                // 5od al data aly gwa al files aly gwa al folder da .. 3mltlha private function bt3ml kda , hnadiha bs 
                await SeedIfEmptyAsync<ProudactBrand>(SeedPath, "brands.json", ct);
                await SeedIfEmptyAsync<ProudactTypes>(SeedPath, "types.json", ct);
                await SeedIfEmptyAsync<Proudact>(SeedPath, "products.json", ct);
                await SeedIfEmptyAsync<DelivaryMethod>(SeedPath, "delivery.json", ct);

               
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Faild To Seed Data");
                throw;
            }

            
        }

        private async Task SeedIfEmptyAsync<T>(string root , string FileName , CancellationToken ct) where T:class
        {
            // check if any items in Context
            if(await dbContext.Set<T>().AnyAsync(ct)) return;
            // if no items in Context = make path to read from file 
            var filepath = Path.Combine(root, FileName); // DataSeed/File(1 , 2 , 3)
            // check if File path exiest inside folder or not 
            if (!File.Exists(filepath))
            {
                logger.LogWarning($"Seed File Not Found {filepath}");
                return;
            }

                // tlma al context fady + al file mwgod = akr2 mn al file 
                await using var stream = File.OpenRead(filepath); // for any 22ra al file 
                var items = await JsonSerializer.DeserializeAsync<List<T>>(stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }, ct); // kda ana 3mlt de serialize ll Josn 34an yb2a m3aya al Items aly gwah + 3mlt ignore ll UpperCase and lower Case bs m4 3arf leh (serach on it)

                // b3d ma b2a m3aya al items -> add al items de fel set<t> aly fel DbContext  
                if (items?.Count > 0)
                    await dbContext.Set<T>().AddRangeAsync(items , ct);

                // save Changes in Context after Seeding 
                await dbContext.SaveChangesAsync(ct);
            }
        }

        #region Explain SeedAsync()
        /*
          htgib mnha al Path aly 3leh al folders nfsha 
          w tmsk kol file tkr2 mno al Data bel Function aly asmha DeSerialize()
          (aly hwa 3nde json seralizer wna b3mlo Deserialize 34an a34f akr2 mno al data)

          fa awl 7aga azay nkwn al Path bta3na ? 
          ana 3ayz awsl ll Bin Folder l2no gwah al Json file aly h2ra mno al Data 

          AppContext.BaseDirectory -> da ywslne l7d al Assembly aly hwa al Pin Folder bta3e
          "DataSeed" -> Asm al folder aly gwa al Pin (al Josn file aly gwah al Data bat3te)

          b3d ma d5lt gwa al folder f ana 3nde 3 files 
          = yb2a n3ml Generic Fucntion w 3la 7sb al type aly d5lha = bd2 a2ra anhe file mn al tlata 
        fa al function de -> SeedIfEmptyAsync() 
        bta5od mne al folder nfso (SeedPath) -> root , bta5od al file name .. (file mn al 3 files aly gwa al folder) , ct

         what Implemntation inside SeedIfEmptyAsync() : 
         - lw l2it ay item fel Context = mtkml4 2rayt al function .. l2ne 3ayz a3ml seed mn al awl lazem al context yb2a fady 
         - lw mfe4 item w fady = yb2a akwn al path bta3 al file aly h2ra mno al Data aly h3mlha Seeding fel Context 
         
         - tb ana b3d ma wslt ll file aly gwa al Folder .. lazem a check 3la al mosmyat al files de 
           fa ht check an al File path da Exist or not ?

         - fel a5r :  tlma al context fady + al file mwgod = akr2 mn al file 

         -  b3d ma b2a m3aya al items -> add al items de fel set<t> aly fel DbContext 
            
            wbkda al SeedIfEmptyAsync() momken ast5dmha 3 mrat 3la 3 types mo5tlfen 
            hdfha ana tgib al file path aly gwa al Json folder + y2ra data mn al Json files aly gwa al Json folder 
            w y3ml al Validations bta3tha 

         - nrg3 tany ll SeedAsync() : 
         
          
           
         */
        #endregion

        #region IDataSeeder Services 
        /*
          b3d ma 5lst process al seeding .. 34an ast3mlha w a3ml seed 3nde fel data base f3ln 
          lazem ast5dm IDataSeeder aly by implemnto CatalogeDataSeeder class 
          w a3mlo al Service bta3to gwa al Program fel DI Container 34an a2dr astdmo w a3ml Seeding b3d ma 7drt kol 7aga 

          wlakn feh mo4la .. an ana hz7m awe al Program.cs b services ktir awe 
          f kan al 7l an ngm3 kol services l kol layer f mkan wa7d b7is al donya mtz7m4 mne gwa al Program file 

          fa a7na hn3ml Class gwa al Infrastructure layer -> n7ot feh al Services bta3t al InfraStracture layer kolha 
          bdl ma a7otha fel Program in DI Container

          wlma ab2a 3nd al Application layer hb2a a3mlha class m5sos 34an a7ot feh al Services bta3t al Application layer
          bdl ma a7otha del Program in DI Contrainer 
         */
        #endregion
    }

