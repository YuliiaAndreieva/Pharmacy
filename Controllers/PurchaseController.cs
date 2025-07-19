using Microsoft.AspNetCore.Mvc;
using WepPha2.Interfaces;
using WepPha2.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using WepPha2.ViewModels;
using Microsoft.AspNetCore.Identity;
using WepPha2.Repository;
using WepPha2.Services;
using System.Reflection.Metadata;

namespace WepPha2.Controllers
{
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<PurchaseController> _logger;

        public PurchaseController(IPurchaseRepository purchaseRepository, UserManager<AppUser> usermanager, IEmployeeRepository employeeRepository, ILogger<PurchaseController> logger)
        {
            _purchaseRepository = purchaseRepository;
            _userManager = usermanager;
            _employeeRepository = employeeRepository;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var purchases = await _purchaseRepository.GetAll();
                return View(purchases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading purchases");
                throw;
            }
        }

        public async Task<IActionResult> Create(double unitPrice)
        {
            try
            {
                if (unitPrice <= 0)
                {
                    _logger.LogWarning("Invalid unit price provided: {UnitPrice}", unitPrice);
                    TempData["Error"] = "Invalid unit price. Please provide a valid price.";
                    return RedirectToAction("Index");
                }

                var userEmail = _userManager.GetUserName(User);
                if (string.IsNullOrEmpty(userEmail))
                {
                    _logger.LogWarning("User not authenticated for purchase creation");
                    return RedirectToAction("Login", "Account");
                }

                var employee = await _employeeRepository.GetEmployeeByEmail(userEmail);
                if (employee == null)
                {
                    _logger.LogWarning("Employee not found for email: {Email}", userEmail);
                    TempData["Error"] = "Employee information not found. Please contact administrator.";
                    return RedirectToAction("Index");
                }

                if (ModelState.IsValid)
                {
                    var purchase = new Purchase
                    {
                        PurchaseDate = DateTime.Now,
                        EmployeeId = employee.EmployeeId,
                        UnitPurchasePrice = unitPrice
                    };
                    
                    _purchaseRepository.Add(purchase);
                    _logger.LogInformation("Purchase created successfully by employee: {EmployeeId}, UnitPrice: {UnitPrice}", employee.EmployeeId, unitPrice);
                    return RedirectToAction("Create","PurchaseDetails",new { id = purchase.PurchaseId });
                }
                else
                {
                    _logger.LogWarning("Invalid model state for purchase creation");
                    TempData["Error"] = "Invalid purchase data. Please try again.";
                }
                
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating purchase with unit price: {UnitPrice}", unitPrice);
                TempData["Error"] = "An error occurred while creating the purchase. Please try again.";
                return RedirectToAction("Index");
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid purchase ID for delete: {Id}", id);
                    return NotFound();
                }

                var purchaseDetails = await _purchaseRepository.GetPurchaseById(id);
                if (purchaseDetails == null)
                {
                    _logger.LogWarning("Purchase not found for delete with ID: {Id}", id);
                    return NotFound();
                }
                
                return View(purchaseDetails);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while loading delete form for purchase ID: {Id}", id);
                throw;
            }
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            try
            {
                if (id <= 0)
                {
                    _logger.LogWarning("Invalid purchase ID for delete: {Id}", id);
                    return NotFound();
                }

                var purchaseDetails = await _purchaseRepository.GetPurchaseById(id);
                if (purchaseDetails == null)
                {
                    _logger.LogWarning("Purchase not found for delete with ID: {Id}", id);
                    return NotFound();
                }

                _purchaseRepository.Delete(purchaseDetails);
                _logger.LogInformation("Purchase deleted successfully: {PurchaseId}", id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting purchase with ID: {Id}", id);
                throw;
            }
        }

        //[HttpGet]
        //public FileStreamResult GetPDF()
        //{
        //    Document document = Document.Create(container =>
        //    {
        //        container.Page(page =>
        //        {
        //            page.Size(PageSizes.A4);
        //            page.Margin(2, Unit.Centimetre);
        //            page.PageColor(Colors.White);
        //            page.DefaultTextStyle(x => x.FontSize(20));
        //            page.Header()
        //                .Text("Hello PDF!")
        //                .SemiBold().FontSize(36).FontColor(Colors.Blue.Medium);

        //            page.Content()
        //                .PaddingVertical(1, Unit.Centimetre)
        //                .Column(x =>
        //            {
        //                x.Spacing(20);

        //                x.Item().Text(Placeholders.LoremIpsum());
        //                x.Item().Image(Placeholders.Image(200, 100));
        //            });

        //            page.Footer()
        //                .AlignCenter()
        //                .Text(x =>
        //            {
        //                x.Span("Page ");
        //                x.CurrentPageNumber();
        //            });
        //        });
        //    });
        //    byte[] pdfBytes = document.GeneratePdf();
        //    MemoryStream ms = new MemoryStream(pdfBytes);
        //    return new FileStreamResult(ms, "application/pdf");
        //}
    }
}
