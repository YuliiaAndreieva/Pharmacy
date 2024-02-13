using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WepPha2.ViewModels
{
    public class EditProfileViewModel
    {
        public string Id { get; set; }  
        [Compare("Email", ErrorMessage = "Email and username do not match")]
        public string UserName { get; set; }
        [Required, EmailAddress]
        public string Email { get; set; }
        public string Phone { get; set; }

        //[Required]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }
    }
}
