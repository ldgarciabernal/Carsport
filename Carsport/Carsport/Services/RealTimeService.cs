using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Carsport.Helpers;
using Carsport.Models;
using Microsoft.AspNetCore.SignalR.Client;
using Xamarin.Forms;

namespace Carsport.Services
{
    public class RealTimeService
    {

        private ObservableCollection<ChatMessage> messages;
        public ChatMessage ChatMessage { get; }
        bool isConnected;
        bool isBusy;

        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        private HubConnection hubConnection;
        Random random;

        public RealTimeService()
        {

            ChatMessage = new ChatMessage();
            messages = new ObservableCollection<ChatMessage>();
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
                isConnected = false;
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
            if (isConnected)
                return;

            try
            {
                await hubConnection.StartAsync();
                await hubConnection.SendAsync("AddToGroup", "test", Settings.Username);
                isConnected = true;
                SendLocalMessage("Connected...");
            }
            catch (Exception ex)
            {
                SendLocalMessage($"Connection error: {ex.Message}");
            }

        }

        private async Task Disconnect()
        {
            if (!isConnected)
                return;

            await hubConnection.SendAsync("RemoveFromGroup", "test", Settings.Username);
            await hubConnection.StopAsync();
            isConnected = false;
            SendLocalMessage("Disonnected...");
        }

        private async Task SendMessage()
        {
            try
            {
                isBusy = true;
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
                isBusy = false;
            }
        }

        private void SendLocalMessage(string message)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                messages.Add(new ChatMessage
                {
                    Message = message
                });
            });
        }
    }
}
