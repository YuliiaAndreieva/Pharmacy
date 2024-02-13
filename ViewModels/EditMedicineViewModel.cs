using WepPha2.Models;

namespace WepPha2.ViewModels
{
    public class EditMedicineViewModel
    {
        public int MedicineId { get; set; }     // Ідентифікатор лікарського засобу
        public string MedicineName { get; set; } // Назва лікарського засобу
        public string ActiveSubstance { get; set; } //діяюча речовина
        public string Contraindication { get; set; }    //протипоказання
        public int BatchNumber { get; set; }     // Номер партії
        public string UnitsInStock { get; set; }    // Кількість одиниць на складі
        public DateTime ExpiryDate { get; set; } // Дата закінчення терміну придатності
        public double UnitPrice { get; set; }       // Ціна за одиницю
        public bool IsOverdued { get; set; }     // Признак простроченості
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public int CategoryId { get; set; }
        public Supplier? Category { get; set; }
        public string? Description { get; set; }
        public string? UseMethod { get; set; }
        public IFormFile Image { get; set; }
        public int Quantity { get; set; }
        public string? URL { get; set; }
        
    }
}
