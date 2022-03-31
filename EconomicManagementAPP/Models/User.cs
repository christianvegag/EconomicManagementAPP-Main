using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Remote(action: "VerificaryUser", controller: "Users")]//Activamos la validacion se dispara peticion http hacia el back 
        public string Email { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [Remote(action: "VerificaryUser", controller: "Users")]//Activamos la validacion se dispara peticion http hacia el back 
        public string StandarEmail { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        public string Password { get; set; }
    }
}
