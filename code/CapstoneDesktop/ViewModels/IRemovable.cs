using System;
using System.Reactive;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    public interface IRemovable
    {
        public ReactiveCommand<Unit, Unit> RemoveCommand { get; }

        public event EventHandler<EventArgs> RemoveEvent;
    }
}