using System;
using Carsport.Common.Models;
using Carsport.Helpers;
using Carsport.Services;
using Carsport.ViewModels;
using Carsport.Views;
using Newtonsoft.Json;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Carsport
{
    public partial class App : Application
    {
        public static NavigationPage Navigator { get; internal set; }
        public static MasterPage Master { get; internal set; }

        public App()
        {
            InitializeComponent();

            var mainViewModel = MainViewModel.GetInstance();
            if (Settings.IsRemembered)
            {

                if (!string.IsNullOrEmpty(Settings.UserASP))
                {
                    mainViewModel.UserASP = JsonConvert.DeserializeObject<CustomUserASP>(Settings.UserASP);
                }

                this.LoadUserSttings();

                mainViewModel.DashBoard = new DashBoardViewModel();
                this.MainPage = new MasterPage();
            }
            else
            {
                mainViewModel.Login = new LoginViewModel();
                this.MainPage = new NavigationPage(new LoginPage());
            }
        }

        private async void LoadUserSttings()
        {
            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var apiService = new ApiService();
            var token = await apiService.GetToken(url, Settings.Username, Settings.Password);

            if (token == null || string.IsNullOrEmpty(token.AccessToken))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.GetTokenWrong,
                    Languages.AcceptDisplay);

                    Settings.AccessToken = string.Empty;
                    Settings.TokenType = string.Empty;
                    Settings.IsRemembered = false;

                    MainViewModel.GetInstance().Login = new LoginViewModel();
                    Application.Current.MainPage = new NavigationPage(new LoginPage());

                return;
            }

            Settings.TokenType = token.TokenType;
            Settings.AccessToken = token.AccessToken;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
