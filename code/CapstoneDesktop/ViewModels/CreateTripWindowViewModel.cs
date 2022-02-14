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
        /// Initializes a new instance of the <see cref="CreateTripWindowViewModel"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        public CreateTripWindowViewModel(TripManager manager)
        {
            _tripManager = manager;
            CreateTripCommand = ReactiveCommand.Create(createTrip);
            CancelCreateTripCommand = ReactiveCommand.Create(cancelCreateTrip);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="CreateTripWindowViewModel"/> class.
        /// </summary>
        public CreateTripWindowViewModel() : this(new TripManager())
        {
        }

        /// <summary>
        /// The error message.
        /// </summary>
        public string ErrorMessage
        {
            get => _error;
            set => this.RaiseAndSetIfChanged(ref _error, value);
        }

        public string? TripName { get; set; }
        public string? Notes { get; set; }
        public DateTime StartDate { get; set; } = DateTime.Today;
        public DateTime EndDate { get; set; }

        public ReactiveCommand<Unit, Unit> CreateTripCommand { get; set; }
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