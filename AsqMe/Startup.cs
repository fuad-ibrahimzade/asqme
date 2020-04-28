using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AsqMe.Data;
using AsqMe.Data.Models;
using AsqMe.Helpers;
using AsqMe.Services;
using AsqMe.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace AsqMe
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            //services.AddControllersWithViews();
            services.AddControllersWithViews()
            .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            )
            .AddSessionStateTempDataProvider();
            //services.AddRazorPages();
            services.AddSession();

            services.AddHttpContextAccessor();
            services.AddAntiforgery();

            if (services.GetHerokuPostgreSQLConnectionString() != null)
            {
                AsqMeDbContext.HerokuPostgreSqlConnectionString = services.GetHerokuPostgreSQLConnectionString();
                services.AddEntityFrameworkNpgsql().AddDbContext<AsqMeDbContext>(opt =>
                    opt.UseNpgsql(services.GetHerokuPostgreSQLConnectionString()).UseLowerCaseNamingConvention());
            }
            else
            {
                AsqMeDbContext.LocalSQLServerConnectionString = Configuration.GetConnectionString("AsqMeDbContext");
                services.AddDbContext<AsqMeDbContext>(options =>
                    options.UseSqlServer(Configuration.GetConnectionString("AsqMeDbContext")));
            }

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/";
                options.User.RequireUniqueEmail = false;
            })
            .AddEntityFrameworkStores<AsqMeDbContext>();

            services.AddSingleton(x =>
            {

                using (var servicesContext = services.BuildServiceProvider())
                using (var dbContext = servicesContext.GetRequiredService<AsqMeDbContext>())
                {
                    dbContext.MigrateDatabse("Initial");
                    while (!(servicesContext.GetRequiredService<AsqMeDbContext>().Database.CanConnect()))
                    {
                        Thread.Sleep(250);
                    }
                }
                return new ConnectionString
                {
                    Value = services.GetHerokuPostgreSQLConnectionString() ??
                        Configuration.GetConnectionString("AsqMeDbContext")
                };
            });
            //services.AddTransient<IUnitOfWork>(x =>
            //{
            //    x.GetRequiredService<AsqMeDbContext>().MigrateDatabse("Initial");
            //    return new UnitOfWork(AsqMeDbContext.LocalSQLServerConnectionString ??
            //                    AsqMeDbContext.HerokuPostgreSqlConnectionString);
            //});
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            //services.AddTransient<IUnitOfWorkEF, UnitOfWorkEF>();
            services.AddTransient<ICloudniaryService, CloudinaryService>();
            //services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            //    .AddCookie(options =>
            //    {
            //        var queryParams = new Dictionary<string, string>()
            //        {
            //            {"returnurl", "/" }
            //        };
            //        var urlString = QueryHelpers.AddQueryString("/ExternalLogin", queryParams);
            //        options.LoginPath = new PathString(urlString);
            //    });
            //.AddCookie(options =>
            //{
            //    options.LoginPath = "/login2";
            //    options.LogoutPath = "/logout2";
            //})
            services.ConfigureApplicationCookie(options =>
            {
                //var queryParams = new Dictionary<string, string>()
                //        {
                //            {"returnurl", "/" }
                //        };
                //var urlString = QueryHelpers.AddQueryString("/ExternalLoginGet", queryParams);
                options.LoginPath = new PathString("/ExternalLoginGet");
            });
            services.AddAuthentication()
            .AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = Environment.GetEnvironmentVariable("FacebookAppId") != null ?
                Environment.GetEnvironmentVariable("FacebookAppId") :
                Configuration["AppSettings:FacebookAppConfig:AppId"];
                facebookOptions.AppSecret = Environment.GetEnvironmentVariable("FacebookAppSecret") != null ?
                Environment.GetEnvironmentVariable("FacebookAppSecret") :
                Configuration["AppSettings:FacebookAppConfig:AppSecret"];

                facebookOptions.SaveTokens = true;

                //var requestContext = HttpContext.Current.Request.RequestContext;
                //new UrlHelper(requestContext).Action("Index", "MainPage");
                //string url = QueryHelpers.AddQueryString("/api/product/list", "foo", "bar");

                //Multiple Parameters
                //var queryParams = new Dictionary<string, string>()
                //{
                //    {"returnurl", "/" }
                //};
                ////ExternalLogin?returnurl=%2F
                //var urlString = QueryHelpers.AddQueryString("/ExternalLogin", queryParams);
                //facebookOptions.AccessDeniedPath = urlString;
                //facebookOptions.ReturnUrlParameter = urlString;
                //facebookOptions.Events.OnRemoteFailure = context =>
                //{
                //    // React to the error here. See the notes below.
                //    //context.Response.Redirect(context.Properties.GetString("returnUrl"));
                //    //context.Response.Redirect("/signin-facebook");
                //    context.Response.Redirect("/signin-facebook");
                //    //context.Response.Redirect("/externallogin");
                //    context.HandleResponse();
                //    return Task.CompletedTask;
                //};
                //var Scopes = new string[] {
                //    "user_birthday", //Access the date and month of a person's birthday.  
                //    "public_profile" //Provides access to a subset of items that are part of a person's public profile.  
                //    //A person's public profile refers to the following properties on the user object by default:  
                //};
                var Scopes = new string[] {
                    "public_profile" //Provides access to a subset of items that are part of a person's public profile.  
                    //A person's public profile refers to the following properties on the user object by default:  
                };
                foreach (var item in Scopes)
                {
                    facebookOptions.Scope.Add(item);
                }
                var Fields = new string[] {
                    "link",
                    "birthday", //User's DOB  
                    "picture", //User Profile Image  
                    "name", //User Full Name  
                    "email", //User Email  
                    "gender", //user's Gender  
                };
                foreach (var item in Fields)
                {
                    facebookOptions.Fields.Add(item);
                }
                //facebookOptions.ClaimActions.MapJsonKey(FacebookClaimTypes.Link, "link", "url");
                //facebookOptions.UserInformationEndpoint = "";
                //facebookOptions.Fields.Add("link");
                //facebookOptions.Events.OnTicketReceived +=  delegate (TicketReceivedContext trContext)
                //{
                //    //string s = o.ToString() + " " + e.ToString();
                //    //Console.WriteLine(s);
                //    foreach (var item in trContext.Principal.Claims)
                //    {
                //        System.IO.File.AppendAllText("claims.txt", $"{item.Type} : {item.Value} , {Environment.NewLine}");
                //    } 
                //    return Task.FromResult(trContext);
                //};
            });
            services.AddAuthorization();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app,
            IWebHostEnvironment env,
            AsqMeDbContext dbcontext,
            IUnitOfWork unitOfWork)
        {
            //if (AsqMeDbContext.HerokuPostgreSqlConnectionString == null)
            //dbcontext.MigrateDatabse("Initial");
            //else
            //dbcontext.Database.Migrate();
            unitOfWork.CreateDefaults();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSession();
            app.UseEndpoints(endpoints =>
            {
                //endpoints.MapRazorPages();
                //endpoints.MapControllerRoute(name: "users",
                //    pattern: "users",
                //    defaults: new { controller = "Home", action = "Users" });
                //endpoints.MapControllerRoute(name: "userQuestions",
                //    pattern: "users/{id}",
                //    defaults: new { controller = "Home", action = "UserQuestions" });
                //endpoints.MapControllerRoute(name: "userQuestions",
                //    pattern: "users/{id}/questions",
                //    defaults: new { controller = "Home", action = "UserQuestions" });
                //endpoints.MapControllerRoute(name: "userAnswers",
                //    pattern: "users/{id}/answers",
                //    defaults: new { controller = "Home", action = "UserAnswers" });
                //endpoints.MapControllerRoute(name: "userEarnings",
                //    pattern: "users/{id}/earnings",
                //    defaults: new { controller = "Home", action = "UserEarnings" });
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                //endpoints.MapDefaultControllerRoute()
            });
        }
    }
}
