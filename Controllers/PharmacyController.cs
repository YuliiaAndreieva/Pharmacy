using Microsoft.AspNetCore.Mvc;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly IPharmacyRepository _pharmacyRepository;

        public PharmacyController(IPharmacyRepository pharmacyRepository)
        {
            _pharmacyRepository = pharmacyRepository;
        }
        public async Task<IActionResult> Index()
        {
            var categories = await _pharmacyRepository.GetAll();
            return View(categories);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Pharmacy pharmacy)
        {
            if (!ModelState.IsValid)
            {
                return View(pharmacy);
            }
            _pharmacyRepository.Add(pharmacy);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Delete(int id)
        {
            var pharmacyDetails = await _pharmacyRepository.GetPharmacyById(id);
            if (pharmacyDetails == null) return View("Error");
            return View(pharmacyDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePharmacy(int id)
        {
            var pharmacyDetails = await _pharmacyRepository.GetPharmacyById(id);

            if (pharmacyDetails == null)
            {
                return View("Error");
            }
            _pharmacyRepository.Delete(pharmacyDetails);
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var userPharmacy = await _pharmacyRepository.GetPharmacyById(id);
            if (userPharmacy== null) return View("Error");
            var pharmacyVM = new EditPharmacyViewModel
            {
                PharmacyId = userPharmacy.PharmacyId,
                Address = userPharmacy.Address
            };
            return View(pharmacyVM);
        }

        [HttpPost, ActionName("Edit")]
        public async Task<IActionResult> Edit(int id, EditPharmacyViewModel pharmacyVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit category");
                return View(pharmacyVM);
            }

            var userPharmacy = await _pharmacyRepository.GetPharmacyByIdAsNoTracking(id);

            if (userPharmacy == null)
            {
                return View("Error");
            }

            var pharmacy = new Pharmacy
            {
                PharmacyId = id,
                Address = pharmacyVM.Address,

            };

            _pharmacyRepository.Update(pharmacy);

            return RedirectToAction("Index");
        }
    }
}
