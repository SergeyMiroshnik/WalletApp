using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;

namespace WalletApp.Validation
{
    public class EnumValidateAttribute : ActionFilterAttribute
    {
        private readonly string _propertyName;
        private readonly Type _enumType;

        public EnumValidateAttribute(string paramName, Type enumType)
        {
            _propertyName = paramName;
            _enumType = enumType;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            foreach (var argument in context.ActionArguments.Values)
            {
                if (argument == null) continue;

                var prop = argument.GetType().GetProperty(_propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null || prop.PropertyType != typeof(string))
                {
                    context.Result = new BadRequestObjectResult($"Property '{_propertyName}' must be of type string.");
                    return;
                }

                var rawValue = prop.GetValue(argument) as string;
                if (string.IsNullOrWhiteSpace(rawValue))
                {
                    context.Result = new BadRequestObjectResult($"Property '{_propertyName}' is required.");
                    return;
                }

                if (!Enum.TryParse(_enumType, rawValue, ignoreCase: true, out _))
                {
                    var allowed = string.Join(", ", Enum.GetNames(_enumType));
                    context.Result = new BadRequestObjectResult($"Property '{_propertyName}' must be a valid value of enum '{_enumType.Name}'. Allowed values: {allowed}.");
                    return;
                }
            }
        }
    }
}
