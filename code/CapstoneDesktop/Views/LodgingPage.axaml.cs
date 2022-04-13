using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneBackend.Utils;
using CapstoneDesktop.ViewModels;
using GMap.NET;
using GMap.NET.Avalonia;
using GMap.NET.MapProviders;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for the Lodging Page Functionality
    /// </summary>
    /// <seealso cref="LodgingPageViewModel" />
    public class LodgingPage : ReactiveUserControl<LodgingPageViewModel>
    {

        public GMapControl? MainMap { get; set; }
        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingPage" /> class.
        /// </summary>
        public LodgingPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(_ =>
            {
                if (DataContext is null) return;
                var viewModel = (LodgingPageViewModel)DataContext;
                MainMap = new GMapControl();
                var lodging = viewModel.Lodging;
                var container = this.Get<Panel>("MapContainer");
                container.Children.Add(MainMap);
                GoogleMapProvider.Instance.ApiKey = "AIzaSyARhel5-jZFkChP1uASkhk0G7qYc5cRiWA";
                MainMap.MapProvider = GMapProviders.GoogleMap;
                var result = GoogleGeocodeService.GetLocationByAddress(lodging.Location);
                MainMap.Position = new PointLatLng(result.Latitude, result.Longitude);
                MainMap.FillEmptyTiles = true;
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}