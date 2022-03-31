using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Models
{
    public class AccountCreateViewModel : Account
    {
        public IEnumerable<SelectListItem> AccountTypes { get; set; }
    }
}
