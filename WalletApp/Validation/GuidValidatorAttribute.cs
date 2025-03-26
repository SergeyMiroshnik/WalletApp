using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace WalletApp.Validation
{
    public class GuidValidateAttribute : ActionFilterAttribute
    {
        private readonly string _paramName;

        public GuidValidateAttribute(string paramName)
        {
            _paramName = paramName;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.TryGetValue(_paramName, out var value) || value == null)
            {
                context.Result = new BadRequestObjectResult($"{_paramName} is required.");
                return;
            }

            if (value is not Guid str || str == Guid.Empty)
            {
                context.Result = new BadRequestObjectResult($"{_paramName} must be a valid GUID.");
            }
        }
    }
}
