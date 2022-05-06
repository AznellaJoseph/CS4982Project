using Avalonia.Controls;
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
    /// <summary>
    ///     User Control for the Event Page Functionality
    /// </summary>
    /// <seealso cref="EventPageViewModel" />
    public class EventPage : ReactiveUserControl<EventPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventPage" /> class.
        /// </summary>
        public EventPage()
        {
            InitializeComponent();
        }

        public GMapControl? MainMap { get; set; }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                var viewModel = (EventPageViewModel?) DataContext;
                if (viewModel?.Event is not Waypoint waypoint) return;
                MainMap = new GMapControl();
                var container = this.Get<Panel>("MapContainer");
                container.Children.Clear();
                container.Children.Add(MainMap);
                GoogleMapProvider.Instance.ApiKey = "AIzaSyB_TdvmfkvpMjDjMQnd3bDvhkNbrjRq5_I";
                MainMap.MapProvider = GMapProviders.GoogleMap;
                var result = GoogleGeocodeService.GetLocationByAddress(waypoint.Location);
                MainMap.Position = new PointLatLng(result.Latitude, result.Longitude);
                MainMap.FillEmptyTiles = true;
                disposables.Add(MainMap);
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}