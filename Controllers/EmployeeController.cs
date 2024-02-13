using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.Services;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<AppUser> _userManager;

        public EmployeeController(IEmployeeRepository employeeRepository, UserManager<AppUser> userManager)
        {
            _employeeRepository = employeeRepository;
            _userManager = userManager; 
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _employeeRepository.GetAll();
            return View(employees);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            ViewBag.PharmacyList = await _employeeRepository.GetAllPharmacy();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEmployeeViewModel employeeVM)
        {
            ViewBag.PharmacyList = await _employeeRepository.GetAllPharmacy();
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(employeeVM.Email);
                if (user != null)
                {
                    TempData["Error"] = "This email address is already in use";
                    return View(employeeVM);
                }
                var employee = new Employee
                {
                    FirstName = employeeVM.FirstName,
                    LastName = employeeVM.LastName,
                    Email = employeeVM.Email,
                    Phone = employeeVM.Phone,
                    StartDate = employeeVM.StartDate,
                    Address = employeeVM.Address,
                    PharmacyId = employeeVM.PharmacyId,
                };
                _employeeRepository.Add(employee);

                return RedirectToAction("Index");
            }

            return View();

        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.PharmacyList = await _employeeRepository.GetAllPharmacy();
            var employee = await _employeeRepository.GetEmployeeById(id);
            if (employee == null) return View("Error");
            var employeeVM = new EditEmployeeViewModel
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                Phone = employee.Phone,
                StartDate = employee.StartDate,
                Address = employee.Address,
                PharmacyId = employee.PharmacyId,
            };
            return View(employeeVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditEmployeeViewModel employeeVM)
        {
            ViewBag.PharmacyList = await _employeeRepository.GetAllPharmacy();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit employer");
                return View(employeeVM);
            }

            var userrEmployee = await _employeeRepository.GetEmployeeByIdAsNoTracking(id);

            if (userrEmployee == null)
            {
                return View("Error");
            }

            var employee = new Employee
            {
                EmployeeId = id,
                FirstName = employeeVM.FirstName,
                LastName = employeeVM.LastName,
                Email = employeeVM.Email,
                Phone = employeeVM.Phone,
                StartDate = employeeVM.StartDate,
                Address = employeeVM.Address,
                PharmacyId = employeeVM.PharmacyId,
            };

            _employeeRepository.Update(employee);

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var employerDetails = await _employeeRepository.GetEmployeeById(id);
            if (employerDetails == null) return View("Error");
            return View(employerDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var employerDetails = await _employeeRepository.GetEmployeeById(id);

            if (employerDetails == null)
            {
                return View("Error");
            }
            _employeeRepository.Delete(employerDetails);
            var user = await _userManager.FindByEmailAsync(employerDetails.Email);
            if ( user != null)
                await _userManager.DeleteAsync(user);
            return RedirectToAction("Index");
        }

    }
}
