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
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<AppUser> userManager, 
            SignInManager<AppUser> signInManager, 
            ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // Conventional route - handled by default route
        [HttpGet]
        public IActionResult Login()
        {
            try
            {
                var response = new LoginViewModel();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading login page");
                throw;
            }
        }

        // Attribute route to resolve ambiguity with GET Login
        [HttpPost]
        [Route("Account/Login")]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for login attempt");
                    return View(loginViewModel);
                }

                if (string.IsNullOrEmpty(loginViewModel.EmailAddress) || string.IsNullOrEmpty(loginViewModel.Password))
                {
                    _logger.LogWarning("Login attempt with empty email or password");
                    TempData["Error"] = "Please provide both email and password";
                    return View(loginViewModel);
                }

                var user = await _userManager.FindByEmailAsync(loginViewModel.EmailAddress);
                if (user != null)
                {
                    var passwordCheck = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                    if (passwordCheck)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, false, false);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("User logged in successfully: {Email}", loginViewModel.EmailAddress);
                            return RedirectToAction("Index", "Medicine");
                        }
                        else
                        {
                            _logger.LogWarning("Failed sign-in attempt for user: {Email}, Result: {Result}", loginViewModel.EmailAddress, result.ToString());
                            TempData["Error"] = "Sign-in failed. Please try again";
                        }
                    }
                    else
                    {
                        _logger.LogWarning("Invalid password for user: {Email}", loginViewModel.EmailAddress);
                        TempData["Error"] = "Invalid password. Please try again";
                    }
                }
                else
                {
                    _logger.LogWarning("Login attempt with non-existent email: {Email}", loginViewModel.EmailAddress);
                    TempData["Error"] = "User not found. Please check your email address";
                }

                return View(loginViewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during login process for email: {Email}", loginViewModel?.EmailAddress);
                TempData["Error"] = "An error occurred during login. Please try again.";
                return View(loginViewModel);
            }
        }

        // Attribute route with constraint
        [Route("Account/Logout")]
        [HttpGet]
        public async Task<IActionResult> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                _logger.LogInformation("User logged out successfully");
                return RedirectToAction("Login","Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred during logout process");
                // Even if logout fails, redirect to login page
                return RedirectToAction("Login", "Account");
            }
        }
    }
}
