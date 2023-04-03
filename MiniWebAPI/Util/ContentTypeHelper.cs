using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWebAPI.Util
{
    /// <summary>
    /// Address about the file ContentType
    /// </summary>
    public class ContentTypeHelper
    {
        private static readonly Dictionary<string, string> data
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        static ContentTypeHelper()
        {
            data[".html"] = "text/html; charset=utf-8";
            data[".htm"] = "text/html; charset=utf-8";
            data[".txt"] = "text/plain; charset=utf-8";
            data[".jpg"] = "image/jpeg";
            data[".jpeg"] = "image/jpeg";
            data[".png"] = "image/png";
            data[".js"] = "application/x-javascript; charset=utf-8";
            data[".css"] = "text/css";
        }

        /// <summary>
        /// Determine if the file is a regular static file
        /// </summary>
        /// <param name="file">FileInfo</param>
        /// <returns></returns>
        public static bool IsValid(IFileInfo file)
        {
            if (file.IsDirectory)
            {
                return false;
            }
            string extension = Path.GetExtension(file.Name);
            return data.ContainsKey(extension);
        }

        /// <summary>
        /// Get the file ContentType
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetContentType(IFileInfo file)
        {
            string extension = Path.GetExtension(file.Name);
            return data[extension];
        }
    }
}
