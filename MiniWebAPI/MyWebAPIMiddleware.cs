using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using MiniWebAPI.Filter;
using MiniWebAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniWebAPI
{
    /** Web API Core class
     * Some simple contract with the MVC Controller
     *1)Controller class name has to end with "Controller", 
     *  the class name without "Controller" is the controller name;
     *2)All public methods in controller class are controller method;
     *3)The request path has to be "/controller_name/action_name";
     *4)The controller methods can't be overloaded, and can't bind any specify Http verbs by using attribute;
     *5)The controller methods can have one or no parameter. 
     *6)The request body will be deserilized as JSON, unless the type of method parameter is HttpContext
     *7)The return value of the controller method will be serilized to JSON string, and delivery to the response
     *  body. The return value doesn't support the IActionResult type
     */
    public class MyWebAPIMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ActionLocator _actionLocator;

        public MyWebAPIMiddleware(RequestDelegate next, ActionLocator actionLocator)
        {
            _next = next;
            _actionLocator = actionLocator;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            (bool ok, string? controllerName, string? actionName) = PathParser.Parse(context.Request.Path);
            if (!ok)
            {
                await _next(context);
                return;
            }

            // Load the MethodInfo object of the Controller Action by using the resolved controller and action name
            var actionMethod = _actionLocator.LocateActionMethod(controllerName!, actionName!);
            if (actionMethod == null)
            {
                await _next(context);
                return;
            }

            Type controllerType = actionMethod.DeclaringType!;
            object controllerInstance = serviceProvider.GetRequiredService(controllerType);
            var paramValues = BindingHelper.GetParameterValues(context, actionMethod);
            // Execute the filter before action executing
            foreach (var filter in ActionFilters.filters)
            {
                filter.Execute();
            }
            var result = actionMethod.Invoke(controllerInstance, paramValues);

            // Limit the return value type as normal type
            string jsonStr = JsonSerializer.Serialize(result);
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(jsonStr);
        }
    }
}
