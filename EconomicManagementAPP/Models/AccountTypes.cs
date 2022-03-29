using EconomicManagementAPP.Validations;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class AccountTypes
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(maximumLength: 50, MinimumLength = 3, ErrorMessage = "La longitud del campo {0} debe estar entre {2} y {1}")]
        [FirstCapitalLetter]
        [Remote(action: "VerificaryAccountType", controller: "AccountTypes")]
        public string Name { get; set; }
        public int UserId { get; set; }
        public int OrderAccount { get; set; }
    }
}
