using Microsoft.AspNetCore.Mvc;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.Services;
using WepPha2.Services.MailSending;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class MedicineController : Controller
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IPhotoService _photoService;
        private readonly NotificationService _notificationService;
        //private readonly IObserver _observer;

        public MedicineController(
            IPhotoService photoService,
            IMedicineRepository medicineRepository,
            NotificationService notificationService)
            //IObserver observer)
        {          
            _medicineRepository = medicineRepository;
            _photoService = photoService;
            _notificationService = notificationService;
            //_observer = observer;
        }

        public async Task<IActionResult> Index(string searchstring, int?CategoryId)
        {
            ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
            var medicines = await _medicineRepository.GetAll();
            if(CategoryId.HasValue)
                medicines = await _medicineRepository.GetMedicineByCategory(CategoryId);
            if (!String.IsNullOrEmpty(searchstring))
                medicines = await _medicineRepository.SearchMedicineByName(searchstring);
            return View(medicines);
        }
        public async Task<IActionResult> Detail(int id)
        {
            Medicine medicine = await _medicineRepository.GetMedicineById(id);
            return View(medicine);
        }
        public async Task<IActionResult> Create()
        {
            ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
            ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicineViewModel medicineVM)
        {
            ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
            ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
            if (ModelState.IsValid)
            {
                var result = await _photoService.AddPhotoAsync(medicineVM.Image);
                var medicine = new Medicine
                {
                    MedicineName = medicineVM.MedicineName,
                    ActiveSubstance = medicineVM.ActiveSubstance,
                    Contraindication = medicineVM.Contraindication,
                    BatchNumber = medicineVM.BatchNumber,
                    UnitsInStock = medicineVM.UnitsInStock,
                    ExpiryDate = medicineVM.ExpiryDate,
                    UnitPrice = medicineVM.UnitPrice,
                    IsOverdued = medicineVM.IsOverdued,
                    SupplierId = medicineVM.SupplierId,
                    CategoryId = medicineVM.CategoryId,
                    Description = medicineVM.Description,
                    UseMethod = medicineVM.UseMethod,
                    Quantity = medicineVM.Quantity,
                    Image = result.Url.ToString()

                };
                _medicineRepository.Add(medicine);
                return RedirectToAction("Index");
            }
            else
            {
                ModelState.AddModelError("", "Photo upload failed");
            }

            return View();

        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
            ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
            
            var medicine = await _medicineRepository.GetMedicineById(id);
            if (medicine == null) return View("Error");
            var medicineVM = new EditMedicineViewModel
            {
                MedicineName = medicine.MedicineName,
                ActiveSubstance = medicine.ActiveSubstance,
                Contraindication = medicine.Contraindication,
                BatchNumber = medicine.BatchNumber,
                UnitsInStock = medicine.UnitsInStock,
                ExpiryDate = medicine.ExpiryDate,
                UnitPrice = medicine.UnitPrice,
                IsOverdued = medicine.IsOverdued,
                SupplierId = medicine.SupplierId,
                CategoryId = medicine.CategoryId,
                Description = medicine.Description,
                UseMethod = medicine.UseMethod,
                Quantity = medicine.Quantity,
                URL = medicine.Image
            };
            return View(medicineVM);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMedicineViewModel medicineVM)
        {
            ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
            ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Failed to edit club");
                return View("Edit", medicineVM);
            }

            var userClub = await _medicineRepository.GetMedicineByIdAsNoTracking(id);

            if (userClub == null)
            {
                return View("Error");
            }

            var photoResult = await _photoService.AddPhotoAsync(medicineVM.Image);

            if (photoResult.Error != null)
            {
                ModelState.AddModelError("Image", "Photo upload failed");
                return View(medicineVM);
            }

            if (!string.IsNullOrEmpty(userClub.Image))
            {
                _ = _photoService.DeletePhotoAsync(userClub.Image);
            }

            var medicine = new Medicine
            {
                MedicineId = id,
                MedicineName = medicineVM.MedicineName,
                ActiveSubstance = medicineVM.ActiveSubstance,
                Contraindication = medicineVM.Contraindication,
                BatchNumber = medicineVM.BatchNumber,
                UnitsInStock = medicineVM.UnitsInStock,
                ExpiryDate = medicineVM.ExpiryDate,
                UnitPrice = medicineVM.UnitPrice,
                IsOverdued = medicineVM.IsOverdued,
                SupplierId = medicineVM.SupplierId,
                CategoryId = medicineVM.CategoryId,
                Description = medicineVM.Description,
                UseMethod = medicineVM.UseMethod,
                Quantity = medicineVM.Quantity,
                Image = photoResult.Url.ToString()
            };

            _medicineRepository.Update(medicine);

            return RedirectToAction("Index");      
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
            ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
            var medicineDetails = await _medicineRepository.GetMedicineById(id);
            if (medicineDetails == null) return View("Error");
            return View(medicineDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            var medicineDetails = await _medicineRepository.GetMedicineById(id);

            if (medicineDetails == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(medicineDetails.Image))
            {
                _ = _photoService.DeletePhotoAsync(medicineDetails.Image);
            }

            _medicineRepository.Delete(medicineDetails);
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> CalculateSuppliersQuantityMedicines()
        {
            var supplierMedicinesCount = await _medicineRepository.GetSupplierMedicinesCount();
            return View(supplierMedicinesCount);
        }
    }
}
