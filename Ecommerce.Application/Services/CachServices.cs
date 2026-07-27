using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class CachServices : ICachServices
    {
        #region Debendancy Injection 
        private readonly ICachRepository cachRepository;
        public CachServices(ICachRepository cachRepository)
        {
            this.cachRepository = cachRepository;
        }
        #endregion
        public Task<string> GetAsync(string CachKey, CancellationToken ct = default)
        {
            return cachRepository.GetAsync(CachKey, ct);
        }

        public Task SetAsync(string CachKey, object CachValue, TimeSpan TimeToLive, CancellationToken ct = default)
        {
            // h7wl al object aly gyly into json .. w feh obtions fel json any a5li al klam gwah by applay CamelCase
            var Json = JsonSerializer.Serialize(CachValue, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });
            // a3ml set ll json da fel Redis Database 
            return cachRepository.SetAsync(CachKey, Json, TimeToLive, ct);
        }
    }
}
