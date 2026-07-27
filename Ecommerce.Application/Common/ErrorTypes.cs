using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ErrorTypes
    {
        Failure =0, // Error from Server 
        Validation = 1 , // Error from Client 
        NotFound = 2 ,
        Conflict = 3 , // lw emlt insert l two proudacts msln with the same id's 
        UnAuthorized = 4 ,
        Forbeddin =5,
        InvalidCredentials = 6 // lw 3mlt Login b4kl 8lt 

        /*
          al Enum byrg3 as al Value nfsha ll Front-end m4 ka string .. fa al rag3 hwa (0 , 1, 2) 
          w da m4 al 4kl aly 3ayz al User y4ofo ana 3ayzo y4of al String nfso 34an hyfm 
          fa b3ml attribute bel Data Annotation b2olo .. 
           kol mra htrg3 al ErrorType da (Enum) 3la hy2t Json .. abd2 wdle al klma nfsha (string) not value
      =>   [JsonConverter(typeof(JsonStringEnumConverter))]
             
         */
    }
}
