namespace Carsport.ViewModels
{
    using Services;
    using System.Collections.ObjectModel;
    using Common.Models;
    using Xamarin.Forms;
    using Carsport.Helpers;
    using System.Collections.Generic;

    public class BycicleViewModel : BaseViewModel
    {
        #region Atributtes
        private ApiService apiService;

        private ObservableCollection<Bycicle> byciclePlace;
        private Bycicle universityPlace;

        private bool isRunning;
        private bool isEnabled;
        #endregion

        #region Propierties
        public ObservableCollection<Bycicle> ByciclePlace
        {
            get { return this.byciclePlace; }
            set { this.SetValue(ref this.byciclePlace, value); }
        }

        public Bycicle UniversityPlace
        {
            get { return this.universityPlace; }
            set { this.SetValue(ref this.universityPlace, value); }
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

        #region Contructors
        public BycicleViewModel()
        {
            this.apiService = new ApiService();
            this.LoadByciclePlaces();
        }

        private async void LoadByciclePlaces()
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

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["BycicleController"].ToString();
            var response = await this.apiService.GetListAsync<Bycicle>(url, prefix, controller, Settings.TokenType, Settings.AccessToken);

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

            var listOfPlaces = (List<Bycicle>)response.Result;
            this.ByciclePlace = new ObservableCollection<Bycicle>(listOfPlaces);

            this.IsRunning = false;
            this.IsEnable = true;
        }
        #endregion
    }
}
