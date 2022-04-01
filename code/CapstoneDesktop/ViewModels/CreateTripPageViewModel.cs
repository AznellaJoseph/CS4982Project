using System;
using System.Reactive;
using System.Reactive.Linq;
using CapstoneBackend.Model;
using CapstoneBackend.Utils;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for the CreateTrip Page
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateTripPageViewModel : ReactiveViewModelBase
    {
        private readonly User _user;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripPageViewModel" /> class.
        /// </summary>
        /// <param name="user">The user that the trip will be created for</param>
        /// <param name="screen">the screen</param>
        public CreateTripPageViewModel(User user, IScreen screen) : base(screen,
            Guid.NewGuid().ToString()[..5])
        {
            _user = user;
            HostScreen = screen;
            CreateTripCommand = ReactiveCommand.CreateFromObservable(createTrip);
            CancelCreateTripCommand =
                ReactiveCommand.CreateFromObservable(() => HostScreen.Router.NavigateBack.Execute());
        }

        /// <summary>
        ///     The trip manager.
        /// </summary>
        public TripManager TripManager { get; set; } = new();

        /// <summary>
        ///     The validation manager.
        /// </summary>
        public ValidationManager ValidationManager { get; set; } = new();

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

            var clashingTripResponse =
                ValidationManager.DetermineIfClashingTripExists(_user.UserId, StartDate.Value.Date, EndDate.Value.Date);

            if (!string.IsNullOrEmpty(clashingTripResponse.ErrorMessage))
            {
                ErrorMessage = clashingTripResponse.ErrorMessage;
                return Observable.Empty<IRoutableViewModel>();
            }

            var resultResponse =
                TripManager.CreateTrip(_user.UserId, TripName, Notes, StartDate.Value.Date, EndDate.Value.Date);
            if (string.IsNullOrEmpty(resultResponse.ErrorMessage))
                return HostScreen.Router.Navigate.Execute(
                    new LandingPageViewModel(_user, HostScreen, TripManager));

            ErrorMessage = resultResponse.ErrorMessage;
            return Observable.Empty<IRoutableViewModel>();
        }
    }
}