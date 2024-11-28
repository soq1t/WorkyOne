$(function () {
    var personalShifts = $('#schedule-personal');
    var controls = $(personalShifts).find('.shifts-table__item--controls');
    var scheduleId = $(personalShifts).data('schedule-id');

    $(controls).find('.btn').off('click');

    $('#schedule-personal-add').on('click', function (e) {
        e.preventDefault();

        $.ajax({
            method: 'GET',
            url: `/shifts/personal?ScheduleId=${scheduleId}`,
            success: function (data) {
                shiftCreationConfirmation(data);
            }
        });  
    });

    $(controls).find('.edit').on('click', function (e) {
        e.preventDefault();

        var shift = $(this.parentElement.parentElement);

        var data = {
            ScheduleId: $(shift).children('input[name*=".ScheduleId"]').attr('value'),
            Id: $(shift).children('input[name*=".Id"]').attr('value'),
            Name: $(shift).children('input[name*=".Name"]').attr('value'),
            Beginning: $(shift).children('input[name*=".Beginning"]').attr('value'),
            Ending: $(shift).children('input[name*=".Ending"]').attr('value'),
            ColorCode: $(shift).children('input[name*=".ColorCode"]').attr('value'),
        }

        $.ajax({
            method: 'GET',
            url: '/shifts/personal/partial',
            data: data,
            contentType: 'application/json',
            success: function (data) {
                updateShiftConfirmation(data, shift);
            }
        });
    });

    $(controls).find('.delete').on('click', function (e) {
        e.preventDefault();
        var id = $(this.parentElement.parentElement).data('id');

        showConfirmation(null, `Удалить смену?`, function () {
            $.ajax({
                method: 'DELETE',
                url: `/shifts/personal?id=${id}`,
                success: function (data) {
                    redirect();
                }
            });
        })
    });
    function redirect() {
        location.replace(`/schedules/${scheduleId}`);
    } 
    function updateShiftConfirmation(data, shift) {
        showConfirmation("Изменение смены", data, function () {
            var form = $("#shift-form");
            var shiftData = form.serialize();

            $.ajax({
                method: 'PUT',
                url: '/shifts/personal',
                data: shiftData,
                success: function (data) {
                    if (data.length > 0) {
                        updateShiftConfirmation(data, shift);
                    } else {
                        redirect();
                    }
                }
            });
        });
    }
    function shiftCreationConfirmation(data) {
        showConfirmation("Создание смены", data, function () {
            var form = $("#shift-form");
            var shift = form.serialize();

            $.ajax({
                method: 'POST',
                url: '/shifts/personal',
                data: shift,
                success: function (data) {
                    if (data.length > 0) {
                        shiftCreationConfirmation(data);
                    } else {
                        redirect();
                    }
                },
            });
        });
    };
});

