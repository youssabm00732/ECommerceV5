using Ecommerce.Application.Params;
using Ecommerce.Domain.Entities.Proudacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specifications
{
    public class ProudactCountSpecifications:BaseSpecifications<Proudact,int>
    {
        public ProudactCountSpecifications(ProudactQueryParams queryParams)
            : base(P => (!queryParams.barndId.HasValue || P.BrandId == queryParams.barndId)
            && (!queryParams.typeId.HasValue || P.BrandId == queryParams.typeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower())))
        {
            
        }
    }
}
