using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Identity;
using Ecommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Ecommerce.Application.Services
{
    public class IdentityServices : IIdentityServices
    {
        #region DI
        private readonly UserManager<ApplicationUser> userManager;
        public IdentityServices(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        #endregion
        public async Task<Result<IdentityUserResult>> FindByEmailAsync(string email, CancellationToken ct = default)
        {
            // hytb3tle email feha w hbd2 a find al email da fel UserManger Table
            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return Result<IdentityUserResult>.Fail(Error.NotFound("User Is Not found"));
            }
            else
            {
                // lw al Email s7 = hyrg3 bynat al User b2a aly fel IdentityUserResult
                return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
            }
        }
        public async Task<Result<bool>> CheckPasswordAsync(string email, string Password, CancellationToken ct = default)
        {
            // hbasi Email lw hwa valid -> hy Check 3la Password al Email da Valid wla eh 
            var user = await userManager.FindByEmailAsync(email);
            if (user is null)
                return Result<bool>.Fail(Error.NotFound("User Is Not Found"));
            // else 
            var isValid = await userManager.CheckPasswordAsync(user,Password);

            return Result<bool>.Ok(isValid);
            
        }

        public async Task<Result<IdentityUserResult>> CreateUser(RegisterDto registerDto, CancellationToken ct = default)
        {
            // hykon gayle mn al Front info for Registration -> RegisterDto .. hbd2 a5odha w a3mlha object 3nde 
            var user = new ApplicationUser()
            {
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber,
                DisplayName = registerDto.DisplayName,
            };
            // a3ml ll object da save fel UserManger table
            var result = await userManager.CreateAsync(user, registerDto.Password);

            // lw al Create m4 success => hrg3 mgmo3a mn al Errors 
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => new Error(e.Code, e.Description)).ToList();
                return Result<IdentityUserResult>.Fail(errors);
            }
            return Result<IdentityUserResult>.Ok(new IdentityUserResult(user.Id, user.Email, user.UserName, user.DisplayName));
        }

        public async Task<Result<IEnumerable<string>>> GetRolesAsync(string email)
        {
            var User = await userManager.FindByEmailAsync(email);
            if (User is null)
            {
                return Result<IEnumerable<string>>.Fail(Error.NotFound("User Is Not Found"));
            }
            var Roles = await userManager.GetRolesAsync(User);
            return Result<IEnumerable<string>>.Ok(Roles);
        }

        #region Email Services 
        public async Task<Result<AddressDto>> GetAddressByEmailAsync(string email, CancellationToken ct = default)
        {
            // Get User by Email 
            var User = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email,ct);

            if (User is null) return Result<AddressDto>.Fail(Error.NotFound("User Is Not Found"));
            if (User.Address is null) return Result<AddressDto>.Fail(Error.NotFound("Address Is Not Found"));
            // else = ghz al Adress Dto b m3lomat al User aly gbto da 34an h3mlo Return fel Response 
            return Result<AddressDto>.Ok(new AddressDto()
            {
                FirstName = User.Address.FirstName , 
                LastName = User.Address.LastName , 
                Street = User.Address.Street ,
                City = User.Address.City , 
                Country = User.Address.Countery
            });
        }

        public async Task<Result<AddressDto>> UpdateAddressAsync(string email, AddressDto addressDto, CancellationToken ct = default)
        {
            // hna b3ml Create or Update .. fa de 3mlyt Post mn al A5r 

            // Get User by Email 
            var User = await userManager.Users.Include(u => u.Address).FirstOrDefaultAsync(u => u.Email == email, ct);
            if (User is null) return Result<AddressDto>.Fail(Error.NotFound("User Is Not Found"));
            // lw al address bta3o = null -> a3mlo Create 
            if (User.Address is null)
            {
                User.Address = new Address()
                {
                    FirstName = addressDto.FirstName,
                    LastName = addressDto.LastName,
                    Street = addressDto.Street,
                    City = addressDto.City,
                    Countery = addressDto.Country
                };
            }
            else 
            {
                // hbd2 a3ml ll address da update 
                User.Address.FirstName = addressDto.FirstName;
                User.Address.LastName = addressDto.LastName;
                User.Address.Street = addressDto.Street;
                User.Address.City = addressDto.City;
                User.Address.Countery = addressDto.Country;
            }
            // a3ml b2a al Update bta3k f table al UserManger 
            var result = await userManager.UpdateAsync(User);
            if(!result.Succeeded)
                return Result<AddressDto>.Fail(Error.Failure("Can Not Update User Address"));
            // else 
            return Result<AddressDto>.Ok(addressDto);
        }

        public async Task<Result<bool>> EmailExistAsync(string email, CancellationToken ct = default)
        {
            return await userManager.FindByEmailAsync(email) is not null; // check al email mwgod wla la w yrg3o lw mwgod 
        } 
        #endregion
    }
}
