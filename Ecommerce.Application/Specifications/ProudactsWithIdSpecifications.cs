using Ecommerce.Domain.Entities.Proudacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specifications
{
    public class ProudactsWithIdSpecifications:BaseSpecifications<Proudact,int>
    {
        public ProudactsWithIdSpecifications(IEnumerable<int>Ids)
            :base(p => Ids.Contains(p.Id)) // where() an al Ids aly 3nde bt Contain al ProudactId 
        {
            
        }
    }
}
