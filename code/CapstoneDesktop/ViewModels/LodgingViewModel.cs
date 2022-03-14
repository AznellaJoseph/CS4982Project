using System;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for a single Lodging
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ReactiveViewModelBase" />
    public class LodgingViewModel : ReactiveViewModelBase
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LodgingViewModel" /> class.
        /// </summary>
        /// <param name="screen">The screen.</param>
        public LodgingViewModel(IScreen screen) : base(screen, Guid.NewGuid().ToString()[..5])
        {
            HostScreen = screen;
        }
    }
}