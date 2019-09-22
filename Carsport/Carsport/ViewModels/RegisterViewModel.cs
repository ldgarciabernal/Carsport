namespace Carsport.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Plugin.Media;
    using Plugin.Media.Abstractions;
    using Services;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class RegisterViewModel : BaseViewModel
    {
        #region Atributtes
        private ApiService apiService;

        private bool isRunning;
        private bool isEnabled;
        private ImageSource imageSource;
        private MediaFile file;

        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private string passwordConfirm;
        #endregion

        #region Propierties
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

        public string PasswordConfirm
        {
            get { return this.passwordConfirm; }
            set { this.SetValue(ref this.passwordConfirm, value); }
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
        public RegisterViewModel()
        {
            this.apiService = new ApiService();
            this.IsEnabled = true;
            this.ImageSource = "no_profile";
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

            if (string.IsNullOrEmpty(this.EMail))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterEMailError,
                    Languages.AcceptDisplay);
                return;
            }

            if (!RegexHelper.IsValidEmailAddress(this.EMail))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterEMailError,
                    Languages.AcceptDisplay);
                return;
            }

            if (!RegexHelper.IsUniversitaryEmailAddress(this.EMail))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterEMailUcaError,
                    Languages.AcceptDisplay);
                return;
            }

            if (string.IsNullOrEmpty(this.Password))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterPasswordError,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.Password.Length < 6)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterPasswordError,
                    Languages.AcceptDisplay);
                return;
            }

            if (string.IsNullOrEmpty(this.PasswordConfirm))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.RegisterPasswordConfirmError,
                    Languages.AcceptDisplay);
                return;
            }


            if (!this.Password.Equals(this.PasswordConfirm))
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
                Password  = this.Password,
                ImageArray = imageArray,
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["UsersController"].ToString();
            var response = await this.apiService.PostAsync(url, prefix, controller, user);

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

            await Application.Current.MainPage.DisplayAlert(
                Languages.ConfirmRegister,
                Languages.RegisterConfirmation,
                Languages.AcceptDisplay);

            await Application.Current.MainPage.Navigation.PopAsync();
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
