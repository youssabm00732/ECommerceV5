using Ecommerce.Application.Common;
using Ecommerce.Application.DTO_s.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface IBasketServices
    {
        // service for get .. (hb3t Dto)
        Task<Result<BasketDto>> GetBasketAsync(string id, CancellationToken ct = default);
        // service for Post .. hstlm dto w a5zno fel Database 
        Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket , CancellationToken ct = default);
        // servicr for Delete Basket from Database 
        Task<Result<bool>> DeleteBasketAsync(string id, CancellationToken ct = default);
    }
}
