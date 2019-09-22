namespace Carsport.ViewModels
{
    using Views;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using System.Windows.Input;

    public class NotificationItemViewModel : Notification
    {
        #region Commands
        public ICommand GotoNotificationCommand
        {
            get
            {
                return new RelayCommand(GotoNotification);
            }
        }

        private async void GotoNotification()
        {
            MainViewModel.GetInstance().News = new NewsViewModel(this);
            await App.Navigator.PushAsync(new NewsPage());
        }
        #endregion
    }
}
