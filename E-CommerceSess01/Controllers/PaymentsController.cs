using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Baskets;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSess01.Controllers
{
    
    public class PaymentsController : ApiBaseController
    {
        #region DI
        private readonly IPaymentService paymentService;
        public PaymentsController(IPaymentService paymentService)
        {
            this.paymentService = paymentService;
        }
        #endregion

        [Authorize]
        [HttpPost("{basketId}")]
        public async Task<ActionResult<BasketDto>> CreateOrUpdatePaymentIntent(string basketId, CancellationToken ct)
        {
            var result = await paymentService.CreateOrUpdatePaymentIntentAsync(basketId, ct);
            return ToActionResult(result);
        }

    }
}
