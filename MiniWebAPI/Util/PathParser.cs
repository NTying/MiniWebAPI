using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MiniWebAPI.Util
{
    /// <summary>
    /// Used to resolve the request path
    /// Get the Controller name and Action name from request path   
    /// </summary>
    public class PathParser
    {
        public static (bool ok, string? controllerName, string? actionName) Parse(PathString pathString)
        {
            string? path = pathString.Value;
            if (path == null)
            {
                return (false, null, null);
            }

            // Get the Controller and Action name
            var match = Regex.Match(path, "/([a-zA-Z0-9]+)/([a-zA-Z0-9])");
            if (!match.Success)
            {
                return (false, null, null);
            }
            string controllerName = match.Groups[1].Value;
            string actionName = match.Groups[2].Value;
            return (true, controllerName, actionName);
        }
    }
}
