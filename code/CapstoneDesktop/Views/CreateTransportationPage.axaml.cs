using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for Create Waypoint Functionality
    /// </summary>
    public class CreateTransportationPage : ReactiveUserControl<CreateTransportationPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPage" /> class.
        /// </summary>
        public CreateTransportationPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}