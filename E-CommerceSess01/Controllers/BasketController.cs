using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Baskets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSess01.Controllers
{
    //[Route("api/[controller]")]
    //[ApiController]  // l2no fel BaseController 
    public class BasketController : /*ControllerBase*/ ApiBaseController
    {
        #region Debendancy Injection 
        private readonly IBasketServices basketServices;
        public BasketController(IBasketServices basketServices)
        {
            this.basketServices = basketServices;
        }
        #endregion

        // Endpoint htb3t BasketDto fel Response (Get).. bs hb3t fel Request Id of basket 34an al request yygy ll Endpoint de ynfzha
        [HttpGet("{id}")]
        public async Task<ActionResult<BasketDto>> GetBasketAsync(string id, CancellationToken ct)
        { 
            var basket = await basketServices.GetBasketAsync(id, ct);
            return ToActionResult(basket);
        }

        // Make EndPoint bt3ml Create Basket 
        [HttpPost]
        public async Task<ActionResult<BasketDto>> CreateOrUpdateBasket (BasketDto basket, CancellationToken ct)
        {
            var ResultBasket = await basketServices.CreateOrUpdateBasketAsync(basket, ct);
            return ToActionResult(ResultBasket);
        }
        // Endpoint to Delete Basket 
        [HttpDelete("{id}")]
        public async Task<ActionResult<bool>> DeleteBasket(string id, CancellationToken ct)
        {
            var Result = await basketServices.DeleteBasketAsync(id, ct);
            return ToActionResult(Result);
        }
    }
}
