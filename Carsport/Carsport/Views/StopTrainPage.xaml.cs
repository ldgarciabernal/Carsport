using Carsport.Models;
using Carsport.Services;
using Plugin.Geolocator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace Carsport.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StopTrainPage : ContentPage
	{
		public StopTrainPage ()
		{
			InitializeComponent ();

            pickerLines.SelectedIndexChanged += PickerLines_SelectedIndexChanged;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            this.Locator();
        }

        private async void PickerLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            var line = (Line)pickerLines.SelectedItem;
            var pins = await this.GetPins(line.LineId);
            this.ShowPins(pins);
        }

        private async void Locator()
        {
            var locator = CrossGeolocator.Current;
            locator.DesiredAccuracy = 50;

            var location = await locator.GetPositionAsync();
            var position = new Position(location.Latitude, location.Longitude);
            this.StopsMap.MoveToRegion(MapSpan.FromCenterAndRadius(position, Distance.FromKilometers(1)));

            try
            {
                this.StopsMap.IsShowingUser = true;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        private void ShowPins(List<Pin> pins)
        {
            foreach (var pin in pins)
            {
                try
                {
                    this.StopsMap.Pins.Add(pin);
                }
                catch (Exception e)
                {
                    var a = e.Message;
                }

            }
        }

        private async Task<List<Pin>> GetPins(string lineId)
        {
            var pins = new List<Pin>();
            var apiService = new ApiService();
            var url = Application.Current.Resources["APIConsorcio"].ToString();
            var consortiumId = Application.Current.Resources["ConsorcioId"].ToString();
            var response = await apiService.GetAsync<Stops>(url, consortiumId, $"/lineas/{lineId}/paradas");
            var stops = (Stops)response.Result;
            foreach (var item in stops.StopList)
            {
                var position = new Position(double.Parse(item.Latitude), double.Parse(item.Longitude));
                pins.Add(new Pin
                {
                    Label = item.Name,
                    Address = "(" + item.Latitude + ", " + item.Longitude + ")",
                    Position = position,
                    Type = PinType.Place,
                });
            }

            return pins;
        }


        private void Handle_ValueChanged(object sender, ValueChangedEventArgs e)
        {
            var zoomLevel = double.Parse(e.NewValue.ToString()) * 18.0;
            var latlongdegrees = 360 / (Math.Pow(2, zoomLevel));
            this.StopsMap.MoveToRegion(new MapSpan(this.StopsMap.VisibleRegion.Center, latlongdegrees, latlongdegrees));
        }
    }
}