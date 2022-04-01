using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EconomicManagementAPP.Filters
{
    public class ExceptionManagerFilter : IExceptionFilter
    {
        private readonly IWebHostEnvironment _hostingEviroment;
        private readonly IModelMetadataProvider _modelMetadataProvider;


        public ExceptionManagerFilter (IWebHostEnvironment hostingEviroment, IModelMetadataProvider modelMetadataProvider)
        {
            this._hostingEviroment = hostingEviroment;
            this._modelMetadataProvider = modelMetadataProvider;  
        }
        public void OnException(ExceptionContext context)
        {
                        
            context.Result = new JsonResult("Something wrong with the app " + _hostingEviroment.ApplicationName +
             " the exception of the type: " + context.Exception.GetType().FullName);

            context.Result = new RedirectResult("/Home/NotFound");
        }
    }
}
