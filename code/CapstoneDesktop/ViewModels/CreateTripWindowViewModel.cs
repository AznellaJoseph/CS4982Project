using System;
using System.Diagnostics;
using System.Reactive;
using CapstoneBackend.Model;
using ReactiveUI;

namespace CapstoneDesktop.ViewModels
{
    /// <summary>
    ///     ViewModel for Create Trip Window
    /// </summary>
    /// <seealso cref="CapstoneDesktop.ViewModels.ViewModelBase" />
    public class CreateTripWindowViewModel : ViewModelBase
    {
        private readonly TripManager _tripManager;

        private string _error = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripWindowViewModel" /> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public CreateTripWindowViewModel(TripManager manager)
        {
            _tripManager = manager;
            CreateTripCommand = ReactiveCommand.Create(createTrip);
            CancelCreateTripCommand = ReactiveCommand.Create(cancelCreateTrip);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CreateTripWindowViewModel" /> class.
        /// </summary>
        public CreateTripWindowViewModel() : this(new TripManager())
        {
        }

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
        public DateTime StartDate { get; set; } = DateTime.Today;

        /// <summary>
        /// The end date.
        /// </summary>
        public DateTime EndDate { get; set; } = DateTime.Today.AddDays(1);

        /// <summary>
        /// The create trip command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CreateTripCommand { get; set; }

        /// <summary>
        /// The cancel create trip command.
        /// </summary>
        public ReactiveCommand<Unit, Unit> CancelCreateTripCommand { get; set; }

        private void createTrip()
        {
            if (string.IsNullOrEmpty(TripName))
            {
                ErrorMessage = "You must enter a name for the trip";
            }

            else

            {
                var resultResponse = _tripManager.CreateTrip(0, TripName, Notes, StartDate, EndDate);
                if (!string.IsNullOrEmpty(resultResponse.ErrorMessage))
                    ErrorMessage = resultResponse.ErrorMessage;
                else
                    Debug.WriteLine("Trip successfully created");
            }
        }

        private void cancelCreateTrip()
        {
        }
    }
}