using MiniWebAPI.Filter;

namespace MiniWebAPIDemo.Filter
{
    public class MyActionFilter1 : IMyActionFilter
    {
        public void Execute()
        {
            Console.WriteLine("Filter1 执行");
        }
    }
}
