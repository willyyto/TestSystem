
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TestSystem.Core.Dtos;

namespace TestSystem.Filters;

/// <summary>
/// Action filter to validate model state and return consistent error responses
/// </summary>
public class ValidateModelAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!context.ModelState.IsValid)
        {
            var errors = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors.Select(e => $"{x.Key}: {e.ErrorMessage}"))
                .ToList();

            var response = new ApiResponseDto<object>(
                Success: false,
                Data: null,
                Message: "Validation failed",
                Errors: errors,
                StatusCode: 400
            );

            context.Result = new BadRequestObjectResult(response);
        }

        base.OnActionExecuting(context);
    }
}