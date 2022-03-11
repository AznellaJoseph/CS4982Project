function _registerCalendarDayClick() {
    $(".vanilla-calendar-date").click(_onCalendarDayClick);
}

function _registerCalendarMonthArrowClick() {
    $(".vanilla-calendar-btn").click(_onMonthArrowClick);
}

function _onCalendarDayClick() {
    const events = $("#events");
    events.empty();
    const lodging = $("#lodging");
    lodging.empty();
    const pad = function(num) { return (`00${num}`).slice(-2) };
    let date = new Date($(this).attr("data-calendar-date"));
    date = date.getUTCFullYear() +
        "-" +
        pad(date.getUTCMonth() + 1) +
        "-" +
        pad(date.getUTCDate());

    const tripId = parseInt($("#tripId").attr("value"));
    $.ajax({
            method: "GET",
            url: `/trip/${tripId}/?handler=Events`,
            beforeSend: function(xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { selectedDate: date },
            success: _onGetEventsSuccess
        }
    );
    $.ajax({
            method: "GET",
            url: `/trip/${tripId}/?handler=Lodging`,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { selectedDate: date },
            success: _onGetLodgingSuccess
        }
    );

}

function _onRemoveClick() {
    const tripId = parseInt($("#tripId").attr("value"));
    const id = parseInt($(this).data("id"));
    const type = $(this).data("event-type");
    $.ajax({
        method: "GET",
        url: `/trip/${tripId}/?handler=Remove${type.charAt(0).toUpperCase() + type.slice(1)}`,
        beforeSend: function(xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: { id: id },
        success: (response) => _onRemoveEventSuccess(response, id)
    });
}

function _createEvent(event) {
    let id = event.method ? event.transportationId : event.waypointId
    let type = event.method ? "transportation" : "waypoint"
    
    $("#events").append(
        `
            <div class="event list-item" data-id=${id}>
                <div class="info-section">
                    ${event.startDate.slice(0, 10)} - ${event.endDate.slice(0, 10)}
                </div>
                <div class="name-section">
                    ${event.displayName}
                </div>
                <div class="icon-section removeButton" data-id="${id}" data-event-type="${type}">
                    Remove
                </div>
            </div>
        `);
}

function _createLodging(lodging) {

    $("#lodging").append(
        `
            <div class="list-item">
                <div class="info-section">
                    LODGING
                </div>
                <div class="name-section">
                    NAME SECTION
                </div>
                <div class="icon-section removeButton">
                    Remove
                </div>
            </div>
        `);
}

function _onGetEventsSuccess(response) {
    $("#events").empty()
    response.data.forEach(_createEvent);
    $(".removeButton").click(_onRemoveClick);
}

function _onGetLodgingSuccess(response) {
    $("#lodging").empty()
    response.data.forEach(_createLodging);
    $(".removeButton").click(_onRemoveClick);
}

function _onRemoveEventSuccess(response, id) {
    if (response.data) {
        $(`.event[data-id=${id}]`).remove();
    }
}

function _onMonthArrowClick() {
    _registerCalendarDayClick();
}

_registerCalendarDayClick();
_registerCalendarMonthArrowClick();