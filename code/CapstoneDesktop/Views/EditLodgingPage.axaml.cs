using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for Edit Lodging Functionality
    /// </summary>
    public class EditLodgingPage : ReactiveUserControl<EditLodgingPageViewModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EditLodgingPage"/> class.
        /// </summary>
        public EditLodgingPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
