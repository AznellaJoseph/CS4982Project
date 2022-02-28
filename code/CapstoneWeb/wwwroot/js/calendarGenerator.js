const startDateString = document.getElementById("startDate").value;
const endDateString = document.getElementById("endDate").value;

const tripStartDate = new Date(startDateString);
const tripEndDate = new Date(endDateString);


Date.prototype.addDays = function(days) {
    const date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
};

function getDateRange(startDate, endDate) {
    const dateRange = [];

    let currentDate = startDate;
    while (currentDate <= endDate) {
        dateRange.push(new Date(currentDate));
        currentDate = currentDate.addDays(1);
    }

    return dateRange;
}

function formatAvailableDates(dateRange) {
    const calendarObject = [];

    dateRange.forEach(date => {
        const calendarItem = {
            date: date.toISOString().slice(0, 10)
        };

        calendarObject.push(calendarItem);
    });

    return calendarObject;
}

const tripDateRange = getDateRange(tripStartDate, tripEndDate);
const calendarData = formatAvailableDates(tripDateRange);


let myCalendar = new VanillaCalendar({
    selector: "#myCalendar",
    availableDates: calendarData,
    datesFilter: true
});