using AsqMe.Data.Models.Interfaces;
using System.Linq;
using System.Security.Claims;

namespace AsqMe.Data.ViewModels
{
    public class AuthViewModel
    {
        public AuthViewModel(IUnitOfWorkEF unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        //@inject Microsoft.AspNetCore.Antiforgery.IAntiforgery antiforgery;
        //@inject Microsoft.AspNetCore.Identity.SignInManager<Fotografolio.Data.Models.ApplicationUser>  signinManager;
        //@inject Microsoft.AspNetCore.Identity.UserManager<Fotografolio.Data.Models.ApplicationUser>  userManager;
        //@using System.Security.Claims;
        //var antiforgeryRequestToken = antiforgery.GetAndStoreTokens(Context).RequestToken;
        public IUnitOfWorkEF unitOfWork { get; private set; }
        public string AntiforgeryRequestToken => unitOfWork.antiforgery.GetAndStoreTokens(unitOfWork.httpContextAccessor.HttpContext).RequestToken;
        public bool LoginStatus
        {
            get
            {
                //if (Fotografolio.Services.UserService.JWTready) return true;
                    ClaimsPrincipal principal = unitOfWork.httpContextAccessor.HttpContext.User as ClaimsPrincipal;
                return unitOfWork.SignInManager.IsSignedIn(principal);
            }
        }
        //public string ApiToken
        //{
        //    get
        //    {
        //        //if (Fotografolio.Services.UserService.JWTready) 
        //        //    return unitOfWork.UserManager.Users.Select(u => u.api_token).Take(1).FirstOrDefault();
        //        var CurrentUer = unitOfWork.UserManager.GetUserAsync(unitOfWork.httpContextAccessor.HttpContext.User).GetAwaiter().GetResult();
        //        if (CurrentUer != null)
        //            return CurrentUer.api_token;
        //        else
        //            return null;
        //    }
        //}
        //string BuildedHeadTags()
        //{
        //    ///link[href='app.0f82a377.css']
        //    ///link[href='app.0426e60b.js']
        //    ///link[href='chunk-vendors.81d2fb61.js']
        //    ///link[href='app.0f82a377.css']
        //    //*[matches(@href, 'app.+css')]
        //    //*[matches(@href, 'app.+js')]
        //    //*[matches(@href, 'chunk-vendors.+js')]
        //    //@*<link href=/css/app.0f82a377.css rel=preload as= style>
        //    //    <link href=/js/app.0426e60b.js rel=preload as=script>
        //    //    <link href=/js/chunk-vendors.81d2fb61.js rel=preload as=script>
        //    //    <link href=/css/app.0f82a377.css rel=stylesheet>*@
        //    return "";
        //}
        //string BuildedBodyTags()
        //{
        //    //@*<script src=/js/chunk-vendors.81d2fb61.js></script>
        //    //<script src=/js/app.0426e60b.js></script>*@
        //    return "";
        //}
    }
}