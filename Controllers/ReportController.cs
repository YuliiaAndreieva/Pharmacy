using Microsoft.AspNetCore.Mvc;
using WepPha2.Models;
using Microsoft.AspNetCore.Identity;
using QuestPDF.Fluent;
using QuestPDF.Infrastructure;
using QuestPDF.Previewer;
using WepPha2.ViewModels;
using System.ComponentModel;
using WepPha2.Interfaces;
using WepPha2.Models.DocumentModels;

namespace WepPha2.Controllers
{
    public class ReportController : Controller
    {
        private readonly IMedicineRepository _medicineRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IEmployeeRepository _employeeRepository;
        public ReportController(IMedicineRepository medicineRepository, 
            IPurchaseRepository purchaseRepository,
            IEmployeeRepository employeeRepository)
        {
            _medicineRepository = medicineRepository;
            _purchaseRepository = purchaseRepository;
            _employeeRepository = employeeRepository;
        }
        public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> CreateMedicineReport() 
        {
            var medicines = await _medicineRepository.GetAll();  
            var document = new InvoiceMedicineDocument(medicines);
            var pdfBytes = document.GeneratePdf();
            string nameOfFile = $"MedicineReport{DateTime.Now}.pdf";
            return File(pdfBytes, "application/pdf", nameOfFile);
        }
        public async Task<IActionResult> CreatePurchaseReport()
        {
            var purchases = await _purchaseRepository.GetAll();
            var document = new InvoisePurchaseDocument(purchases);
            var pdfBytes = document.GeneratePdf();
            string nameOfFile = $"PurchaseReport{DateTime.Now}.pdf";
            return File(pdfBytes, "application/pdf", nameOfFile);
        }
        public async Task<IActionResult> CreateEmployeeReport()
        {
            var employees = await _employeeRepository.GetAll();
            var document = new InvoiceEmployeeDocument(employees);
            var pdfBytes = document.GeneratePdf();
            string nameOfFile = $"EmployeeReport{DateTime.Now}.pdf";
            return File(pdfBytes, "application/pdf", nameOfFile);
        }
    }
}
