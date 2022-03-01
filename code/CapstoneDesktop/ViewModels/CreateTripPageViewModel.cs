using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for Create Trip Window
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateTripPageViewModel : ReactiveViewModelBase
    {
        private readonly TripManager _tripManager;
        private readonly User _user;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripPageViewModel" /> class.
        /// </summary>
        /// <param name="user">The current user</param>
        /// <param name="manager">The manager.</param>
        /// <param name="screen">the host screen</param>
        public CreateTripPageViewModel(User user, TripManager manager, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _tripManager = manager;
            _user = user;
            HostScreen = screen;
            CreateTripCommand = ReactiveCommand.CreateFromObservable(createTrip);
            CancelCreateTripCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripPageViewModel" /> class.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <param name="screen">The screen.</param>
        public CreateTripPageViewModel(User user, IScreen screen) : this(user, new TripManager(), screen)
        {
        }

        /// <summary>
        ///     The create trip command.
        /// </summary>
        public ReactiveCommand<Unit, IRoutableViewModel> CreateTripCommand { get; }

        /// <summary>
        ///     The cancel create trip command.
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
        ///     The trip name.
        /// </summary>
        public string? TripName { get; set; }

        /// <summary>
        ///     The notes.
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        ///     The start date.
        /// </summary>
        public DateTimeOffset? StartDate { get; set; }

        /// <summary>
        ///     The end date.
        /// </summary>
        public DateTimeOffset? EndDate { get; set; }

        private IObservable<IRoutableViewModel> createTrip()
        {
            if (string.IsNullOrEmpty(TripName))
            {
                ErrorMessage = Ui.ErrorMessages.EmptyTripName;
                return Observable.Empty<IRoutableViewModel>();
            }

            if (StartDate is null || EndDate is null)
            {
                ErrorMessage = Ui.ErrorMessages.NullTripDate;
                return Observable.Empty<IRoutableViewModel>();
            }

            var resultResponse =
                _tripManager.CreateTrip(_user.UserId, TripName, Notes, StartDate.Value.Date, EndDate.Value.Date);
            if (string.IsNullOrEmpty(resultResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(new LandingPageViewModel(_user, HostScreen));

            ErrorMessage = resultResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}