$(".vanilla-calendar-date").click(
    function () {
        const events = $("#events");
        events.empty();
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
                url: `/trip/${tripId}/?handler=Ajax`,
                beforeSend: function(xhr) {
                    xhr.setRequestHeader("XSRF-TOKEN",
                        $('input:hidden[name="__RequestVerificationToken"]').val());
                },
                data: { selectedDate: date },
                success: function (result) {
                    result.data.forEach(function(waypoint, index) {
                        events.append(
                         `
                        <div class="event list-item" data-id=${waypoint.waypointId} data-listIndex=${index}>
                            <div class="info-section">
                                ${waypoint.startDate.slice(0, 10)} - ${waypoint.startDate.slice(0, 10)}
                            </div>
                            <div class="name-section">
                                ${waypoint.location}
                            </div>
                            <div class="icon-section removeButton" data-id=${waypoint.waypointId}>
                                Remove
                            </div>
                        </div>
                        `
                        );
                    });

                    $(".removeButton").click(function() {
                        const tripId = parseInt($("#tripId").attr("value"));
                        const id = parseInt($(this).data("id"));
                        const index = parseInt($(this).data("listIndex"));
                        $.ajax({
                            method: "GET",
                            url: `/trip/${tripId}/?handler=Remove`,
                            beforeSend: function(xhr) {
                                xhr.setRequestHeader("XSRF-TOKEN",
                                    $('input:hidden[name="__RequestVerificationToken"]').val());
                            },
                            data: { waypointId: id },
                            success: function(response) {
                                if (response.data) {
                                    $(`.event[listIndex=${index}]`).remove();
                                }
                            }
                        });
                    });
                }
            }
        );
    }
);