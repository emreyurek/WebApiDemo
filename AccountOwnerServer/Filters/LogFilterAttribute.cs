using Contracts;
using Entities.LogModel;
using Microsoft.AspNetCore.Mvc.Filters;

namespace AccountOwnerServer.Filters
{
    public class LogFilterAttribute : IActionFilter
    {
        private ILoggerManager _logger;

        public LogFilterAttribute(ILoggerManager logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            _logger.LogInfo(Log("OnActionExecuting", context.RouteData));
        }
        private string Log(string modelName, RouteData routeData)
        {
            var logDetails = new LogDetails
            {
                ModelName = modelName,
                Controller = routeData.Values["controller"],
                Action = routeData.Values["action"]
            };

            if (routeData.Values.ContainsKey("id"))
            {
                logDetails.Id = routeData.Values["id"];
            }

            return logDetails.ToString();
        }
        public void OnActionExecuted(ActionExecutedContext context) { }

    }
}
