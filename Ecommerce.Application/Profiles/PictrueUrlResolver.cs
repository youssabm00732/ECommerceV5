using AutoMapper;
using Ecommerce.Application.DTO_s.Proudacts;
using Ecommerce.Domain.Entities.Proudacts;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Profiles
{
    public class PictrueUrlResolver : IValueResolver<Proudact, ProudactDto, string?>
    {
        #region Dependancy Injection 
        private readonly IOptions<UrlSettings> options;

        public PictrueUrlResolver(IOptions<UrlSettings> options)
        {
            this.options = options;
        } 
        #endregion
        public string? Resolve(Proudact source, ProudactDto destination, string? destMember, ResolutionContext context)
        {
            #region Last Step 
            // check if URL is Empty 
            if (string.IsNullOrEmpty(source.PictureUrl)) return null;
            // else .. l2it URL = abd2 gm3 al Kym 
            var BaseUrl = options.Value.BaseUrl.TrimEnd('/'); // value -> UrlSettings .. kda l7d dlw2t ana m3aya al {{BaseUrl}} .. na2s agm3 ba2i al Path bta3e l2n al Swr 34an awslha b3ml request kda fel postman {{BaseUrl}}/files/proudacts/photoname

            // agm3 ba2i path al sora b3d ma gbt al {{BaseUrl}}
            var Path = source.PictureUrl.TrimStart('/');

            return $"{BaseUrl}/Files/{Path}"; // da al path al kaml ll sora {{BaseUrl}}/files/proudacts/photoname 
            #endregion

        }
    }
    #region Explain Class 
    /*
     IValueResolver -> da interface by2olk anta 3ayz t7wl mnen lfen (sourse -> destination)
     source -> Proudact , Destination -> ProudactDto
     
      el interface has function -> Resolve() 
      dorha t2ra al Code aly bygm3lk al URL w t3mlo Return .. 34an kda bt return string 
      
      fa fel a5r lma a5ls hna 4o8l fel Class .. bro7 3la al ProudactProfile al5as bel Mapping bta3e w3ml kda 
      .ForMember(dist => dist.PictureUrl, opt => opt.MapFrom<PictrueUrlResolver>());
      
       da kda hy5l2 object mn PictrueUrlResolverb w ynady 3la function al Resolve() 
       aly mgm3a al Path bta3 al Picture w bt3mlo return 
    */
    #endregion

    #region Resolve Implemntation
    /*
       var Url = "https://localhost:7014"; al mo4kla an da link local server bta3e ana bgrb 3leh 
       lakn f mr7lt al Proudaction al URl da al Domain Name bta3o hyt8ir akid .. fa hdtr a5o4 a8ir fel URl da fe mr7lt al Proudation w da m4 mntke
      
       fa ana hro7 a7ot al URl da fel AppSetting.json kda : 
       "UrlSettings": {
    "BaseUrl": "https://localhost:7014"
  }

    ana 3ayz a3ml 7aga tbd2 tgm3le al Settings al 5asa bel URl fmkan wa7d fa h3ml hna fel Class Class tany asmo 
    UrlSettings 
    w3mlt gwah Proberty asmha BaseUrl brdo 

    tb leh ana 3mlt kda ? 
    34an ana 3ayz lma a5l2 object mn al Class da => y2dr ygib b4kl Automatic al Data mn al Scopd aly 3mlto fel AppSettings da 
      "UrlSettings": {
    "BaseUrl": "https://localhost:7014"

     tb da yt3ml azay -> by two steps : 
     1 - hro7 fel main Program : 
      =>  builder.Services.Configure<UrlSettings>(builder.Configuration.GetSection("UrlSettings"));
    y3ne ana h3ml Configuration l 7aga mn no3 UrlSettings .. wel Configurations aly ht7sl de hgbha mn al UrlSettings
    aly hwa al Scope aly fel AppSettings.json 
    wbkda ana b3ml map mn UrlSettings aly fel Appsetting (aly feh al local url ) 3la hy2t al Class  UrlSettings

    y3ne da m3nah ank lma t5l2 object mn al UrlSettings hat al kym bta3to mn UrlSettings wtt5zn gwa al object da 

    2 - ana 3ayz al object bta3 al UrlSettings da yb2a mwgod b2a fel PictrueUrlResolver class aly feh function Resolve()
     => bel Dependancy injection nady 3la object mn al Class da 
    kda ana b2a m3aya object gwah al Kym aly mwgoda fel Appsetting .. bst5dm al Object da fel PictrueUrlResolver class

    fa btl3 a3ml al Implemntation bta3e -> fel PictrueUrlResolver .. zy mna 3mlo f region al Last Steps 

    3 - a5r 7aga fel proudact profile -> ForMember(dist => dist.PictureUrl, opt => opt.MapFrom<PictrueUrlResolver>());
         hto7 aghz object mno fel services bta3t al DI Container aly 3mltha f class ApplicationServiceRegister 
         34an ana ast5dmt mno object bel Debendancy Injection fel PictrueUrlResolver w m4 hy4t8l 8ir lw ro7t 3mltlo service tghz al object bta3o 

           services.AddAutoMapper(c => c.AddProfile(new ProudactsProfile()) , typeof(ApplicationServicesRegister).Assembly);
    zwdt de bs f goz2 al service bta3 al Mapper : 
    typeof(ApplicationServicesRegister).Assembly 
    m3naha any lw a7tagt object mn al Resolver = hydelo al Object by defult 34an hwa fel Current Asymbly 
  }
     */
    #endregion

    public class UrlSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
    }
}
