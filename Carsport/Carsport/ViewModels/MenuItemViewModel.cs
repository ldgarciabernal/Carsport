namespace Carsport.ViewModels
{
    using Carsport.Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class MenuItemViewModel
    {
        #region Propierties

        public string Icon { get; set; }

        public string Title { get; set; }

        public string PageName { get; set; }
        #endregion

        #region Commands
        public ICommand GotoCommand
        {
            get
            {
                return new RelayCommand(GoToAsync);
            }
        }

        private async void GoToAsync()
        {
            App.Master.IsPresented = false;
            var mainViewModel = MainViewModel.GetInstance();

            if (this.PageName == "NotificationPage")
            {
                mainViewModel.Notification = new NotificationViewModel();
                await App.Navigator.PushAsync(new NotificationPage());
            }
            else if (this.PageName == "ProfilePage")
            {
                var userRequest = new UserRequest()
                {
                    EMail = mainViewModel.UserASP.Email,
                    FirstName = (!string.IsNullOrEmpty(mainViewModel.getNameClaim())) ? mainViewModel.getNameClaim() : string.Empty,
                    LastName = (!string.IsNullOrEmpty(mainViewModel.getSurnameClaim())) ? mainViewModel.getSurnameClaim() : string.Empty,
                    ImagePath = (!string.IsNullOrEmpty(mainViewModel.getImageFullPathClaim())) ? mainViewModel.getImageFullPathClaim() : string.Empty,
                    Password = mainViewModel.UserASP.PasswordHash,
                };

                mainViewModel.Profile = new ProfileViewModel(userRequest);
                await App.Navigator.PushAsync(new ProfilePage());
            }
            else if (this.PageName == "MyRoutesPage")
            {
                mainViewModel.MyRoutes = new MyRoutesViewModel();
                await App.Navigator.PushAsync(new MyRoutesPage());
            }
            else if (this.PageName == "CarPage")
            {
                mainViewModel.Routes = new RoutesViewModel();
                await App.Navigator.PushAsync(new RoutesPage());
            }
            else if (this.PageName == "MyChatsPage")
            {
                mainViewModel.AllMyConversations = new AllMyConversationsViewModel();
                await App.Navigator.PushAsync(new AllMyConversationsPage());
            }
            else if (this.PageName == "AddRoutePage")
            {
                mainViewModel.AddRoute = new AddRouteViewModel();
                await App.Navigator.PushAsync(new AddRoutePage());
            }
            else if (this.PageName == "BusPage")
            {
                mainViewModel.Bus = new BusViewModel();
                await App.Navigator.PushAsync(new BusPage());
            }
            else if (this.PageName == "TrainPage")
            {
                mainViewModel.Train = new TrainViewModel();
                await App.Navigator.PushAsync(new TrainPage());
            }
            else if (this.PageName == "ByciclePage")
            {
                mainViewModel.Bycicle = new BycicleViewModel();
                await App.Navigator.PushAsync(new BikePage());
            }
            else if (this.PageName == "ConfigurationPage")
            {
                mainViewModel.Configuration = new ConfigurationViewModel();
                await App.Navigator.PushAsync(new ConfigurationPage());
            }
            else if (this.PageName == "LogOutPage")
            {
                Settings.AccessToken = string.Empty;
                Settings.TokenType = string.Empty;
                Settings.IsRemembered = false;
                Settings.Username = string.Empty;
                Settings.Password = string.Empty;

                mainViewModel.Login = new LoginViewModel();
                Application.Current.MainPage = new NavigationPage(new LoginPage());
            }
        } 
        #endregion
    }
}
