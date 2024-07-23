using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace dotnetApiCourse.Filters
{
    public class ActionFilter : IActionFilter
    {
        private readonly ILogger<ActionFilter> logger;

        public ActionFilter(ILogger<ActionFilter> logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Action executing...");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Action executed!!!");
        }
    }
}