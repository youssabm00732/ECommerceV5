using AutoMapper;
using Ecommerce.Application.Profiles;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Entities.Baskets;
using Ecommerce.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace Ecommerce.Tests.CoreTest.Products
{
    public class BasketServicesIntegrationTest
    {

        #region Test Case 1
        [Fact]
        public async Task GetBasketAsync_BasketExists_ReturnsBasket()
        {
            // Arrange
            using var connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            var database = connection.GetDatabase();
            var basketId = $"integration-test-{Guid.NewGuid()}";

            var basket = new CustomerBasket
            {
                Id = basketId,
                Items =
                [
                    new BasketItem
            {
                Id = 10,
                Name = "Test Product",
                PictureUrl = "test.jpg",
                Price = 500,
                Quantity = 2
            }
                ]
            };

            var json = JsonSerializer.Serialize(basket);
            await database.StringSetAsync(basketId, json, TimeSpan.FromMinutes(1));
            var repository = new BasketRepository(connection);
            var mapper = new Mapper(new MapperConfiguration(cfg => cfg.AddProfile<BasketProfile>(), Microsoft.Extensions.Logging.Abstractions.NullLoggerFactory.Instance));
            var service = new BasketServices(repository, mapper);

            // Act
            var result = await service.GetBasketAsync(basketId);

            // Assert
            Assert.True(result.IsSuccessed);
            Assert.NotNull(result.data);
            Assert.Equal(basketId, result.data.Id);

            var item = Assert.Single(result.data.Items);

            Assert.Equal(10, item.Id);
            Assert.Equal("Test Product", item.Name);
            Assert.Equal("test.jpg", item.PictureUrl);
            Assert.Equal(500m, item.Price);
            Assert.Equal(2, item.Quantity);
        }

        #region Integration Test 
        /*
          leh 3mlna Integration test 3la Service -> GetBasketAsync() ? 
          = al Mock a7na bnghz al Debendancies fel test Function .. 3n tri2 msln a4of al service de bt3ml eh ? 
            btst5dm msln repo bygib data al Proudact .. lw mwgoda fel DB = 7wlha l lDto .. lw m4 mwgoda rg3 Fild 
            fa bagy fel Mock msln b3ml Mock Repo (Fake repo) .. hdfo any a3dl 3la behaviour al repo al asly bta3 al service 
            b7is bdl ma y return Dto msln .. a5lih y return null aw return proudact ana 3mlo bnfsy gwa al Test function 
            w da bhdf any a test (Unit = case .. case if field .. case of Success) w hkza 
            w de Fkrt al Unit Test
          lakn fkrt al Integration Test... 
            hya any ast5dm al Dependancies al 72i2ya bta3t al Service m4 any a3ml Dependances bnfsy fel Test Function 
            zy ma kont b3ml Fel Unit test bel mock .. l2 ana bst5dm Repo al 72i2e bta3 al Service .. w al mapper al 72i2e .. wel  integrated parts 
            aly al service btst5dmha .. fa b test m4 logic b2a .. ana b test al Service nfsha bkol 7aga 72i2ya feha 
         */

        #region what we Doing Before Integration Test 
        /*
        1-  b 4of al Service bt3ml eh -> GetBasketAsync()
          = btt3aml m3 repo BasketRepo .. byt3aml m3 al Redis Database 
            w dor al Function anha t dwr fel Redis DB 3la al BasketId .. lw l2t al Basket = t3mlo Return (Json -> Object in C#) .. else return Faild

        2-  b4of eh al 7gat aly momken a3mlha Test fel Service de 
            msln a testha in Case mrg3t4 Basket .. test lw rg3t Basket .. test an al redis bydwr s7 3la al BasketId

        3- 4of al Dependancies al 72i2ya bta3t al Service de .. fel DI region bta3ha btst5dm mapper eh 
           btklm repo eh w tt check an kol 7aga tmam 

        4 - b4of al service de btt3ml m3 IConnectionMultiblexer .. fa b4of al Obj bta3o mt create fel DI Container
            b4of al Connection string bta3 al Redis tmam 
            btest al Server bta3 al Aubentu 48al wla eh 34an al Redis y4t8l 

        5 - b4of al Project test aly 3amlo al Refrences bta3to htst5dm eh .. fa zwdt ano hy shof al Infrastarcture layer 34an ana 3ayz ast5dm al Basket Repository 

        a5r 7aga fel Test dymn feh Database feha al Data bta3t al Project .. wna 3ayz a test 3la Database tanya b7is mt2sr4 3la al Database al 72i2ya 
        bs de lsa 3ayz a3rf btt3ml azay ... 

         */
        #endregion

        #endregion

        #region intro for Test Function 
        /*
          Arrange -> bn Connect m3 al Redis + bnghz Basket tgribya + nsglha fel Database 
                     w bnghz al Debendancies .. yene kol 7aga m7tagnha mn abl ma nnady 3la al function fel Act 
          bn Create al Mapper + al Service + repository al 722yyen aly btst5dmha Function GetBasketAsync()
          34an n5tbr an al Service 48ala S7 m3 al Dependancies bta3tha 

          Act -> anna n call al GetBasketAsync() bel Id aly ghznah 
          Assert -> nt2kd mn al ntiga .. al Expected result = Actual or not ? 

          eh aly hn3ml 3leh test belzbt : 
          awln al Test de bt5tbr 7alt al nga7 bs ..an al Basket Already Exist .. bs btt Check 3la al Integrations of Service 
          fa a7na lazem n connect bel Redis -> n5zn Basket -> ntlob al Basket 3n tri2 BasketService and Change it Into dto -> al data aly rg3t bt match = al data aly 5znaha (y3ne al Service  GetBasketAsync() f 7alt an feh Baskt ) 

          note : a7na m4 hntest kol Goz2 48al lw7do azay .. a7na hn test al Integrations behnom w ben b3d 48al s7 3la b3do wla la 
          b7is a3rf msln lw feh Goz2 m4 3arf yklm goz2 akt4fo .. zy msln Repo m4 3arf yklm al Redis .. wde fydt al Integration Testing

         note : GetBasketAsync_BasketExists_ReturnsBasket .. al Function asmha kda leh ? 
         34an yofdl wna b3ml ay Test Function tkon al Syntax of this name = MethodUnderTest_Condition_ExpectedResult
         */
        #endregion

        #region Explain Test Function 
        /*
          Arrange : 
          using var connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
          ConnectionMultiplexer -> ms2ol an al C# t Connect m3 al Redis Database 
          localhost:6379 -> da rkm al Port aly redis 48al 3leh 
           leh bnst5dm using var ?? 34an ana h3ml Connection w2t al Test m3 al Redis database .. w 3ayz an w2t al Test ma y5ls al Connection y5ls 
           fa using var btstd3e Disopose() 34an t3ml da 

            var database = connection.GetDatabase();
            hna a7na bn3ml Object mn al Connection 34an yt3aml m3 al Redis Database lw hn3ml feha any aCtion (Get , add ..)
            note : a7na fel test hna .. al Object da hyt3aml m3 al Redis database al 72i2ya bta3t al Project bta3e 
                   34an nshl al test .. wlakn al mfrod anna n3ml Redis database tanya for Testing Operations b3ida 3n al Redis database of broject 

            var basketId = $"integration-test-{Guid.NewGuid()}";
            a7na m7tagin BasketId 34an n5zn al Basket Gwa al redis ..w ntlobha bnfs al key 

             b3d kda 3mlna object mn al Basket aly 3ayzen n5znha fel redis 
             CustomerBasket hwa al Entity aly al repository of Basket byt3aml m3ah 

            var json = JsonSerializer.Serialize(basket); 
            bn7wl al Basket from Object -> Json 34an a2dr a5zno fel Redis Database 

             await database.StringSetAsync(basketId,json,TimeSpan.FromMinutes(1));
              bn5zn al Basket in Redis .. leh ast5dmna StringSetAsync ? 
              l2n al repo bt3na by5zn al basket in Redis as redis String 
          kda a7na ghzna al Basket w5znah fel Redis

          3ayzen nghz al Dependancis aly al Basket service btst5dmha .. public BasketServices( IBasketRepository basketRepository,IMapper mapper)
          IBasketRepository -> 34an da repo byt3aml m3 al Redis Database w bygib al Basket (Json -> Object in C#)
           mapper -> 34an y7wl al basket from CustomerBasket -> BasketDTO

          fa a7na ghzna al Dependncies al 722iya bta3t al Repo -> var repository = new BasketRepository(connection);
          kda mn 5lal al Test hna ll repo h4of f3ln al Repo 48al m3 al Redis f3ln .. bdl ma a3ml Mock 

          zbtna Dependancies al Mapper : w fkrto an al Repo btrg3 -> CustomerBasket .. lakn al Service bta3te btrg3 BasketDTO
          fa al Mapper by3mle al l2ta de 

          var service = new BasketServices(repository, mapper); 
          hna ana ghzt al Service bel Dependancies aly ana 3mltha hna (w de 3mltha bfkrt al DI).. y3ne fnshtha b7is lma andh 3leha b2a fel Act 
          tnfz l service al aslya bs bel Depedancies aly ana 3mltha fel Test Funcion hna 

          Act : 
          var result = await service.GetBasketAsync(basketId); 
          hna ana astd3it al service al asasya .. bs bast5dam al service object hna aly feha al Dependancis bta3te 
          y3ne kda al Service al aslya ht4t8l bel Dependancies aly fel Test Function .. 
          w b3tlha basketId .. aly hwa ana 3amlo brdo hna fel test Function wmghzo 

          fa aly hy7sl an al Service httnfz .. w twsl 3nd goz : 
          if (Basket is null)
            return Result<BasketDto>.Fail(
                Error.NotFound(
                    "GetBasket.NotFound",
                    "Can not find the Basket"));

        return Result<BasketDto>.Ok(
            mapper.Map<BasketDto>(Basket));

        hya m4 ht5o4 fel Null 34an Already ana b test wna m5zn object fel redis ..

        Assert : 
        hna bt2kd mn al Basket aly rg3li hwa hwa al Basket aly tlbnah bel BasketId ? .. m4 ay basket mwgod f redis 
        w bt Check 3la kza 7aga tanya 
         */
        #endregion

        #endregion

        #region Test Case 2 
        [Fact]
        public async Task GetBasketAsync_BasketNotFound_ReturnsNotFound()
        {
            // Arrange
            using var connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");

            var basketId = $"integration-test-{Guid.NewGuid()}";
            var repository = new BasketRepository(connection);
            var services = new ServiceCollection();
            services.AddAutoMapper(cfg =>cfg.AddProfile<BasketProfile>());
            using var serviceProvider = services.BuildServiceProvider();
            var mapper = serviceProvider.GetRequiredService<IMapper>();
            var service = new BasketServices(repository, mapper);

            // Act
            var result = await service.GetBasketAsync(basketId);

            // Assert
            Assert.False(result.IsSuccessed);
            Assert.Null(result.data);
        }

        #region Important Notes 
        /*
          var basketId = $"integration-test-{Guid.NewGuid()}"; 
          da a7na hna bn3ml Random Id m3 kol run 34an admn an al Basket da mykon4 mwgod fel Database 
          
        var repository = new BasketRepository(connection);
        bn3ml Repo 72i2e w bnmrr leh al Connection bta3 al Redis 
        34an lma al Service ttlob basket b3d kda al repo yt3aml m3 al Redis db 
         */
        #endregion

        #endregion

    }







}
