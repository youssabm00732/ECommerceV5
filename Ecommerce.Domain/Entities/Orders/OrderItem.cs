using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
    public class OrderItem : BaseEntity<int> // for Id 
    {
        public ProudactItemOrdered Proudact { get; set; } = default!; // 3 cols 
        // cols 5asa bel OrderItem nfso 
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
