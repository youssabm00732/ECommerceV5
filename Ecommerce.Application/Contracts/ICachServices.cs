using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface ICachServices
    {
        Task<string> GetAsync(string CachKey, CancellationToken ct = default);
        Task SetAsync(string CachKey,object CachValue, TimeSpan TimeToLive,  CancellationToken ct = default);
        //object CachValue -> l2n ana hkon ba3tlk 7aga mn ay no3 bs ana h7wlha b3d kda into string 
    }
}
