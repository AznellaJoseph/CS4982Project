using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for Edit Transportation Functionality
    /// </summary>
    public class EditTransportationPage : ReactiveUserControl<EditTransportationPageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditTransportationPage"/> class.
        /// </summary>
        public EditTransportationPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
