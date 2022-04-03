using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Models
{
    public class AccountViewModel : Account
    {
        public IEnumerable<SelectListItem> AccountTypes { get; set; }
    }
}
