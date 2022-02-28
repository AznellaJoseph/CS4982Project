$(".vanilla-calendar-date").click(
    function () {
        let pad = function (num) { return ('00' + num).slice(-2) };
        let date = new Date($(this).attr("data-calendar-date"));
        date = date.getUTCFullYear() +
            '-' +
            pad(date.getUTCMonth() + 1) +
            '-' +
            pad(date.getUTCDate());

        let tripId = parseInt($("#tripId").attr("value"));
        $.ajax({
            method: "GET",
            url: `/trip/${tripId}/?handler=Ajax`,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { selectedDate: date },
            success: function (result) {
                result.data.forEach(function (waypoint) {
                    let events = $("#events");
                    events.empty();
                    events.append(
                        `
                        <div class="event" data-id=${waypoint.waypointId}>
                            <p>
                                ${waypoint.startDate} - ${waypoint.startDate}
                            </p>
                            <p>
                                ${waypoint.location}
                            </p>
                            <button class="removeButton" data-id=${waypoint.waypointId}>Remove</button>
                        </div>
                        `
                    );
                });

                $(".removeButton").click(function () {
                    let tripId = parseInt($("#tripId").attr("value"));
                    let id = parseInt($(this).data("id"));
                    $.ajax({
                        method: "GET",
                        url: `/trip/${tripId}/?handler=Remove`,
                        beforeSend: function (xhr) {
                            xhr.setRequestHeader("XSRF-TOKEN",
                                $('input:hidden[name="__RequestVerificationToken"]').val());
                        },
                        data: { waypointId: id },
                        success: function (response) {
                            if (response.data) {
                                $(`.event[data-id=${id}]`).remove();
                            }
                        }
                    });
                });
            }
        }
        );
    }
);