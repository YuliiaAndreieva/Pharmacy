using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WepPha2.Models;

namespace WepPha2.ViewModels
{
    public class CreatePurchaseViewModel
    {
        [Key]
        public int PurchaseId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
        //public ICollection<PurchaseDetails> Purchases { get; set; }
    }
}
