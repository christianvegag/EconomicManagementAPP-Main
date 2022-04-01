using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class OperationType
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "The length of field {0} must be between {2} and {1}")]
        [FirstCapitalLetter]
        [Remote(action: "VerificaryOperationType", controller: "OperationTypes")]
        public string Description { get; set; }
    }
}
