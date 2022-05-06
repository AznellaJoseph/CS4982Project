using System;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the Lodging Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class LodgingPageViewModel : ReactiveViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingPageViewModel" /> class.
        /// </summary>
        /// <param name="lodging">The selected lodging to view.</param>
        /// <param name="screen">The screen.</param>
        public LodgingPageViewModel(Lodging lodging, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            Lodging = lodging;
            BackCommand = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
            LogoutCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LoginPageViewModel(HostScreen)));
        }

        /// <summary>
        ///     The lodging
        /// </summary>
        public Lodging Lodging { get; }

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