using System;
using System.Diagnostics;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for Create Trip Window
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateTripPageViewModel : ViewModelBase, IRoutableViewModel
    {
        private readonly TripManager _tripManager;
        private readonly User _user;

        private string _error = string.Empty;

        public IScreen HostScreen { get; }
        public string? UrlPathSegment { get; } = Guid.NewGuid().ToString().Substring(0, 5);

        /// <summary>
        /// The create trip command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateTripCommand { get; }

        /// <summary>
        /// The cancel create trip command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateTripCommand { get; }
        
        /// <summary>
        ///     The error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        /// <summary>
        /// The trip name.
        /// </summary>
        public string? TripName { get; set; }

        /// <summary>
        /// The notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// The start date.
        /// </summary>
        public DateTimeOffset? StartDate
        {
            get;
            set;
        }

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripPageViewModel" /> class.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">the host screen</param>
        public CreateTripPageViewModel(User user, TripManager manager, IScreen screen)
        {
            _tripManager = manager;
            this._user = user;
            this.HostScreen = screen;
            CreateTripCommand = ReactiveCommand.CreateFromObservable(createTrip);
            CancelCreateTripCommand = ReactiveCommand.CreateFromObservable(() => this.HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripPageViewModel" /> class.
        /// </summary>
        public CreateTripPageViewModel(User user, IScreen screen) : this(user ,new TripManager(), screen)
        {
        }

        private IObservable<IRoutableViewModel> createTrip()
        {
            if (string.IsNullOrEmpty(TripName))
            {
                ErrorMessage = "You must enter a name for the trip.";
                return Observable.Empty<IRoutableViewModel>();
            }
            if (this.StartDate is null || this.EndDate is null)
            {
                ErrorMessage = "You must enter a start and end date for the trip.";
                return Observable.Empty<IRoutableViewModel>();
            }
            var resultResponse = _tripManager.CreateTrip(this._user.UserId, TripName, Notes, StartDate.Value.Date, EndDate.Value.Date);
            if (resultResponse.StatusCode != 200U || resultResponse.ErrorMessage is not null)
            {
                ErrorMessage = resultResponse.ErrorMessage ?? string.Empty;
                return Observable.Empty<IRoutableViewModel>();
            }
            return this.HostScreen.Router.Navigate.Execute(new LandingPageViewModel(this._user, this.HostScreen));
        }
    }
}