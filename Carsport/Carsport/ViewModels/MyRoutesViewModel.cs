namespace Carsport.ViewModels
{
    using Carsport.Common.Models;
    using Carsport.Helpers;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class MyRoutesViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private ObservableCollection<RouteItemViewModel> myCreateRoutes;
        private bool isRunning;
        private bool isEnabled;
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Propierties
        public List<Route> MyRoutes { get; set; }

        public ObservableCollection<RouteItemViewModel> MyCreateRoutes
        {
            get { return this.myCreateRoutes; }
            set { this.SetValue(ref this.myCreateRoutes, value); }
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

        public string Filter
        {
            get { return this.filter; }
            set
            {
                this.filter = value;
                this.RefreshList();
            }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }
        #endregion

        #region Constructors
        public MyRoutesViewModel()
        {
            instance = this;

            this.apiService = new ApiService();
            this.LoadMyRoutes();
        }

        private async void LoadMyRoutes()
        {
            this.IsRefreshing = true;
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
            var controller = Application.Current.Resources["RoutesController"].ToString();
            var userMail = MainViewModel.GetInstance().UserASP.Email;
            var response = await this.apiService.GetListByEmailAsync<Route>(url, prefix, $"{controller}GetRouteByEmail", Settings.TokenType, Settings.AccessToken, userMail);

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnabled = true;
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            this.MyRoutes = (List<Route>)response.Result;

            if (MyRoutes.Count > 0)
            {
                this.RefreshList();
            }


            this.IsRunning = false;
            this.IsEnabled = true;
            this.IsRefreshing = false;
        }

        private void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter))
            {
                var myListRouteItemViewModel = this.MyRoutes.Select(r => new RouteItemViewModel
                {
                    RouteId = r.RouteId,
                    OriginID = r.OriginID,
                    DestinyID = r.DestinyID,
                    Date = r.Date,
                    Description = r.Description,
                    NumSeats = r.NumSeats,
                    IsDeleted = r.IsDeleted,
                    UserId = r.UserId,
                    Origin = r.Origin,
                    Destiny = r.Destiny,
                });

                this.MyCreateRoutes = new ObservableCollection<RouteItemViewModel>(
                    myListRouteItemViewModel.OrderByDescending(r => r.RouteId));
            }
            else
            {
                var myListRouteItemViewModel = this.MyRoutes.Select(r => new RouteItemViewModel
                {
                    RouteId = r.RouteId,
                    OriginID = r.OriginID,
                    DestinyID = r.DestinyID,
                    Date = r.Date,
                    Description = r.Description,
                    NumSeats = r.NumSeats,
                    IsDeleted = r.IsDeleted,
                    UserId = r.UserId,
                    Origin = r.Origin,
                    Destiny = r.Destiny,
                }).Where(p => p.Description.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.MyCreateRoutes = new ObservableCollection<RouteItemViewModel>(
                    myListRouteItemViewModel.OrderByDescending(r => r.RouteId));
            }
        }
        #endregion

        #region Singleton
        private static MyRoutesViewModel instance;

        public static MyRoutesViewModel GetInstance()
        {
            if(instance == null)
            {
                instance = new MyRoutesViewModel();
            }

            return instance;
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

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadMyRoutes);
            }
        }
        #endregion    
    }
}
