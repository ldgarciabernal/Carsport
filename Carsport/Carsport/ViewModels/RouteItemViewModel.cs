namespace Carsport.ViewModels
{
    using Carsport.Helpers;
    using Common.Models;
    using GalaSoft.MvvmLight.Command;
    using Services;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;
    using Views;
    using Xamarin.Forms;

    public class RouteItemViewModel : Route
    {
        #region Attribures
        private ApiService apiService;
        #endregion

        #region Propierties
        public UserRequest Creator { get; set; }
        #endregion

        #region Constructors
        public RouteItemViewModel()
        {
            this.apiService = new ApiService();
        }
        #endregion

        #region Methods
        private async Task LoadUserEmail(string id)
        {
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["UsersController"].ToString();
            var response = await this.apiService.GetUserByIdAsync<UserRequest>(url, prefix, $"{controller}/GetUserById", id, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            this.Creator = (UserRequest)response.Result;
        }
        #endregion

        #region Commands
        public ICommand RouteDetailsCommand
        {
            get
            {
                return new RelayCommand(RouteDetails);
            }
        }

        private async void RouteDetails()
        {
            await this.LoadUserEmail(this.UserId);
            MainViewModel.GetInstance().DetailRoute = new DetailRouteViewModel(this);
            await App.Navigator.PushAsync(new DetailRoutePage());
        }
        #endregion
    }
}
