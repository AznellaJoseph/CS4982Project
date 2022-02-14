using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     Window for the Main Functionality (Login and Create Account)
    /// </summary>
    /// <seealso cref="Avalonia.Controls.Window" />
    public class MainWindow : Window
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
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