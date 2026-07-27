using Ecommerce.Application.Common;
using Ecommerce.Application.DTO_s.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface IAuthenticationServices
    {
        Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default);
        Task<Result<UserDto>> RegisterAsync(RegisterDto RegisterDto, CancellationToken ct = default);

        // Email Services 
        Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto,string email, CancellationToken ct = default);
        Task<Result<UserDto>> GetCurrentUser(string email, CancellationToken ct = default);

    }
}
