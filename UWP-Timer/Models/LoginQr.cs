using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UWP_Timer.Models
{
    /// <summary>
    /// 登录二维码
    /// </summary>
    public class LoginQr
    {
        public int Status { get; set; }

        public int UserId { get; set; }
    }

    public class QrData
    {
        public string Token { get; set; }

        public string Qr { get; set; }
    }
}
