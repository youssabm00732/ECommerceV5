using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Application.Services
{
    public class AuthenticationServices : IAuthenticationServices
    {
        #region DI
        private readonly IIdentityServices identityServices;
        private readonly ITokenServices tokenServices;

        public AuthenticationServices(IIdentityServices identityServices , ITokenServices tokenServices)
        {
            this.identityServices = identityServices;
            this.tokenServices = tokenServices;
        }

       
        #endregion
        public async Task<Result<UserDto>> LoginAsync(LoginDto loginDto, CancellationToken ct = default)
        {
            // ht findByEmail .. b3den Check Password .. lw kolo tmam = rg3 UserDto 
            var UserResult = await identityServices.FindByEmailAsync(loginDto.Email, ct);
            if (!UserResult.IsSuccessed)
            {
                return Result<UserDto>.Fail(UserResult.Errors);
            }
            // else .. Check on password 
            var PasswordCheck = await identityServices.CheckPasswordAsync(loginDto.Email, loginDto.Password, ct);
            //if (!PasswordCheck.IsSuccessed)
            //{
            //    return Result<UserDto>.Fail(Error.UnAuthorized("InValid Email or Password"));
            //}
            if (!PasswordCheck.IsSuccessed || !PasswordCheck.data)
            {
                return Result<UserDto>.Fail(Error.UnAuthorized("InValid Email or Password"));
            }
            // Token make 
            var RolesResult = await identityServices.GetRolesAsync(UserResult.data.Email);
            var token = tokenServices.CreateToken(UserResult.data.Id, UserResult.data.Email, UserResult.data.UserName, RolesResult.data);
            // lw kolo tmam -> return UserDto 
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = UserResult.data.Email,
                DisplayName = UserResult.data.DisplayName ,
                //Token = "Token" // lsa feha 4o8l 
                Token = token
            });
        }

        public async Task<Result<UserDto>> RegisterAsync(RegisterDto RegisterDto, CancellationToken ct = default)
        {
            var result = await identityServices.CreateUser(RegisterDto, ct);
            if (!result.IsSuccessed || result.data is null) 
                return Result<UserDto>.Fail(result.Errors);
            // else 

            // Token make 
            var RolesResult = await identityServices.GetRolesAsync(result.data.Email);
            var token = tokenServices.CreateToken(result.data.Id, result.data.Email, result.data.UserName, RolesResult.data);
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = result.data.Email,
                DisplayName = result.data.DisplayName,
                //Token = "Token" // lsa feha 4o8l 
                Token = token
            });
        }

        #region Email Services 

        public async Task<Result<bool>> CheckEmailAsync(string email, CancellationToken ct = default)
        {
            return await identityServices.EmailExistAsync(email, ct);
        }
        public async Task<Result<AddressDto>> UpdateUserAddressAsync(AddressDto addressDto, string email, CancellationToken ct = default)
        {
            return await identityServices.UpdateAddressAsync(email, addressDto, ct);
        }

        public async Task<Result<AddressDto>> GetUserAddressAsync(string email, CancellationToken ct = default)
        {
            var result = await identityServices.GetAddressByEmailAsync(email, ct);
            if (!result.IsSuccessed)
                return Result<AddressDto>.Fail(result.Errors);
            // else 
            return Result<AddressDto>.Ok(result.data);
        }
        public async Task<Result<UserDto>> GetCurrentUser(string email, CancellationToken ct = default)
        {
            // htrg3 dto fa 3yza mnk token wnta btrg3 fel response 

            // get al user by email bta3o 
            var UserResult = await identityServices.FindByEmailAsync(email, ct);
            if(!UserResult.IsSuccessed)
                return Result<UserDto>.Fail(UserResult.Errors);
            // else 
            var user = UserResult.data;
            // hgib al Roles bta3t al user 
            var UserRoles = await identityServices.GetRolesAsync(user.Email);
            if (!UserRoles.IsSuccessed)
                return Result<UserDto>.Fail(UserRoles.Errors);

            // if user and this roles tmam = ghz al token 
            var token = tokenServices.CreateToken(user.Id, user.Email, user.UserName, UserRoles.data);
            // return user dto 
            return Result<UserDto>.Ok(new UserDto()
            {
                Email = user.Email,
                DisplayName = user.DisplayName,
                
                Token = token
            });
        }

        #endregion
    }
}
