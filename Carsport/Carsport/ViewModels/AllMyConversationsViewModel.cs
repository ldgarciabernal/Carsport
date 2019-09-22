using Carsport.Common.Models;
using Carsport.Helpers;
using Carsport.Services;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Carsport.ViewModels
{
    public class AllMyConversationsViewModel : BaseViewModel
    {
        #region Attributes
        private ApiService apiService;

        private ObservableCollection<ConversationUserItemViewModel> conversations;

        private bool isEnabled;
        private bool isRefreshing;
        private string filter;
        #endregion

        #region Properties
        public ObservableCollection<ConversationUserItemViewModel> Conversations
        {
            get { return this.conversations; }
            set { this.SetValue(ref this.conversations, value); }
        }

        public bool IsEnable
        {
            get { return this.isEnabled; }
            set { this.SetValue(ref this.isEnabled, value); }
        }

        public bool IsRefreshing
        {
            get { return this.isRefreshing; }
            set { this.SetValue(ref this.isRefreshing, value); }
        }

        public string Filter
        {
            get { return this.filter; }
            set {
                this.filter = value;
                this.RefreshList();
            }
        }

        public List<ConversationUser> MyConversations { get; set; }
        #endregion

        #region Constructors
        public AllMyConversationsViewModel()
        {
            this.apiService = new ApiService();

            this.LoadConversations();
        }

        private async void LoadConversations()
        {
            this.IsEnable = false;
            this.IsRefreshing = true;
            var connection = await this.apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                this.IsEnable = true;
                this.IsRefreshing = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var request = new UserIdRequest()
            {
                UserId = MainViewModel.GetInstance().UserASP.Id,
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["ConversationController"].ToString();
            var response = await this.apiService.GetListAsync<ConversationUser, UserIdRequest>(url, prefix, $"{controller}/GetMyConversations", request, Settings.TokenType, Settings.AccessToken);

            if (!response.IsSuccess)
            {
                this.IsRefreshing = false;
                this.IsEnable = true;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            this.MyConversations = (List<ConversationUser>)response.Result;
            if (MyConversations.Count() > 0)
            {
                this.RefreshList();
            }


            this.IsRefreshing = false;
            this.IsEnable = true;
        }
        #endregion

        #region Methods
        private void RefreshList()
        {
            if (string.IsNullOrEmpty(this.Filter) || string.IsNullOrWhiteSpace(this.Filter))
            {
                var myListConversationRoutes = this.MyConversations.Select(c => new ConversationUserItemViewModel
                {
                    ConversationId = c.ConversationId,
                    LastDate = (c.Messages.Count > 0) ? c.Messages.LastOrDefault().Date : new DateTime(),
                    LastMessage = (c.Messages.Count > 0) ? c.Messages.LastOrDefault().Text : string.Empty,
                    RouteId = 0,
                    Email = c.Email,
                    UserName = c.UserName,
                    UserId = (MainViewModel.GetInstance().UserASP.Id == c.UserIdOne) ? c.UserIdTwo : c.UserIdOne,
                });

                this.Conversations = new ObservableCollection<ConversationUserItemViewModel>(
                    myListConversationRoutes.OrderByDescending(c => c.LastDate));
            }
            else
            {
                var myListConversationRoutes = this.MyConversations.Select(c => new ConversationUserItemViewModel
                {
                    ConversationId = c.ConversationId,
                    LastDate = (c.Messages.Count > 0) ? c.Messages.LastOrDefault().Date : new DateTime(),
                    LastMessage = c.Messages.LastOrDefault().Text,
                    RouteId = 0,
                    Email = c.Email,
                    UserName = c.UserName,
                    UserId = (MainViewModel.GetInstance().UserASP.Id == c.UserIdOne) ? c.UserIdTwo : c.UserIdOne,
                }).Where(c => c.Email.ToLower().Contains(this.Filter.ToLower()) || c.UserName.ToLower().Contains(this.Filter.ToLower())).ToList();

                this.Conversations = new ObservableCollection<ConversationUserItemViewModel>(
                    myListConversationRoutes.OrderByDescending(c => c.LastDate));
            }
        }
        #endregion

        #region Commands
        public ICommand SearchCommand
        {
            get
            {
                return new RelayCommand(RefreshList);
            }
        }

        public ICommand RefreshCommand
        {
            get
            {
                return new RelayCommand(LoadConversations);
            }
        }

        #endregion
    }
}
