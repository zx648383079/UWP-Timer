using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Utils
{
    public static class Arr
    {
        public static bool Contain(object search, IEnumerable<object> items)
        {
            foreach (var item in items)
            {
                if (search == item)
                {
                    return true;
                }
            }
            return false;
        }

        public static Dictionary<string, string> ToMap(object data)
        {
            return data.GetType().GetProperties()
                .ToDictionary(q => Str.UnStudly(q.Name), q => q.GetValue(data).ToString());
        }
    }
}
