namespace Carsport.Views
{
    using Common.Models;
    using Plugin.Geolocator;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xamarin.Forms;
    using Xamarin.Forms.Maps;
    using Xamarin.Forms.Xaml;

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BikePage : ContentPage
	{
		public BikePage ()
		{
			InitializeComponent ();

            pickerUniversity.SelectedIndexChanged += PickerUniversity_SelectedIndexChanged;

        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Locator();
        }

        private async void Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync();
            var position = new Position(location.Latitude, location.Longitude);
            this.ByciclesMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            try
            {
                this.ByciclesMap.IsShowingUser = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void PickerUniversity_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            var university = (Bycicle)pickerUniversity.SelectedItem;
            var pins = this.GetPins(university);
            this.ShowPins(pins);
        }

        private void ShowPins(List<Pin> pins)
        {
            foreach (var pin in pins)
            {
                try
                {
                    this.ByciclesMap.Pins.Add(pin);
                }
                catch (Exception e)
                {
                    var a = e.Message;
                }

            }
        }

        private List<Pin> GetPins(Bycicle uni)
        {
            List<Pin> pins = new List<Pin>();
            var position = new Position(double.Parse(uni.Latitude), double.Parse(uni.Longitude));

            pins.Add(new Pin
            {
                Label = uni.Street,
                Address = uni.Description,
                Position = position,
                Type = PinType.Place,
            });

            return pins;
        }

        private void Handle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var zoomLevel = double.Parse(e.NewValue.ToString()) * 18.0;
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            this.ByciclesMap.MoveToRegion(new MapSpan(this.ByciclesMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
        }
    }
}