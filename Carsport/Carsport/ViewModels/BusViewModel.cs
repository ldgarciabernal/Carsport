namespace Carsport.ViewModels
{
    using Carsport.Helpers;
    using GalaSoft.MvvmLight.Command;
    using Models;
    using Services;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class BusViewModel : BaseViewModel
    {
        #region Atributtes
        private ApiService apiService;

        private ObservableCollection<Commune> originCommunes;
        private ObservableCollection<Commune> destinyCommunes;
        private ObservableCollection<Core> originCores;
        private ObservableCollection<Core> destinyCores;

        private Commune originCommune;
        private Commune destinyCommune;
        private Core originCore;
        private Core destinyCore;

        private bool isEnabled;
        private bool isRunning;
        #endregion

        #region Properties
        public ObservableCollection<Commune> OriginCommunes
        {
            get { return this.originCommunes; }
            set { this.SetValue(ref this.originCommunes, value); }
        }

        public ObservableCollection<Commune> DestinyCommunes
        {
            get { return this.destinyCommunes; }
            set { this.SetValue(ref this.destinyCommunes, value); }
        }

        public ObservableCollection<Core> OriginCores
        {
            get { return this.originCores; }
            set { this.SetValue(ref this.originCores, value); }
        }

        public ObservableCollection<Core> DestinyCores
        {
            get { return this.destinyCores; }
            set { this.SetValue(ref this.destinyCores, value); }
        }

        public Commune OriginCommune
        {
            get { return this.originCommune; }
            set {
                this.SetValue(ref this.originCommune, value);
                this.LoadCoresByComune(true);
            }
        }

        public Commune DestinyCommune
        {
            get { return this.destinyCommune; }
            set {
                this.SetValue(ref this.destinyCommune, value);
                this.LoadCoresByComune(false);
            }
        }

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
        public BusViewModel()
        {
            this.IsEnabled = true;
            this.apiService = new ApiService();
            this.LoadCommunes();
            this.LoadCores();
        }
        #endregion

        #region Methods

        private async void LoadCores()
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
            
            var url = Application.Current.Resources["APIConsorcio"].ToString();
            var consortiumId = Application.Current.Resources["ConsorcioId"].ToString();
            var controller = Application.Current.Resources["AllCores"].ToString();
            var response = await this.apiService.GetAsync<Cores>(url, consortiumId, controller);

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
            
            var currentCrores = (Cores)response.Result;
            this.OriginCores = new ObservableCollection<Core>(currentCrores.CoreList.OrderBy(c => c.Name));
            this.DestinyCores = new ObservableCollection<Core>(currentCrores.CoreList.OrderBy(c => c.Name));

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        private async void LoadCommunes()
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

            var url = Application.Current.Resources["APIConsorcio"].ToString();
            var consortiumId = Application.Current.Resources["ConsorcioId"].ToString();
            var controller = Application.Current.Resources["AllCommunes"].ToString();
            var response = await this.apiService.GetAsync<Communes>(url, consortiumId, controller);

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
            
            var currentCommunes = (Communes)response.Result;
            this.OriginCommunes = new ObservableCollection<Commune>(currentCommunes.CommuneList.OrderBy(c => c.Name));
            this.DestinyCommunes = new ObservableCollection<Commune>(currentCommunes.CommuneList.OrderBy(c => c.Name));

            this.IsRunning = false;
            this.IsEnabled = true;
        }
        
        private async void LoadCoresByComune(bool isOrigin)
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

            var selectedCoreId = (isOrigin) ? this.OriginCommune.CommuneId : this.DestinyCommune.CommuneId;
            var url = Application.Current.Resources["APIConsorcio"].ToString();
            var consortiumId = Application.Current.Resources["ConsorcioId"].ToString();
            var response = await this.apiService.GetAsync<Cores>(url, consortiumId, $"/municipios/{selectedCoreId}/nucleos");

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

            var currentCrores = (Cores)response.Result;
            if (isOrigin)
            {
                this.OriginCores.Clear();
                this.OriginCores = new ObservableCollection<Core>(currentCrores.CoreList.OrderBy(c => c.Name));
            } else
            {
                this.DestinyCores.Clear();
                this.DestinyCores = new ObservableCollection<Core>(currentCrores.CoreList.OrderBy(c => c.Name));
            }

            this.IsRunning = false;
            this.IsEnabled = true;
        }

        #endregion

        #region Commands
        public ICommand GoToBusSheduleCommand
        {
            get
            {
                return new RelayCommand(GoToBusShedule);
            }
        }

        private async void GoToBusShedule()
        {
            if(this.OriginCommune == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CommuneOriginValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.OriginCore == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CoreOriginValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.DestinyCommune == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CommuneDestinyValidation,
                    Languages.AcceptDisplay);
                return;
            }

            if (this.DestinyCore == null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    Languages.CoreDestinyValidation,
                    Languages.AcceptDisplay);
                return;
            }

            MainViewModel.GetInstance().SheduleBus = new SheduleBusViewModel(this.OriginCore, this.DestinyCore);
            await App.Navigator.PushAsync(new SheduleBusPage());
        }
        #endregion
    }
}

