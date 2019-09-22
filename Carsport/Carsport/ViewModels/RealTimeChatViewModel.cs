using Carsport.Models;
using GalaSoft.MvvmLight.Command;
using System.Collections.ObjectModel;
using System.Windows.Input;

using Microsoft.AspNetCore.SignalR.Client;
using System.Threading.Tasks;
using System;
using Xamarin.Forms;
using Carsport.Helpers;

namespace Carsport.ViewModels
{
    public class RealTimeChatViewModel : BaseViewModel
    {
        public ChatMessage ChatMessage { get; }

        private ObservableCollection<ChatMessage> messages;
        public ObservableCollection<ChatMessage> Messages
        {
            get { return this.messages; }
            set
            { this.SetValue(ref this.messages, value); }
        }

        bool isConnected;
        public bool IsConnected
        {
            get { return this.isConnected; }
            set {
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

        public RealTimeChatViewModel()
        {
            instance = this;
            ChatMessage = new ChatMessage();
            Messages = new ObservableCollection<ChatMessage>();
            SendMessageCommand = new Command(async () => await SendMessage());
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
            random = new Random();

            var ip = "localhost";
            if (Device.RuntimePlatform == Device.Android)
                ip = "10.0.2.2";

            hubConnection = new HubConnectionBuilder()
                .WithUrl($"http://{ip}:5000/chatHub")
                .Build();

            hubConnection.Closed += async (error) =>
            {
                IsConnected = false;
                SendLocalMessage("Connection Closed...");
                await Task.Delay(random.Next(0, 5) * 1000);
                await Connect();
            };

            hubConnection.On<string, string>("ReceiveMessage", (user, message) => 
            {
                var finalMessage = $"{user} says {message}";
                SendLocalMessage(finalMessage);
            });

            hubConnection.On<string>("EnteredOrLeft", (message) =>
            {
                SendLocalMessage(message);
            });
        }
        
        private async Task Connect()
        {
            if (IsConnected)
                return;

            try
            {
                await hubConnection.StartAsync();
                await hubConnection.SendAsync("AddToGroup", "test", Settings.Username);
                IsConnected = true;
                SendLocalMessage("Connected...");
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Connection error: {ex.Message}");
            }
            
        }

        private async Task Disconnect()
        {
            if (!IsConnected)
                return;

            await hubConnection.SendAsync("RemoveFromGroup", "test", Settings.Username);
            await hubConnection.StopAsync();
            IsConnected = false;
            SendLocalMessage("Disonnected...");
        }

        private async Task SendMessage()
        {
            try
            {
                IsBusy = true;
                await hubConnection.InvokeAsync("SendMessageGroup",
                    "test",
                    Settings.Username,
                    ChatMessage.Message);
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Send failed: {ex.Message}");
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
                Messages.Add(new ChatMessage
                {
                    Message = message
                });
            });
        }

        public static RealTimeChatViewModel instance;
        public static RealTimeChatViewModel GetInstance()
        {
            if (instance == null)
            {
                instance = new RealTimeChatViewModel();
            }

            return instance;
        }
    }
}
