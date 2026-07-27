using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSess01.Controllers
{
    public class OrderController : ApiBaseController
    {
        #region DI
        private readonly IOrderServices orderServices;
        public OrderController(IOrderServices orderServices)
        {
            this.orderServices = orderServices;
        }
        #endregion
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<OrderToReturnDto>> CreateOrder([FromBody] OrderDto orderDto, [FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await orderServices.CreateOrderAsync(orderDto, email, ct));
        }

        
        [HttpGet("DelivaryMethods")]
        public async Task<ActionResult<IReadOnlyList<DeliveryMethodDto>>> GetAllDelivaryMethods(CancellationToken ct)
        {
            return ToActionResult(await orderServices.GetAllDelivaryMethodsAsync(ct));
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderToReturnDto>>> GetAllOrdersByEmail([FromQuery] string email,CancellationToken ct)
        {
            return ToActionResult(await orderServices.GetAllOrdersByEmailAsync(email,ct));
        }

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<OrderToReturnDto>> GetOrderByIdAndEmail(Guid id,[FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await orderServices.GetOrderByIdAndEmailAsync(id,email, ct));
        }
    }
}
