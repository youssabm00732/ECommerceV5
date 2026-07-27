using Ecommerce.Application.Common;
using Ecommerce.Application.DTO_s.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Contracts
{
    public interface IIdentityServices
    {
        Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default); // function de ht return Id , Email , UserName , DisplayName
        Task<Result<bool>> CheckPasswordAsync(string email, string Password ,CancellationToken ct = default);
        Task<Result<IdentityUserResult>> CreateUser( RegisterDto registerDto,CancellationToken ct = default);
        // function to Collect all Roles for Token paramter
        Task<Result<IEnumerable<string>>> GetRolesAsync(string email);

        // Email Services 
        Task<Result<AddressDto>> GetAddressByEmailAsync(string email, CancellationToken ct = default);
        Task<Result<AddressDto>> UpdateAddressAsync(string email, AddressDto addressDto ,CancellationToken ct = default);

        Task<Result<bool>> EmailExistAsync(string email, CancellationToken ct = default);
    }
}
