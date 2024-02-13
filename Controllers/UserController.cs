using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.AccessControl;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.Services;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IEmployeeRepository _employeeRepository;
        public UserController(IUserRepository userRepository, UserManager<AppUser> userManager, IEmployeeRepository employeeRepository)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _employeeRepository = employeeRepository;
        }

        [HttpGet("users")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllUsers();
            List<UserViewModel> result = new List<UserViewModel>();
            foreach (var user in users)
            {
                var userViewModel = new UserViewModel()
                {
                    Id = user.Id,
                    Email = user.UserName,
                    Phone = user.PhoneNumber
                };
                result.Add(userViewModel);
            }
            return View(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> EditProfile(string id)
        {
            var user = await _userRepository.GetUserById(id);

            if (user == null)
            {
                return View("Error");
            }

            var editMV = new EditProfileViewModel()
            {
               Id =user.Id,
               Email = user.Email,
               Phone = user.PhoneNumber,
               UserName = user.Email
            };
            return View(editMV);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(EditProfileViewModel editVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit profile");
                return View("EditProfile", editVM);
            }

            var user2 = await _userRepository.GetUserById(editVM.Id);
            string email = user2.Email; 
            if (user2 == null)
                return View("Error");

            var roles = await _userManager.GetRolesAsync(user2);

            user2.UserName = editVM.UserName;
            user2.PhoneNumber = editVM.Phone;
            user2.Email = editVM.Email;

            if (roles.FirstOrDefault() == "user")
            {
                await _employeeRepository.UpdateUserEmployee(user2,email);
            }       
            await _userManager.UpdateAsync(user2);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(string id)
        {
            var userDetails = await _userRepository.GetUserById(id);
            if (userDetails == null) return View("Error");
            return View(userDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var userDetails = await _userRepository.GetUserById(id);

            if (userDetails == null)
            {
                return View("Error");
            }
            _userRepository.Delete(userDetails);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CreateUserEmployee()
        {
            ViewBag.EmployeeList = await _userRepository.GetAllEmployee();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserEmployee(CreateUserEmployeeViewModel UserVM)
        {
            ViewBag.EmployeeList = await _userRepository.GetAllEmployee();
            var employer = await _userRepository.GetEmployeeById(UserVM.EmployeId);
            
            if (ModelState.IsValid)
            {
                //return View(medicine);
                var user = await _userManager.FindByEmailAsync(employer.Email);
                if (user != null)
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(UserVM);
                }

                var newUser = new AppUser()
                {
                    Email = employer.Email,
                    UserName = employer.Email,
                    PhoneNumber = employer.Phone

                };
                var newUserResponse = await _userManager.CreateAsync(newUser, UserVM.Password);

                if (newUserResponse.Succeeded)
                    await _userManager.AddToRoleAsync(newUser, UserRoles.User);
              

                return RedirectToAction("Index");
            }

            return View();

        }
        public async Task<IActionResult> CreateUserAdmin()
        {
            ViewBag.EmployeeList = await _userRepository.GetAllEmployee();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAdmin(CreateAdminUserViewModel UserVM)
        {
            if (ModelState.IsValid)
            {
                //return View(medicine);
                var user = await _userManager.FindByEmailAsync(UserVM.Email);
                if (user != null)
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(UserVM);
                }

                var newUser = new AppUser()
                {
                    Email = UserVM.Email,
                    UserName = UserVM.Email,
                };
                var newUserResponse = await _userManager.CreateAsync(newUser, UserVM.Password);

                if (newUserResponse.Succeeded)
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Admin);


                return RedirectToAction("Index");
            }

            return View();

        }
    }
}

