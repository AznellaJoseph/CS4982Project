using CapstoneBackend.Model;

namespace CapstoneDesktop.ViewModels
{
    public interface IEventViewModel : IRemovable
    {
        public IEvent Event { get; }
    }
}