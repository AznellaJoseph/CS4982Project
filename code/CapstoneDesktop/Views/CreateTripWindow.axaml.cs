using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CapstoneDesktop.Views
{
    public partial class CreateTripWindow : Window
    {
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
