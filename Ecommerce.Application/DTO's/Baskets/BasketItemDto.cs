using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.DTO_s.Baskets
{
    public class BasketItemDto
    {
        // nfs aly fel Basket Item aly fel Domain Layer 
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
        [Range(1 , double.MaxValue)] // on price .. 1 to there price 
        public decimal Price { get; set; }
        [Range(1 , 99)]
        public int Quantity { get; set; }
    }
}
