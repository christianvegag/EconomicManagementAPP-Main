using Microsoft.AspNetCore.Mvc.Rendering;

namespace EconomicManagementAPP.Models
{
    public class CategoriesViewModel : Category
    {
        public IEnumerable<SelectListItem> OperationTypes { get; set; }
    }
}
