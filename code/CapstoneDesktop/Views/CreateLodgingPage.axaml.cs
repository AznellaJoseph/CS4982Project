using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using GMap.NET;
using GMap.NET.Avalonia;
using GMap.NET.MapProviders;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    public class CreateLodgingPage : ReactiveUserControl<CreateLodgingPageViewModel>
    {

        public GMapControl MainMap { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CreateLodgingPage"/> class.
        /// </summary>
        public CreateLodgingPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                MainMap = new GMapControl();
                var container = this.Get<Panel>("MapContainer");
                container.Children.Add(MainMap);
                GoogleMapProvider.Instance.ApiKey = "AIzaSyDmYx_C23N0TLFO234gBQBBL3EMZ9HYIG4";
                MainMap.MapProvider = GMapProviders.GoogleMap;
                MainMap.Position = new PointLatLng(30, -30);
                MainMap.FillEmptyTiles = true;
            });
            AvaloniaXamlLoader.Load(this);
        }

        private void AutoCompleteBox_OnDropDownClosed(object? sender, EventArgs e)
        {
            if (DataContext is null) return;
            var viewmodel = (CreateLodgingPageViewModel)DataContext;
            var result = GoogleGeocodeService.GetLocationByAddress(viewmodel.Location);
            MainMap.Position = new PointLatLng(result.Latitude, result.Longitude);
            MainMap.FillEmptyTiles = true;
        }
    }
}
