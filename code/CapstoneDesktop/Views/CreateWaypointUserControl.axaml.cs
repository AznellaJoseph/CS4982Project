using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for Create Waypoint Functionality
    /// </summary>
    /// <seealso cref="Avalonia.Controls.UserControl" />
    public class CreateWaypointUserControl : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateWaypointUserControl" /> class.
        /// </summary>
        public CreateWaypointUserControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}