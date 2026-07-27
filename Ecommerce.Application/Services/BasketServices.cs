using AutoMapper;
using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Baskets;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class BasketServices : IBasketServices
    {
        #region Dependancy Injection 
        private readonly IBasketRepository basketRepository;
        private readonly IMapper mapper;
        public BasketServices(IBasketRepository basketRepository , IMapper mapper)
        {
            this.basketRepository = basketRepository;
            this.mapper = mapper;
        }
        #endregion
        public async Task<Result<BasketDto>> CreateOrUpdateBasketAsync(BasketDto basket, CancellationToken ct = default)
        {
            // Gayle BasketDto -> Basket 
            var CustomerBasket = mapper.Map<CustomerBasket>(basket);
            // create basket in database 
            var Result = await basketRepository.CreateOrUpdateBasketAsync(CustomerBasket, TimeSpan.FromDays(1), ct);
            // b3d ma 3mlt create fel Database h7wl al Basket -> BasketDto tany fel response + ok() 
            // so2al hwa ana m4 b3ml save fel Database bta3t al Redis .. w hya btst2bl mne object mn json bs ?? tb leh 7wlt mn BasketDto -> Basket .. y3ne basket da m4 json 
            return Result is not null ? Result<BasketDto>.Ok(mapper.Map<BasketDto>(Result)) : Result<BasketDto>.Fail(Error.Failure("CreateOrUpdateBasket.Failure", "Can Not Set This Basket"));
        }

        public async Task<Result<bool>> DeleteBasketAsync(string id, CancellationToken ct = default)
        {
            var Result = await basketRepository.DeleteBasketAsync(id, ct);
            return Result ? Result<bool>.Ok(true) : Result<bool>.Fail(Error.Failure("DeleteBasket.Failure", "Can Not Delete This Basket"));
        }

        public async Task<Result<BasketDto>> GetBasketAsync(string id, CancellationToken ct = default)
        {
            var Basket = await basketRepository.GetBasketAsync(id, ct);

            if (Basket is null)
                return Result<BasketDto>.Fail(Error.NotFound("GetBasket.NotFound", "Can not find the Basket"));

            // else 
            return Result<BasketDto>.Ok(mapper.Map<BasketDto>(Basket));
        }
    }
}
