namespace Carsport.ViewModels
{
    using Carsport.Helpers;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System;
    using Plugin.Media.Abstractions;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Plugin.Media;

    public class AddRouteViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;

        private ObservableCollection<City> originCities;
        private City originCity;
        private ObservableCollection<City> destinyCities;
        private City destinyCity;
        private int numOfSeats;
        private string description;
        private DateTime date;
        private DateTime minimumDate;
        private TimeSpan time;
        private ImageSource imageSource;
        private MediaFile file;
        #endregion

        #region Propierties
        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnable
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public ObservableCollection<City> OriginCities
        {
            get { return this.originCities; }
            set { this.SetValue(ref this.originCities, value); }
        }

        public City OriginCity
        {
            get { return this.originCity; }
            set { this.SetValue(ref this.originCity, value); }
        }

        public ObservableCollection<City> DestinyCities
        {
            get { return this.destinyCities; }
            set { this.SetValue(ref this.destinyCities, value); }
        }

        public City DestinyCity
        {
            get { return this.destinyCity; }
            set { this.SetValue(ref this.destinyCity, value); }
        }

        public int NumOfSeats
        {
            get { return this.numOfSeats; }
            set { this.SetValue(ref this.numOfSeats, value); }
        }

        public string Description
        {
            get { return this.description; }
            set { this.SetValue(ref this.description, value); }
        }
        
        public DateTime Date
        {
            get { return this.date; }
            set { this.SetValue(ref this.date, value); }
        }
        
        public DateTime MinimumDate
        {
            get { return this.minimumDate; }
            set { this.SetValue(ref this.minimumDate, value); }
        }
        
        public TimeSpan Time
        {
            get { return this.time; }
            set { this.SetValue(ref this.time, value); }
        }

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Constructors
        public AddRouteViewModel()
        {
            this.apiService = new ApiService();
            this.Date = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            this.minimumDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            this.Time = new TimeSpan(12, 00, 00);
            this.ImageSource = "no_image";
            this.LoadCities();
        }
        #endregion

        #region Methods
        private async void LoadCities()
        {
            this.IsRunning = true;
            this.IsEnable = false;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.AcceptDisplay);
                return;
            }

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["CitiesController"].ToString();
            var response = await this.apiService.GetListAsync<City>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.AcceptDisplay);
                return;
            }

            var cities = (List<City>)response.Result;
            this.OriginCities = new ObservableCollection<City>(cities);
            this.DestinyCities = new ObservableCollection<City>(cities);

            this.IsRunning = false;
            this.IsEnable = true;
        }
        #endregion

        #region Commands
        public ICommand SaveRouteCommand
        {
            get
            {
                return new RelayCommand(SaveRoute);
            }
        }

        private async void SaveRoute()
        {
            if (string.IsNullOrEmpty(this.NumOfSeats.ToString()))
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NumOfSeatValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.NumOfSeats < 1)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.NumOfSeatMinValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.OriginCity == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.OriginCityValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.DestinyCity == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DestinyCityValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.DestinyCity.CityId == this.OriginCity.CityId)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.OriginDestinyCityValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.Date == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DateValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.Time == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.TimeValidation,
                    Languages.AcceptDisplay);
                return;
            }

            this.IsRunning = true;
            this.IsEnable = false;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.AcceptDisplay);
                return;
            }
            
            var dateline = Date.Date;
            var fullDate = new DateTime(dateline.Year, dateline.Month, dateline.Day, Time.Hours, Time.Minutes, Time.Seconds);

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var route = new Route() {
                NumSeats = this.NumOfSeats.ToString(),
                OriginID = this.OriginCity.CityId,
                DestinyID = this.DestinyCity.CityId,
                Description = this.Description,
                Date = fullDate,
                UserId = MainViewModel.GetInstance().UserASP.Id,
                IsDeleted = false,
                Inbackend = false,
                ImageArray = imageArray
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["RoutesController"].ToString();
            var response = await this.apiService.PostAsync(url, prefix, controller, route, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, response.Message, Languages.AcceptDisplay);
                return;
            }

            this.IsRunning = false;
            this.IsEnable = true;
            var routesVM = RoutesViewModel.GetInstance();
            routesVM.MyRoutes.Add((Route)response.Result);
            routesVM.RefreshList();
            await App.Navigator.PopAsync();
        }

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
        #endregion


    }
}
