using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WepPha2.Models
{
    public class PurchaseDetails
    {
        [Key]
        public int PurchaseDetailId { get; set; }
        [ForeignKey("Medicine")]
        public int PurchasedMedicineId { get; set; }
        public Medicine? PurchasedMedicine { get; set; }
        public int Count { get; set; }

        [ForeignKey("Purchase")]
        public int PurchaseId { get; set; }

    }
}
