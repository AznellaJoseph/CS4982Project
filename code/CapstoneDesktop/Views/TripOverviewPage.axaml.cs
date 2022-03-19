using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    /// <summary>
    ///     User Control for the Trip Overview Functionality
    /// </summary>
    /// <seealso cref="Avalonia.ReactiveUI.ReactiveUserControl&lt;CapstoneDesktop.ViewModels.TripOverviewPageViewModel&gt;" />
    public class TripOverviewPage : ReactiveUserControl<TripOverviewPageViewModel>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TripOverviewPage" /> class.
        /// </summary>
        public TripOverviewPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                if (DataContext is null) return;
                var context = (TripOverviewPageViewModel) DataContext;
                var calendar = this.FindControl<Calendar>("TripCalendar");
                calendar.BlackoutDates?.Add(
                    new CalendarDateRange(DateTime.MinValue, context.Trip.StartDate.AddDays(-1)));
                calendar.BlackoutDates?.Add(new CalendarDateRange(context.Trip.EndDate.AddDays(1), DateTime.MaxValue));
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}