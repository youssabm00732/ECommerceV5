using Ecommerce.Domain.Contracts;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class CachRepository : ICachRepository
    {
        #region Debendancy Injection 
        private readonly IDatabase _database;
        public CachRepository(IConnectionMultiplexer connection )
        {
            _database = connection.GetDatabase();
        }
        #endregion
        public async Task<string> GetAsync(string CachKey, CancellationToken ct = default)
        {
            // Get Json mn al Redis Database depend on this id (CachKey)
            var Value = await _database.StringGetAsync(CachKey);
            // lw al Vlaue is Empty Return null .. else retrun al Value as String .. (wel string da hyt3mlo Deseralize b3d kda) 
            return Value.IsNullOrEmpty ? null : Value.ToString();
        }

        public async Task SetAsync(string CachKey, string CachValue, TimeSpan TimeToLive, CancellationToken ct = default)
        {
            await _database.StringSetAsync(CachKey, CachValue, TimeToLive);
        }
    }
}
