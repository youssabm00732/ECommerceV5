using E_CommerceSess01.Attributes;
using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Proudacts;
using Ecommerce.Application.Params;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSess01.Controllers
{
    public class ProudactController : ApiBaseController
    {
        #region Dependancy Injection 
        private readonly IProudactServices services;
        public ProudactController(IProudactServices services)
        {
            this.services = services;
        }
        #endregion

        // make Action to GetAllProudacts 
        [HttpGet]
        [Authorize]
        // attribute for Cach Filter 
        [RedisCach]
        public async Task<ActionResult<PaginatedResult<ProudactDto>>> GetAllProudacts([FromQuery]ProudactQueryParams queryParams,CancellationToken ct)
        {
            // a call al service aly btgble kol al Proudacts 
            var Proudacts = await services.GetAllProudactsAsync(queryParams, ct);
          
            return ToActionResult<PaginatedResult<ProudactDto>>(Proudacts);

            /*
                var result = ToActionResult(Proudacts);
                return result;
              bdl 
             return Result.ok(Proudacts);

             ToActionResult -> y3ne al Controller bynnady 3la class al ApiBaseController w y5o4 gwa al Function 
             aly bta5od mne ProudactDto (awl function) w tt check 34an t4of al result tal3a azay fel response 
             lw al ProudactDto is ok = tmam
             lw ProudactDto feh problem -> hyreturn al problem + no3o fel response 

             */
        }

        [HttpGet("Brands")]
        public async Task<ActionResult<IReadOnlyList<BrandDto>>> GetAllBrands(CancellationToken ct)
        {
            // a call al service aly btgble kol al Proudacts 
            var Brands = await services.GetAllProudactsBrandAsync();
            var result = ToActionResult(Brands);
            return result;

            /*
                var result = ToActionResult(Proudacts);
                return result;
              bdl 
             return Result.ok(Proudacts);
             */
        }

        [HttpGet("Types")]
        public async Task<ActionResult<IReadOnlyList<TypeDto>>> GetAllTypes(CancellationToken ct)
        {
            // a call al service aly btgble kol al Proudacts 
            var Types = await services.GetAllProudactsTypeAsync();
            var result = ToActionResult(Types);
            return result;

            /*
                var result = ToActionResult(Proudacts);
                return result;
              bdl 
             return Result.ok(Proudacts);
             */
        }


        [HttpGet("{id}")]
        public async Task<ActionResult <ProudactDto>> GetProudactById( int id ,CancellationToken ct)
        {
            // a call al service aly btgble kol al Proudacts 
            var Proudact = await services.GetProudactByIdAsync(id);
            var result = ToActionResult(Proudact);
            return result;

            /*
                var result = ToActionResult(Proudacts);
                return result;
              bdl 
             return Result.ok(Proudacts);
             */
        }

    }
}
