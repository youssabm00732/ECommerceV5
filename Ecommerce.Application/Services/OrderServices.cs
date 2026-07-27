using AutoMapper;
using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Orders;
using Ecommerce.Application.Specifications;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Orders;
using Ecommerce.Domain.Entities.Proudacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class OrderServices : IOrderServices
    {

        #region DI
        private readonly IMapper mapper;
        private readonly IUnitOfWork unitOfWork;
        private readonly IBasketRepository basketRepository;
        public OrderServices(IMapper mapper , IUnitOfWork unitOfWork , IBasketRepository basketRepository)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
            this.basketRepository = basketRepository;
        }
        #endregion
        public async Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct = default)
        {
            // 1. Validate Basket Is Found and has Items 
            var Basket = await basketRepository.GetBasketAsync(orderDto.BasketId, ct);
            if (Basket is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Basket.NotFound", "Basket Is Not Found"));

            if (Basket.Items.Count == 0)
                return Result<OrderToReturnDto>.Fail(Error.Validation("Basket.Empty", "Basket Is Empty"));
            //-----------------------------------------------------------------------------------------------
            // 2. Get Items From Basket and Validate As Proudact
            // then Get Data from Proudact => Make it as Order Item
            var ProudactRepo = unitOfWork.GetRepository<Proudact, int>();
            // Basket Items Ids 
            var ProudactIds = Basket.Items.Select(i => i.Id).ToHashSet();
            // anta ht search bel Idehat de gwa al Proudacts nfsha .. tb anta ht get azay al Proudacts .. m7tag a3ml Filteration Specification bel Where() .. y3ne 22olo lma al2i al ProudactId wa23 wst al Ids aly gbtha de = hat al Proudact .. bs ana m3nde4 specification ya5od mne mgmo3a .. fa hro7 a3mlo
            // in Specification -> new Class -> ProudactsWithIdSpecifications
            var Proudacts = await ProudactRepo.GetAllWithSpeceficationsAsync(new ProudactsWithIdSpecifications(ProudactIds),ct); // anta kda htgib al proudacts aly al Id bta3tha w23a fel ProudactIds aly gbtha  

            // kda ana gbt mgmo3a mn al Proudacts .. hlf b2a 3la kol al Items aly fel Basket .. leh ? = 34an asawe al ItemId bel Id bta3 al Proudact aly m3aya
            var OrderItems = new List<OrderItem>(Basket.Items.Count); // list fadya feha 3dd amakn 3la ad 3dd al amakn aly mwgoda fel Basket 
            foreach (var item in Basket.Items)
            {
                var Proudct = Proudacts.FirstOrDefault(p => p.Id == item.Id); // kda ana m3aya one Proudact 
                if(Proudct is null)
                    return Result<OrderToReturnDto>.Fail(Error.NotFound("Proudact.NotFound", "Proudact Is Not Found"));

                // lw l2it al Proudact Mwgod a3mlo 3la hy2t OrderItem .. fen al order item m3nde4 = abl al ForEach A3ml List =>   var OrderItems = new List<OrderItem>(Basket.Items.Count);
                OrderItems.Add(new OrderItem()
                {
                    Price = Proudct.Price,
                    Quantity = item.Quantity , 
                    Proudact = new ProudactItemOrdered()
                    {
                        ProudactId = Proudct.Id , 
                        PictureUrl = Proudct.PictureUrl,
                        ProudactName = Proudct.Name
                    }

                });
            }
            //--------------------------------------------------------------------------------------------------------
            // 3. store al Orderr Address 
            var OrderAddress = mapper.Map<OrderAddress>(orderDto.ShippingAddress);
            //--------------------------------------------------------------------------------------------------------
            // 4. store al Delivary Method
            var DelivaryMethod = await unitOfWork.GetRepository<DelivaryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId, ct);

            if (DelivaryMethod is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("DelivaryMethod.NotFound", "DelivaryMethod Is Not Found"));
            //--------------------------------------------------------------------------------------------------------
            // 5 . Calculate SubTotal 
            var SubTotal = OrderItems.Sum(i => i.Price * i.Quantity);
            //--------------------------------------------------------------------------------------------------------
            // 6 . Generate Order 
            var Order = new Order()
            {
                BuyerEmail = email , 
                Items = OrderItems,
                ShippingAddress =OrderAddress,
                DelivaryMethodId=DelivaryMethod.Id,
                SubTotal = SubTotal,
                delivaryMethod = DelivaryMethod,
                // added line For Payment 
                PaymentIntentId = Basket.PaymentIntentId
                
            };
            unitOfWork.GetRepository<Order, Guid>().Add(Order);
           var result = await unitOfWork.SaveChangesAsync();
            //--------------------------------------------------------------------------------------------------------
            // 7 . Return Order  
            if (result <=0)
                return Result<OrderToReturnDto>.Fail(Error.Failure("Order.Failure", "Order Can Not Be Created"));
            await basketRepository.DeleteBasketAsync(orderDto.BasketId, ct);

            return Result<OrderToReturnDto>.Ok(mapper.Map<OrderToReturnDto>(Order));
        }

        public async Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDelivaryMethodsAsync(CancellationToken ct = default)
        {
            var delivaryMethods = await unitOfWork.GetRepository<DelivaryMethod, int>().GetAllAsync(ct);
            return Result<IReadOnlyList<DeliveryMethodDto>>.Ok(mapper.Map<IReadOnlyList<DeliveryMethodDto>>(delivaryMethods));
        }

        public async Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersByEmailAsync(string email, CancellationToken ct = default)
        {
            var Orders = await unitOfWork.GetRepository<Order, Guid>().GetAllWithSpeceficationsAsync(new OrderSpecifications(email));
            return Result<IReadOnlyList<OrderToReturnDto>>.Ok(mapper.Map<IReadOnlyList<OrderToReturnDto>>(Orders));
        }

        public async Task<Result<OrderToReturnDto>> GetOrderByIdAndEmailAsync(Guid Id, string email, CancellationToken ct = default)
        {
            var Order = await unitOfWork.GetRepository<Order, Guid>().GetByIdWithSpecificationAsync(new OrderSpecifications(Id,email));
            if(Order is null)
                return Result<OrderToReturnDto>.Fail(Error.NotFound("Order.NotFound", "Order Is Not Found"));

            return Result<OrderToReturnDto>.Ok(mapper.Map<OrderToReturnDto>(Order));
        }
    }
}
