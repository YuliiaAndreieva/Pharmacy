using Microsoft.AspNetCore.Mvc;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.Services;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepository;

        public SupplierController(ISupplierRepository supplierRepository)
        {
            _supplierRepository = supplierRepository;
        }
        public async Task<IActionResult> Index()
        {
            var suppliers = await _supplierRepository.GetAll();
            return View(suppliers);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Supplier supplier)
        {
            if (!ModelState.IsValid)
                return View(supplier);
            _supplierRepository.Add(supplier);
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var supplier = await _supplierRepository.GetSupplierById(id);
            if (supplier == null) return View("Error");
            var supplierVM = new EditSupplierViewModel
            {
                CompanyName = supplier.CompanyName,
                MobilePhone = supplier.MobilePhone,
                EmailAddress = supplier.EmailAddress,
                Address = supplier.Address
            };
            return View(supplierVM);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditSupplierViewModel supplierVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit supplier");
                return View(supplierVM);
            }

            var userSupplier = await _supplierRepository.GetSupplierByIdAsNoTracking(id);

            if (userSupplier == null)
                return View("Error");
           
            var supplier = new Supplier
            {
                SupplierId = id,
                CompanyName = supplierVM.CompanyName,
                MobilePhone = supplierVM.MobilePhone,
                EmailAddress = supplierVM.EmailAddress,
                Address = supplierVM.Address,
            };

            _supplierRepository.Update(supplier);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Detail(int id)
        {
            Supplier supplier = await  _supplierRepository.GetSupplierById(id);
            return View(supplier);
        }
        public async Task<IActionResult> Delete(int id)
        {
            var supplierDetails = await _supplierRepository.GetSupplierById(id);
            if (supplierDetails == null) return View("Error");
            return View(supplierDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteSupplier(int id)
        {
            var supplierDetails = await _supplierRepository.GetSupplierById(id);

            if (supplierDetails == null)
            {
                return View("Error");
            }      
            _supplierRepository.Delete(supplierDetails);
            return RedirectToAction("Index");
        }


    }
}
