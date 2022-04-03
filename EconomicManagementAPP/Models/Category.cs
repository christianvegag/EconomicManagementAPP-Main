using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        public int OperationTypeId { get; set; }
        public string OperationType { get; set; }
        public int UserId { get; set; }

    }
}
