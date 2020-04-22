using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Repositories.Rest
{
    public class HttpException
    {
        public int Code { get; set; } = 0;

        public object Errors { get; set; }

        public string Description { get; set; }

        public string Message { get; set; }

        public HttpException()
        {

        }

        public HttpException(string message)
        {
            Message = message;
        }

        public HttpException(int code, string message)
        {
            Code = code;
            Message = message;
        }


    }
}
