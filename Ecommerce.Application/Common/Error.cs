using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common
{
    // Error Record da hyzhr m3aya in Case bs an feh Error 
    // bs al Record da hyt3rd gwa al Result .. wel result hykon gwaha y2ma Error wa7d or array of Errors 
    public record Error(string Code , string Description , ErrorTypes type = ErrorTypes.Failure)
    {
        // h3ml gwa al Record Functions l kol al Errors aly 3nde fl Enum bta3 al ErrorTypes 
        // w kol al functions de byrg3oly nfs al type aly hwa Error
        // fa aly ana h3mlo hwa Errors Functions hbd2 any agm3hom b3den gwa al Result (da lw feh Error rag3)
        // fa ana h3ml Class asmo Result Class gwah hst5dm al Funnctions aly h3mlha de f 7alt lw feh errors y3ne rag3a 

        // Function for Failer 
        public static Error Failure(string Code = "General.Failure", string Description = "General Failure Description")
            => new Error(Code, Description, ErrorTypes.Failure);

        // Function for Validation 
        public static Error Validation(string Code = "General.Validation", string Description = "General Validation Description")
            => new Error(Code, Description, ErrorTypes.Validation);

        // Function for NotFound 
        public static Error NotFound(string Code = "General.NotFound", string Description = "General NotFound Description")
            => new Error(Code, Description, ErrorTypes.NotFound);

        // Function for Conflict 
        public static Error Conflict(string Code = "General.Conflict", string Description = "General Conflict Description")
            => new Error(Code, Description, ErrorTypes.Conflict);

        // Function for UnAuthorized 
        public static Error UnAuthorized(string Code = "General.UnAuthorized", string Description = "General UnAuthorized Description")
            => new Error(Code, Description, ErrorTypes.UnAuthorized);

        // Function for Forbeddin 
        public static Error Forbeddin(string Code = "General.Forbeddin", string Description = "General Forbeddin Description")
            => new Error(Code, Description, ErrorTypes.Forbeddin);

        // Function for InvalidCredentials 
        public static Error InvalidCredentials(string Code = "General.InvalidCredentials", string Description = "General InvalidCredentials Description")
            => new Error(Code, Description, ErrorTypes.InvalidCredentials);
    }
}
