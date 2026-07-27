using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
    public class Order:BaseEntity<Guid> // for Id 
    {
        public string BuyerEmail { get; set; } = default!;
        public DateTime OrderDate { get; set; }

        // hngm3 b2a al Order 
        public ICollection<OrderItem> Items { get; set; } = []; // M3ak mgmo3a mn al OrderItems 1 :m 
        public OrderAddress ShippingAddress { get; set; } = default!;

        #region Navigatioal Property 
        public DelivaryMethod delivaryMethod { get; set; } = default!;
        #endregion

        #region FK
        [ForeignKey("DelivaryMethod")]
        public int DelivaryMethodId { get; set; }
        #endregion
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        #region For Payment Stripe method 
        public string? PaymentIntentId { get; set; } 
        #endregion
        public decimal SubTotal { get; set; } // 7sbto .. tmsk kol order item * Quantity // da drived Attribute m4 hy2a col fel table .. fa ana h3mlo funtion t calculate for Display Result bs 

        // function 
        public decimal GetTotal() => SubTotal + (delivaryMethod?.Cost ?? 0); // al Function de Hfdeha fel DTO odam .. h3ml col fel dto asmo total .. wel col da bynade al function de 
    }
}
