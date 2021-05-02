using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Repositories.Rest;

namespace UWP_Timer.Repositories
{
    public interface RequestInterceptor
    {
        RestClient Request(RestClient client);

        T Response<T>(object data);
    }
}
