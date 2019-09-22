namespace Carsport.ViewModels
{
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Helpers;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Xamarin.Forms;

    public class RoutesViewModel : BaseViewModel
    {
        #region Atributtes
        private ApiService apiService;

        private bool isRefreshing;
        private string filter;

        private ObservableCollection<RouteItemViewModel> routes;
        #endregion

        #region Propierties
        public List<Route> MyRoutes { get; set; }

        public ObservableCollection<RouteItemViewModel> Routes
        {
            get { return this.routes; }
            set { this.SetValue(ref this.routes, value); }
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

        #region Controlers
        public RoutesViewModel()
        {
            instance = this;
            this.apiService = new ApiService();

            this.LoadRoutes();
        }
        #endregion

        #region Singleton
        private static RoutesViewModel instance;

        public static RoutesViewModel GetInstance()
        {
            return instance;
        }
        #endregion

        #region Methods
        private async void LoadRoutes()
        {
            this.IsRefreshing = true;

            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(Languages.Error, connection.Message, Languages.AcceptDisplay);
                return;
            }

            var answer = await this.LoadRoutesFromAPI();
            if (answer)
            {
                this.RefreshList();
            }

            this.IsRefreshing = false;
        }
        /*
        private async Task LoadRoutesFromDB()
        {
            this.MyRoutes = await this.dataService.GetAllRoutes();
        }

        private async Task SaveRoutesToDB()
        {
            await this.dataService.DeleteAllRoutess();
            this.dataService.Insert(this.MyRoutes);
        }
        */
        private async Task<bool> LoadRoutesFromAPI()
        {
            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["RoutesController"].ToString();
            var response = await this.apiService.GetListAsync<Route>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);
            if (!response.IsSuccess)
            {
                return false;
            }

            this.MyRoutes = (List<Route>)response.Result;
            return true;
        }

        public void RefreshList()
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
                    Inbackend = r.Inbackend,
                    ImagePath = r.ImagePath,
                });

                this.Routes = new ObservableCollection<RouteItemViewModel>(
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
                    Inbackend = r.Inbackend,
                    ImagePath = r.ImagePath,
                }).Where(p => p.Origin.Name.ToLower().Contains(this.Filter.ToLower()) || p.Destiny.Name.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Routes = new ObservableCollection<RouteItemViewModel>(
                    myListRouteItemViewModel.OrderByDescending(r => r.RouteId));
            }
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
                return new RelayCommand(LoadRoutes);
            }
        }
        #endregion
    }
}
