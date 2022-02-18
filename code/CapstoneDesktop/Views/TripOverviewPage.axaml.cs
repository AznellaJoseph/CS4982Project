using System;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using CapstoneDesktop.ViewModels;
using ReactiveUI;

namespace CapstoneDesktop.Views
{
    public class TripOverviewPage : ReactiveUserControl<TripOverviewPageViewModel>
    {

        public TripOverviewPage()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.WhenActivated(disposables =>
            {
                if (this.DataContext is not null)
                {
                    var context = (TripOverviewPageViewModel)this.DataContext;
                    var calendar = this.FindControl<Calendar>("TripCalendar");
                    calendar.BlackoutDates?.Add(new CalendarDateRange(DateTime.MinValue, context.Trip.StartDate.AddDays(-1)));
                    calendar.BlackoutDates?.Add(new CalendarDateRange(context.Trip.EndDate.AddDays(1), DateTime.MaxValue));
                }
            });
            AvaloniaXamlLoader.Load(this);
        }
    }
}