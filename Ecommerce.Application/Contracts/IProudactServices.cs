using Ecommerce.Application.Common;
using Ecommerce.Application.DTO_s.Proudacts;
using Ecommerce.Application.Params;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface IProudactServices
    {
        // To Get All Proudacts 
        Task<Result<PaginatedResult<ProudactDto>>> GetAllProudactsAsync(ProudactQueryParams queryParams, CancellationToken ct = default);
        // To Get All Proudacts Brands 
        Task<Result<IReadOnlyList<BrandDto>>> GetAllProudactsBrandAsync(CancellationToken ct = default);
        // To Get All Proudacts Types 
        Task<Result<IReadOnlyList<TypeDto>>> GetAllProudactsTypeAsync(CancellationToken ct = default);

        // Get Proudact By Id 
        Task<Result<ProudactDto>> GetProudactByIdAsync(int id, CancellationToken ct = default);
    }
}
