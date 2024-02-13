using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WepPha2.Models
{
    public class Purchase
    {
        [Key]
        public int PurchaseId { get; set; }

        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }
        public Employee? Employee { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }
        public double UnitPurchasePrice { get; set; }
        public ICollection<PurchaseDetails> Purchases { get; set; }
    }
}
