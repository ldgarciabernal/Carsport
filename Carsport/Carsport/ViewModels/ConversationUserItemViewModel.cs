using Carsport.Views;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;

namespace Carsport.ViewModels
{
    public class ConversationUserItemViewModel
    {
        public int ConversationId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public int RouteId { get; set; }

        public DateTime LastDate { get; set; }

        public string LastMessage { get; set; }

        public string UserId { get; set; }


        public ICommand GotoConversationCommand
        {
            get
            {
                return new RelayCommand(GotoConversation);
            }
        }

        private async void GotoConversation()
        {

            var route = new RouteItemViewModel()
            {
                RouteId = this.RouteId,
                UserId = this.UserId,
            };

            MainViewModel.GetInstance().MyConversations = new MyConversationsViewModel(route);
            await App.Navigator.PushAsync(new MyConversationPage());
        }
    }
}
