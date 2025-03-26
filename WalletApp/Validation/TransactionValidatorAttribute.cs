using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using WalletApp.Extensions;

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
                    if (prop == null)
                    {
                        context.Result = new BadRequestObjectResult($"Property '{propertyName}' must exist.");
                        return;
                    }

                    var value = prop.GetValue(argument.Value);
                    Guid guid;

                    if (prop.PropertyType == typeof(Guid))
                    {
                        guid = (Guid)value;
                    }
                    else if (prop.PropertyType == typeof(string) && value is string str && str.TryToGuid(out var g))
                    {
                        guid = g;
                    }
                    else
                    {
                        context.Result = new BadRequestObjectResult($"{propertyName} must have a valid GUID value.");
                        return;
                    }

                    if (guid == Guid.Empty)
                    {
                        context.Result = new BadRequestObjectResult($"Property '{propertyName}' cannot be empty Guid.");
                        return;
                    }
                }
            }
        }
    }
}
