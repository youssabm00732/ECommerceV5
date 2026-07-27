using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Baskets;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Repositories
{
    public class BasketRepository : IBasketRepository
    {

        #region Dependancy Injection (to connect with Redis Database)
        private readonly IDatabase _database;
        public BasketRepository(IConnectionMultiplexer connection)
        {
            _database = connection.GetDatabase();
        }
        #endregion
        public async Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? TimeToLive = null, CancellationToken ct = default)
        {
            // ana b5zn Json wel Basket 3obara 3n obj -> 7wlo l Json string 
            var Json = JsonSerializer.Serialize(basket);
            // craete basket in Database .. wna b create in redis database bel StringSetAsync() w bta5od mne 3 7gat  .. b create al Basket + al json bta3o + al TimeToLive bta3 al Object da wna b create it 
            var Success = await _database.StringSetAsync(basket.Id, Json, TimeToLive ?? TimeSpan.FromDays(30)); // kda lw mbstlk4 time span zy ma fel function .. 5li al defult = 30 yom w ams7 al object mn al database automatic 

            return Success ? basket : null;
        }

        public async Task<bool> DeleteBasketAsync(string id, CancellationToken ct = default)
        {
            // hdi id al basket bs ll Redis database w hyms7o b function KeyDeleteAsync()
            return await _database.KeyDeleteAsync(id);
        }

        public async Task<CustomerBasket> GetBasketAsync(string BasketId, CancellationToken ct = default)
        {
            // hat al Basket mn al Redis DB 
            var Basket = await _database.StringGetAsync(BasketId);
            // check lw al Basket aly gy da is Empty wla la 
            if (Basket.IsNullOrEmpty)
                return null;
            // else 
            return JsonSerializer.Deserialize<CustomerBasket>(Basket.ToString()); // json -> object 

        }

        #region Important Note In Seralize and Desiralize 
        /*
         lma b5zn 7aga gwa Redis Database .. btt5zn 3la ht2t Json string [key , value]
         fa ana b7wl al Object aly 3nde fel C# l josn .. 34an mynf34 a5zn object fel Redis Database .. 

        lakn lma b get 7aga mn al Redis Database into C# Code hna .. fa ana b7wl mn Json-> Object 
        34an al C# byt3aml m3 objects fa al str da m3nah : 
        return JsonSerializer.Deserialize<CustomerBasket>(Basket.ToString());
        ano hy7wl al json into tostring .. y3ne hyb2a kolo string m4 object lsa y3ne m4 h3rf a3ml (Basket.Id)
        w b3d kda 3mlt ll ToString (DeSerialize) -> Object 
         */
        #endregion
    }
}
