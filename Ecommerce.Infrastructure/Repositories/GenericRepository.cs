using Ecommerce.Domain.Common;
using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey>
        where TEntity : BaseEntity<Tkey>
    {
        #region Dependancy Injection 
        private readonly StoreDbContext dbContext;
        public GenericRepository(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        } 
        #endregion
        public void Add(TEntity entity)
        {
            dbContext.Set<TEntity>().Add(entity);
        }

        public void Update(TEntity entity)
        {
            dbContext.Set<TEntity>().Update(entity);
        }
        public void Delete(TEntity entity)
        {
            dbContext.Set<TEntity>().Remove(entity);
        }

        public async Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default)
        {
            return await dbContext.Set<TEntity>().AsNoTracking().ToListAsync(ct);
        }

        public async Task<TEntity?> GetByIdAsync(Tkey id, CancellationToken ct = default)
        {
           return await dbContext.Set<TEntity>().FindAsync(id, ct);
        }
        // Specifications Pattern For Include() 
        public async Task<IReadOnlyList<TEntity>> GetAllWithSpeceficationsAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default)
        {
            var Result = SpecificationEvaluator.CreateQuery<TEntity, Tkey>(dbContext.Set<TEntity>(), specifications);

            return await Result.ToListAsync(ct); // Query for Get Multible Items mn kza table ->  dbContext.Set<TEntity>(

            /*
              CreateQuery() btbne al Query b3mlomten -> (bdelo mgmo3t al tables aly 3nde fel Context , wel spesifications)
            fel a5r al Function de bta5od aly gyly mn al CreateQuery() aly gwa al Evaluator Class 
            aly hwa 3obara 3n query feha kza include from kza table .. include(brands).include(Types)
            w t return al Data aly fel Query de as anha List

            hna Function al GetAllWithSpeceficationsAsync() btndh 3la al CreateQuery() aly gwa al Evaluator class 
            Lakn men aly byndh 3la GetAllWithSpeceficationsAsync() ?? 
            aly byst3ml al GenericRepo -> aly hwa al Service 

            
             */
        }

        //  // Specifications Pattern For Where()
        public async Task<TEntity?> GetByIdWithSpecificationAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default)
        {
            var Result = SpecificationEvaluator.CreateQuery<TEntity, Tkey>(dbContext.Set<TEntity>(), specifications);

            return await Result.FirstOrDefaultAsync(ct); // Query for Get One Item 7sb al Id bta3o 
        }

        // Specifications Pattern for Count 
        public async Task<int> GetProudactCountWithSpecificationAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default)
        {
            var Result = SpecificationEvaluator.CreateQuery<TEntity, Tkey>(dbContext.Set<TEntity>(), specifications);

            return await Result.CountAsync(ct);
        }
    }
}
