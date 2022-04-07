using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Event Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class EventPageViewModel : ReactiveViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EventPageViewModel" /> class.
        /// </summary>
        /// <param name="selectedEvent">The selected event being displayed.</param>
        /// <param name="screen">The screen.</param>
        public EventPageViewModel(IEvent selectedEvent, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            Event = selectedEvent;
            BackCommand = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
            LogoutCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LoginPageViewModel(HostScreen)));
        }

        /// <summary>
        ///     The event.
        /// </summary>
        public IEvent Event { get; }

        /// <summary>
        ///     The back command
        /// </summary>
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <summary>
        ///     The logout command
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> LogoutCommand { get; }
    }
}