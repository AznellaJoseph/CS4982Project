using Avalonia.Media.Imaging;
using CapstoneBackend.Model;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     IEventViewModel for the ViewModels that make use of the EventManager and Event Models
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.IRemovable" />
    public interface IEventViewModel : IRemovable
    {
        /// <summary>
        ///     The event.
        /// </summary>
        public IEvent Event { get; }
    }
}