using System.ComponentModel.DataAnnotations;

namespace WepPha2.Models
{
    public class Pharmacy
    {
        [Key]
        public int PharmacyId { get; set; }
        public string Address { get; set; }
        
    }
}
