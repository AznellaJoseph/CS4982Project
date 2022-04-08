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

        public GMapControl MainMap { get; set; }
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventPage" /> class.
        /// </summary>
        public EventPage()
        {
            InitializeComponent();


        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                EventPageViewModel? viewModel = (EventPageViewModel?)DataContext;
                if (viewModel?.Event is Waypoint waypoint)
                {
                    MainMap = new GMapControl();
                    var container = this.Get<Panel>("MapContainer");
                    container.Children.Add(MainMap);
                    GoogleMapProvider.Instance.ApiKey = "AIzaSyARhel5-jZFkChP1uASkhk0G7qYc5cRiWA";
                    MainMap.MapProvider = GMapProviders.GoogleMap;
                    var result = GoogleGeocodeService.GetLocationByAddress(waypoint.Location);
                    MainMap.Position = new PointLatLng(result.Latitude, result.Longitude);
                    MainMap.FillEmptyTiles = true;
                }
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}