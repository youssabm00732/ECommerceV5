using Ecommerce.Application.Params;
using Ecommerce.Domain.Entities.Proudacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Specifications
{
    public class ProudactSpecification:BaseSpecifications<Proudact , int>
    {
        public ProudactSpecification(ProudactQueryParams queryParams)
            : base(P => (!queryParams.barndId.HasValue || P.BrandId == queryParams.barndId)
            && (!queryParams.typeId.HasValue || P.BrandId == queryParams.typeId)
            && (string.IsNullOrEmpty(queryParams.SearchValue) || P.Name.ToLower().Contains(queryParams.SearchValue.ToLower()))) // momken ab3t ll base bta3e 7aga (BaseSpecification Class)
        {
            // 7ot kol al Specifications aly m7tagha
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);

            /*
              hna 2ylo BaseSpecifications<Proudact> 
              fa hyro7 3nd al BaseSpec .. ybdl al TEntity -> Proudcat 
              fa bel taly a3tbr hnak an al list of Expressions of Proudcats .. f3rf ygblk fel AddInclude aly properties aly gwa al Proudacts
             */

            // specifications for Sorting 
            switch (queryParams.sort)
            {
                case Params.Enum.ProudactSortingOptions.NameAsc: AddOrderBy(P => P.Name);break;
                case Params.Enum.ProudactSortingOptions.NameDesc: AddOrderByDesc(P => P.Name);break;
                case Params.Enum.ProudactSortingOptions.PriceAsc: AddOrderBy(P => P.Price);break;
                case Params.Enum.ProudactSortingOptions.PriceDesc: AddOrderByDesc(P => P.Price);break;
                    _:AddOrderBy(P => P.Name);break;
            }
            // specification for pagination 
            ApplayPagination(queryParams.pageSize , queryParams.PageIndex);
        }

        public ProudactSpecification(int id) : base(p=>p.Id == id) // hb3t ll base class al Expression bta3 al Where 
        {
            // 7ot kol al Specifications aly m7tagha
            AddInclude(p => p.Brand);
            AddInclude(p => p.Type);

            /*
              hna 2ylo BaseSpecifications<Proudact> 
              fa hyro7 3nd al BaseSpec .. ybdl al TEntity -> Proudcat 
              fa bel taly a3tbr hnak an al list of Expressions of Proudcats .. f3rf ygblk fel AddInclude aly properties aly gwa al Proudacts
             */
        }
    }
}
