using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WepPha2.Data;
using WepPha2.Models;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ApplicationDbContext _context;
        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult Login()
        {
            var response = new LoginViewModel();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid) return View(loginViewModel);

            var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
            if (user != null)
            {
                var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                if (passwordCheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Medicine");
                    }
                }

                TempData["Error"] = "Wrong credentials. Please try again";
                return View(loginViewModel);
            }
            TempData["Error"] = "Wrong credentials. Please try again";
            return View(loginViewModel);
        }
       
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login","Account");
        }

    }
}
