using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Orders
{
     public class ProudactItemOrdered
    {
        public int ProudactId { get; set; }
        public string ProudactName { get; set; } = default!;
        public string PictureUrl { get; set; } = default!;
    }
}
