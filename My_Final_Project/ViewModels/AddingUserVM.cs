using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace My_Final_Project.ViewModels
{
    public class AddingUserVM
    {
        [DisplayName("User Name")]
        [Required]
        public string UserName { get; set; }
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [DisplayName("Confirm Email")]
        [Compare("Email")]
        public string ConfirmEmail { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password")]
        [DisplayName("Confirm Password")]
        public string ConfirmPassword { get; set; }
        public string Address { get; set; }
        [DisplayName("Phone Number")]
        [RegularExpression("^01[0125][0-9]{8}$", ErrorMessage = "Enter Valid Egyptian Number")]
        public string Phone { get; set; }
        public int Gender { get; set; }
        [Required(ErrorMessage = "Age is required")]
        [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
        public int Age { get; set; }
        public string roleId { get; set; }
    }
}
