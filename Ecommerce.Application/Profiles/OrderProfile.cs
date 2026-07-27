using AutoMapper;
using Ecommerce.Application.DTO_s.Identity;
using Ecommerce.Application.DTO_s.Orders;
using Ecommerce.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Profiles
{
    public class OrderProfile:Profile
    {
        public OrderProfile()
        {
            CreateMap<DelivaryMethod, DeliveryMethodDto>();

            CreateMap<OrderAddress, AddressDto>().ReverseMap();

            CreateMap<Order, OrderToReturnDto>()
                .ForMember(dest => dest.DeliveryMethod, opt => opt.MapFrom(src => src.delivaryMethod.ShortName))
                .ForMember(dest => dest.DeliveryCost, opt => opt.MapFrom(src => src.delivaryMethod.Cost))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.GetTotal()));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProudactId, opt => opt.MapFrom(src => src.Proudact.ProudactId))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Proudact.ProudactName))
                //.ForMember(dest => dest.PictrueUrl, opt => opt.MapFrom(src => src.Proudact.PictureUrl))
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom<OrderItemPictureUrlResolver>());
                // .ForMember(dest => dest.PictrueUrl, opt => opt.MapFrom(src => src.Proudact.PictureUrl))
                // al path bta3 al Pic m4 kaml fa m7tagen n3ml Resolver Class y resolve al image 
                // steps : 
                // 1 - make Class in Profile -> OrderItemPictureUrlResolver 

        }
    }
}
