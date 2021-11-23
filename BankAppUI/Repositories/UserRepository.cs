using BankAppUI.Models;
using BankAppUI.ModelsIdentity;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.Repositories
{
    public interface IUserRepository
    {
        public IdentityResult CreateUser(NewUser user);
    }
    public class UserRepository:IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserRepository(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public IdentityResult CreateUser(NewUser newUser)
        {
            IdentityResult result = new();
            if (_userManager.FindByNameAsync(newUser.UserName).Result == null)
            {
                ApplicationUser user = new ApplicationUser();
                user.UserName = newUser.UserName;
                user.CustomerId = newUser.CustomerId;

                 result = _userManager.CreateAsync
                (user, newUser.Password).Result;

                if (result.Succeeded)
                {
                    _userManager.AddToRoleAsync(user, "NormalUser").Wait();

                    
                }
                
            }
            return result;
        }
    }
}
