using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     UserControl for CreateTrip Functionality
    /// </summary>
    /// <seealso cref="Avalonia.ReactiveUI.ReactiveUserControl&lt;CapstoneDesktop.Views.CreateTripUserControl&gt;" />
    public class CreateTripUserControl : UserControl
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripUserControl" /> class.
        /// </summary>
        public CreateTripUserControl()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}