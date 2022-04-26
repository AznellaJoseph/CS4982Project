using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     Page for CreateAccount
    /// </summary>
    /// <seealso cref="Avalonia.ReactiveUI.ReactiveUserControl&lt;CreateAccountPageViewModel&gt;" />
    public class CreateAccountPage : ReactiveUserControl<CreateAccountPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateAccountPage" /> class.
        /// </summary>
        public CreateAccountPage()
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