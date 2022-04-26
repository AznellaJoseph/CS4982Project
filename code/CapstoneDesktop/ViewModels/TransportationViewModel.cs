using System;
using System.Reactive;
using Avalonia;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for a single Transportation
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    /// <seealso cref="CapstoneDesktop.ViewModels.IEventViewModel" />
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class TransportationViewModel : ReactiveViewModelBase, IEventViewModel
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TransportationViewModel" /> class.
        /// </summary>
        /// <param name="transportation">The transportation.</param>
        /// <param name="screen">The screen.</param>
        public TransportationViewModel(Transportation transportation, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            HostScreen = screen;
            Transportation = transportation;
            RemoveCommand = ReactiveCommand.Create(removeTransportation);
            ViewCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new EventPageViewModel(transportation, screen)));
            EditCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new EventPageViewModel(transportation, screen)));
        }

        /// <summary>
        ///     The transportation
        /// </summary>
        public Transportation Transportation { get; }

        /// <summary>
        ///     The transportation manager
        /// </summary>
        public TransportationManager TransportationManager { get; set; } = new();

        /// <summary>
        ///     The remove command
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        /// <summary>
        ///     The remove event
        /// </summary>
        public event EventHandler<EventArgs>? RemoveEvent;

        /// <summary>
        ///     The view command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> ViewCommand { get; }

        /// <summary>
        ///     The edit command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> EditCommand { get; }

        /// <summary>
        ///     The Event.
        /// </summary>
        public IEvent Event => Transportation;

        /// <summary>
        ///     The image path.
        /// </summary>
        public string ImagePath => "../Assets/transportation_icon.png";

        private void removeTransportation()
        {
            if (TransportationManager.RemoveTransportation(Transportation.TransportationId).Data)
                RemoveEvent?.Invoke(this, EventArgs.Empty);
        }

    }
}