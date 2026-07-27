using Ecommerce.Application.Contracts;
using Ecommerce.Application.Profiles;
using Ecommerce.Application.Services;
using Ecommerce.Domain.Contracts;
using Ecommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application
{
    public static class ApplicationServicesRegister
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            // make object from mapper 
            services.AddAutoMapper(c => c.AddProfile(new ProudactsProfile()) , typeof(ApplicationServicesRegister).Assembly);
            // make object from IProudactServices 
            services.AddScoped<IProudactServices, ProudactServices>();
            services.AddScoped<IBasketServices, BasketServices>();
            services.AddSingleton<ICachServices, CachServices>();
            services.AddScoped<IIdentityServices, IdentityServices>();
            services.AddScoped<IAuthenticationServices, AuthenticationServices>();
            services.AddScoped<ITokenServices, TokenServices>();
            services.AddScoped<IOrderServices, OrderServices>();
            services.AddScoped<IPaymentService, PaymentServices>();
            
            return services;
        }
    }
}
