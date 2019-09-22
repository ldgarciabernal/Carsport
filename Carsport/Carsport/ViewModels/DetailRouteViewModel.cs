using Carsport.Common.Models;
using Carsport.Helpers;
using Carsport.Services;
using Carsport.Views;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace Carsport.ViewModels
{
    public class DetailRouteViewModel : BaseViewModel
    {
        #region Propierties
        public RouteItemViewModel RouteView
        {
            get { return this.routeView; }
            set { this.SetValue(ref this.routeView, value); }
        }

        public bool IsVisible
        {
            get { return this.isVisible; }
            set { this.SetValue(ref this.isVisible, value); }
        }

        public bool IsOwner
        {
            get { return this.isOwner; }
            set { this.SetValue(ref this.isOwner, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Attributes
        private ApiService apiService;

        private RouteItemViewModel routeView;
        private bool isVisible;
        private bool isOwner;
        private ImageSource imageSource;
        #endregion

        #region Constructors
        public DetailRouteViewModel(RouteItemViewModel routeView)
        {
            this.apiService = new ApiService();

            this.RouteView = routeView;
            var loggerUser = JsonConvert.DeserializeObject<CustomUserASP>(Settings.UserASP);

            this.IsOwner = false;
            this.IsVisible = true;

            this.ImageSource = (!string.IsNullOrEmpty(RouteView.ImagePath)) ? RouteView.ImageFullPath : "no_image";

            if (routeView.UserId == loggerUser.Id)
            {
                this.IsOwner = true;
                this.IsVisible = false;
            }
            
        }
        #endregion

        #region Commands
        public ICommand DeleteRouteCommand
        {
            get
            {
                return new RelayCommand(DeleteRoute);
            }
        }

        private async void DeleteRoute()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.ConfirmDeleteRoute,
                Languages.DeleteRouteConfirmation,
                Languages.Yes,
                Languages.No);
            if (!answer)
            {
                return;
            }

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["RoutesController"].ToString();
            var response = await this.apiService.DeleteAsync(url, prefix, controller, this.RouteView.RouteId, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var routesViewModel = RoutesViewModel.GetInstance();
            var deletedRoute = routesViewModel.MyRoutes.Where(p => p.RouteId == this.RouteView.RouteId).FirstOrDefault();
            if (deletedRoute != null)
            {
                routesViewModel.MyRoutes.Remove(deletedRoute);
            }

            routesViewModel.RefreshList();
            await App.Navigator.PopAsync();
        }


        public ICommand EditRouteCommand
        {
            get
            {
                return new RelayCommand(EditRoute);
            }
        }

        private async void EditRoute()
        {
            MainViewModel.GetInstance().EditRoute = new EditRouteViewModel(this.RouteView);
            await App.Navigator.PushAsync(new EditRoutePage());
        }

        public ICommand GoToChatListCommand
        {
            get
            {
                return new RelayCommand(GoToChatList);
            }
        }

        private async void GoToChatList()
        {
            MainViewModel.GetInstance().MyConversationsRoute = new MyConversationsRouteViewModel(this.RouteView);
            await App.Navigator.PushAsync(new MyConversationRoutePage());
        }

        public ICommand GoToConversationCommand
        {
            get
            {
                return new RelayCommand(GoToConversation);
            }
        }

        private async void GoToConversation()
        {
            MainViewModel.GetInstance().MyConversations = new MyConversationsViewModel(this.RouteView);
            await App.Navigator.PushAsync(new MyConversationPage());
        }
        #endregion
    }
}
