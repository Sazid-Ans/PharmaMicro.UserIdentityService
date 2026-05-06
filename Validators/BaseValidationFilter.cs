using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PharmaMicro.UserIdentityService.Models;

namespace PharmaMicro.UserIdentityService.Validators
{
    public class BaseValidationFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var errors = context.ModelState
                    .Where(x => x.Value.Errors.Any())
                    .ToDictionary(x=>x.Key, x => x.Value.Errors.Select(x=>x.ErrorMessage).ToList());

                context.Result = new JsonResult(new { message = "one or more validation error occured", errors = errors }) { StatusCode = StatusCodes.Status400BadRequest };
                return;
            }

            await next();
        }
    }
}
