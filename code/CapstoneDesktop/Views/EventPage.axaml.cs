using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for the Event Page Functionality
    /// </summary>
    /// <seealso cref="Avalonia.ReactiveUI.ReactiveUserControl&lt;CapstoneDesktop.ViewModels.EventPageViewModel&gt;" />
    public class EventPage : ReactiveUserControl<EventPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventPage" /> class.
        /// </summary>
        public EventPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables => { });
            AvaloniaXamlLoader.Load(this);
        }
    }
}