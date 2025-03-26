using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace WalletApp.Validation
{
    public class TransactionValidateAttribute : ActionFilterAttribute
    {
        private readonly string[] _propertyNames;

        public TransactionValidateAttribute(params string[] propertyNames)
        {
            _propertyNames = propertyNames;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value == null)
                {
                    context.Result = new BadRequestObjectResult($"Argument '{argument.Key}' is null.");
                    return;
                }

                foreach (var propertyName in _propertyNames)
                {
                    var prop = argument.Value.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                    if (prop == null || prop.PropertyType != typeof(Guid))
                    {
                        context.Result = new BadRequestObjectResult($"Property '{propertyName}' must exist and be of type Guid.");
                        return;
                    }

                    var value = (Guid)prop.GetValue(argument.Value);
                    if (value == Guid.Empty)
                    {
                        context.Result = new BadRequestObjectResult($"Property '{propertyName}' cannot be empty Guid.");
                        return;
                    }
                }
            }
        }
    }
}
