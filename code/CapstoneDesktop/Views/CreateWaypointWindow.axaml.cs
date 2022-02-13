using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CapstoneDesktop.Views
{
    public class CreateWaypointWindow : Window
    {
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
