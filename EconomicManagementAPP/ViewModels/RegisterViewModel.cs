using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "{0} is required")]
        [EmailAddress(ErrorMessage = "The field must be an valid email ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [DataType(DataType.Password)]
         public string Password { get; set; }    
    }
}
