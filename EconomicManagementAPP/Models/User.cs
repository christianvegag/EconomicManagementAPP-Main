using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EconomicManagementAPP.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string StandarEmail { get; set; }
        public string Password { get; set; }
    }
}
