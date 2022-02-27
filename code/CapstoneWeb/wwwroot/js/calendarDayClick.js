$(".vanilla-calendar-date").click(
    function () {
        let pad = function(num) { return ('00'+num).slice(-2) };
        let date = new Date($(this).attr("data-calendar-date"));
        date = date.getUTCFullYear()         + '-' +
            pad(date.getUTCMonth() + 1)  + '-' +
            pad(date.getUTCDate())
        
        let tripId = parseInt($("#tripId").attr("value"))
        $.ajax({
            method: "GET",
            url:`/trip/${tripId}/?handler=Ajax`,
            beforeSend: function (xhr) {
                xhr.setRequestHeader("XSRF-TOKEN",
                    $('input:hidden[name="__RequestVerificationToken"]').val());
            },
            data: { selectedDate: date },
            success: function (result) {
                result.data.forEach(function(waypoint) {
                    let events = $("#events")
                    events.empty();
                    events.append(
                        `
                        <div>
                            <p>
                                ${waypoint.startDate} - ${waypoint.startDate}
                            </p>
                            <p>
                                ${waypoint.location}
                            </p>
                            <a>Remove</a>
                        </div>
                        `
                    );
                })
            }
        })
    }
)