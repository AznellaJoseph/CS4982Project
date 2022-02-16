using System.Collections.ObjectModel;
using System.Reactive;
using CapstoneDesktop.Views;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public class WaypointListViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> CreateWaypointCommand { get; }
        public ObservableCollection<WaypointViewModel> WaypointViewModels { get; set; }
    }
}