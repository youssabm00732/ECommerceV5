using Ecommerce.Application.DTO_s.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO_s.Orders
{
    public class OrderToReturnDto
    {
        public Guid Id { get; set; }
        public string ByerEmail { get; set; } = default!;
        public DateTime OrderDate { get; set; }
        public ICollection<OrderItemDto> Items { get; set; } = [];
        public AddressDto ShippingAddress { get; set; } = default!;
        public string DeliveryMethod { get; set; } = default!;
        public string Status { get; set; } = default!;
        public decimal SubTotal { get; set; }
        public decimal DeliveryCost { get; set; }
        public decimal Total { get; set; }
    }
}
