namespace Carsport.ViewModels
{
    using System.Windows.Input;
    using Carsport.Common.Models;
    using Carsport.Views;
    using GalaSoft.MvvmLight.Command;
    using Xamarin.Forms;

    public class SheduleTrainViewModel : BaseViewModel
    {
        #region Atributtes
        private Station originStation;
        private Station destinyStation;
        #endregion

        #region Properties
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
        #endregion

        #region Constructors
        public SheduleTrainViewModel(Station originStation, Station destinyStation)
        {
            instance = this;
            this.OriginStation = originStation;
            this.DestinyStation = destinyStation;
        }
        #endregion

        #region Singleton
        private static SheduleTrainViewModel instance;

        public static SheduleTrainViewModel GetInstance()
        {
            return instance;
        }
        #endregion

        #region Commands
        public ICommand GoToTrainStopsCommand
        {
            get
            {
                return new RelayCommand(GoToTrainStops);
            }
        }

        private async void GoToTrainStops()
        {
            MainViewModel.GetInstance().StopTrain = new StopTrainViewModel(this.OriginStation);
            await App.Navigator.PushAsync(new StopTrainPage());
        }
        #endregion
    }
}
