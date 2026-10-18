using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

using Ecommerce.Application;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Identity;
using Ecommerce.Application.DTO_s.Orders;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Baskets;
using Ecommerce.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;
using Xunit;
namespace Ecommerce.Tests.CoreTest.Orders
{
    public class OrderServicesIntegrationTests
    {
        #region Private Function for preparing Dependancies insted of Arrange 
        private ServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:StoreDbConnection"] =
                        "server=.;Database=StoreDb;Trusted_Connection=true;TrustServerCertificate=true",

                    ["ConnectionStrings:IdentityConnection"] =
                        "server=.;Database=StoreIdentityDb;Trusted_Connection=true;TrustServerCertificate=true",

                    ["ConnectionStrings:RedisConnection"] = "localhost"
                })
                .Build();

            var services = new ServiceCollection();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();

            return services.BuildServiceProvider();
        } 
        #endregion

        #region Test Case 1 
        [Fact]
        public async Task GetOrderByIdAndEmailAsync_OrderNotFound_ReturnsNotFound()
        {
            // Arrange
            using var serviceProvider = BuildServiceProvider();

            var orderService = serviceProvider.GetRequiredService<IOrderServices>();

            // Act
            var result = await orderService.GetOrderByIdAndEmailAsync(Guid.NewGuid(),"integration-test@example.com");

            // Assert
            Assert.False(result.IsSuccessed);
            Assert.Null(result.data);
        }
        #region What this Test Case Doing 
        /*
          bn Test hna f Order Services -> GetOrderByIdAndEmailAsync() 
          whya 3obara 3n anna bntlob Order by Id and Email al 45s aly tlb al Order lw m4 mygodin 
          nt check an al Service btrg3 Not Found 
          
          note : a7na hnst5dm Integration Test .. fa hna al Service bb3aml m3 al Sql server Database 3ady 
          
         */
        #endregion

        #region New Way for Prepar Debendancies 
        /*
          a7na kona bnghz al Debendancies aly btst5dmha al Service al 72i2ya fel test Function 5astn gwa al Arrange 
          y3ne al Service m7taga (IMabber , BasketRepository) .. fa kont bgzhom hna fel test Function 
          
          lakn feh tri2a tanya t3tmd 3la al DI Container .. whya any a3ml Function hna fel Class dorha anha tnady 
          3la al Debendancies aly ana Already 3amlha fel DI Contair in Proj .. w anady 3leha hna 
          w bst5dm al Way de lw al Service bt7tag Debendancies ktir + de a7sn l2n ana m4 b3ml da Manually 

          al service de GetOrderByIdAndEmailAsync() bt7tag eh fel Debndancies bta3tha ? 
          IMapper , IUnitOfWork , IBasketRepository .. fa b5li al Di hwa aly y7drly al Objects de 

          tb how To Make it ? 
          1- bnday 3la Al using bta3t al Layers aly m7tagha : 
             Ecommerce.Application -> feha AddApplicationServices() -> aly btsgl al Services + AutoMapper 
             Ecommerce.Infrastructure -> Feha AddInfrastructureServices() -> aly feha al Connection bel DB + Redis + IunitOfWork
          
          2 - hn3ml al Function aly btstd3i al Container  .. tb al Private Function de 3obara 3n eh ? 
              awl 7aga 3mlna Configuration feha .. tb a7na m7tagino f eh ? 
              l2n ana m7tag a Connect bel Database msln 34an agib data bta3t al Orders .. m7tag Connect al Redis 34an agib al Basket bta3 al Order 
              fa al ConfigurationBuilder() hwa aly bysa3dna ngm3 al Confiurations aly m7taginha 
             
               tyb men byst5dm al Configurations aly 7drnaha de ? -> services.AddInfrastructureServices(configuration);
               fa kda gwa al InfrastructureServices atlob any a connect mn gwaha bel storeDbContext kda -> configuration.GetConnectionString("storeDbContext")
               
               Note : a7na kda mtslna4 bel Database f3lyn lsa lomgrd anna ktbna al Configurations .. a7na bs ghzna al Connections Information 
                       kda k2ne ktbt 3nwan 3la wr2a lakn lsa mra7t4 ll mkan da f3ln 

               lma 3mlna al step de services.AddInfrastructureServices(configuration); .. ana kda wslt ll 3nwan (StoreDbContext)
               
               var services = new ServiceCollection(); 
               da al mkan aly hn7ot feh al Object aly m7taga al Order service ( IMapper , IUnitOfWork , IBasketRepository)
               w htstd3iha bel DI Container 
              
              services.AddApplicationServices(); -> 34an mwgod feha al Services 
              services.AddInfrastructureServices(configuration); -> 34an mwgod feha al Databases (IUnitOfWork , IBasketRepository)
            
              return services.BuildServiceProvider(); -> b build al Brovider aly ghzna feh al Objects bta3tna 
         */
        #endregion

        #region Explain Test Function 
        /*
          Arrange : 
          using var serviceProvider = BuildServiceProvider(); .. de al Private Function aly 3mlnaha 
          btghzlna al Configurations + al services aly m7tagnha mn al DI Container for Each Layer 
          
          var orderService = serviceProvider.GetRequiredService<IOrderServices>(); 
          bntlob kda bel DI container yghz al OrderServices aly fel Debendancies bta3to byst5dm IMapper ,IUnitOfWork,IBasketRepository w ana mghz al donya hna mn 7is al Debendancies 

          ACT : 
          ndena al Service w adinaha OrderId = Guid 34an nb2a mot2kden anna m4 hnal2i al Order da fel Database 
          w tany Param hwa al Email wna ktbto ay 7aga 34an ab2a mot2kd an al Service t return not found 

          Assert : 
           Assert.False(result.IsSuccessed); -> bnt2kd an IsSuccessed is False 
           Assert.Null(result.data); -> bnt2kd an mfe4 data rag3a mn al Service al aslya 
         */
        #endregion

        #endregion

        #region Test Case 2 

        [Fact]
        public async Task CreateOrderAsync_ValidData_ReturnsOrder()
        {
            // Arrange
            using var connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");

            var database = connection.GetDatabase();

            var basketId = $"integration-test-{Guid.NewGuid()}";

            var basket = new CustomerBasket
            {
                Id = basketId,
                Items = [new BasketItem
            {
                Id = 1,
                Name = "Classic White T-Shirt",
                PictureUrl = "test.jpg",
                Price = 120m,
                Quantity = 2
            }
                ]
            };

            var basketJson = JsonSerializer.Serialize(basket);

            await database.StringSetAsync(basketId,basketJson,TimeSpan.FromMinutes(5));

            // Arrange: Get the real OrderServices from DI
            using var serviceProvider = BuildServiceProvider();

            var orderService =serviceProvider.GetRequiredService<IOrderServices>();
            var email = "integration-test@example.com";

            var orderDto = new OrderDto
            {
                BasketId = basketId,
                DeliveryMethodId = 1,
                ShippingAddress = new AddressDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Street = "Test Street",
                    City = "Cairo",
                    Country = "Egypt"
                }
            };
            // Act
            var result = await orderService.CreateOrderAsync(orderDto, email);

            // Assert
            Assert.True(result.IsSuccessed,string.Join(" | ", result.Errors.Select(e => $"{e.Code}: {e.Description}")) ); // Check an al Proccess ng7t 
            Assert.NotNull(result.data); // check an al Result not Empty 
            Assert.Equal(email, result.data.ByerEmail); // Check an al Oder Eamil + ByerEmail = email + ByerEmail aly b3tnah 

            // Verify that the basket was deleted from Redis
            var basketStillExists = await database.KeyExistsAsync(basketId);
            Assert.False(basketStillExists);

            // Assert for Check order are saved in Database b3d ma 3mlnah 
            var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>();
            var savedOrder = await unitOfWork.GetRepository<Ecommerce.Domain.Entities.Orders.Order, Guid>().GetByIdAsync(result.data.Id);

            Assert.NotNull(savedOrder);
            Assert.Equal(email, savedOrder.BuyerEmail);

            // Assert for Check al kym aly gwa al Order b3d ma 3rft an 7slo save in DB : 
            Assert.Single(savedOrder.Items);

            var savedItem = savedOrder.Items.First();

            Assert.Equal(1, savedItem.Proudact.ProudactId);
            Assert.Equal(2, savedItem.Quantity);
            Assert.Equal(120m, savedItem.Price);

            Assert.Equal(240m, savedOrder.SubTotal);
        }

        #region What This Test Case Doing 
        /*
          hn test anna ntlob kol al Orders aly mortbta b Email mo3yn 
          y3ne hnb3t Email + Id 722yen wel mfrod ano yrg3 m3aya kol al Orders aw 7ta lw kan Order wa7d bs 
          w tb3n hngib Order 722ie mt5zn fel Database bma anna 48alen Integration test 
          
         */
        #endregion

        #region Test Function Explain 
        /*
          Arrange : 
          - ana klmt al Redis Database w 3mlt Creation l Basket fel Redis Database 
            b3ml al Basket da hna fel C# fa 34an yt5zn fel Redis Database b7wlo into Json File 
            l7d dlw2t a7na 3mlna Create ll Basket .. lakn lsa m3mlna4 test ll order l7d dlw2t 

          Arrange: Get the real OrderServices from DI : 
            b3d ma 3mlna Create ll Basket fel Redis .. hnst5dmo 34an ntlob mn OrderServices ano y create Order 
            y3ne h create Order 72i2e fel Sql server .. 3mlt kda l2n ana mkont4 m5zn Order f Table al ordrs in Sql abl kda fel project 
            fa 3mlna eh fel Part da ka Code : 
           - gebt al OrderService bel DI (Bghz al Debendancies) .. brbot al Order bel BasketId + Email aly 3mlto fel Test Function de 
             w bst5dm al deleviary method aly 3ndy fel DB DeliveryMethodId = 1
             ShippingAddress -> aly hyt7wl l AddressDto bel Mapping .. kda ana bgm3 al Order Test bel Info bta3to y3ne 

          Act : 
          bstd3i al Service CreateOrderAsync() hya bt3ml eh fel Code bta3ha ? 
          - bt2ra Basket Test mn al Redis -> Check an Baskt Not Empty -> tgib Proudact mn sql server -> t7sb al Subtotal -> t7fz al Oder fel sql -> t7zf al Basket mn redis b3d ma al Oder yt create

          Assert :
          - ana b test al Order 7slo Create f3ln 
          - b test an al Order at7fz fel Database 
          - b test al Details bta3t al Order 
          - b test an al Basket atms7 mn al Redis b3d ma Create al order 
         */
        #endregion

        #endregion

        #region Test Case 3 
        [Fact]
        public async Task CreateOrderAsync_BasketNotFound_ReturnsNotFound()
        {
            // Arrange
            using var connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            var database = connection.GetDatabase();

            var basketId = $"missing-basket-{Guid.NewGuid()}";

            using var serviceProvider = BuildServiceProvider();
            var orderService = serviceProvider.GetRequiredService<IOrderServices>();

            var orderDto = new OrderDto
            {
                BasketId = basketId,
                DeliveryMethodId = 1,
                ShippingAddress = new AddressDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Street = "Test Street",
                    City = "Cairo",
                    Country = "Egypt"
                }
            };

            // Act
            var result = await orderService.CreateOrderAsync(orderDto,"integration-test@example.com");

            // Assert
            Assert.False(result.IsSuccessed);
            Assert.Null(result.data);
            Assert.Contains(result.Errors, error => error.Code == "Basket.NotFound");
            Assert.False(await database.KeyExistsAsync(basketId));
        }
        #region What this Test Case Doing 
        /*
          al fr2 benha w ben al test Case al ola eh ? 
          First Test Case -> ana btlob Order b Id Guid 34wa2i m4 mwgod -> fa al service t retuen Failer 
          this Case -> anna bn7awl n create Order lma al basket mykon4 mwgod fel Redis 
                       34an 4rt asasy l Create Order an Ykon feh Basket 5as beh 
          w tb3n b4t8l 3la same service CreateOrderAsync() 
          fa al Scenario : 
          nghz BasketId m4 mwgod (m4 hn create Basket fel redis al mra de) -> call Service -> return fail (Basket.NotFound)
        fa ana b5tbr al Goz2 da fel Service al aslya : 

        var Basket = await basketRepository.GetBasketAsync(orderDto.BasketId, ct);

        if (Basket is null)
            return Result<OrderToReturnDto>.Fail(
                Error.NotFound("Basket.NotFound", "Basket Is Not Found")
            ); 
            wfel a5r al Order m4 hyt Create 
         */
        #endregion

        #region Explain Test Function 
        /*
          Arrange : 
          bn connect m3 al Redis Database 34an al basket searching .. wb3den 3mlna BasketId M4 Mwgod 
          w ghzt al OrderService class 34an ast5dm al Debendancis bta3to + nnady 3la al service aly hn3mlha test 
          
          b3den ghzna bynat al Order .. bs 5li balk hwa mlo4 Basket in Redis database ..wtlma mfe4 Basket -> Order m4 hyt3mlo Save 
          
          Act : 
          CreateOrderAsync() 3mltlha Calling .. fa t7awl tgib al Basket mn Redis -> m4 htla2ih -> htrg3 Result.Fail b Basket.NotFound

          Assert : 
          Assert.False(result.IsSuccessed); -> lw al service a3tbrt al 3mlya nag7a r8m an al Basket m4 mwgod = al a5tbar hyf4l
          Assert.Null(result.data); -> y3ne al data bt null 
          Assert.Contains(result.Errors, error => error.Code == "Basket.NotFound"); -> m4 kfaya an al 3mlya tf4l .. bs tzkor sbb al F4l al asasy 
          Assert.False(await database.KeyExistsAsync(basketId)); -> zyadt t2kid anna m2n42na4 key asln l2nna m3mlna4 basket fel redis 
         */
        #endregion

        #endregion

        #region Test Case 4 
        [Fact]
        public async Task CreateOrderAsync_EmptyBasket_ReturnsValidationError()
        {
            // Arrange
            using var connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            var database = connection.GetDatabase();

            var basketId = $"empty-basket-{Guid.NewGuid()}";

            var basket = new CustomerBasket
            {
                Id = basketId,
                Items = []
            };

            var basketJson = JsonSerializer.Serialize(basket);

            await database.StringSetAsync(basketId,basketJson,TimeSpan.FromMinutes(5));

            using var serviceProvider = BuildServiceProvider();
            var orderService = serviceProvider.GetRequiredService<IOrderServices>();

            var orderDto = new OrderDto
            {
                BasketId = basketId,
                DeliveryMethodId = 1,
                ShippingAddress = new AddressDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Street = "Test Street",
                    City = "Cairo",
                    Country = "Egypt"
                }
            };

            // Act
            var result = await orderService.CreateOrderAsync(orderDto,"integration-test@example.com");

            // Assert
            Assert.False(result.IsSuccessed);
            Assert.Null(result.data);
            Assert.Contains(result.Errors, error => error.Code == "Basket.Empty");

            Assert.True(await database.KeyExistsAsync(basketId));
        }
        #region What This Test Case Doing 
        /*
          anha t test an feh Basket f3ln fel Redis .. w ykon mwgod f3ln bs m3ndo4 Items w da 34an n5tbr al 
          Goz2 da fel Service al aslya : 

          if (Basket.Items.Count == 0)
          return Result<OrderToReturnDto>.Fail(
          Error.Validation("Basket.Empty", "Basket Is Empty")
         */
        #endregion

        #region Explain Test Case 
        /*
          Arrange : 
          - awl 7aga bt Connect m3 al Redis Database bta3t al Project 
          - w b3den bn7dr hna Basket al items bta3to fadya .. b3den n7wlo from ObJect into Json by (serialize)
          - b3d kda bn3ml StringSetAsync 34an n5zn al basket da fel Redis Database l2no mn 8irha al basket m4 hyt5zn wel Service htrg3 -> Basket.NotFound bdl Basket.Empty
          - BuildServiceProvider(); -> 34an n48l al service al 72i2ya bel Debendancies al 72i2ya bta3tha + bnghz OrderService 
          - ghzt OrderDto .. tb leh 3mlto r8m an al Service m4 ht create al Order tlama al Basket is Empy ? = L2n al Service al aslya bttlob OrderDto
            ka Input .. bs al service m4 htwsl anha t Create al Order l2n al Basket fel Redis Fadya 
         
          Act : 
          - bstd3i al service 34an t3ml : 
            t2ra al Basket mn al Redis (aly 3mltlo Create hna b Empty Items) -> Items.Count == 0 -> return Basket.Empty abl ma t craete al Order 

         Assert : 
         - Assert.False(result.IsSuccessed); -> Check an al 3mlya f4lt 
         - Assert.Null(result.data); -> Check an al result is null 
         -  Assert.Contains(result.Errors, error => error.Code == "Basket.Empty"); -> sbb al F4l al 72i2e fel service 
         
         */
        #endregion

        #endregion

        #region Test Case 5 

        [Fact]
        public async Task CreateOrderAsync_DeliveryMethodNotFound_ReturnsNotFound()
        {
            // Arrange
            using var connection = await ConnectionMultiplexer.ConnectAsync("localhost:6379");
            var database = connection.GetDatabase();
            var basketId = $"missing-delivery-basket-{Guid.NewGuid()}";

            var basket = new CustomerBasket
            {
                Id = basketId,
                Items =
                [
                    new BasketItem
            {
                Id = 1,
                Name = "Classic White T-Shirt",
                PictureUrl = "test.jpg",
                Price = 120m,
                Quantity = 1
            }
                ]
            };

            var basketJson = JsonSerializer.Serialize(basket);
            await database.StringSetAsync(basketId,basketJson,TimeSpan.FromMinutes(5));
            using var serviceProvider = BuildServiceProvider();
            var orderService =serviceProvider.GetRequiredService<IOrderServices>();

            var orderDto = new OrderDto
            {
                BasketId = basketId,
                DeliveryMethodId = int.MaxValue,
                ShippingAddress = new AddressDto
                {
                    FirstName = "Test",
                    LastName = "User",
                    Street = "Test Street",
                    City = "Cairo",
                    Country = "Egypt"
                }
            };
            // Act
            var result = await orderService.CreateOrderAsync(orderDto,"integration-test@example.com");

            // Assert
            Assert.False(result.IsSuccessed);
            Assert.Null(result.data);
            Assert.Contains( result.Errors, error => error.Code == "DelivaryMethod.NotFound");

            // Cleanup
            await database.KeyDeleteAsync(basketId);
        }

        #region What this Test Case is Doing 
        /*
          lma bnb3t DelivaryMethodId m4 mwgod fel DB al Service al mfrod t return DelivaryMethod.NotFound w my7fz4 al Order al Gdid
          w 7na bn test al Goz2 da fel Service al aslya : 

          var DelivaryMethod = await unitOfWork.GetRepository<DelivaryMethod, int>().GetByIdAsync(orderDto.DeliveryMethodId, ct);

            if (DelivaryMethod is null)
           return Result<OrderToReturnDto>.Fail(Error.NotFound("DelivaryMethod.NotFound","DelivaryMethod Is Not Found"));
         */
        #endregion

        #region Explain Test Function 
        /*
          Arrange : 
          - 3mlna Connection bel Redis Database 34an n create al Basket 
          - b3den adena ll Basket da Guid Id (BasketId) .. 34an ndmn anna m4 hnla2i ano m4 mwgod f Table al Dilavery Method wbeltaly n test an mfe4 DevliaveyMethod
          - b3den 3mlna al al Basket w 7wlnah into Json 34an yt save in Redis Database
          - ghzna al OrderDto b3d ma 3mlna al Basket 
          - ghzna al Debendancies bta3t al service aly htst5dmha 

          Act : 
          - bt3aml m3 al OrderService aly gbnah bel DI 34an nt3aml m3 al Service al 72i2ya CreateOrderAsync() 
          - al Service ht3ml order w a7na mghzen OrderDto gwah -> DeliveryMethodId = int.MaxValue 
            fa De dilavery Method m4 mwgoda fel Database 
         fa al service htst2bl Email + OrderDto -> tgib al Basket mn Redis orderDto.BasketId -> tt2kd an al Basket mwgod m4 fady 
         -> tgib al Proudacts mn al Sql server w tt2kd an kol Proudact mwgod -> tghz al OrderItems + 3nwan al 47n 
            w hna al Test hyo2f l2no hydwr 3la bynat al DelivaryMethod bta3t al Order mn al Dto al mb3ot .. w hydwr fel DB m4 hyla2i 
            fa al service t5o4 f code aly ktbnah f What TestCase is Doing Region == DelivaryMethod.NotFound 
         */
        #endregion

        #endregion

        #region Test 5 (Idea)

        #region What This Test Case Doing 
        /*
          hnghz Basket mwgoda f3ln fel Redis lakn hn7ot Gwaha ProudactId m4 mwgod fel Sql Server 
          fa al mfrod al Service de t3ml eh : 
          tgib Basket mn al Redis -> tla2i feha Proudact -> t7awl tgib al Proudact da mn al Sql server b2st5dam al Id 
          -> mtl2ho4 fa t return Proudact.NotFound -> fa mt7fz4 order gdid fel Db
         */
        #endregion

        #region Explain Test Case 
        /*
          Arrange : 
          - ft7t Connection m3 redis database w bn3ml ll Basket Id uninqe fel Redis 
          - int.MaxValue -> 5lit al Id bta3 al Basket kda 34an admn any m4 hla2ih fel DB
          - b3d ma nghz al basket bn7wlo mn object into Json 34an yt5zn fel Redis (Seralize)
          -  var orderService = serviceProvider.GetRequiredService<IOrderServices>(); -> da 34an nghz al Service al aslya bel Debendancies bta3tha 
          -  var unitOfWork = serviceProvider.GetRequiredService<IUnitOfWork>(); -> 34an hn3ml Checking 3la al Database bta3t al Project feha al proudact da wla la 
          - bnghz al OrderDto l2n al Service httlbo as Input 

          ACT : 
          - al service ht2ra al Basket mn al Redis -> w b3dha ht7awl Tgib al Proudacts mn sql server .. lma mtl2i4 al Proudact 
            al mfrod trg3 Proudact.NotFound 

          Assert : 
          - bnt2kd an al 3mlya f4lt w an mfe4 order rg3 "Proudact.NotFound"
          - Assert.Equal(ordersBefore.Count, ordersAfter.Count); -> leh ? 
          
         */
        #endregion

        #endregion

    }
}
