using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     Window for Create Trip Functionality
    /// </summary>
    /// <seealso cref="Avalonia.Controls.Window" />
    public class CreateTripWindow : Window
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripWindow" /> class.
        /// </summary>
        public CreateTripWindow()
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