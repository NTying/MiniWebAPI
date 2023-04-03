using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using MiniWebAPI.Util;

namespace MiniWebAPI
{
    /// <summary>
    /// Work with the static file in the website
    /// </summary>
    public class MyStaticFilesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _hostEnv;

        public MyStaticFilesMiddleware(IWebHostEnvironment hostEnv, RequestDelegate next)
        {
            this._hostEnv = hostEnv;
            this._next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Get file path from request path
            string path = context.Request.Path.Value ?? "";
            // Access the static file in wwwroot directory
            var file = _hostEnv.WebRootFileProvider.GetFileInfo(path);
            if (!file.Exists || !ContentTypeHelper.IsValid(file))
            {
                await _next(context);
                return;
            }
            context.Response.ContentType = ContentTypeHelper.GetContentType(file);
            context.Response.StatusCode = 200;
            using var stream = file.CreateReadStream();
            byte[] bytes = await ToArrayAsync(stream);
            await context.Response.Body.WriteAsync(bytes);
        }

        /// <summary>
        /// Transform the file stream into byte array
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        private static async Task<byte[]> ToArrayAsync(Stream stream)
        {
            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            byte[] bytes = memoryStream.ToArray();
            return bytes;
        }
    }
}
