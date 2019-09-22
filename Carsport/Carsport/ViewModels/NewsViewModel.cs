using System;
using System.Collections.Generic;
using System.Text;

namespace Carsport.ViewModels
{
    public class NewsViewModel : BaseViewModel
    {
        #region Atributtes
        private NotificationItemViewModel notification;
        #endregion

        #region Properties
        public NotificationItemViewModel Notification
        {
            get { return this.notification; }
            set { this.SetValue(ref this.notification, value); }
        }
        #endregion

        public NewsViewModel(NotificationItemViewModel notificationItemViewModel)
        {
            this.notification = notificationItemViewModel;
        }
    }
}
