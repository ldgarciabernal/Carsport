namespace Carsport.ViewModels
{
    using Views;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;
    using Plugin.Media.Abstractions;
    using Plugin.Media;

    public class EditRouteViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;
        private bool isRunning;
        private bool isEnabled;

        private RouteItemViewModel routeItem;
        private ObservableCollection<City> originCities;
        private City originCity;
        private ObservableCollection<City> destinyCities;
        private City destinyCity;
        private DateTime date;
        private TimeSpan time;
        private DateTime minimumDate;
        private int numOfSeats;
        private string description;

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

        public RouteItemViewModel RouteItem
        {
            get { return this.routeItem; }
            set { this.SetValue(ref this.routeItem, value); }
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

        public DateTime Date
        {
            get { return this.date; }
            set { this.SetValue(ref this.date, value); }
        }

        public TimeSpan Time
        {
            get { return this.time; }
            set { this.SetValue(ref this.time, value); }
        }

        public DateTime MinimumDate
        {
            get { return this.minimumDate; }
            set { this.SetValue(ref this.minimumDate, value); }
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

        public ImageSource ImageSource
        {
            get { return this.imageSource; }
            set { this.SetValue(ref this.imageSource, value); }
        }
        #endregion

        #region Constructors
        public EditRouteViewModel(RouteItemViewModel routeItemViewModel)
        {
            this.apiService = new ApiService();

            this.RouteItem = routeItemViewModel;
            this.Date = new DateTime(this.RouteItem.Date.Year, this.RouteItem.Date.Month, this.RouteItem.Date.Day);
            this.Time = new TimeSpan(this.RouteItem.Date.Hour, this.RouteItem.Date.Minute, this.RouteItem.Date.Second);
            this.MinimumDate = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);

            this.NumOfSeats = Int32.Parse(routeItemViewModel.NumSeats);
            this.Description = routeItemViewModel.Description;
            this.ImageSource = (!string.IsNullOrEmpty(RouteItem.ImagePath)) ? RouteItem.ImageFullPath : "no_image";

            this.LoadCities();
        }

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

            this.OriginCity = this.OriginCities.FirstOrDefault(c => c.CityId == this.RouteItem.OriginID);
            this.DestinyCity = this.DestinyCities.FirstOrDefault(c => c.CityId == this.RouteItem.DestinyID);

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

            byte[] imageArray = null;
            if (this.file != null)
            {
                imageArray = FilesHelper.ReadFully(this.file.GetStream());
            }

            var dateline = Date.Date;
            var fullDate = new DateTime(dateline.Year, dateline.Month, dateline.Day, Time.Hours, Time.Minutes, Time.Seconds);
            var route = new Route()
            {
                RouteId = this.RouteItem.RouteId,
                Date = fullDate,
                Description = this.Description,
                OriginID = this.OriginCity.CityId,
                DestinyID = this.DestinyCity.CityId,
                IsDeleted = false,
                NumSeats = this.NumOfSeats.ToString(),
                UserId = this.RouteItem.UserId,
                Inbackend = this.RouteItem.Inbackend,
                ImageArray = imageArray,
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["RoutesController"].ToString();
            var response = await this.apiService.PutAsync<Route>(url, prefix, controller, route, route.RouteId, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, Languages.GetTokenWrong, Languages.AcceptDisplay);
                return;
            } else
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.SuccessfulEdit,
                    Languages.EditMessages,
                    Languages.AcceptDisplay);
            }
            
            MainViewModel.GetInstance().Routes = new RoutesViewModel();
            await App.Navigator.PushAsync(new RoutesPage());
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
