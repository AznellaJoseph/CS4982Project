using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for a single Lodging, used in the trip overview lodging list
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    public class LodgingViewModel : ReactiveViewModelBase, IRemovable, IEditable
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingViewModel" /> class.
        /// </summary>
        /// <param name="lodging">The lodging.</param>
        /// <param name="screen">The screen.</param>
        public LodgingViewModel(Lodging lodging, IScreen screen) : base(screen, Guid.NewGuid().ToString()[..5])
        {
            HostScreen = screen;
            Lodging = lodging;
            RemoveCommand = ReactiveCommand.Create(removeLodging);
            ViewCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LodgingPageViewModel(lodging, screen)));
            EditCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new EditLodgingPageViewModel(lodging, screen)));
        }

        /// <summary>
        ///     The lodging.
        /// </summary>
        public Lodging Lodging { get; }

        /// <summary>
        ///     The lodging manager.
        /// </summary>
        public LodgingManager LodgingManager { get; set; } = new();

        /// <summary>
        ///     The view command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> ViewCommand { get; }

        /// <summary>
        ///     The edit command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> EditCommand { get; }

        /// <summary>
        ///     The remove command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        /// <summary>
        ///     The remove event.
        /// </summary>
        public event EventHandler<EventArgs>? RemoveEvent;

        private void removeLodging()
        {
            if (LodgingManager.RemoveLodging(Lodging.LodgingId).Data) RemoveEvent?.Invoke(this, EventArgs.Empty);
        }
    }
}