using AsqMe.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsqMe.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> AuthenticateAsync(string username, string password);
        Task<bool> LogoutAsync(string username);
        IEnumerable<ApplicationUser> GetAll();
    }
}
