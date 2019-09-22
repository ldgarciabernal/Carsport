namespace Carsport.Views
{
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MasterPage : MasterDetailPage
	{
		public MasterPage ()
		{
			InitializeComponent ();
           
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            App.Navigator = Navigator;
            App.Master = this;

            App.Navigator.BarBackgroundColor = Color.FromHex("#0D1935");
        }

    }
}