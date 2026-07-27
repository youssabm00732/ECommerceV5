using Ecommerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface IBasketRepository
    {
        // make Function to Create or Update in same time .. return al Basket after create or update  .. note : TimeToLive (al w2t aly hyfdl feh al basket in memnory b3d ma y7slo Create or Update)
        Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null ,CancellationToken ct = default);
        // function to get a basket return al Basket after get it 
        Task<CustomerBasket> GetBasketAsync(string BasketId, CancellationToken ct = default);
        // function to delete basket 
        Task<bool> DeleteBasketAsync(string id, CancellationToken ct = default);
    }
}
