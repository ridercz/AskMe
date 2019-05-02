using System;
using System.Threading.Tasks;
using DotVVM.Framework.Hosting;
using DotVVM.Framework.Runtime.Filters;

namespace Altairis.AskMe.Web.DotVVM.Filters {
    public class ErrorHandlingFilter : ExceptionFilterAttribute {

        protected override async Task OnPageExceptionAsync(IDotvvmRequestContext context, Exception exception) {
            if (exception is ObjectNotFoundException) {
                context.HttpContext.Response.StatusCode = 404;
                context.InterruptRequest();
            }
            await base.OnPageExceptionAsync(context, exception);
        }
    }
}
