using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniWebAPI.Filter
{
    public class ActionFilters
    {
        public static List<IMyActionFilter> filters = new List<IMyActionFilter>();
    }

    public interface IMyActionFilter
    {
        void Execute();
    }
}
