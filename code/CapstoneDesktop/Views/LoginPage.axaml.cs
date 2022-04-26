using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for Login Functionality
    /// </summary>
    /// <seealso cref="Avalonia.ReactiveUI.ReactiveUserControl&lt;LoginPageViewModel&gt;" />
    public class LoginPage : ReactiveUserControl<LoginPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LoginPage" /> class.
        /// </summary>
        public LoginPage()
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