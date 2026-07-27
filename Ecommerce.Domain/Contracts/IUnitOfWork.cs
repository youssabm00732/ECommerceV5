using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface IUnitOfWork
    {
        // make Function to saveChanges() 
        Task<int> SaveChangesAsync(CancellationToken ct = default);

        // make function GetRepository .. 
        IGenericRepository <TEntity,Tkey> GetRepository<TEntity, Tkey>()
            where TEntity : BaseEntity<Tkey>;
    }
}
