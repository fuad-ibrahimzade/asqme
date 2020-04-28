using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using AsqMe.Data.Models.Interfaces;
using AsqMe.Data.Models;
using AsqMe.Helpers;
using AsqMe.Services;
using Microsoft.AspNetCore.Antiforgery;

namespace AsqMe.Data
{
    public interface IUnitOfWorkEF
    {
        public IAntiforgery antiforgery { get; }
        static Dictionary<string, int> SeededMaxIds { get; set; }
        void MigrateDatabse(string targetMigration);
        static string HerokuPostgreSqlConnectionString { get; set; }
        string BaseUrl { get; set; }
        //GenericRepository<About> AboutRepository { get; }
        //GalleryRepository GalleryRepository { get; }
        //GenericRepository<Link> LinkRepository { get; }
        //GenericRepository<Logo> LogoRepository { get; }
        //PhotoRepository PhotoRepository { get; }
        //GenericRepository<Slide> SlideRepository { get; }
        IHttpContextAccessor httpContextAccessor { get; }
        UserManager<ApplicationUser> UserManager { get; }
        RoleManager<IdentityRole> RoleManager { get; }
        SignInManager<ApplicationUser> SignInManager { get; }
        IOptions<AppSettings> appSettings { get; set; }
        //UserService UserService { get; }
        string ClientIpAddress();
        bool CreateDefaults();
        Task<int> SaveAsync();
        void Dispose();
    }

    public class UnitOfWorkEF : IUnitOfWorkEF, IDisposable
    {
        private bool _disposed = false;
        public string BaseUrl { get; set; }
        public UserManager<ApplicationUser> UserManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }
        public SignInManager<ApplicationUser> SignInManager { get; }
        public IOptions<AppSettings> appSettings { get; set; }
        //private UserService userService;
        //public UserService UserService
        //{
        //    get
        //    {
        //        if (this.userService == null)
        //        {
        //            this.userService = new UserService(appSettings, this);
        //        }
        //        return userService;
        //    }
        //}

        //private GenericRepository<About> aboutRepository;
        //public GenericRepository<About> AboutRepository
        //{
        //    get
        //    {
        //        if (this.aboutRepository == null)
        //        {
        //            this.aboutRepository = new GenericRepository<About>(_context);
        //        }
        //        return aboutRepository;
        //    }
        //}
        //private GalleryRepository galleryRepository;
        //public GalleryRepository GalleryRepository
        //{
        //    get
        //    {
        //        if (this.galleryRepository == null)
        //        {
        //            this.galleryRepository = new GalleryRepository(_context);
        //        }
        //        return galleryRepository;
        //    }
        //}
        //private GenericRepository<Link> linkRepository;
        //public GenericRepository<Link> LinkRepository
        //{
        //    get
        //    {
        //        if (this.linkRepository == null)
        //        {
        //            this.linkRepository = new GenericRepository<Link>(_context);
        //        }
        //        return linkRepository;
        //    }
        //}
        //private GenericRepository<Logo> logoRepository;
        //public GenericRepository<Logo> LogoRepository
        //{
        //    get
        //    {
        //        if (this.logoRepository == null)
        //        {
        //            this.logoRepository = new GenericRepository<Logo>(_context);
        //        }
        //        return logoRepository;
        //    }
        //}
        //private PhotoRepository photoRepository;
        //public PhotoRepository PhotoRepository
        //{
        //    get
        //    {
        //        if (this.photoRepository == null)
        //        {
        //            this.photoRepository = new PhotoRepository(_context);
        //        }
        //        return photoRepository;
        //    }
        //}
        //private GenericRepository<Slide> slideRepository;
        //public GenericRepository<Slide> SlideRepository
        //{
        //    get
        //    {
        //        if (this.slideRepository == null)
        //        {
        //            this.slideRepository = new GenericRepository<Slide>(_context);
        //        }
        //        return slideRepository;
        //    }
        //}

        public IHttpContextAccessor httpContextAccessor { get; }
        public IAntiforgery antiforgery { get; }

        //private IUrlHelper urlHelper;
        public static Dictionary<string, int> SeededMaxIds { get; set; }

        public UnitOfWorkEF(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            IOptions<AppSettings> appSettings,
            IHttpContextAccessor httpContextAccessor,
            IAntiforgery antiforgery)
        {
            UserManager = userManager;
            RoleManager = roleManager;
            SignInManager = signInManager;
            this.appSettings = appSettings;
            this.httpContextAccessor = httpContextAccessor;
            this.antiforgery = antiforgery;

            if(SeededMaxIds == null)
            {
                //SeededMaxIds = new Dictionary<string, int>();
                //foreach (var item in context.Model.GetEntityTypes())
                //{
                //    var clrType = item.ClrType.Name;
                //    if (clrType == typeof(Gallery).Name)
                //        SeededMaxIds.Add(typeof(Gallery).Name, 2);
                //    else if (clrType == typeof(Photo).Name)
                //        SeededMaxIds.Add(typeof(Photo).Name, 17);
                //    else if (clrType == typeof(Link).Name)
                //        SeededMaxIds.Add(typeof(Link).Name, 4);
                //    else if (clrType == typeof(About).Name)
                //        SeededMaxIds.Add(typeof(About).Name, 1);
                //    else if (clrType == typeof(Logo).Name)
                //        SeededMaxIds.Add(typeof(Logo).Name, 1);
                //    else if (clrType == typeof(Slide).Name)
                //        SeededMaxIds.Add(typeof(Slide).Name, 3);
                //}
                //if (FotografolioDbContext.SeededMaxIds == null)
                //    FotografolioDbContext.SeededMaxIds = SeededMaxIds;
            }
            
            this.BaseUrl = string.Format("{0}://{1}/", httpContextAccessor.HttpContext.Request.Scheme, httpContextAccessor.HttpContext.Request.Host);
        }

        //public void DeleteUsers()
        //{
        //    UserManager.Users.ToList().ForEach(async (user) =>
        //    {
        //        var logins = await UserManager.GetLoginsAsync(user);
        //        var rolesForUser = await UserManager.GetRolesAsync(user);

        //        using (var transaction = _context.Database.BeginTransaction())
        //        {
        //            IdentityResult result = IdentityResult.Success;
        //            foreach (var login in logins)
        //            {
        //                result = await UserManager.RemoveLoginAsync(user, login.LoginProvider, login.ProviderKey);
        //                if (result != IdentityResult.Success)
        //                    break;
        //            }
        //            if (result == IdentityResult.Success)
        //            {
        //                foreach (var item in rolesForUser)
        //                {
        //                    result = await UserManager.RemoveFromRoleAsync(user, item);
        //                    if (result != IdentityResult.Success)
        //                        break;
        //                }
        //            }
        //            if (result == IdentityResult.Success)
        //            {
        //                result = await UserManager.DeleteAsync(user);
        //                if (result == IdentityResult.Success)
        //                    transaction.Commit(); //only commit if user and all his logins/roles have been deleted  
        //            }
        //        }
        //    });


        //}

        public bool CreateDefaults()
        {
            //bool aboutIsEmpty = AboutRepository.IsEmpty().GetAwaiter().GetResult();
            ////aboutIsEmpty = false;
            //if (aboutIsEmpty == false) return false;
            //if (aboutIsEmpty)
            //{
            //    try
            //    {
            //        var newAbout = new About
            //        {
            //            created_at = DateTime.Now,
            //            updated_at = DateTime.Now,
            //            photo = $@"http://res.cloudinary.com/djqq6o8su/image/upload/mkvg51472rnjves4etg0",
            //            title = "Mr Photographer",
            //            desc = "Photographer based in Somewhere"
            //        };
            //        AboutRepository.Insert(newAbout);
            //        SaveAsync().GetAwaiter().GetResult();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Exception caught.", e);
            //    }
            //}
            //bool galleryIsEmpty = GalleryRepository.IsEmpty().GetAwaiter().GetResult();
            ////galleryIsEmpty = false;
            //if (galleryIsEmpty)
            //{
            //    try
            //    {
            //        string[] names = new string[] { "People", "Landscape" };
            //        foreach (var name in names)
            //        {
            //            var newItem = new Gallery
            //            {
            //                created_at = DateTime.Now,
            //                updated_at = DateTime.Now,
            //                name = name
            //            };
            //            GalleryRepository.Insert(newItem);
            //        }
            //        SaveAsync().GetAwaiter().GetResult();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Exception caught.", e);
            //    }
            //}
            //bool linkIsEmpty = LinkRepository.IsEmpty().GetAwaiter().GetResult();
            ////linkIsEmpty = false;
            //if (linkIsEmpty)
            //{
            //    try
            //    {
            //        string[] names = new string[] { "yourfacebookname", "yourinsntagramname", "yourtwittername", "youryourtubename" };
            //        foreach (var name in names)
            //        {
            //            var newItem = new Link
            //            {
            //                created_at = DateTime.Now,
            //                updated_at = DateTime.Now,
            //                name = name
            //            };
            //            LinkRepository.Insert(newItem);
            //        }
            //        SaveAsync().GetAwaiter().GetResult();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Exception caught.", e);
            //    }
            //}
            //bool logoIsEmpty = LogoRepository.IsEmpty().GetAwaiter().GetResult();
            ////logoIsEmpty = false;
            //if (logoIsEmpty)
            //{
            //    try
            //    {
            //        string[] photos = new string[] { "http://res.cloudinary.com/djqq6o8su/image/upload/iwrwnlkuxlbypgdn05kp" };
            //        foreach (var photo in photos)
            //        {
            //            var newItem = new Logo
            //            {
            //                created_at = DateTime.Now,
            //                updated_at = DateTime.Now,
            //                photo = photo
            //            };
            //            LogoRepository.Insert(newItem);
            //        }
            //        SaveAsync().GetAwaiter().GetResult();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Exception caught.", e);
            //    }
            //}
            //bool slideIsEmpty = SlideRepository.IsEmpty().GetAwaiter().GetResult();
            ////slideIsEmpty = false;
            //if (slideIsEmpty)
            //{
            //    try
            //    {
            //        string[] names = new string[]
            //        {
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/rrcbpzjxm5mesjptu9lc",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/iib9veyowj3wpyirbmlo",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/qgo6zbmwwd0kyanaoynz"
            //        };
            //        foreach (var name in names)
            //        {
            //            var newItem = new Slide
            //            {
            //                created_at = DateTime.Now,
            //                updated_at = DateTime.Now,
            //                name = name
            //            };
            //            SlideRepository.Insert(newItem);
            //        }
            //        SaveAsync().GetAwaiter().GetResult();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Exception caught.", e);
            //    }
            //}
            //bool photoIsEmpty = PhotoRepository.IsEmpty().GetAwaiter().GetResult();
            ////photoIsEmpty = false;
            //if (photoIsEmpty)
            //{
            //    try
            //    {
            //        string[] names = new string[]
            //        {
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/tub2v7olse0lwbq9vzrv",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/hvxogea37wkzqv5o6kkv",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/dp62d2vwochkoy2y82ee",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/lifml3ceohhebylxgue6",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/ki6pdlhihsvaakimfnml",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/itwquw92jfpht6yi1ilk",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/jplpld8tp4bxvskdpdzr"
            //        };
            //        string[] names2 = new string[]
            //        {
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/vp8ahga7g1h8f6zx2d0x",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/ozljewvxh83ahvob1aml",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/tgrcyaoqx8jwi7mksakd",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/nnsii3wvw6gn40qcldwf",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/e23tgxmvw8jkeje3apjk",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/tb0rpo4gvhxv0cyyb31z",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/k90gojfb903m7hoknvs5",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/yqmqqvltyfgt6wbza9zg",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/vrlwiz6u8gcrygbruzw3",
            //            "http://res.cloudinary.com/djqq6o8su/image/upload/xapqjqz9vs5kwhqh0cr5"
            //        };
            //        var galleryPeople = GalleryRepository.GetAsync(item => item.name == "People").GetAwaiter().GetResult().FirstOrDefault();
            //        var galleryLandscape = GalleryRepository.GetAsync(item => item.name == "Landscape").GetAwaiter().GetResult().FirstOrDefault();
            //        foreach (var name in names)
            //        {
            //            var newItem = new Photo
            //            {
            //                created_at = DateTime.Now,
            //                updated_at = DateTime.Now,
            //                name = name,
            //                Gallery = galleryPeople
            //            };
            //            PhotoRepository.Insert(newItem);
            //        }
            //        foreach (var name in names2)
            //        {
            //            var newItem = new Photo
            //            {
            //                created_at = DateTime.Now,
            //                updated_at = DateTime.Now,
            //                name = name,
            //                Gallery = galleryLandscape
            //            };
            //            PhotoRepository.Insert(newItem);
            //        }
            //        SaveAsync().GetAwaiter().GetResult();
            //    }
            //    catch (Exception e)
            //    {
            //        Console.WriteLine("{0} Exception caught.", e);
            //    }
            //}

            //bool userIsEmpty = UserManager.Users.Count() == 0;
            //if (userIsEmpty)
            //{
            //    //var user = new ApplicationUser
            //    //{
            //    //    ApiToken = "some",
            //    //    UserName = "admin@admin.com",
            //    //    Email = "admin@admin.com",
            //    //    EmailConfirmed = true,
            //    //    DateAdd = DateTime.Now,
            //    //    DateUpd = DateTime.Now
            //    //};
            //    //await UserManager.CreateAsync(user, "admin");
            //    ApplicationDbInitializer.SeedRoles(RoleManager);
            //    ApplicationDbInitializer.SeedUsers(UserManager, appSettings);
            //    //await SaveAsync();
            //    //new SignInManager<User>().SignInAsync
            //    //UserManager.Users.ToList().ForEach( async(user)=> {
            //    //    await _userManager.DeleteAsync(user);
            //    //});
            //}

            //if (aboutIsEmpty || galleryIsEmpty || linkIsEmpty || logoIsEmpty || photoIsEmpty || slideIsEmpty || userIsEmpty)
            //{
            //    return true;
            //}
            //else
                return false;
        }

        public string ClientIpAddress()
        {
            return httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString();
        }

        //public async Task<int> SaveAsync()
        //{
        //    return await _context.SaveChangesAsync();
        //}

        //public void MigrateDatabse(string targetMigration)
        //{
        //    _context.GetInfrastructure().GetService<IMigrator>().Migrate(targetMigration);
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    //_context.Dispose();
                }
            }
            this._disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void MigrateDatabse(string targetMigration)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveAsync()
        {
            throw new NotImplementedException();
        }
    }
}
