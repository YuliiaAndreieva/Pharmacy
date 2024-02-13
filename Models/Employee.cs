using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WepPha2.Models
{
    public class Employee
    {
        [Key]
        public int EmployeeId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public string Address { get; set; }

        [ForeignKey("Pharmacy")]
        public int PharmacyId { get; set; }
        public Pharmacy? Pharmacy { get; set; }
    }
}
