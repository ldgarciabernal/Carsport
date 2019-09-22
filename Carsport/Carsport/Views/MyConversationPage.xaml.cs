using Carsport.ViewModels;
using System;
using System.Linq;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Carsport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MyConversationPage : ContentPage
	{
        MyConversationsViewModel vm;
        MyConversationsViewModel VM
        {
            get => vm ?? (vm = MyConversationsViewModel.GetInstance());
        }

        public MyConversationPage ()
		{
			InitializeComponent ();

            messagesList.ItemAppearing += (sender, e) =>
            {
                var lastItem = messagesList.ItemsSource.Cast<MessageItemViewModel>().LastOrDefault();
                if (e.Item == lastItem)
                {
                    messagesList.ScrollTo(e.Item, ScrollToPosition.End, true);
                }
            };
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            VM.ConnectCommand.Execute(null);
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            VM.DisconnectCommand.Execute(null);
        }
    }
}