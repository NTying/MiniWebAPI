using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MiniWebAPI
{
    /// <summary>
    /// Used to locate the action
    /// </summary>
    public class ActionLocator
    {
        private Dictionary<string, MethodInfo> data = new Dictionary<string, MethodInfo>(StringComparer.OrdinalIgnoreCase);
        private static bool IsControllerType(Type type)
        {
            return type.IsClass && !type.IsAbstract && type.Name.EndsWith("Controller");
        }

        public ActionLocator(IServiceCollection services, Assembly assembly)
        {
            var controllerTypes = assembly.GetTypes().Where(t => IsControllerType(t));
            foreach (var controllerType in controllerTypes)
            {
                services.AddScoped(controllerType);
                // Get the controller name
                int index = controllerType.Name.LastIndexOf("Controller");
                string controllerName = controllerType.Name.Substring(0, index);
                // Get the controller method
                var methods = controllerType.GetMethods(BindingFlags.Instance | BindingFlags.Public);
                foreach (var method in methods)
                {
                    string actionName = method.Name;
                    data[$"{controllerName}.{actionName}"] = method;
                }
            }
        }

        public MethodInfo? LocateActionMethod(string controllerName, string actionName)
        {
            string key = $"{controllerName}.{actionName}";
            data.TryGetValue(key, out MethodInfo? method);
            return method;
        }
    }
}
