let startDateString = document.getElementById("startDate").value;
let endDateString = document.getElementById("endDate").value;

const tripStartDate = new Date(startDateString);
const tripEndDate = new Date(endDateString)


Date.prototype.addDays = function (days) {
    var date = new Date(this.valueOf());
    date.setDate(date.getDate() + days);
    return date;
}

function getDateRange(startDate, endDate) {
    var dateRange = [];

    var currentDate = startDate;
    while (currentDate <= endDate) {
        dateRange.push(new Date(currentDate));
        currentDate = currentDate.addDays(1);
    }

    return dateRange;
}

function formatAvailableDates(dateRange) {
    calendarObject = [];

    dateRange.forEach(date => {
        calendarItem = {
            date: date.toISOString().slice(0, 10)
        }

        calendarObject.push(calendarItem);
    });

    return calendarObject;
}

let tripDateRange = getDateRange(tripStartDate, tripEndDate);
let calendarObject = formatAvailableDates(tripDateRange);


let myCalendar = new VanillaCalendar({
    selector: "#myCalendar",
    availableDates: calendarObject,
    datesFilter: true
});