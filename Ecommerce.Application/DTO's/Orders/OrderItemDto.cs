using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO_s.Orders
{
    public class OrderItemDto
    {
        public int ProudactId { get; set; }
        public string ProductName { get; set; } = default!;
        public string PictrueUrl { get; set; } = default!;
        public decimal Price { get; set; }
        public int Quantity { get; set; }
    }
}
