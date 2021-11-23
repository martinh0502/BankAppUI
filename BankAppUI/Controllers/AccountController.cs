using BankAppUI.Models;
using BankAppUI.ModelsIdentity;
using BankAppUI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankAppUI.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private SignInManager<ApplicationUser> _signinmanager;
        private UserManager<ApplicationUser> _usermanager;
        private readonly IUserRepository _userRepository;
        public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _signinmanager = signInManager;
            _usermanager = userManager;
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public async Task<IActionResult> Login(NewUser user)
        {

            var result = await _signinmanager.PasswordSignInAsync(user.UserName, user.Password, false, true);



            var current = _usermanager.Users.Where(u => u.UserName == user.UserName).SingleOrDefault();

            if (result.Succeeded)
            {
                if (await _usermanager.IsInRoleAsync(current, "Administrator"))
                {
                    return RedirectToAction("Main", "Admin");
                }
                else
                {
                    return RedirectToAction("Main", "Customer");
                } 
            }
            else
            {
                ViewBag.Message = "Felaktig inloggning";
                return View();
            }


        }

        public async Task<IActionResult> LogOff()
        {
            await _signinmanager.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(NewUser newUser)
        {


            var result = _userRepository.CreateUser(newUser);

            if (result.Succeeded)
            {

                return RedirectToAction("Main", "Admin");
            }
            else
            {
                
                ViewBag.Message = string.Join(", ", result.Errors.Select(x => "Code " + x.Code + " Description" + x.Description));

                return View();
            }



        }

    }
}

