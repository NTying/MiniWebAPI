using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MiniWebAPI.Util
{
    /// <summary>
    /// Binding the request packets to the controller action
    /// </summary>
    public class BindingHelper
    {
        public static object?[] GetParameterValues (HttpContext httpContext, MethodInfo actionMethod)
        {
            var parameters = actionMethod.GetParameters ();
            if (parameters.Length <= 0)
            {
                return new object?[0];
            }
            else if (parameters.Length > 1)
            {
                throw new Exception("The Action can only have zero or one parameter");
            }

            if (parameters.Single().ParameterType == typeof(HttpContext))
            {
                return new object?[] { httpContext };
            }
            if (!httpContext.Request.HasJsonContentType())
            {
                throw new Exception("If the Action have a parameter, its ContentType has to be HttpContext or application/json");
            }
            // Sets the action parameter to null if the request packets is null;
            if (httpContext.Request.ContentLength == 0)
            {
                return new object?[1] { null };
            }

            // Get the parameter type and deserialize the request body stream
            var reqStream = httpContext.Request.BodyReader.AsStream();                       
            Type paramType = parameters.Single().ParameterType;
            object? paramValue = JsonSerializer.Deserialize(reqStream, paramType);
            return new object?[1] { paramValue };
        }
    }
}
