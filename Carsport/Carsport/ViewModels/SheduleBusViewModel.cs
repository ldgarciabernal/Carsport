namespace Carsport.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Carsport.Models;
    using Carsport.Services;
    using Carsport.Views;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class SheduleBusViewModel : BaseViewModel
    {
        #region Atributtes
        private ApiService apiService;
        private Core originCore;
        private Core destinyCore;

        private ObservableCollection<Timetable> busShedule;
        private Timetable timeTableBus;
        private ObservableCollection<Block> blockBusShedule;
        private ObservableCollection<Shedule> timeBusShedule;

        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Properties
        public Core OriginCore
        {
            get { return this.originCore; }
            set { this.SetValue(ref this.originCore, value); }
        }

        public Core DestinyCore
        {
            get { return this.destinyCore; }
            set { this.SetValue(ref this.destinyCore, value); }
        }

        public ObservableCollection<Timetable> BusShedule
        {
            get { return this.busShedule; }
            set { this.SetValue(ref this.busShedule, value); }
        }

        public Timetable TimeTableBus
        {
            get { return this.timeTableBus; }
            set { this.SetValue(ref this.timeTableBus, value); }
        }

        public ObservableCollection<Block> BlockBusShedule
        {
            get { return this.blockBusShedule; }
            set { this.SetValue(ref this.blockBusShedule, value); }
        }

        public ObservableCollection<Shedule> TimeBusShedule
        {
            get { return this.timeBusShedule; }
            set { this.SetValue(ref this.timeBusShedule, value); }
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
        public SheduleBusViewModel(Core originCore, Core destinyCore)
        {
            instance = this;
            this.OriginCore = originCore;
            this.DestinyCore = destinyCore;

            this.apiService = new ApiService();
        }
       
        #endregion

        #region Methods
        private async void LoadShedule()
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                //this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    "Languages.Error",
                    "connection.Message.WIFI",
                    "Languages.Accept");
                return;
            }
            
            var url = Application.Current.Resources["APIConsorcio"].ToString();
            var consortiumId = Application.Current.Resources["ConsorcioId"].ToString();
            var response = await this.apiService.GetAsync<Timetable>(
                url, 
                consortiumId, 
                $"/horarios_origen_destino?destino={DestinyCore.CoreId}&lang=ES&origen={OriginCore.CoreId}");

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Languages.Error",
                    response.Message,
                    "Languages.Accept");
                return;
            }

            var timetableOrder = (Timetable)response.Result;
            var hours = timetableOrder.Shedules.Where(s => !s.Code.ToLower().Contains("MD".ToLower())
                && !s.Code.ToLower().Contains("MD".ToLower())).ToList();

            timetableOrder.Shedules = hours;
            var timeTable = new List<Timetable> {
                timetableOrder
            };

            this.TimeTableBus = timetableOrder;
            this.BusShedule = new ObservableCollection<Timetable>(timeTable);
            this.BlockBusShedule = new ObservableCollection<Block>(timetableOrder.Bloks);
            this.TimeBusShedule = new ObservableCollection<Shedule>(timetableOrder.Shedules);
        }
        #endregion

        #region Singleton
        private static SheduleBusViewModel instance;

        public static SheduleBusViewModel GetInstance()
        {
            return instance;
        }

        #endregion

        #region Commands
        public ICommand GoToBusStopsCommand
        {
            get
            {
                return new RelayCommand(GoToBusStops);
            }
        }

        private async void GoToBusStops()
        {
            MainViewModel.GetInstance().StopBus = new StopBusViewModel(this.OriginCore);
            await App.Navigator.PushAsync(new StopBusPage());
        }
        #endregion
    }
}
