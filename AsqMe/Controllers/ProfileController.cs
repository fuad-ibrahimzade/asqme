using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AsqMe.Data;
using AsqMe.Data.Models;
using AsqMe.Helpers;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace AsqMe.Controllers
{
    [Authorize]
    [Route("/[action]")]
    public class ProfileController : Controller
    {
        public ProfileController(SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment env)
        {
            SignInManager = signInManager;
            UserManager = userManager;
            Env = env;
        }
        [TempData]
        public string ErrorMessage { get; set; }
        [TempData]
        public string SerializedData { get; set; }

        public SignInManager<ApplicationUser> SignInManager { get; }
        public UserManager<ApplicationUser> UserManager { get; }
        public IWebHostEnvironment Env { get; }

        public IActionResult Asq()
        {
            return View("~/Views/Profile/Asq.cshtml");
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLogin2Async()
        {
            var email = "email@email.com";
            var info = await SignInManager.GetExternalLoginInfoAsync();
            if (info == null) info = new ExternalLoginInfo(User, "Facebook", "SomeKey", "FacebookSomekey");
            var signInResult = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                SerializedData = Newtonsoft.Json.Linq.JObject.FromObject(new
                {
                    email,
                    username = "TempFacebookName",
                    message = "successifully signed in"
                }).ToString();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var user = new ApplicationUser { UserName = "TempFacebookName", Email = email };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false);
                    }
                }

                SerializedData = Newtonsoft.Json.Linq.JObject.FromObject(new
                {
                    email,
                    username = "TempFacebookName",
                    message = "successifully registered"
                }).ToString();
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpGet("/logout")]
        public async Task<IActionResult> Logout()
        {
            await SignInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        [Route("/signin-facebook")]
        [Route("/signin-facebook/{code}/{state}")]
        public IActionResult SignInFacebook(string code,string state)
        {
            SerializedData = Newtonsoft.Json.Linq.JObject.FromObject(new
            {
                message = "from SignInFacebook"
            }).ToString();
            return RedirectToAction("Index", "Home");
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("/ExternalLoginGet")]
        public async Task<IActionResult> ExternalLoginGetAsync()
        {
            var provider = "Facebook";
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Profile", new { returnUrl= Url.Content("~/") });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            //var authProperties = new AuthenticationProperties
            //{
            //    RedirectUri = Url.Action(nameof(ExternalLoginCallback), "ExternalLoginAsync", new { returnUrl = Url.Action("Index", "Home") })
            //};
            //return Challenge(authProperties, "Facebook");
        }
        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginAsync(string provider, string returnUrl = null)
        {
            //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ////var authProperties = new AuthenticationProperties
            ////{
            ////    RedirectUri = Url.Action(nameof(ExternalLoginCallback), "Profile", new { returnUrl= Url.Action("Index", "Home") })
            ////};
            //var properties2 = SignInManager.ConfigureExternalAuthenticationProperties(provider, Url.Action("Index", "Home"));
            //return Challenge(properties2, "Facebook");
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Profile", new { returnUrl });
            var properties = SignInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
        {
            var info = await SignInManager.GetExternalLoginInfoAsync();

            if (info == null)
            {
                ErrorMessage = "Error loading external login information.";
                var data = new { error = "can not login" };
                //ModelState.AddModelError(string.Empty, "can not login");
                return RedirectToAction("Index", "Home", data);
            }
            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            var userFacebookName = info.Principal.FindFirstValue(ClaimTypes.Name);
            var profileLink = info.Principal.FindFirstValue(FacebookClaimTypes.Link);
            var identifier = info.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
            //foreach (var item in info.AuthenticationTokens)
            //{
            //    System.IO.File.AppendAllText("claims.txt", $"{item.Name} : {item.Value} , {Environment.NewLine}");
            //}
            var picture = $"https://graph.facebook.com/{identifier}/picture?type=large";
            picture = $"https://graph.facebook.com/{identifier}/picture?type=square";
            picture = $"https://graph.facebook.com/{identifier}/picture";
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.DownloadFileAsync(new Uri(picture), System.IO.Path.Combine(
                System.IO.Path.GetFullPath(Env.WebRootPath),
                "assets", "profile_pictures", identifier + "_facebook.jpg"
                ));
            //https://graph.facebook.com/3451169548245677/picture?type=large

            var signInResult = await SignInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);
            if (signInResult.Succeeded)
            {
                SerializedData = Newtonsoft.Json.Linq.JObject.FromObject(new
                {
                    email,
                    username = userFacebookName,
                    picture,
                    message = "successifully signed in"
                }).ToString();
                return RedirectToAction("Index", "Home");
                //return RedirectToRoute(returnUrl);
            }
            if (signInResult.IsLockedOut)
            {
                var data = new { error = "locked out , password is wrong" };
                //ModelState.AddModelError(string.Empty, "locked out , password is wrong");
                ErrorMessage = "locked out , password is wrong";
                return RedirectToAction("Index", "Home", data);
            }
            else
            {
                ViewData["ReturnUrl"] = returnUrl;
                ViewData["Provider"] = info.LoginProvider;

                var user = new ApplicationUser
                {
                    UserName = email,
                    Email = email,
                    FacebookName = userFacebookName,
                    ProfilePicture = System.IO.Path.Combine("assets", "profile_pictures", identifier + "_facebook.jpg"),
                    ProfilePictureWeb = picture
                };
                var result = await UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await UserManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false);

                        SerializedData = Newtonsoft.Json.Linq.JObject.FromObject(new
                        {
                            email,
                            username = userFacebookName,
                            picture,
                            message = "successifully registered",
                        }).ToString();
                    }
                    else
                    {
                        ErrorMessage = "can not add facebook login";
                    }
                }
                else
                {
                    ErrorMessage = "can not create user";
                }


                //new Microsoft.AspNetCore.Routing.RouteValueDictionary(data)
                return RedirectToAction("Index", "Home");
            }
        }

    }
}