using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZoDream.Shared.Http;

namespace ZoDream.LogTimer.Repositories
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
