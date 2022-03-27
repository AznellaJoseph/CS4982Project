using System.Reactive;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public interface IViewable
    {
        /// <summary>
        ///     The view command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> ViewCommand { get; }
    }
}