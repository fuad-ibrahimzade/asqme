using AsqMe.Data.Models;
using AsqMe.Data.Models.Interfaces;
using AsqMe.Helpers;
using AsqMe.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using AsqMe.Data;

namespace AsqMe.Services
{
    public class UserService : IUserService, IDisposable
    {
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications
        private List<ApplicationUser> _users = new List<ApplicationUser>
        {
            new ApplicationUser { UserName = "admin@admin.com"}
            //, Password = "admin"
        };

        private readonly AppSettings _appSettings;
        public IUnitOfWorkEF unitOfWork { get; set; }

        public UserService(IOptions<AppSettings> appSettings, IUnitOfWorkEF _unitOfWork)
        {
            _appSettings = appSettings.Value;
            unitOfWork = _unitOfWork;
        }

        public async Task<ApplicationUser> AuthenticateAsync(string username, string password)
        {
            //var user = _users.SingleOrDefault(x => x.UserName == username);
            ////&& x.Password == password

            //// return null if user not found
            //if (user == null)
            //    return null;
            var user = await unitOfWork.UserManager.FindByEmailAsync(username);
            if (user == null || !await unitOfWork.UserManager.CheckPasswordAsync(user, password))
            {
                return null;
                //var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);
                //identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));
                //identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                //await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                //    new ClaimsPrincipal(identity));
                //await unitOfWork.SignInManager.SignInAsync(user,false);
                //return RedirectToAction(nameof(AdminController.Index), "Home");
            }

            //    var appSettingsSection = Configuration.GetSection("AppSettings");
            //    services.Configure<AppSettings>(appSettingsSection);

            //    // configure jwt authentication
            //    var appSettings = appSettingsSection.Get<AppSettings>();
            //    var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            //    return key;

            // authentication successful so generate jwt token


            return user.WithoutPassword();
        }

        public static bool JWTready = false;
        public async Task<ClaimsPrincipal> LoginAsync(string email, string password)
        {
            var user = await unitOfWork.UserManager.FindByEmailAsync(email);
            if (user == null || !await unitOfWork.UserManager.CheckPasswordAsync(user, password))
            {
                return null;
            }
            var result = await unitOfWork.SignInManager.PasswordSignInAsync(email,
                   password, isPersistent: false, lockoutOnFailure: true);
            //var result = new { Succeeded =true};
            if (result.Succeeded)
            {

                var userPrincipial = await unitOfWork.SignInManager.CreateUserPrincipalAsync(user);
                //JWTready = true;

                //var claimsIdentityCookie = new ClaimsIdentity(userPrincipial.Claims,
                // Microsoft.AspNetCore.Authentication.Cookies.CookieAuthenticationDefaults.AuthenticationScheme);
                //var claimsIdentityJwt = new ClaimsIdentity(userPrincipial.Claims,
                //    Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
                //userPrincipial.AddIdentity(claimsIdentityCookie);
                //userPrincipial.AddIdentity(claimsIdentityJwt);

                //var claimsIdentityCookie = new ClaimsIdentity(userPrincipial.Claims,
                //    AuthorizationPolicies.Cookie);
                //var claimsIdentityJwt = new ClaimsIdentity(userPrincipial.Claims,
                //    AuthorizationPolicies.JWT);
                //ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal();
                //claimsPrincipal.AddIdentity(claimsIdentityCookie);
                //claimsPrincipal.AddIdentity(claimsIdentityJwt);
                //userPrincipial.AddIdentity(new ClaimsIdentity(AuthorizationSchemes.Cookie));
                //userPrincipial.AddIdentity(new ClaimsIdentity(AuthorizationSchemes.JWT));
                await unitOfWork.httpContextAccessor.HttpContext.SignInAsync(userPrincipial);
                unitOfWork.httpContextAccessor.HttpContext.User = userPrincipial;

                //var userIdentity = (ClaimsIdentity)userPrincipial.Identity;
                //userIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email.ToString()));

                //return user.WithoutPassword();
                return userPrincipial;
            }
            return null;
        }
        public async Task<bool> LogoutAsync(string email = null)
        {
            //JWTready = false;
            ApplicationUser user = null;
            if (email == null)
            {
                user = await unitOfWork.UserManager.GetUserAsync(unitOfWork.httpContextAccessor.HttpContext.User);

            }
            else
            {
                user = await unitOfWork.UserManager.FindByEmailAsync(email);
            }

            if (user == null)
            {
                return false;
            }
            await unitOfWork.SignInManager.SignOutAsync();
            await unitOfWork.httpContextAccessor.HttpContext.SignOutAsync();
            return true;
        }


        public IEnumerable<ApplicationUser> GetAll()
        {
            return _users.WithoutPasswords();
        }

        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    unitOfWork.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
