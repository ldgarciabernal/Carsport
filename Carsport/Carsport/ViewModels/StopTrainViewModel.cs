namespace Carsport.ViewModels
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using Carsport.Common.Models;
    using Carsport.Helpers;
    using Carsport.Models;
    using Carsport.Services;
    using Xamarin.Forms;

    public class StopTrainViewModel : BaseViewModel
    {
        #region Atributtes
        private ApiService apiService;

        private Station originStation;
        private bool isRunning;
        private bool isEnable;
        private ObservableCollection<Line> lineStop;
        #endregion

        #region Propierties
        public Station OriginStation
        {
            get { return this.originStation; }
            set { this.SetValue(ref this.originStation, value); }
        }

        public ObservableCollection<Line> LineStop
        {
            get { return this.lineStop; }
            set { this.SetValue(ref this.lineStop, value); }
        }

        public bool IsRunning
        {
            get { return this.isRunning; }
            set { this.SetValue(ref this.isRunning, value); }
        }

        public bool IsEnable
        {
            get { return this.isEnable; }
            set { this.SetValue(ref this.isEnable, value); }
        }
        #endregion

        #region Constructors
        public StopTrainViewModel(Station originStation)
        {
            this.OriginStation = originStation;
            this.apiService = new ApiService();

            this.LoadTrainLines();
        }

        private async void LoadTrainLines()
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
                $"/nucleos/{this.OriginStation.CoreId}/lineas");

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
            this.LineStop = new ObservableCollection<Line>(listOfStops.LineList.Where(l => l.Code.ToLower().Contains("MD".ToLower())
                    || l.Code.ToLower().Contains("C-1".ToLower())).ToList());

            this.IsRunning = false;
            this.IsEnable = true;
        }
        #endregion
    }
}
