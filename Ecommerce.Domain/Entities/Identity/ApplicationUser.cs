using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Domain.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        // KDA ana wars 7agt al tbale bta3 al IdentityUser + hzwd 3leh Col 
        public string DisplayName { get; set; } = default!;

        #region Navigational Property 
        public Address? Address { get; set; }
        #endregion
    }
}
