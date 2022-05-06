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
        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingPage" /> class.
        /// </summary>
        public LodgingPage()
        {
            InitializeComponent();
        }

        public GMapControl? MainMap { get; set; }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                if (DataContext is null) return;
                var viewModel = (LodgingPageViewModel) DataContext;
                MainMap = new GMapControl();
                var lodging = viewModel.Lodging;
                var container = this.Get<Panel>("MapContainer");
                container.Children.Clear();
                container.Children.Add(MainMap);
                GoogleMapProvider.Instance.ApiKey = "AIzaSyB_TdvmfkvpMjDjMQnd3bDvhkNbrjRq5_I";
                MainMap.MapProvider = GMapProviders.GoogleMap;
                var result = GoogleGeocodeService.GetLocationByAddress(lodging.Location);
                MainMap.Position = new PointLatLng(result.Latitude, result.Longitude);
                MainMap.FillEmptyTiles = true;
                disposables.Add(MainMap);
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}