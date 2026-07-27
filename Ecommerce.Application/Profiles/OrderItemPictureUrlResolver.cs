using AutoMapper;
using Ecommerce.Application.DTO_s.Orders;
using Ecommerce.Domain.Entities.Orders;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Profiles
{
    public class OrderItemPictureUrlResolver(IOptions<UrlSettings> options) : IValueResolver<OrderItem, OrderItemDto, string>
    {
        private readonly UrlSettings settings = options.Value;// da 3mlenlo Resolve Configuration fel program bra 
        public string Resolve(OrderItem source, OrderItemDto destination, string destMember, ResolutionContext context)
        {
            if (string.IsNullOrEmpty(source.Proudact.PictureUrl))
                return string.Empty;

            return $"{settings.BaseUrl}/Files/{source.Proudact.PictureUrl}";
        }
    }
}
