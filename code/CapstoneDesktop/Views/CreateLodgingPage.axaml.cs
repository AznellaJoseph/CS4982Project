using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;

namespace CapstoneDesktop.Views
{
    public class CreateLodgingPage : ReactiveUserControl<CreateLodgingPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateLodgingPage" /> class.
        /// </summary>
        public CreateLodgingPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}