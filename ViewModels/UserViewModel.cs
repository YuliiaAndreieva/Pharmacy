using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WepPha2.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        //[Required]
        //public string FirstName { get; set; }
        //[Required]
        //public string LastName { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, Phone]
        public string Phone { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public string Address { get; set; }

        [ForeignKey("Pharmacy")]
        public int? PharmacyId { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password do not match")]
        public string ConfirmPassword { get; set; }
    }
}
