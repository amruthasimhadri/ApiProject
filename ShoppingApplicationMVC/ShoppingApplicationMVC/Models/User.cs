using System.ComponentModel.DataAnnotations;

namespace ShoppingApplicationMVC.Models
{
    public class User
    {
        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress(ErrorMessage = "Email Format is incorrect")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
