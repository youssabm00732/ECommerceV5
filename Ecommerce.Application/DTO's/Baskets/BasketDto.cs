using Ecommerce.Domain.Entities.Baskets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO_s.Baskets
{
    public class BasketDto
    {
        // gwah nfs al Details aly mwgoda fel CustomerBasket .. bs byrg3 Collection of BasketItemDto
        public string Id { get; set; } = default!;
        public ICollection<BasketItemDto> Items { get; set; } = [];

        #region for Stripe Payment Method 
        public string? CleintSecret { get; set; }
        public string? PaymentIntentId { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal? ShippingPrice { get; set; }
        #endregion
    }
}
