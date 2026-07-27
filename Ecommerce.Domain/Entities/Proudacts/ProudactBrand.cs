using Ecommerce.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Proudacts
{
    public class ProudactBrand:BaseEntity<int> // wars Id no3o int  
    {
        public string Name { get; set; } = null!;
    }
}
