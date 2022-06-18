using Blog6.Configurations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Blog6.Attributes
{
  [AttributeUsage(validOn: AttributeTargets.Method)]
  public class ApiKeyAttribute : Attribute, IAsyncActionFilter
  {
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      if (context.HttpContext.Request.Headers.TryGetValue(Configuration.ApiKeyName, out var apiKeyValue) && Configuration.ApiKey.Equals(apiKeyValue))
      {
        await next();
      }

      context.Result = new ContentResult()
      {
        StatusCode = 401
      };
    }
  }
}