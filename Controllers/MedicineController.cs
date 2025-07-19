using Microsoft.AspNetCore.Mvc;
using WepPha2.Data;
using WepPha2.Interfaces;
using WepPha2.Models;
using WepPha2.Repository;
using WepPha2.ViewModels;

namespace WepPha2.Controllers
{
    public class MedicineController : Controller
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IPhotoService _photoService;
        private readonly ILogger<MedicineController> _logger;

        public MedicineController(IMedicineRepository medicineRepository, IPhotoService photoService, ILogger<MedicineController> logger)
        {          
            _medicineRepository = medicineRepository;
            _photoService = photoService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string searchstring, int?CategoryId)
        {
            try
            {
                ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
                var medicines = await _medicineRepository.GetAll();
                if(CategoryId.HasValue)
                    medicines = await _medicineRepository.GetMedicineByCategory(CategoryId);
                if (!String.IsNullOrEmpty(searchstring))
                    medicines = await _medicineRepository.SearchMedicineByName(searchstring);
                return View(medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading medicines. Search: {SearchString}, CategoryId: {CategoryId}", searchstring, CategoryId);
                throw;
            }
        }

        [Route("Medicine/Detail/{id:int:min(1)}")]
        public async Task<IActionResult> Detail(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid medicine ID requested: {Id}", id);
                    return NotFound();
                }

                Medicine medicine = await _medicineRepository.GetMedicineById(id);
                if (medicine == null)
                {
                    _logger.LogWarning("Medicine not found with ID: {Id}", id);
                    return NotFound();
                }
                return View(medicine);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading medicine details for ID: {Id}", id);
                throw;
            }
        }

        public async Task<IActionResult> Create()
        {
            try
            {
                ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
                ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading create medicine form");
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateMedicineViewModel medicineVM)
        {
            try
            {
                ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
                ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
                
                if (ModelState.IsValid)
                {
                    if (medicineVM.Image == null)
                    {
                        ModelState.AddModelError("Image", "Please select an image");
                        return View(medicineVM);
                    }

                    var result = await _photoService.AddPhotoAsync(medicineVM.Image);
                    if (result.Error != null)
                    {
                        _logger.LogError("Photo upload failed: {Error}", result.Error.Message);
                        ModelState.AddModelError("Image", "Photo upload failed");
                        return View(medicineVM);
                    }

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
                    _logger.LogInformation("Medicine created successfully: {MedicineName}", medicine.MedicineName);
                    return RedirectToAction("Index");
                }
                else
                {
                    _logger.LogWarning("Invalid model state for medicine creation");
                    ModelState.AddModelError("", "Please check the form data");
                }

                return View(medicineVM);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating medicine");
                ModelState.AddModelError("", "An error occurred while creating the medicine. Please try again.");
                return View(medicineVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid medicine ID for edit: {Id}", id);
                    return NotFound();
                }

                ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
                ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
                
                var medicine = await _medicineRepository.GetMedicineById(id);
                if (medicine == null)
                {
                    _logger.LogWarning("Medicine not found for edit with ID: {Id}", id);
                    return NotFound();
                }

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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading edit form for medicine ID: {Id}", id);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, EditMedicineViewModel medicineVM)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid medicine ID for edit: {Id}", id);
                    return NotFound();
                }

                ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
                ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
                
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for medicine edit, ID: {Id}", id);
                    ModelState.AddModelError("", "Please check the form data");
                    return View("Edit", medicineVM);
                }

                var existingMedicine = await _medicineRepository.GetMedicineByIdAsNoTracking(id);
                if (existingMedicine == null)
                {
                    _logger.LogWarning("Medicine not found for edit with ID: {Id}", id);
                    return NotFound();
                }

                var photoResult = await _photoService.AddPhotoAsync(medicineVM.Image);
                if (photoResult.Error != null)
                {
                    _logger.LogError("Photo upload failed for medicine edit: {Error}", photoResult.Error.Message);
                    ModelState.AddModelError("Image", "Photo upload failed");
                    return View(medicineVM);
                }

                if (!string.IsNullOrEmpty(existingMedicine.Image))
                {
                    _ = _photoService.DeletePhotoAsync(existingMedicine.Image);
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
                _logger.LogInformation("Medicine updated successfully: {MedicineName}, ID: {Id}", medicine.MedicineName, id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while editing medicine with ID: {Id}", id);
                ModelState.AddModelError("", "An error occurred while updating the medicine. Please try again.");
                return View(medicineVM);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid medicine ID for delete: {Id}", id);
                    return NotFound();
                }

                ViewBag.SupplierList = await _medicineRepository.GetAllSuppliers();
                ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
                var medicineDetails = await _medicineRepository.GetMedicineById(id);
                
                if (medicineDetails == null)
                {
                    _logger.LogWarning("Medicine not found for delete with ID: {Id}", id);
                    return NotFound();
                }
                
                return View(medicineDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading delete form for medicine ID: {Id}", id);
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteMedicine(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid medicine ID for delete: {Id}", id);
                    return NotFound();
                }

                var medicineDetails = await _medicineRepository.GetMedicineById(id);
                if (medicineDetails == null)
                {
                    _logger.LogWarning("Medicine not found for delete with ID: {Id}", id);
                    return NotFound();
                }

                if (!string.IsNullOrEmpty(medicineDetails.Image))
                {
                    _ = _photoService.DeletePhotoAsync(medicineDetails.Image);
                }

                _medicineRepository.Delete(medicineDetails);
                _logger.LogInformation("Medicine deleted successfully: {MedicineName}, ID: {Id}", medicineDetails.MedicineName, id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting medicine with ID: {Id}", id);
                throw;
            }
        }

        // Attribute route with optional parameters and default values
        [Route("Medicine/Search/{searchString?}/{categoryId:int?}")]
        public async Task<IActionResult> Search(string searchString = "", int? categoryId = null)
        {
            try
            {
                ViewBag.CategoryList = await _medicineRepository.GetAllCategories();
                var medicines = await _medicineRepository.GetAll();
                
                if (categoryId.HasValue)
                    medicines = await _medicineRepository.GetMedicineByCategory(categoryId);
                if (!String.IsNullOrEmpty(searchString))
                    medicines = await _medicineRepository.SearchMedicineByName(searchString);
                
                return View("Index", medicines);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while searching medicines. Search: {SearchString}, CategoryId: {CategoryId}", searchString, categoryId);
                throw;
            }
        }

        public async Task<IActionResult> CalculateSuppliersQuantityMedicines()
        {
            try
            {
                var supplierMedicinesCount = await _medicineRepository.GetSupplierMedicinesCount();
                return View(supplierMedicinesCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while calculating suppliers quantity medicines");
                throw;
            }
        }
    }
}
