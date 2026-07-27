using Azure.Core;
using Ecommerce.Application.Contracts;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Identity;
using Ecommerce.Infrastructure.Data;
using Ecommerce.Infrastructure.Payments;
using Ecommerce.Infrastructure.Repositories;
using Ecommerce.Infrastructure.Seeding;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure
{
    public static class InfrastrucrureServicesRegistrations
    {
        // h3ml Function tkon as extention method ll Service Container aly mwgod fel DI Container 
        // y3ne 3ayz fel Program -> Builder.Services.al Function bta3te() 
        // al Function de htrg3 nfs al type aly 48aln beh fel DI Container -> IServiceCollection (m4 fahmha awe)
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services , IConfiguration configuration)
        {
            services.AddDbContext<StoreDbContext>(Options =>
            {
                //Options.UseSqlServer(builder.Configuration.GetConnectionString("StoreDbConnection"));
                Options.UseSqlServer(configuration.GetConnectionString("StoreDbConnection"));
            });

            services.AddDbContext<StoreIdentityDbContext>(Options =>
            {
                
                Options.UseSqlServer(configuration.GetConnectionString("IdentityConnection"));
            });

            // kml ba2i al Services bta3t al Infrastructure Layer 
            // services.AddScoped<IDataSeeder, CatalogDataSeeder>(); // bghz object of Data Seeding 
            services.AddKeyedScoped<IDataSeeder, CatalogDataSeeder>("Catalog");
            // for Identity Database 
            services.AddKeyedScoped<IDataSeeder, IdentityDataSeeder>("Identity");
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            #region Redis Database 
            services.AddSingleton<IConnectionMultiplexer>(opt => 
            {
                return ConnectionMultiplexer.Connect(configuration.GetConnectionString("RedisConnection"));
            });

            services.AddScoped<IBasketRepository, BasketRepository>();
            #endregion
            services.AddSingleton<ICachRepository, CachRepository>();

           

            services.AddIdentityCore<ApplicationUser>().AddRoles<IdentityRole>().AddEntityFrameworkStores<StoreIdentityDbContext>();

            // jwt Config on Authentication Service (from Cookie -> JWT Authnetication)
            services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; // lma t5l2 ay object 5as bel Secutity yb2a JWT 
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme; // a8ir al defult Authentication from Cookie -> Jwt
            }).AddJwtBearer(opt => 
            {
                opt.SaveToken = true;
                opt.RequireHttpsMetadata = true;
                opt.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    RequireExpirationTime = true,
                    ValidIssuer= configuration.GetSection("JWT")["Issuer"],
                    ValidAudience = configuration.GetSection("JWT")["Audiance"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("JWT")["SecretKey"]))

                };

                // AddJwtBearer() -> t3rfk no3 al Authnetication bta3k -> JWT 
                // TokenValidationParameters() ->  bt3ml Validation 3la al Request aly feh al token of user 
            });

            services.AddScoped<IPaymentGetWay, StripePaymentGetway>();

            return services; // lma y return nfs la type aly DI Container btst5dmo s3tha a2dr a3ml kda .. Builder.Services.AddInfrastructureServices() in main program
        }

        #region Summary 
        // fa kda ana 3mlt bra fel main program in DI Container : 
        //  builder.Services.AddInfrastructureServices(builder.Configuration);
        // fa y5o4 gwa hna del Class y3ml 7agten .. y7dr object mn al Context .. y7dr object mn al Data Seeding 
        // for Dependancy Injection 
        #endregion
    }
}
