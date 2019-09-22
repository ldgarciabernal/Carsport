using Carsport.Common.Models;
using Carsport.Helpers;
using Carsport.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.ObjectModel;
using System.Security.Claims;
using System.Windows.Input;
using Xamarin.Forms;

namespace Carsport.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region ViewModels
        public LoginViewModel Login { get; set; }

        public RegisterViewModel Register { get; set; }

        public ForgotPasswordViewModel ForgotPassword { get; set; }

        public DashBoardViewModel DashBoard { get; set; }

        public BusViewModel Bus { get; set; }

        public SheduleBusViewModel SheduleBus { get; set; }

        public StopBusViewModel StopBus { get; set; }

        public TrainViewModel Train { get; set; }

        public SheduleTrainViewModel SheduleTrain { get; set; }

        public StopTrainViewModel StopTrain { get; set; }

        public BycicleViewModel Bycicle { get; set; }

        public NotificationViewModel Notification { get; set; }

        public NewsViewModel News { get; set; }

        public RoutesViewModel Routes { get; set; }

        public EditRouteViewModel EditRoute { get; set; }

        public AddRouteViewModel AddRoute { get; set; }

        public DetailRouteViewModel DetailRoute { get; set; }

        public ConfigurationViewModel Configuration { get; set; }

        public ProfileViewModel Profile { get; set; }

        public MyRoutesViewModel MyRoutes { get; set; }

        public MyConversationsViewModel MyConversations { get; set; }

        public MyConversationsRouteViewModel MyConversationsRoute { get; set; }

        public AllMyConversationsViewModel AllMyConversations { get; set; }

        public RealTimeChatViewModel RealTiemChat { get; set; }
        #endregion

        #region Propierties
        public ObservableCollection<MenuItemViewModel> Menu { get; set; }
        public ObservableCollection<MenuItemViewModel> MenuSettings { get; set; }

        private CustomUserASP userASP;
        public CustomUserASP UserASP
        {
            get { return this.userASP; }
            set { this.SetValue(ref this.userASP, value); }
        }

        public string UserFullName
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 1)
                {
                    return $"{this.getNameClaim()} {this.getSurnameClaim()}";
                }

                return null;
            }
        }

        public string getNameClaim()
        {
            var name = "";

            for (int i = 0; i < this.UserASP.Claims.Count; i++)
            {
                if(this.UserASP.Claims[i].ClaimType == ClaimTypes.GivenName)
                {
                    name = this.UserASP.Claims[i].ClaimValue;
                }
            }

            return name;
        }

        public string getSurnameClaim()
        {
            var surName = "";

            for (int i = 0; i < this.UserASP.Claims.Count; i++)
            {
                if (this.UserASP.Claims[i].ClaimType == ClaimTypes.Name)
                {
                    surName = this.UserASP.Claims[i].ClaimValue;
                }
            }

            return surName;
        }

        public string getImageFullPathClaim()
        {
            var fullPath = "";

            for (int i = 0; i < this.UserASP.Claims.Count; i++)
            {
                if (this.UserASP.Claims[i].ClaimType == ClaimTypes.Uri)
                {
                    fullPath = this.UserASP.Claims[i].ClaimValue;
                }
            }

            return fullPath;
        }

        public string UserImageFullPath
        {
            get
            {
                if (this.UserASP != null && this.UserASP.Claims != null && this.UserASP.Claims.Count > 2)
                {
                    return $"{Application.Current.Resources["APIMovilidad"].ToString()}{getImageFullPathClaim().Substring(1)}";
                }

                return "no_profile";
            }
        }
        #endregion

        #region Constructors
        public MainViewModel()
        {
            instance = this;
            this.LoadMenu();
        }
        #endregion

        #region Singleton
        private static MainViewModel instance;

        public static MainViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MainViewModel();
            }

            return instance;
        }
        #endregion

        #region Methods
        private void LoadMenu()
        {
            this.Menu = new ObservableCollection<MenuItemViewModel>();
            this.MenuSettings = new ObservableCollection<MenuItemViewModel>();

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_profile_color",
                PageName = "ProfilePage",
                Title = Languages.ProfileTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_new_color",
                PageName = "NotificationPage",
                Title = Languages.NotificationTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_car_color",
                PageName = "CarPage",
                Title = Languages.CarTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "MyRoutesPage",
                Title = Languages.MyRouteMenuTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "MyChatsPage",
                Title = Languages.ChatMenuTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "",
                PageName = "AddRoutePage",
                Title = Languages.AddRouteMenuTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_bus_color",
                PageName = "BusPage",
                Title = Languages.BusMenuTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_train_color",
                PageName = "TrainPage",
                Title = Languages.TrainTitle,
            });

            this.Menu.Add(new MenuItemViewModel
            {
                Icon = "ic_bycicle_color",
                PageName = "ByciclePage",
                Title = Languages.BikeTitle,
            });

            this.MenuSettings.Add(new MenuItemViewModel()
            {
                Icon = "ic_info_dark",
                PageName = "ConfigurationPage",
                Title = Languages.ConfigurationMenuTitle,
            });

            this.MenuSettings.Add(new MenuItemViewModel()
            {
                Icon = "icon_exit_blue",
                PageName = "LogOutPage",
                Title = Languages.LogOutTitle,
            });
        }
        #endregion

        #region Commands
        public ICommand AddRouteCommand
        {
            get
            {
                return new RelayCommand(GoToAddRoute);
            }
        }

        private async void GoToAddRoute()
        {
            this.AddRoute = new AddRouteViewModel();
            await App.Navigator.PushAsync(new AddRoutePage());
        }

        public ICommand GoToDashBoardCommand
        {
            get
            {
                return new RelayCommand(GoToDashBoard);
            }
        }

        private void GoToDashBoard()
        {
            this.DashBoard = new DashBoardViewModel();
            Application.Current.MainPage = new MasterPage();
        }
        #endregion
    }
}
