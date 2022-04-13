using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    /// <summary>
    /// User Control for the Landing Page Functionality
    /// </summary>
    /// <seealso cref="Avalonia.ReactiveUI.ReactiveUserControl&lt;LandingPageViewModel&gt;" />
    /// <seealso />
    public class LandingPage : ReactiveUserControl<LandingPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LandingPage" /> class.
        /// </summary>
        public LandingPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(_ => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}