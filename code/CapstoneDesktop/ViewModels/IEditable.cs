using System.Reactive;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public interface IEditable
    {
        /// <summary>
        ///     The edit command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> EditCommand { get; }
    }
}