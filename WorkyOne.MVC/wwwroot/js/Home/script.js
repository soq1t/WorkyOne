$(function () {
    var year = $("#calendar").data("year");
    var month = $("#calendar").data("month");

    RenderCalendar(year, month);
});

function AddClickEvents() {
    $(".monthSelector__arrow").on('click', function () {
        var direction = $(this).data("dir");

        var year = $(this.parentElement).data("year");
        var month = $(this.parentElement).data("month");

        RenderCalendarWithDirection(year, month, direction);
    });
}

function RenderCalendar(year, month) {

    $.ajax({
        contentType: "application/json",
        type: 'POST',
        url: 'calendar',
        data: JSON.stringify({ "Year": parseInt(year), "Month": parseInt(month) }),
        success: function (data) {
            $("#calendar").html(data);
            AddClickEvents();
        },
        error: function () {

        }
    });
}

function RenderCalendarWithDirection(year, month, direction)
{
    if (direction == "back") {
        month--;

        if (month < 1) {
            month = 12;
            year--;
        }
    } else {
        month++;

        if (month > 12) {
            month = 1;
            year++;
        }
    }

    RenderCalendar(year, month);
}