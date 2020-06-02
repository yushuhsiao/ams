using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace GLT
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Assembly, AllowMultiple = true)]
    public class ApiAttribute : Attribute, IActionFilter, IResultFilter, IExceptionFilter
    {
        public ApiAttribute()
        {
            
        }

        public ApiAttribute(string acl_name)
        {
            this.ACL_Name = acl_name;
        }

        public string ACL_Name { get; set; }

        void IActionFilter.OnActionExecuting(ActionExecutingContext context)
        {
        }

        void IActionFilter.OnActionExecuted(ActionExecutedContext context)
        {
        }

        void IResultFilter.OnResultExecuting(ResultExecutingContext context)
        {
            context.Result = ApiResult.FromActionResult(context.Result);
        }

        void IResultFilter.OnResultExecuted(ResultExecutedContext context)
        {
        }

        void IExceptionFilter.OnException(ExceptionContext context)
        {
            context.Result = ApiResult.FromException(context.Exception);
        }
    }
}
