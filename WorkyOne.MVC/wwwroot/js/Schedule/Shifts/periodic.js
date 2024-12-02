$(function () {
    var table = $('#schedule-shifts-periodic');
    var controls = $(table).find('.shifts-table__item--controls');
    var scheduleId = $(table).data('schedule-id');

    $(controls).find('.btn').off('click');

    $('#schedule-shifts-periodic-add').on('click', function (e) {
        e.preventDefault();

        $.ajax({
            method: 'POST',
            url: `/shifts/periodic/partial?scheduleId=${scheduleId}`,
            success: function (data) {
                create(data);
            }
        });
    });

    $(controls).find('.edit').on('click', function (e) {
        e.preventDefault();

        var id = $(this.parentElement.parentElement).data('id');

        $.ajax({
            method: 'GET',
            url: `/shifts/periodic/partial?id=${id}`,
            success: function (data) {
                update(data);
            }
        });
    });

    $(controls).find('.delete').on('click', function (e) {
        e.preventDefault();

        var id = $(this.parentElement.parentElement).data('id');

        showConfirmation(null, "Удалить смену?", function () {
            $.ajax({
                method: 'DELETE',
                url: `/shifts/periodic?id=${id}`,
                success: function (data) {
                    location.reload();
                }
            });
        });
    });
    function create(data) {
        showConfirmation("Создание смены на определённый период", data, function () {
            var form = $('#shifts-periodic-form');
            var shift = form.serialize();

            $.ajax({
                method: 'POST',
                url: '/shifts/periodic',
                data: shift,
                success: function (data) {
                    if (data.length > 0) {
                        create(data);
                    } else {
                        location.reload();
                    }
                }
            });
        });
    }
    function update(data) {
        showConfirmation("Обновление смены на определённый период", data, function () {
            var form = $('#shifts-periodic-form');
            var shift = form.serialize();

            $.ajax({
                method: 'PUT',
                url: '/shifts/periodic',
                data: shift,
                success: function (data) {
                    if (data.length > 0) {
                        update(data);
                    } else {
                        location.reload();
                    }
                }
            });
        });
    }
});

