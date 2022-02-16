using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     Window for Create Waypoint Functionality
    /// </summary>
    /// <seealso cref="Avalonia.Controls.Window" />
    public class CreateWaypointWindow : Window
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointWindow" /> class.
        /// </summary>
        public CreateWaypointWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}