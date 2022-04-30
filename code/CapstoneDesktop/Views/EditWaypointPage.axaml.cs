using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for Edit Waypoint Functionality
    /// </summary>
    public class EditWaypointPage : ReactiveUserControl<EditWaypointPageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditWaypointPage"/> class.
        /// </summary>
        public EditWaypointPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
