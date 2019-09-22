namespace Carsport.Views
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Xamarin.Forms;
    using Xamarin.Forms.Xaml;
    using Carsport.ViewModels;
    using Carsport.Models;
    using Carsport.Services;
    using Carsport.Helpers;

    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SheduleBusPage : ContentPage
    {
        SheduleBusViewModel viewModel = SheduleBusViewModel.GetInstance();
        Timetable timetable = null;
        string firstCore = String.Empty;
        string secondCore = String.Empty;

        public SheduleBusPage()
        {
            InitializeComponent();

            this.LoadShedule(viewModel.DestinyCore.CoreId, viewModel.OriginCore.CoreId);
            firstCore = viewModel.DestinyCore.CoreId;
            secondCore = viewModel.OriginCore.CoreId;
            switchShedule.Clicked += SwitchShedule_Clicked;
        }

        private void SwitchShedule_Clicked(object sender, EventArgs e)
        {
            var aux = firstCore;
            firstCore = secondCore;
            secondCore = aux;
            originTitle.Text = (firstCore == viewModel.OriginCore.CoreId) ? viewModel.OriginCore.Name : viewModel.DestinyCore.Name;
            destinyTitle.Text = (secondCore == viewModel.DestinyCore.CoreId) ? viewModel.DestinyCore.Name : viewModel.DestinyCore.Name;
            this.LoadShedule(firstCore, secondCore);
        }

        private async void LoadShedule(string destinyId, string originId)
        {
            indicator.IsRunning = true;
            notShedule.IsVisible = false;
            sheduleBusTable.IsVisible = true;
            btnStops.IsEnabled = true;
            var apiService = new ApiService();
            var connection = await apiService.CheckConnection();
            if (!connection.IsSuccess)
            {
                indicator.IsRunning = false;
                await Application.Current.MainPage.DisplayAlert(
                    Languages.Error,
                    connection.Message,
                    Languages.AcceptDisplay);
                return;
            }

            var url = Application.Current.Resources["APIConsorcio"].ToString();
            var consortiumId = Application.Current.Resources["ConsorcioId"].ToString();
            var response = await apiService.GetAsync<Timetable>(
                url,
                consortiumId,
                $"/horarios_origen_destino?destino={destinyId}&lang=ES&origen={originId}");

            if (!response.IsSuccess)
            {
                indicator.IsRunning = false;
                notShedule.IsVisible = true;
                sheduleBusTable.IsVisible = false;
                btnStops.IsEnabled = false;
                return;
            }

            var timetableOrder = (Timetable)response.Result;
            var hours = timetableOrder.Shedules.Where(s => !s.Code.ToLower().Contains("C-1".ToLower())
                && !s.Code.ToLower().Contains("MD".ToLower())
                && !s.Code.ToLower().Contains("B".ToLower())).ToList();

            timetableOrder.Shedules = hours;
            var timeTable = new List<Timetable> {
                timetableOrder
            };

            this.timetable = timetableOrder;
            
            if (timetable != null)
            {
                foreach (var child in sheduleBusTable.Children.Reverse())
                {
                    var childTypeName = child.GetType().Name;
                    if (childTypeName == "Label")
                    {
                        sheduleBusTable.Children.Remove(child);
                    }
                }

                var gridWidth = timetable.Bloks.Count();
                var gridHeight = timetable.Shedules.Count();

                for (int i = 0; i < gridWidth; i++)
                {
                    sheduleBusTable.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(5, GridUnitType.Auto) });
                }

                for (int j = 0; j < gridHeight; j++)
                {
                    sheduleBusTable.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1.5, GridUnitType.Auto) });
                }

                for (int x = 0; x < gridHeight; x++)
                {
                    var count = 0;
                    for (int y = 0; y < gridWidth; y++)
                    {
                        if (x == 0)
                        {
                            var label = new Label
                            {
                                Text = timetable.Bloks[y].Name,
                                BackgroundColor = Color.FromHex(timetable.Bloks[y].Colour),
                                FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                            };

                            sheduleBusTable.Children.Add(label, y, x);
                        }
                        else
                        {
                            if (y == 0)
                            {
                                var label = new Label
                                {
                                    Text = timetable.Shedules[x].Code,
                                    BackgroundColor = (x % 2 == 0) ? Color.White : Color.FromHex("#ccddff"),
                                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                };

                                sheduleBusTable.Children.Add(label, y, x);
                            }

                            if (y == gridWidth-1)
                            {
                                var label = new Label
                                {
                                    Text = timetable.Shedules[x].Remarks,
                                    BackgroundColor = (x % 2 == 0) ? Color.White : Color.FromHex("#ccddff"),
                                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                };

                                sheduleBusTable.Children.Add(label, y, x);
                            }

                            if (y == gridWidth-2)
                            {
                                var label = new Label
                                {
                                    Text = timetable.Shedules[x].Days,
                                    BackgroundColor = (x % 2 == 0) ? Color.White : Color.FromHex("#ccddff"),
                                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                };

                                sheduleBusTable.Children.Add(label, y, x);
                            }

                            if (y != gridWidth - 2 && y != gridWidth - 1 && y != 0)
                            {
                                var label = new Label
                                {
                                    Text = timetable.Shedules[x].Hours[count],
                                    BackgroundColor = (x % 2 == 0) ? Color.White : Color.FromHex("#ccddff"),
                                    FontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label)),
                                };

                                sheduleBusTable.Children.Add(label, y, x);
                                count++;
                            }
                        }
                    }
                }

            } else
            {
                notShedule.IsVisible = true;
                sheduleBusTable.IsVisible = false;
            }
            indicator.IsRunning = false;
        }
    }
}