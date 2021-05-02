using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UWP_Timer.Repositories.Rest;

namespace UWP_Timer.Repositories
{
    public abstract class RestRepositoryBase
    {
        public RestRepositoryBase(RestRequest client)
        {
            Client = client;
        }
        private readonly RestRequest Client;
    }
}
