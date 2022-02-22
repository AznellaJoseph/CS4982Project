using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     Page for Create Waypoint Functionality
    /// </summary>
    public class CreateWaypointPage : ReactiveUserControl<CreateWaypointPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointPage" /> class.
        /// </summary>
        public CreateWaypointPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}