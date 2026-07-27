using Ecommerce.Domain.Common;
using Ecommerce.Domain.Contracts;
using Ecommerce.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        #region Debendancy Injection
        private readonly StoreDbContext dbContext;
        public UnitOfWork(StoreDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        #endregion

        // step 1 : make Dictionary 
        private readonly Dictionary<string, object> _Repos = [];
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            // step 1 : make Dictionary 

           // step 2 : 7dd al Type (name of Class)
           var TypeName = typeof(TEntity).Name;

            // step 3 : check in dictiornay -> hl al Repo name da mwgod abl kda wla la .. w da 34an m5l24 al Repo bta3e aktr mn mra 3la mostwa al Request 
            if(_Repos.TryGetValue(TypeName , out object OldRepos))
                return (IGenericRepository<TEntity,Tkey>) OldRepos; // lw l2it al name da fel dectonary = rg3 al repo da lya tane

            // step 4 : lw ml2t4 al repo da fel Dictionary = a5l2ly new generic Repository gwa al Dictionary 
            var NewRepo = new GenericRepository<TEntity, Tkey>(dbContext);

            // step 5 : 5zno fel _Repos
            _Repos[TypeName] = NewRepo;

            // step 6 : return it 
            return NewRepo;


        }

        public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        {
            return await dbContext.SaveChangesAsync(ct);
        }
    }
}
