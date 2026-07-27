using Ecommerce.Application.Common;
using Ecommerce.Application.DTO_s.Baskets;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface IPaymentService
    {
        // h3ml Function bt return al Basket bta3k + Payment Intent bta3to 
        Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string BasketId, CancellationToken ct=default);
    }
}
