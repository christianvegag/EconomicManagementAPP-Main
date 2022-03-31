using EconomicManagementAPP.Validations;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "The length of field {0} must be between {2} and {1}")]
        [FirstCapitalLetter]
        public string Name { get; set; }


        [Display(Name = "OperationType")]
        public OperationType OperationTypeId { get; set; }

        public int UserId { get; set; }

    }
}
