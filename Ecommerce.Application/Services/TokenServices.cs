using Ecommerce.Application.Contracts;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class TokenServices(IOptions<JwtSettings> options) : ITokenServices
    {
        private readonly JwtSettings settings = options.Value;
        public string CreateToken(string UserId, string Email, string UserName, IEnumerable<string> Roles)
        {
            //1 - Private Claims (anta aly bt Castomize al donya) -> data for each User 
            var Claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier,UserId),
                new(ClaimTypes.Email,Email),
                new(ClaimTypes.Name,UserName),
                // new("Hamada" , "Hambozo") 3ady 
            };
            // hdif Claim l kol role fel List
            Claims.AddRange(Roles.Select(r => new Claim(ClaimTypes.Role, r)));

            // b3d ma gbt al Configrations of AppSetting hna k options 
            // 2 - hat al Secrete key w a3mlo EnCoding 
            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(settings.SecretKey)); // signature 
            // 3 - header of JWT 
            var Credentials = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);//Header 

            // 4 - kwn al Token 
            var Token = new JwtSecurityToken(
                issuer: settings.Issuer,
                audience:settings.Audiance,
                claims: Claims,
                expires:DateTime.Now.AddMinutes(settings.ExpirationMinutes),
                signingCredentials: Credentials
                );
            // htdrb al klam da fel 5lat = string (al token bta3k)
            return new JwtSecurityTokenHandler().WriteToken(Token); // al 5latt  
        }
      
    }
    public class JwtSettings
    {
        public string SecretKey { get; set; } = default!;
        public string Issuer { get; set; } = default!;
        public string Audiance { get; set; } = default!;
        public int ExpirationMinutes { get; set; } = 60;
    }
    #region Explain CreateToken() 
    /*
      hwa al Code bta3ha f ay mkan hla2ih , bs ana hfhm al Logic bta3ha eh .. 
      1 - a3ml al Private Claims al awl bta3t al token 
      2 - tb al Register Claims , SecretKey agbhom mnen .. al 7gat de m4 5asa bel User bl bel Solution nfso 
          w momken t5tlf mn Enviroment ll tany fa de btb2a Configuration bttgab mn bra (AppSetting File)

       fa hagy 3nd al AppSettingFile azwd kam Configuration : 
       "JWT": {
     "SecretKey": "c839149f7e6346f1378a7898c064e851fbecaa2e4f4d5bf2b19723267adf9dd4",
     "Issuer": "https://localhost:7014/",
     "Audiance": "https://localhost:7014/",
     "ExpirationMinutes": 60
    }
    da Configrations ana b3mlha fel AppSetting File Customize l2n fel A5r de data ana bdrbha fel 5lat ttl3le al token 
    SecretKey -> gbto mn Site Jwt secure 
    Issuer -> al Server bta3y 
    Audiance -> al mkan aly hyb3t request ll Server bta3e 
    ExpirationMinutes -> al token lma yt create a5ro sa3a 

    b3d kda ana 3ayz anady 3la al Configurations de fel Service bta3te hna .. 
    3ayz awsl mn al AppSetting -> ll service hna fa h3ml Class al Cols bta3to tkon nfs al Cols aly fel AppSetting 
    
    b3d kda hgib al Options aly t48li al Configurations 3nde fel CreateToken() 
    - hro7 ll Program.cs 
    -  builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JWT")); .. zy ma 3mlt fel PictureResolver 
    - gwa Ctor al CreateToken() h3ml object mn no3 IOptions<JWtSettings> y4an ybd2 ygbli al Configrations mn al AppSettings 

    3 - hat al Secrete key w a3mlo EnCoding 
    4 - hat al Header bta3 al JWt 

    5 - b3d ma kwent al header + payload + secretkey => adrb fel 5lat w kwn al Token 

    arg3 ll main program ragon l 4r7 b2it al steps ... 
     */
    #endregion
}
