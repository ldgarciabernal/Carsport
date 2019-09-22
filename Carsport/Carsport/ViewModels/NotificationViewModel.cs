namespace Carsport.ViewModels
{
    using Helpers;
    using Common.Models;
    using Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using Xamarin.Forms;
    using System.Linq;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;

    public class NotificationViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private string filter;
        private bool isRefreshing;

        private ObservableCollection<NotificationItemViewModel> notifications;
        #endregion

        #region Propierties
        public string Filter
        {
            get { return this.filter; }
            set {
                this.filter = value;
                this.RefreshList();
            }
        }

        public List<Notification> MyNotifications { get; set; }

        public ObservableCollection<NotificationItemViewModel> Notifications
        {
            get { return this.notifications; }
            set { this.SetValue(ref this.notifications, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public NotificationViewModel()
        {
            this.apiService = new ApiService();
            this.LoadNotifications();
        }
        #endregion

        #region Methods
        private async void LoadNotifications()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["NotificationsController"].ToString();
            var response = await this.apiService.GetListAsync<Notification>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            this.MyNotifications = (List<Notification>)response.Result;
            this.RefreshList();
            this.IsRefreshing = false;
        }

        private void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListNotificationItemViewModel = this.MyNotifications.Select(c => new NotificationItemViewModel
                {
                    NotificationId = c.NotificationId,
                    Title = c.Title,
                    Body = c.Body,
                    ImagePath = c.ImagePath,
                    IsAvailable = c.IsAvailable,
                    PublishedOn = c.PublishedOn,
                    UserId = c.UserId
                });

                this.Notifications = new ObservableCollection<NotificationItemViewModel>(
                    myListNotificationItemViewModel.OrderByDescending(c => c.PublishedOn));
            }
            else
            {
                var myListNotificationItemViewModel = this.MyNotifications.Select(c => new NotificationItemViewModel
                {
                    NotificationId = c.NotificationId,
                    Title = c.Title,
                    Body = c.Body,
                    ImagePath = c.ImagePath,
                    IsAvailable = c.IsAvailable,
                    PublishedOn = c.PublishedOn,
                    UserId = c.UserId,
                }).Where(c => c.Title.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Notifications = new ObservableCollection<NotificationItemViewModel>(
                    myListNotificationItemViewModel.OrderByDescending(c => c.PublishedOn));
            }
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }
        #endregion
    }
}
