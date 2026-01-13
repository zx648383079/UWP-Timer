namespace ZoDream.LogTimer.Repositories.Models
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
