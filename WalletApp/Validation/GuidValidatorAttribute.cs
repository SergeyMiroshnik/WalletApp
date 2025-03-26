using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WalletApp.Extensions;

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

            Guid guid;

            if (value is Guid g)
            {
                guid = g;
            }
            else if (value is string str && str.TryToGuid(out var parsed))
            {
                guid = parsed;
            }
            else
            {
                context.Result = new BadRequestObjectResult($"{_paramName} must be a valid GUID.");
                return;
            }

            if (guid == Guid.Empty)
            {
                context.Result = new BadRequestObjectResult($"{_paramName} must be a valid GUID.");
            }
        }
    }
}
