using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the TripOverview Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    /// <seealso cref="ReactiveUI.IRoutableViewModel" />
    public class TripOverviewPageViewModel : ReactiveViewModelBase
    {
        private DateTime? _selectedDate;

        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewPageViewModel" /> class.
        /// </summary>
        /// <param name="trip">The trip being viewed.</param>
        /// <param name="screen">The screen.</param>
        /// <param name="lodgingManager">The lodging manager.</param>
        public TripOverviewPageViewModel(Trip trip,
            IScreen screen, LodgingManager lodgingManager) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            Trip = trip;
            HostScreen = screen;
            LodgingManager = lodgingManager;
            LogoutCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new LoginPageViewModel(HostScreen)));
            CreateWaypointCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateWaypointPageViewModel(Trip, HostScreen)));
            CreateTransportationCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateTransportationPageViewModel(Trip, HostScreen)));
            CreateLodgingCommand = ReactiveCommand.CreateFromObservable(() =>
                HostScreen.Router.Navigate.Execute(new CreateLodgingPageViewModel(Trip, HostScreen)));
            BackCommand = ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
            EventViewModels = new ObservableCollection<IEventViewModel>();
            LodgingViewModels = new ObservableCollection<LodgingViewModel>();
            updateLodging();
        }

        /// <summary>
        ///     The event manager.
        /// </summary>
        public EventManager EventManager { get; set; } = new();

        /// <summary>
        ///     The lodging manager.
        /// </summary>
        public LodgingManager LodgingManager { get; set; }

        /// <summary>
        ///     The logout command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> LogoutCommand { get; }

        /// <summary>
        ///     The back command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> BackCommand { get; }

        /// <summary>
        ///     The create waypoint command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateWaypointCommand { get; }

        /// <summary>
        ///     The create transportation command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateTransportationCommand { get; }

        /// <summary>
        ///     The create lodging command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateLodgingCommand { get; }

        /// <summary>
        ///     The event viewmodels.
        /// </summary>
        public ObservableCollection<IEventViewModel> EventViewModels { get; }

        /// <summary>
        ///     The lodging viewmodels.
        /// </summary>
        public ObservableCollection<LodgingViewModel> LodgingViewModels { get; }

        /// <summary>
        ///     The trip.
        /// </summary>
        public Trip Trip { get; }

        /// <summary>
        ///     The selected date.
        /// </summary>
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                this.RaiseAndSetIfChanged(ref _selectedDate, value, nameof(SelectedDate));
                updateEvents();
            }
        }

        private void updateLodging()
        {
            var response = LodgingManager.GetLodgingsByTripId(Trip.TripId);

            foreach (var lodging in response.Data ?? new List<Lodging>())
            {
                var lodgingViewModel = new LodgingViewModel(lodging, HostScreen);
                lodgingViewModel.RemoveEvent += (sender, _) =>
                {
                    if (sender is not null) LodgingViewModels.Remove(lodgingViewModel);
                };
                LodgingViewModels.Add(lodgingViewModel);
            }
        }

        private void updateEvents()
        {
            EventViewModels.Clear();
            if (SelectedDate is null) return;
            var response = EventManager.GetEventsOnDate(Trip.TripId, SelectedDate.Value);

            foreach (var aEvent in response.Data ?? new List<IEvent>())
            {
                IEventViewModel viewModel;
                switch (aEvent)
                {
                    case Waypoint waypoint:
                        viewModel = new WaypointViewModel(waypoint, HostScreen);
                        break;
                    case Transportation transportation:
                        viewModel = new TransportationViewModel(transportation, HostScreen);
                        break;
                    default:
                        return;
                }

                viewModel.RemoveEvent += (sender, _) =>
                {
                    if (sender is not null)
                        EventViewModels.Remove((IEventViewModel) sender);
                };
                EventViewModels.Add(viewModel);
            }
        }
    }
}