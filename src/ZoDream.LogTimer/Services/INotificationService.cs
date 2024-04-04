using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ZoDream.LogTimer.Services
{
    public interface INotificationService
    {
        public void Push(string message);
        public void Success(string message);
        public void Info(string message);
        public void Error(string message);
        public void Warning(string message);
        public void Register(ICommand pushCommand);

        public void Loading(bool loading = true);

        public void RegisterLoading(ICommand loadCommand);
    }
}
