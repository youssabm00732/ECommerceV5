using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface IGenericRepository<TEntity , Tkey> where TEntity : BaseEntity<Tkey>
    {
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);

        #region With/Without Specification 
        Task<TEntity?> GetByIdAsync(Tkey id, CancellationToken ct = default);

        // for Where() Specifications
        Task<TEntity?> GetByIdWithSpecificationAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default);
        #endregion

        #region With/Without Specification 
        Task<IReadOnlyList<TEntity>> GetAllAsync(CancellationToken ct = default); // ht3rd list of Proudcts aw ay 7aga tany ll end-user
        Task<IReadOnlyList<TEntity>> GetAllWithSpeceficationsAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default);
        #endregion

        #region Count Specification 
        Task<int> GetProudactCountWithSpecificationAsync(ISpecifications<TEntity, Tkey> specifications, CancellationToken ct = default);
        #endregion
    }
}
