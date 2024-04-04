using System.Windows.Input;

namespace ZoDream.LogTimer.Services
{
    public class NotificationService : INotificationService
    {
        private ICommand? _pushCommand;
        private ICommand? _loadCommand;
        public void Push(string message)
        {
            _pushCommand?.Execute(message);
        }

        public void Success(string message)
        {
            Push(message);
        }
        public void Info(string message)
        {
            Push(message);
        }
        public void Error(string message)
        {
            Push(message);
        }
        public void Warning(string message)
        {
            Push(message);
        }

        public void Loading(bool loading = true)
        {
            _loadCommand?.Execute(loading);
        }

        public void Register(ICommand pushCommand)
        {
            _pushCommand = pushCommand;
        }

        public void RegisterLoading(ICommand loadCommand)
        {
            _loadCommand = loadCommand;
        }
    }
}
