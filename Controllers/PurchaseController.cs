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
        //public const string CartSessionKey = "CartId";


        public PurchaseController(IPurchaseRepository purchaseRepository, UserManager<AppUser> usermanager, IEmployeeRepository employeeRepository)
        {
            _purchaseRepository = purchaseRepository;
            _userManager = usermanager;
            _employeeRepository = employeeRepository;
        }
        public async Task<IActionResult> Index()
        {
            var purchases = await _purchaseRepository.GetAll();
            return View(purchases);
        }
        public async Task<IActionResult> Create(double unitPrice)
        {
            var userEmail = _userManager.GetUserName(User);

            var employe = await _employeeRepository.GetEmployeeByEmail(userEmail);

            if (ModelState.IsValid)
            {
                var purchase = new Purchase
                {
                    PurchaseDate = DateTime.Now,
                    EmployeeId = employe.EmployeeId,
                    UnitPurchasePrice = unitPrice
                };
                _purchaseRepository.Add(purchase);
                return RedirectToAction("Create","PurchaseDetails",new { id = purchase.PurchaseId });
            }
            return View("Index");          
        }
        public async Task<IActionResult> Delete(int id)
        {
            var purchaseDetails = await _purchaseRepository.GetPurchaseById(id);
            if (purchaseDetails == null) return View("Error");
            return View(purchaseDetails);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePurchase(int id)
        {
            var purchaseDetails = await _purchaseRepository.GetPurchaseById(id);

            if (purchaseDetails == null)
            {
                return View("Error");
            }
            _purchaseRepository.Delete(purchaseDetails);
            return RedirectToAction("Index");
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
        //                {
        //                    x.Spacing(20);

        //                    x.Item().Text(Placeholders.LoremIpsum());
        //                    x.Item().Image(Placeholders.Image(200, 100));
        //                });

        //            page.Footer()
        //                .AlignCenter()
        //                .Text(x =>
        //                {
        //                    x.Span("Page ");
        //                    x.CurrentPageNumber();
        //                });
        //        });
        //    });
        //    byte[] pdfBytes = document.GeneratePdf();
        //    MemoryStream ms = new MemoryStream(pdfBytes);
        //    return new FileStreamResult(ms, "application/pdf");

        //}

    }
}
