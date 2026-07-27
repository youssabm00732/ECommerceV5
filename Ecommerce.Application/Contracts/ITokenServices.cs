using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface ITokenServices
    {
        string CreateToken(string UserId , string Email , string UserName , IEnumerable<string> Roles);
    }
}
