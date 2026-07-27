using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Contracts
{
    public interface ICachRepository
    {
        // Get
        Task<string> GetAsync(string CachKey, CancellationToken ct = default);
        // Set (CachValue) hwa al Object aly hyt5zn k json .. (CachKey) .. l2n al Json 3obara 3n Key : Value .. TimeToLive -> al w2t aly al json object da hy3i4 feh whwa Cached in RAM
        Task SetAsync(string CachKey, string CachValue, TimeSpan TimeToLive ,CancellationToken ct = default);
    }
}
