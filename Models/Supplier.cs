using System.ComponentModel.DataAnnotations;

namespace WepPha2.Models
{
    public class Supplier
    {
        public int SupplierId { get; set; }

        [Required]
        public string CompanyName { get; set; }

        [Required, Phone]
        public string MobilePhone { get; set; }

        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public string Address { get; set; } 
    }
}
