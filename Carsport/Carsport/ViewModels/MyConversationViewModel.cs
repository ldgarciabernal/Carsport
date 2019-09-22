namespace Carsport.ViewModels
{
    using Carsport.Helpers;
    using Carsport.Models;
    using Common.Models;
    using Microsoft.AspNetCore.SignalR.Client;
    using Services;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class MyConversationsViewModel : BaseViewModel
    {
        #region RealTime
        public MessageItemViewModel ChatMessage { get; }

        bool isConnected;
        public bool IsConnected
        {
            get { return this.isConnected; }
            set
            {
                Device.BeginInvokeOnMainThread(() => {
                    this.SetValue(ref this.isConnected, value);
                });
            }
        }

        bool isBusy;
        public bool IsBusy
        {
            get { return this.isBusy; }
            set
            {
                Device.BeginInvokeOnMainThread(() => {
                    this.SetValue(ref this.isBusy, value);
                });
            }
        }
       
        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }

        private HubConnection hubConnection;
        Random random;

        public ChatMessage ChatMessageRT { get; }

        private ObservableCollection<ChatMessage> messagesRT;
        public ObservableCollection<ChatMessage> MessagesRT
        {
            get { return this.messagesRT; }
            set
            { this.SetValue(ref this.messagesRT, value); }
        }
        #endregion

        #region Attributes
        private ApiService apiService;
        private RouteItemViewModel route;

        private Conversation conversation;
        private ObservableCollection<MessageItemViewModel> messages;
        private string outText;
        #endregion

        #region Propierties
        public Conversation Conversation
        {
            get { return this.conversation; }
            set { this.SetValue(ref this.conversation, value); }
        }

        public ObservableCollection<MessageItemViewModel> Messages
        {
            get { return this.messages; }
            set { this.SetValue(ref this.messages, value); }
        }

        public string OutText
        {
            get { return this.outText; }
            set { this.SetValue(ref this.outText, value); }
        }
        #endregion

        #region Constructors
        public MyConversationsViewModel(RouteItemViewModel routeView)
        {
            instance = this;
            this.apiService = new ApiService();
            this.route = routeView;

            #region RealTime Section
            ChatMessage = new MessageItemViewModel();
            ChatMessageRT = new ChatMessage();

            SendMessageCommand = new Command(async () => await SendMessage());
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
            random = new Random();
            #endregion

            LoadMessages();

            #if DEBUG
            var ip = "localhost";
            if (Device.RuntimePlatform == Device.Android)
                ip = "10.0.2.2";

            hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://{ip}:5000/chatHub")
                .Build();
            #else
            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://movilidaducarealtime.azurewebsites.net/chatHub")
                .Build();
            #endif

            hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                SendLocalMessage("Connection Closed...");
                await Task.Delay(random.Next(0, 5) * 1000);
                await Connect();
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                var finalMessage = new MessageItemViewModel
                {
                    Sender = user,
                    Message = message,
                    Date = DateTime.Now,
                    ConectionId = this.Conversation.ConnectionId
                };

                if (user != MainViewModel.GetInstance().UserASP.Email)
                    Messages.Add(finalMessage);

                var finMessage = $"{user} says {message}";
                SendLocalMessage(finMessage);
            });

            hubConnection.On<string>("EnteredOrLeft", (message) =>
            {
                SendLocalMessage(message);
            });
        }

        private async void LoadMessages()
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

            var loggerUser = MainViewModel.GetInstance().UserASP;
            var request = new UsersIdRequest()
            {
                UserFrom = loggerUser.Id,
                UserTo = route.UserId,
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["ConversationController"].ToString();
            var response = await this.apiService.GetConversationByUsersAsync<Conversation>(
                url, prefix, controller, Settings.TokenType, Settings.AccessToken, request);

            if (!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var ListConversation = (List<Conversation>)response.Result;

            if (ListConversation.Count > 0)
            {
                this.Conversation = ListConversation[0];
                this.Messages = new ObservableCollection<MessageItemViewModel>(this.Conversation.Messages.Select(m => new MessageItemViewModel
                {
                    Message = m.Text,
                    Date = m.Date,
                    Sender = m.Sender,
                    ConectionId = Conversation.ConnectionId
                }));

                this.MessagesRT = new ObservableCollection<ChatMessage>(this.Conversation.Messages.Select(m => new ChatMessage
                {
                    Message = m.Text,
                    User = m.Sender,
                }));
            }
            else
            {
                this.CreateConversation(request);
            }
        }

        private async void CreateConversation(UsersIdRequest request)
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

            var conversation = new ConversationRequest
            {
                UserIdOne = request.UserFrom,
                UserIdTwo = request.UserTo,
                Date = DateTime.Now,
                RouteId = this.route.RouteId,
                ConnectionId = request.UserFrom + request.UserTo
            };

            var url = Application.Current.Resources["APIMovilidad"].ToString();
            var prefix = Application.Current.Resources["APIprefix"].ToString();
            var controller = Application.Current.Resources["ConversationController"].ToString();
            var response = await this.apiService.PostConversationAsync<Conversation>(
                url, prefix, controller, conversation, Settings.TokenType, Settings.AccessToken);

            if(!response.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    response.Message,
                    Languages.AcceptDisplay);
                return;
            }

            this.Conversation = (Conversation)response.Result;
            this.Messages = new ObservableCollection<MessageItemViewModel>();
            this.MessagesRT = new ObservableCollection<ChatMessage>();
        }
#endregion

        #region Commands

        private async void SaveMessage()
        {
            if (!string.IsNullOrWhiteSpace(this.OutText))
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

                var loggerUser = MainViewModel.GetInstance().UserASP;
                var message = new Message
                {
                    Sender = loggerUser.Id,
                    Date = DateTime.Now,
                    ConversationId = this.Conversation.ConversationId,
                    Text = this.OutText,
                };

                var url = Application.Current.Resources["APIMovilidad"].ToString();
                var prefix = Application.Current.Resources["APIprefix"].ToString();
                var controller = Application.Current.Resources["MessagesController"].ToString();
                var response = await this.apiService.PostAsync(
                    url, prefix, controller, message, Settings.TokenType, Settings.AccessToken);

                if (!response.IsSuccess)
                {
                    await Application.Current.MainPage.DisplayAlert(
                        Languages.Error,
                        response.Message,
                        Languages.AcceptDisplay);
                    return;
                }

                var mess = (Message)response.Result;
                var newMessage = new MessageItemViewModel()
                {
                    Message = mess.Text,
                    Date = mess.Date,
                    Sender = mess.Sender,
                };

                var newMessageRT = new ChatMessage()
                {
                    Message = mess.Text,
                    User = mess.Sender,
                };

                this.Messages.Add(newMessage);
                this.MessagesRT.Add(newMessageRT);
                this.OutText = string.Empty;
            }
        }
        #endregion

        #region Singlenton
        public static MyConversationsViewModel instance;

        public static MyConversationsViewModel GetInstance()
        {
            return instance;
        }
        #endregion


        private async Task Connect()
        {
            if (IsConnected)
                return;

            try
            {
                await hubConnection.StartAsync();
                await hubConnection.SendAsync("AddToGroup", 
                    Conversation.ConnectionId, Settings.Username);
                IsConnected = true;
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    ex.Message,
                    Languages.AcceptDisplay);
                return;
            }

        }

        private async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.SendAsync("RemoveFromGroup", 
                this.Conversation.ConnectionId, 
                Settings.Username);
            await hubConnection.StopAsync();
            IsConnected = false;
        }

        private async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                await hubConnection.InvokeAsync("SendMessageGroup",
                    this.Conversation.ConnectionId,
                    Settings.Username,
                    this.OutText);

                SaveMessage();
            }
            catch (Exception ex)
            {
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    ex.Message,
                    Languages.AcceptDisplay);
                return;
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void SendLocalMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                MessagesRT.Add(new ChatMessage
                {
                    Message = message
                });
            });
        }

    }
}
