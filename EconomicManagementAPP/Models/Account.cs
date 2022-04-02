using EconomicManagementAPP.Validations;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "The length of field {0} must be between {2} and {1}")]
        [FirstCapitalLetter]
        public string Name { get; set; }

        [Display(Name = "AccountType")]
        public int AccountTypeId { get; set; }

        [Required(ErrorMessage = "Only numbers, for decimal write with ' . ' ")]
        public decimal Balance { get; set; }

        [StringLength(maximumLength: 1000)]
        public string Description { get; set; }

        public string AccountType { get; set; }

    }
}
