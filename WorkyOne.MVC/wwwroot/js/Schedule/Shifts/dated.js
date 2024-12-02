$(function () {
    var table = $('#schedule-shifts-dated');
    var controls = $(table).find('.shifts-table__item--controls');
    var scheduleId = $(table).data('schedule-id');

    $(controls).find('.btn').off('click');

    $('#schedule-shifts-dated-add').on('click', function (e) {
        e.preventDefault();

        $.ajax({
            method: 'POST',
            url: `/shifts/dated/partial?scheduleId=${scheduleId}`,
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
            url: `/shifts/dated/partial?id=${id}`,
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
                url: `/shifts/dated?id=${id}`,
                success: function (data) {
                    location.reload();
                }
            });
        });        
    });
    function create(data) {
        showConfirmation("Создание смены на определённую дату", data, function () {
            var form = $('#shifts-dated-form');
            var shift = form.serialize();

            $.ajax({
                method: 'POST',
                url: '/shifts/dated',
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
        showConfirmation("Обновление смены на определённую дату", data, function () {
            var form = $('#shifts-dated-form');
            var shift = form.serialize();

            $.ajax({
                method: 'PUT',
                url: '/shifts/dated',
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

