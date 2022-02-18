using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     Window for Create Trip Functionality
    /// </summary>
    /// <seealso cref="Avalonia.Controls.Window" />
    public class CreateTripPage : ReactiveUserControl<CreateTripPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripPage" /> class.
        /// </summary>
        public CreateTripPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}