var currentDate = null;

function _registerCalendarDayClick() {
    $(".vanilla-calendar-date").click(_onCalendarDayClick);
}

function _registerCalendarMonthArrowClick() {
    $(".vanilla-calendar-btn").click(_onMonthArrowClick);
}

function _onCalendarDayClick() {
    const events = $("#events-list");
    events.empty();

    const pad = function (num) { return (`00${num}`).slice(-2) };
    let date = new Date($(this).attr("data-calendar-date"));
    currentDate = date;
    date = date.getUTCFullYear() +
        "-" +
        pad(date.getUTCMonth() + 1) +
        "-" +
        pad(date.getUTCDate());

    const tripId = parseInt($("#tripId").attr("value"));
    $.ajax({
        method: "GET",
        url: `/trip/${tripId}/?handler=Events`,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: { selectedDate: date },
        success: _onGetEventsSuccess
    }
    );
}

function _formatDate(event) {
    let startDate = new Date(event.startDate);
    let endDate = new Date(event.endDate);

    let options = { hour: '2-digit', minute: '2-digit' };

    if (startDate.getDate() != endDate.getDate()) {
        if (startDate.getDate() < currentDate.getDate()) {
            startDate = "PREV";
        }
        else {
            startDate = startDate.toLocaleTimeString('en-US', options);
        }

        if (endDate.getDate() > currentDate.getDate()) {
            endDate = "CONT'D";
        }
        else {
            endDate = endDate.toLocaleTimeString('en-US', options);
        }
    }
    else {
        startDate = startDate.toLocaleTimeString('en-US', options);
        endDate = endDate.toLocaleTimeString('en-US', options);
    }

    return `${ startDate } - ${ endDate }`
}

function _onRemoveClick() {
    const tripId = parseInt($("#tripId").attr("value"));
    const id = parseInt($(this).data("id"));
    let type = $(this).data("event-type");

    $.ajax({
        method: "GET",
        url: `/trip/${tripId}/?handler=Remove${type}`,
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: { id: id },
        success: (response) => _onRemoveEventSuccess(response, id)
    });
}

function _createEvent(event, _index) {
    let id = event.id;
    let type = event.eventType;
    const tripId = parseInt($("#tripId").attr("value"));

    $("#events-list").append(
        `
        <div class="event list-item" data-id=${id}>
            <div class="icon-section">
                <img src="../../png/${type}_icon.png" alt="${type}"/>
            </div>
            <div class="info-section">
                ${_formatDate(event)}
            </div>
            <div class="name-section">
                ${event.displayName}
            </div>

            <div class="icon-section">
                <a href="/trip/${tripId}/?handler=View${type}&id=${id}">
                    <img src="../../png/view_icon.png" alt="View"/>
                </a>
                <a href="/trip/${tripId}/${type}/edit/${id}">
                    <img src="../../png/edit_icon.png" alt="Edit"/>
                </a>
                <div class="icon-section removeButton" data-id="${id}" data-event-type="${type}">
                    <img src="../../png/remove_icon.png" alt="Remove"/>
                </div>
            </div>
        </div>
    `);
}

function _onGetEventsSuccess(response) {
    $("#events-list").empty();
    response.data.forEach(_createEvent);
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