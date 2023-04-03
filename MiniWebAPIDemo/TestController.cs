using Microsoft.Extensions.Caching.Memory;

namespace MiniWebAPIDemo
{
    public class TestController
    {
        private IMemoryCache _memoryCache;

        public TestController(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        public object[] Get2(HttpContext context)
        {
            DateTime now = _memoryCache.GetOrCreate("Now", e => DateTime.Now);
            string name = context.Request.Query["name"];
            return new object[] { $"Hello {name},{now}" };
        }
    }
}
