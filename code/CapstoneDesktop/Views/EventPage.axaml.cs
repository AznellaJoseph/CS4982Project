using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
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
            
            GoogleMapProvider.Instance.ApiKey = "AIzaSyARhel5-jZFkChP1uASkhk0G7qYc5cRiWA";

            MainMap = this.Get<GMapControl>("GMap");
            MainMap.MapProvider = GMapProviders.GoogleMap;
            MainMap.Position = new PointLatLng(44.4268, 26.1025);
            MainMap.FillEmptyTiles = true;
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}