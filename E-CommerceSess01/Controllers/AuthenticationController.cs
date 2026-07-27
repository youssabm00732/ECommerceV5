using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceSess01.Controllers
{
    public class AuthenticationController : ApiBaseController
    {
        #region DI 
        private readonly IAuthenticationServices authenticationServices;
        public AuthenticationController(IAuthenticationServices authenticationServices)
        {
            this.authenticationServices = authenticationServices;
        }
        #endregion

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.LoginAsync(loginDto, ct));
        }

        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.RegisterAsync(registerDto, ct));
        }

        [HttpGet("emailexists")] // de al Front-end aly ba3tha fa m4 m7taga mne ay tokens [Authorize]
        public async Task<ActionResult<bool>> CheckEmail([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.CheckEmailAsync(email, ct));
        }

        [HttpGet("CurrentUser")]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.GetCurrentUser(email, ct));
        }

        [HttpGet("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> GetUserAddress([FromQuery] string email, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.GetUserAddressAsync(email, ct));
        }
        [HttpPost("Address")]
        [Authorize]
        public async Task<ActionResult<AddressDto>> UpdateUserAddress([FromQuery] string email, [FromBody] AddressDto addressDto, CancellationToken ct)
        {
            return ToActionResult(await authenticationServices.UpdateUserAddressAsync(addressDto, email, ct));
        }
    }
}
