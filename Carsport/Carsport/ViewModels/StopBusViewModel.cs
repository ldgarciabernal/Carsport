namespace Carsport.ViewModels
{
    using Carsport.Helpers;
    using Models;
    using Services;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Xamarin.Forms;

    public class StopBusViewModel : BaseViewModel
    {
        #region Atributtes
        private ApiService apiService;

        private Core originCore;
        private ObservableCollection<Line> lineStop;
        private Line currentStop;

        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Properties
        public Core OriginCore
        {
            get { return this.originCore; }
            set { this.SetValue(ref this.originCore, value); }
        }

        public ObservableCollection<Line> LineStop
        {
            get { return this.lineStop; }
            set { this.SetValue(ref this.lineStop, value); }
        }

        public Line CurrentStop
        {
            get { return this.currentStop; }
            set { this.SetValue(ref this.currentStop, value); }
        }

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
        #endregion

        #region Constructors
        public StopBusViewModel(Core originCore)
        {
            this.originCore = originCore;
            this.apiService = new ApiService();
            this.IsEnable = true;

            this.LoadLinePicker();
        }

        private async void LoadLinePicker()
        {
            this.IsRunning = true;
            this.IsEnable = false;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var url = Application.Current.Resources["APIConsorcio"].ToString();
            var consortiumId = Application.Current.Resources["ConsorcioId"].ToString();
            var response = await this.apiService.GetAsync<Lines>(
                url,
                consortiumId,
                $"/nucleos/{this.OriginCore.CoreId}/lineas");

            if (!response.IsSuccess)
            {
                this.IsRunning = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var listOfStops = (Lines)response.Result;
            this.LineStop = new ObservableCollection<Line>(listOfStops.LineList.Where(l => !l.Code.ToLower().Contains("MD".ToLower()) 
                    && !l.Code.ToLower().Contains("B".ToLower()) 
                    && !l.Code.ToLower().Contains("C-1".ToLower())).ToList());
            
            this.IsRunning = false;
            this.IsEnable = true;
        }
        #endregion
    }
}
