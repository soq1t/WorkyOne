$(function () {

    var year = $('._calendar__month-selector').data('year');
    var month = $('._calendar__month-selector').data('month');
    var scheduleId = $('#schedules').val();

    RenderCalendar(year, month, scheduleId);

    $('.month-selector__arrow').on('click', function ()
    {
        var direction = $(this).data('dir');

        var year = $(this.parentElement).data("year");
        var month = $(this.parentElement).data("month");
        var scheduleId = $('#schedules').val();

        RenderCalendarWithDirection(year, month, direction, scheduleId);
    });

    $('#schedules').on('change', function () {

        var year = $('._calendar__month-selector').data("year");
        var month = $('._calendar__month-selector').data("month");
        var scheduleId = $(this).val();

        RenderCalendar(year, month, scheduleId);
    });
});

function RenderCalendar(year, month, scheduleId) {

    $.ajax({
        type: 'GET',
        url: 'calendar/graphic?Year=' + parseInt(year) + '&Month=' + parseInt(month) + '&ScheduleId=' + scheduleId,
        success: function (data) {
            $("._calendar__main").html(data);

            UpdateCalendarInfo(year, month);
            UpdateLegend(year, month, scheduleId);
        },
        error: function () {

        }
    });
}
function UpdateCalendarInfo(year, month) {

    $.ajax({
        type: 'GET',
        url: 'calendar/info?Year=' + parseInt(year) + '&Month=' + parseInt(month),
        success: function (data) {

            $('.month-selector__info').children('.month').html(data.monthName);
            $('.month-selector__info').children('.year').html(data.year);
            $('._calendar__month-selector').data('year', data.year);
            $('._calendar__month-selector').data('month', data.monthNumber);
        },
        error: function () {

        }
    });
}

function UpdateLegend(year, month, scheduleId) {

    var url = 'calendar/graphic/legend?Year=' + parseInt(year) + '&Month=' + parseInt(month) + '&ScheduleId=' + scheduleId

    $.ajax({
        type: 'GET',
        url: url,
        success: function (data) {

            $('._calendar__footer').html(data);
        },
        error: function () {

        }
    });
}

function RenderCalendarWithDirection(year, month, direction, scheduleId) {
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

    RenderCalendar(year, month, scheduleId);
}