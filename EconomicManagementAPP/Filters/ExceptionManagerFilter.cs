using Azure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

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
            context.Result = new BadRequestResult();
            context.ExceptionHandled = true;

            Console.WriteLine("Something wrong with the app " + _hostingEviroment.ApplicationName +
             " the exception of the type: " + context.Exception.GetType().FullName);

            //context.Result = new JsonResult("Something wrong with the app " + _hostingEviroment.ApplicationName +
            // " the exception of the type: " + context.Exception.GetType().FullName);

        }
    }
}
