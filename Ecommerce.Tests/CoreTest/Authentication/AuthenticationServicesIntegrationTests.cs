using Ecommerce.Application.Common;
using Ecommerce.Application.Contracts;
using Ecommerce.Application.DTO_s.Identity;
using Ecommerce.Infrastructure;
using Ecommerce.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Ecommerce.Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Ecommerce.Application.Services;

namespace Ecommerce.Tests.CoreTest.Authentication
{
    public class AuthenticationServicesIntegrationTests
    {
        #region Private Function for preparing Dependancies insted of Arrange 
        private ServiceProvider BuildServiceProvider()
        {
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:StoreDbConnection"] =
                        "server=.;Database=StoreDb;Trusted_Connection=true;TrustServerCertificate=true",

                    ["ConnectionStrings:IdentityConnection"] =
                        "server=.;Database=StoreIdentityDb;Trusted_Connection=true;TrustServerCertificate=true",

                    ["ConnectionStrings:RedisConnection"] = "localhost",

                    ["JWT:SecretKey"] ="c839149f7e6346f1378a7898c064e851fbecaa2e4f4d5bf2b19723267adf9dd4",
                    ["JWT:Issuer"] = "https://localhost:7014/",
                    ["JWT:Audiance"] = "https://localhost:7014/",
                    ["JWT:ExpirationMinutes"] = "60"
                })
                .Build();

            var services = new ServiceCollection();

            services.AddInfrastructureServices(configuration);
            services.AddApplicationServices();
            services.Configure<JwtSettings>(configuration.GetSection("JWT"));

            return services.BuildServiceProvider();
        }
        #endregion

        #region Test Case 1 

        [Fact]
        public async Task LoginAsync_UserNotFound_ReturnsNotFound()
        {
            // Arrange
            using var provider = BuildServiceProvider();

            var authServices = provider.GetRequiredService<IAuthenticationServices>();

            var loginDto = new LoginDto
            {
                Email = "notfound.integration@test.com",
                Password = "AnyPassword123!"
            };

            // Act
            var result = await authServices.LoginAsync(loginDto);

            // Assert
            Assert.False(result.IsSuccessed);

            Assert.Contains(result.Errors, error => error.type == ErrorTypes.NotFound);
        }

        #region Explain what This Test Case Is Doing 
        /*
          hn Test Service al LoginAsync() fel AuthenticationServices (Aly btst5dm GetByEmailAsync() fel IdentityService Class) 
          dor al Service GetByEmailAsync() de anha t find al Email mn al UserManger Table (Da table 5as bel Identity)
          lw Ml2a4 al Email hy return "Email Not Found" w de al test Case aly m7tagen n test 3leha 
          else -> hy return al User + al Data bta3to mn al Table 
          
          w fel Test Case de hnt3aml bel Integration Testing 34an n5tbr kol 7aga fel service 48ala M3 b3d wla la l2n bel Integration test ana b test :  
          AuthenticationServices , IdentityServices, UserManager<ApplicationUser> ,StoreIdentityDbContext .. w dol aly btst5dmhom al service m3 b3d 
          fa hnb3t LoginDto b Email m4 mwgod -> FindByEmailAsync() t search fel Identity Database -> Email not Found

          fel service al aslya (LoginAsync) bn test al goz2 da : 
            var UserResult = await identityServices.FindByEmailAsync(loginDto.Email, ct);
            if (!UserResult.IsSuccessed)
            {
                return Result<UserDto>.Fail(UserResult.Errors);
            }
         */
        #endregion

        #region Explain Test Case 
        /*
          Arrange : 
          - bnnady 3la al Debendanices bta3tna aly 7drnaha fel Private Function 
            tb kant eh al Debendancis de -> anta 3ndk fel Ctor bta3 al AuthenticationServices : 
            public AuthenticationServices( IIdentityServices identityServices,ITokenServices tokenServices)         
            {
                this.identityServices = identityServices;
                this.tokenServices = tokenServices;
            }
        note : 
        mynf34 an42 al Debendancies bel 4kl da -> var authServices = new AuthenticationServices();
        l2nha m7taga Debendancies wel Ctor m4 bysm7 b 2n4a2ha mn 8irhom .. momken n3ml AL debendancy Manally bs ana 3mlt way al A7sn 
        any a create BuildServiceProvider() dorha : 
        - tghz Connection strings , tsgl al services bta3t al Infrastructure and Application Layer 34an ast5dm al Services aly gwaha
          y3ne  services.AddApplicationServices(); momken aklm -> IIdentityServices , IAuthenticationServices aly hy7droli bel DI object mn IdentityServices AuthenticationServices .. 34an a2dr ast5dm al Service aly gwahom w atest 3lehom 

        - ghzna al LoginDto b Email + Password m4 mwgoden 3ndna fel Identity Database

         Act : 
         - bn Call al Service LoginAsync() gwaha 
           var UserResult = await identityServices.FindByEmailAsync(loginDto.Email, ct);
           y3ne htb3t al LoginDto aly 3mlnah fel test hna -> FindByEmailAsync() aly btt3aml m3 al IdentityDB (In UserManger Table).. 34an t find al m3lomat aly fel LoginDto mwgoda wla la
           w gwa al Identity Service t5o4 3la al code da : 
          if (user == null)
            {
                return Result<IdentityUserResult>.Fail(
                    Error.NotFound("User Is Not found"));
            }
        fa yrg3 User not found ..ll LoginAsync() fa tnfz gwaha al Code da : 
        if (!UserResult.IsSuccessed)
        return Result<UserDto>.Fail(UserResult.Errors);
        m4 hywsl l Bassword Check or Create Tokens 
         */
        #endregion

        #endregion

        #region Test Case 2 

        [Fact]
        public async Task LoginAsync_InvalidPassword_ReturnsUnAuthorized()
        {
            // Arrange
            using var provider = BuildServiceProvider();

            var authServices = provider.GetRequiredService<IAuthenticationServices>();

            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

            var email = "invalidpassword.integration@test.com";
            var correctPassword = "CorrectPassword123!";

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                DisplayName = "Integration Test User"
            };

            var createResult = await userManager.CreateAsync(user,correctPassword);

            Assert.True(createResult.Succeeded,string.Join(", ", createResult.Errors.Select(e => e.Description)));

            var loginDto = new LoginDto
            {
                Email = email,
                Password = "WrongPassword123!"
            };

            // Act
            var result = await authServices.LoginAsync(loginDto);

            // Assert
            Assert.False(result.IsSuccessed);

            Assert.Contains(result.Errors, error => error.type == ErrorTypes.UnAuthorized);
        }

        #region What Does this Test Function is Doing 
        /*
          check b invalid Password ll User .. fa 3ayzen n test al Flow da : 
          arrange tghz user mwgod fel Database -> Act tstd3y LoginAsync() b Email w Pass 8lt -> Assert tt2kd an al User is UnAuthorized
          fa a7na bn test al goz2 da fel LoginAsync() : 
        var PasswordCheck = await identityServices.CheckPasswordAsync( loginDto.Email, loginDto.Password,ct);
        if (!PasswordCheck.IsSuccessed)
        return Result<UserDto>.Fail(Error.UnAuthorized("InValid Email or Password"));
         */
        #endregion

        #region Explain Test Function 
        /*
          hna lazem ykon feh User f3ln mtsgl fel DB fa hnst5dm al UserManager<ApplicationUser> 34an n Create gwah al User .. 
          Arrange : 
          - a7na 3mlna Create l User bel (UserName ,Email ,DisplayName) w 5znah gwa al Table UserRole 
           var createResult = await userManager.CreateAsync(user,correctPassword); -> kda a7na dfnah fel Table 

          - b3d kda ghnza al LoginDto b bassword mo5tlfa 34an LoginAsync() bta5od al LoginDto as input paramter 

          Act : 
          - var result = await authServices.LoginAsync(loginDto); 
            FindByEmailAsync() htla2i al User -> CheckPasswordAsync() htt check al Pass aly fel LoginDto .. w tkarno bel mwgod fel UserManger Table .. fa htla2ih ano Wrong Pass 
            fa fel LoginAsync() data = false -> fa t3ml al Code bta3 Invalid Email or Password 
         */
        #endregion
        // note : al Test function de bt save User aly dfnah hna fel StoreIdentityDb da asasy y3ne 
        #endregion

        #region Test Case 3 

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsUserWithToken()
        {
            // Arrange
            using var provider = BuildServiceProvider();

            var authServices = provider.GetRequiredService<IAuthenticationServices>();

            var userManager = provider.GetRequiredService<UserManager<ApplicationUser>>();

            var email = $"validlogin_{Guid.NewGuid()}@test.com";
            var password = "CorrectPassword123!";

            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                DisplayName = "Integration Test User"
            };

            var createResult = await userManager.CreateAsync(user, password);

            Assert.True(createResult.Succeeded,string.Join(", ", createResult.Errors.Select(e => e.Description)));

            var loginDto = new LoginDto
            {
                Email = email,
                Password = password
            };

            // Act
            var result = await authServices.LoginAsync(loginDto);

            // Assert
            Assert.True(result.IsSuccessed);
            Assert.NotNull(result.data);
            Assert.Equal(email, result.data.Email);
            Assert.Equal("Integration Test User", result.data.DisplayName);
            Assert.False(string.IsNullOrWhiteSpace(result.data.Token));
        }

        #region What This Test Function is Doing 
        /*
          Fel test Case de hn5tbr an al User by Login Succesfully 
         */
        #endregion

        #region Explain Test Function 
        /*
          Arrange : 
          - a7na bnnady al Dependancies + bn create User 72i2e feL Identity Database 
            var createResult = await userManager.CreateAsync(user, password); 
          
           Act : 
           - var result = await authServices.LoginAsync(loginDto); 
           - FindByEmailAsync() tla2i al User -> CheckPasswordAsync() tt check an al Password slim -> GetRolesAsync() tgib al Role al 5asa bel user 
             -> TokenServices.CreateToken (t create al Token bta3o) -> LoginAsync() t return UserDto feh al Email , DisplayName , Token bta3 al User 
         */
        #endregion

        #region JWT Settings Configuration

        // Fel Integration Test, e7na mesh bn48l Program.cs bta3 el application.
        // 3ashan keda, lazm ne3mel setup lel JWT settings benafesna.

        // AddInMemoryCollection: bet7ot JWT settings fel test configuration.

        // services.Configure<JwtSettings>:
        // betrebot el settings bel class JwtSettings,
        // 3ashan TokenServices ye2dar ye2ra el values 3an taree2 IOptions<JwtSettings>.

        // Law mansagelnaash el settings, SecretKey momken teb2a null
        // w el JWT token creation ye7sal feha error.

        // 34an kda a7na dfna al Configuration of Jwt token hna fel test Case fel Private function aly 3mlnaha mn al awl 

        #endregion

        #endregion
    }
}
