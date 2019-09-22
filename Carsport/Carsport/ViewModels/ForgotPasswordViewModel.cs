namespace Carsport.ViewModels
{
    using Carsport.Helpers;
    using Carsport.Views;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class ForgotPasswordViewModel : BaseViewModel
    {
        #region Propierties
        private ApiService apiService;
        private string userEmail;
        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Propierties
        public string UserEmail
        {
            get { return this.userEmail; }
            set { this.SetValue(ref this.userEmail, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }
        #endregion

        #region Constructors
        public ForgotPasswordViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Commands
        public ICommand ResetCommand
        {
            get
            {
                return new RelayCommand(ResetPassword);
            }
        }

        private async void ResetPassword()
        {
            if (string.IsNullOrEmpty(this.UserEmail))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.EmailValidation,
                    Languages.AcceptDisplay);
                return;
            }

            this.IsRunning = true;
            this.IsEnabled = false;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["UsersController"].ToString();
            var response = await this.apiService.PostAsync(url, prefix, $"{controller}/GetUser", this.UserEmail);

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        public ICommand GoToLoginCommand
        {
            get
            {
                return new RelayCommand(GoToLogin);
            }
        }

        private async void GoToLogin()
        {
            MainViewModel.GetInstance().Login = new LoginViewModel();
            await Application.Current.MainPage.Navigation.PopAsync();
        }
        #endregion
    }
}
