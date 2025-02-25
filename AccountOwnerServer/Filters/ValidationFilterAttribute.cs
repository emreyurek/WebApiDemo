using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccountOwnerServer.Filters
{
    public class ValidationFilterAttribute : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var param = context.ActionArguments.FirstOrDefault();
            if (param.Value is null)
            {
                context.Result = new BadRequestObjectResult("Object is null");
                return;
            }

            if (!context.ModelState.IsValid)
            {
                context.Result = new UnprocessableEntityObjectResult(context.ModelState);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}
