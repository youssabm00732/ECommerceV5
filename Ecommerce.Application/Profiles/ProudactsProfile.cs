using AutoMapper;
using Ecommerce.Application.DTO_s.Proudacts;
using Ecommerce.Domain.Entities.Proudacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Profiles
{
    public class ProudactsProfile : Profile
    {
        public ProudactsProfile()
        {
            CreateMap<ProudactBrand, BrandDto>();
            CreateMap<ProudactTypes, TypeDto>();

            CreateMap<Proudact, ProudactDto>()
                .ForMember(dist => dist.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dist => dist.TypeName, opt => opt.MapFrom(src => src.Type.Name))
                .ForMember(dist => dist.PictureUrl, opt => opt.MapFrom<PictrueUrlResolver>()); // mapp for photo url 

            /*
              note in Proudact model : 
               public int BrandId { get; set; }
               public int TypeId { get; set; }

              and in ProudactDto : 
               public string BrandName { get; set; } = null!;
               public string TypeName { get; set; } = null!;

             fa al Mapper m4 ht3rf ykarn de b de l2nhom two types mo5tlfen 3n b3d 
             fa h3mlha manual mapping b2dya ana 

                 CreateMap<Proudact, ProudactDto>()
                .ForMember(dist => dist.BrandName, opt => opt.MapFrom(src => src.Brand.Name))
                .ForMember(dist => dist.TypeName, opt => opt.MapFrom(src => src.Type.Name));
             */
        }
    }
}
