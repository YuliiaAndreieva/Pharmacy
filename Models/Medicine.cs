using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace WepPha2.Models
{
    public class Medicine
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

        [ForeignKey("Supplier")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Supplier? Category { get; set; }
        public string? Description { get; set; }
        public string? UseMethod { get; set; } 
        public string? Image { get; set; }
        public int Quantity { get; set; }   
        //public List<Medicine>? Items { get; set; }
    }
}
