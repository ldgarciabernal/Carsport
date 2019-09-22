namespace Carsport.ViewModels
{
    using System;
    using System.Windows.Input;
    using GalaSoft.MvvmLight.Command;
    using Views;
    using Xamarin.Forms;

    public class DashBoardViewModel : BaseViewModel
    {
        #region Properties
        #endregion

        #region Commands
        public ICommand GoToCarCommand
        {
            get
            {
                return new RelayCommand(GoToCar);
            }
        }

        private async void GoToCar()
        {
            MainViewModel.GetInstance().Routes = new RoutesViewModel();
            await App.Navigator.PushAsync(new RoutesPage());
        }

        public ICommand GoToBusCommand
        {
            get
            {
                return new RelayCommand(GoToBus);
            }
        }

        private async void GoToBus()
        {
            MainViewModel.GetInstance().Bus = new BusViewModel();
            await App.Navigator.PushAsync(new BusPage());
        }

        public ICommand GoToTrainCommand
        {
            get
            {
                return new RelayCommand(GoToTrain);
            }
        }

        private async void GoToTrain()
        {
            MainViewModel.GetInstance().Train = new TrainViewModel();
            await App.Navigator.PushAsync(new TrainPage());
        }

        public ICommand GoToBikeCommand
        {
            get
            {
                return new RelayCommand(GoToBike);
            }
        }

        private async void GoToBike()
        {
            MainViewModel.GetInstance().Bycicle = new BycicleViewModel();
            await App.Navigator.Navigation.PushAsync(new BikePage());
        }
        #endregion
    }
}
