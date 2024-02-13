using System.ComponentModel.DataAnnotations;

namespace WepPha2.ViewModels
{
    public class CreateUserEmployeeViewModel
    {
        [Required]
        public int EmployeId { get; set; }
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
