using Ecommerce.Application.Common;
using Ecommerce.Application.DTO_s.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface IOrderServices
    {
        Task<Result<OrderToReturnDto>> CreateOrderAsync(OrderDto orderDto, string email, CancellationToken ct=default);
        Task<Result<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersByEmailAsync(string email, CancellationToken ct=default); // hyrg3oli mgmo3a
        Task<Result<IReadOnlyList<DeliveryMethodDto>>> GetAllDelivaryMethodsAsync(CancellationToken ct=default);// hyrg3oli mgmo3a
        Task<Result<OrderToReturnDto>>GetOrderByIdAndEmailAsync(Guid Id,string email, CancellationToken ct = default);// hyrg3oli One Order


    }
}
