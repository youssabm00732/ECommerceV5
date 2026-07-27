using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Common
{
    public class IdentityUserResult
    {
        public IdentityUserResult(string id, string email, string userName, string displayName)
        {
            Id = id;
            Email = email;
            UserName = userName;
            DisplayName = displayName;
        }
        // 34an lma a5l2 mno object a basy feh al kym 3ltol

        public string Id { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string UserName { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
    }
}
