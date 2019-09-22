namespace Carsport.ViewModels
{
    using Helpers;
    using Services;
    using Views;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using System.Windows.Input;
    using Xamarin.Forms;
    using System;
    using Newtonsoft.Json;

    public class ProfileViewModel : BaseViewModel
    {

        #region Attributes
        private ApiService apiService;

        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;

        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private string newPassword;
        private string passwordConfirm;
        private string newPasswordConfirm;
        #endregion

        #region Properties
        public string FirstName
        {
            get { return this.firstName; }
            set { this.SetValue(ref this.firstName, value); }
        }

        public string LastName
        {
            get { return this.lastName; }
            set { this.SetValue(ref this.lastName, value); }
        }

        public string EMail
        {
            get { return this.email; }
            set { this.SetValue(ref this.email, value); }
        }

        public string Password
        {
            get { return this.password; }
            set { this.SetValue(ref this.password, value); }
        }

        public string NewPassword
        {
            get { return this.newPassword; }
            set { this.SetValue(ref this.newPassword, value); }
        }

        public string PasswordConfirm
        {
            get { return this.passwordConfirm; }
            set { this.SetValue(ref this.passwordConfirm, value); }
        }

        public string NewPasswordConfirm
        {
            get { return this.newPasswordConfirm; }
            set { this.SetValue(ref this.newPasswordConfirm, value); }
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

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Constructors
        public ProfileViewModel(UserRequest user)
        {
            this.apiService = new ApiService();

            this.IsEnabled = true;
            var apiUri = Application.Current.Resources["APIMovilidad"].ToString();
            this.ImageSource = (!string.IsNullOrEmpty(user.ImagePath)) ? $"{apiUri}{user.ImagePath.Substring(1)}" : "no_profile";

            this.FirstName = user.FirstName;
            this.LastName = user.LastName;
            this.Password = user.Password;
            this.EMail = user.EMail;
            this.PasswordConfirm = user.Password;
        }
        #endregion

        #region Commands
        public ICommand ChangeImageCommand
        {
            get
            {
                return new RelayCommand(ChangeImage);
            }
        }

        private async void ChangeImage()
        {
            await CrossMedia.Current.Initialize();

            var source = await Application.Current.MainPage.DisplayActionSheet(
                Languages.ImageSource,
                Languages.Cancel,
                null,
                Languages.FromGallery,
                Languages.NewPicture);

            if (source == Languages.Cancel)
            {
                this.file = null;
                return;
            }

            if (source == Languages.NewPicture)
            {
                this.file = await CrossMedia.Current.TakePhotoAsync(
                    new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "test.jpg",
                        PhotoSize = PhotoSize.Small,
                    }
                );
            }
            else
            {
                this.file = await CrossMedia.Current.PickPhotoAsync();
            }

            if (this.file != null)
            {
                this.ImageSource = ImageSource.FromStream(() =>
                {
                    var stream = file.GetStream();
                    return stream;
                });
            }

        }

        public ICommand SaveCommand
        {
            get
            {
                return new RelayCommand(Save);
            }
        }

        private async void Save()
        {
            if (string.IsNullOrEmpty(this.FirstName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterFirstNameError,
                    Languages.AcceptDisplay);
                return;
            }

            if (string.IsNullOrEmpty(this.LastName))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterLastNameError,
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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var user = new UserRequest
            {
                FirstName = this.FirstName,
                LastName = this.LastName,
                EMail = this.EMail,
                Password = this.Password,
                ImageArray = imageArray,
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["UsersController"].ToString();
            var response = await this.apiService.PutUserAsync(url, prefix, $"{ controller}/Put", user, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            this.IsRunning = false;
            this.IsEnabled = true;

            var prefixUser = Application.Current.Resources["APIprefix"].ToString();
            var controllerUser = Application.Current.Resources["UsersController"].ToString();
            var responseUser = await this.apiService.GetUser(url, prefixUser, $"{controllerUser}/GetUser", this.EMail, Settings.TokenType, Settings.AccessToken);
            if (responseUser.IsSuccess)
            {
                var userASP = (CustomUserASP)responseUser.Result;
                MainViewModel.GetInstance().UserASP = userASP;
                Settings.UserASP = JsonConvert.SerializeObject(userASP);
            }

            await Application.Current.MainPage.DisplayAlert(
                    Languages.UpdateProfileTitle,
                    Languages.UpdateProfileMessage,
                    Languages.AcceptDisplay);

            await App.Navigator.PopAsync();
            #endregion
        }

        public ICommand ChangePasswordCommand
        {
            get
            {
                return new RelayCommand(ChangePassword);
            }
        }

        private async void ChangePassword()
        {
            if (string.IsNullOrEmpty(this.NewPassword))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterPasswordError,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.NewPassword.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterPasswordError,
                    Languages.AcceptDisplay);
                return;
            }

            if (string.IsNullOrEmpty(this.NewPasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterPasswordConfirmError,
                    Languages.AcceptDisplay);
                return;
            }

            if (!this.NewPassword.Equals(this.NewPasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterPasswordsNoMatch,
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

            var passRequest = new PasswordRequest()
            {
                Email = this.EMail,
                Password = this.NewPassword,
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["UsersController"].ToString();
            var response = await this.apiService.ChangePasswordAsync(url, prefix, controller, passRequest, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }
            else
            {
                this.IsRunning = false;
                this.IsEnabled = true;

                await Application.Current.MainPage.DisplayAlert(
                   Languages.ChangePassTitle,
                   Languages.SuccessfulChangePassword,
                   Languages.AcceptDisplay);

                MainViewModel.GetInstance().DashBoard = new DashBoardViewModel();
                Application.Current.MainPage = new MasterPage();
            }
        }

        public ICommand RemoveAccountCommand
        {
            get
            {
                return new RelayCommand(RemoveAccount);
            }
        }

        private async void RemoveAccount()
        {
            var answer = await Application.Current.MainPage.DisplayAlert(
                Languages.ConfirmDeleteAccount,
                Languages.DeleteAccountConfirmation,
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
            var controller = Application.Current.Resources["UsersController"].ToString();
            var response = await this.apiService.GetUser(url, prefix, controller, this.EMail, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            Settings.AccessToken = string.Empty;
            Settings.TokenType = string.Empty;
            Settings.IsRemembered = false;
            Settings.Username = string.Empty;
            Settings.Password = string.Empty;

            MainViewModel.GetInstance().Login = new LoginViewModel();
            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}
