using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{

    /// <summary>
    ///     Window for displaying all windows
    /// </summary>
    /// <seealso cref="Avalonia.Controls.Window" />
    public class MainWindow : ReactiveWindow<MainWindowViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MainWindow" /> class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}