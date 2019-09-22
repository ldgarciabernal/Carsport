using Carsport.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Carsport.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class RealTimeChatPage : ContentPage
    {
        RealTimeChatViewModel vm;
        RealTimeChatViewModel VM
        {
            get => vm ?? (vm = RealTimeChatViewModel.GetInstance());
        }
        public RealTimeChatPage()
        {
            InitializeComponent();
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