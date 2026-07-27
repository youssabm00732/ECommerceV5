using AutoMapper;
using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Baskets;
using Ecommerce.Application.Specifications;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Proudacts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class PaymentServices : IPaymentService
    {
        #region DI 
        private readonly IBasketRepository basketRepository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IPaymentGetWay paymentGetWay;
        //private readonly IOptions<PaymentGetwaySettings> _Stripe;
        private readonly PaymentGetwaySettings _stripe;
        private readonly IMapper mapper;

        public PaymentServices(IBasketRepository basketRepository,IUnitOfWork unitOfWork , IPaymentGetWay paymentGetWay
            ,IOptions<PaymentGetwaySettings> StripeSettings,IMapper mapper )
        {
            this.basketRepository = basketRepository;
            this.unitOfWork = unitOfWork;
            this.paymentGetWay = paymentGetWay;
            this.mapper = mapper;
            //_Stripe = (IOptions<PaymentGetwaySettings>) StripeSettings.Value;
            _stripe = StripeSettings.Value;
        }
        #endregion
        public async Task<Result<BasketDto>> CreateOrUpdatePaymentIntentAsync(string BasketId, CancellationToken ct = default)
        {
            // 1. Check on Basket And Items 
            var Basket = await basketRepository.GetBasketAsync(BasketId, ct);
            if (Basket is null)
                return Result<BasketDto>.Fail(Error.NotFound("Basket Is Not Found", $"Basket With Id {BasketId} Was Not Found"));

              // check on items aly fel Basket 
              if(Basket.Items.Count == 0)
                return Result<BasketDto>.Fail(Error.Validation("Basket Is Empty", $"Basket With Id {BasketId} Is Empty"));
            //-------------------------------------------------------------------------------------------------------------
            // 2. Check on Proudact Items is Exist or Not (ana 3mltlha Check mra fel payment w hcheck mra fel Order)
            // and if Exist = hghz al Price bta3e 

            // get all Proudcats 
            var ProudactRepo = unitOfWork.GetRepository<Proudact, int>();
            // amsk kol al Ids bta3t al Proudacts aly 3ndk 
            var ProudactIds = Basket.Items.Select(i => i.Id).ToHashSet();
            // hat al Proudacts aly 3ndk bel Specification  
            var Proudacts = await ProudactRepo.GetAllWithSpeceficationsAsync(new ProudactsWithIdSpecifications(ProudactIds));
            // hnlf 3la kol item gwa al Basket 
            foreach (var item in Basket.Items)
            {
                // hdwr 3la al Proudact bta3k 
                var Proudact = Proudacts.FirstOrDefault(p => p.Id == item.Id);
                if (Proudact == null)
                    return Result<BasketDto>.Fail(Error.NotFound("Proudact Is Not Found", $"Proudact With Id {item.Id} Is Not Found"));
                // else .. kol 7aga tmam 
                item.Price = Proudact.Price; // amsk al price aly gwa al Basket 5leh al price al asasy aly hwa al Proudact 
            }
            // get all Delivary methods 
            var DeliveryRepo = unitOfWork.GetRepository<DelivaryMethod, int>();
            // h find 3n tri2 Al Delivery Method Id aly gwa al Basket 
            var DeliveryMethod = await DeliveryRepo.GetByIdAsync(Basket.DeliveryMethodId.Value, ct);
            if(DeliveryMethod is null)
                return Result<BasketDto>.Fail(Error.NotFound("Delivery Method Is Not Found", $"Delivery Method With Id {Basket.DeliveryMethodId.Value} Is Not Found"));
            // else .. kol 7aga tmam 
            Basket.ShippingPrice = DeliveryMethod.Cost;
            // hgm3 al total amount 
            var SubTotal = Basket.Items.Sum(i => i.Quantity * i.Price);
            var amount = (long)Math.Round(SubTotal * DeliveryMethod.Cost * 100m);

            // check lw tl3 3ndk payment intent id 
            if (!string.IsNullOrEmpty(Basket.PaymentIntentId))
            {
              var result = await paymentGetWay.UpdatePaymentIntentAsync(Basket.PaymentIntentId, amount,ct);
                Basket.PaymentIntentId = result.PaymentIntentId;
                Basket.CleintSecret = result.ClientSecret;
            }
            // lw m3nde4 Payment intent = a3mlha create 
            else 
            {
                var result = await paymentGetWay.CreatePaymentIntentAsync(amount, _stripe.DefultCurrency, ct);
                Basket.PaymentIntentId = result.PaymentIntentId;
                Basket.CleintSecret = result.ClientSecret;
            }
            // update b2a al Basket bta3k 
            await basketRepository.CreateOrUpdateBasketAsync(Basket,ct:ct);
            return Result<BasketDto>.Ok(mapper.Map<BasketDto>(Basket));
        }
    }
    // make Another Class 34an azbt al Configurations bta3t al AppSettings.json 
    public class PaymentGetwaySettings
    {
        public string SecretKey { get; set; } = default!;
        public string DefultCurrency { get; set; } = "USD";
    }
}
