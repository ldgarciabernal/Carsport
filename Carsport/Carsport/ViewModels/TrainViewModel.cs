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
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class TrainViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private Station originStation;
        private Station destinyStation;

        private ObservableCollection<Station> originStations;
        private ObservableCollection<Station> destinyStations;

        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Propierties
        public Station OriginStation
        {
            get { return this.originStation; }
            set { this.SetValue(ref this.originStation, value); }
        }

        public Station DestinyStation
        {
            get { return this.destinyStation; }
            set { this.SetValue(ref this.destinyStation, value); }
        }

        public ObservableCollection<Station> OriginStations
        {
            get { return this.originStations; }
            set { this.SetValue(ref this.originStations, value); }
        }

        public ObservableCollection<Station> DestinyStations
        {
            get { return this.destinyStations; }
            set { this.SetValue(ref this.destinyStations, value); }
        }

        public bool IsEnabled
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }
        #endregion

        #region Constructors
        public TrainViewModel()
        {
            this.apiService = new ApiService();
            this.isEnabled = true;
            
            this.LoadStations();
        }

        private async void LoadStations()
        {
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
            var controller = Application.Current.Resources["StationController"].ToString();
            var response = await this.apiService.GetListAsync<Station>(url, prefix, controller);

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

            var currentStations = (List<Station>)response.Result;
            this.OriginStations = new ObservableCollection<Station>(currentStations.OrderBy(c => c.Name));
            this.DestinyStations = new ObservableCollection<Station>(currentStations.OrderBy(c => c.Name));

            this.IsRunning = false;
            this.IsEnabled = true;
        }
        #endregion

        #region Commands
        public ICommand GoToTrainSheduleCommand
        {
            get { return new RelayCommand(GoToTrainShedule); }
        }

        private async void GoToTrainShedule()
        {
            if (this.OriginStation == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.OriginStationValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.DestinyStation == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.DestinyStationValidation,
                    Languages.AcceptDisplay);
                return;
            }

            MainViewModel.GetInstance().SheduleTrain = new SheduleTrainViewModel(this.OriginStation, this.DestinyStation);
            await App.Navigator.Navigation.PushAsync(new SheduleTrainPage());
        }
        #endregion
    }
}
