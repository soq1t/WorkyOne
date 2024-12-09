$(function () {

    var table = $('#admin-shifts').find('.shifts__table');

    $('#admin-shifts').find('button.create')
        .on('click', function () {
            $.ajax({
                method: 'GET',
                url: 'shifts/partial',
                success: function (data) {
                    create(data);
                }
            });
        });    

    $(table).find('button.delete')
        .on('click', function () {
            var id = $(this.parentElement.parentElement).data('id');

            showConfirmation(null, "Удалить смену?", function () {
                $.ajax({
                    method: 'DELETE',
                    url: `shifts/${id}`,
                    success: function (data) {
                        location.reload();
                    }
                });
            });
        });

    $(table).find('button.edit')
        .on('click', function () {
            var id = $(this.parentElement.parentElement).data('id');

            $.ajax({
                method: 'GET',
                url: `shifts/${id}`,
                success: function (data) {
                    update(data);
                }
            });
        });

    function create(data) {
        showConfirmation("Создание смены", data, function () {
            var data = $('#shift-form').serialize();

            $.ajax({
                method: 'POST',
                url: 'shifts',
                data: data,
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
        showConfirmation("Редактирование смены", data, function () {
            var data = $('#shift-form').serialize();

            $.ajax({
                method: 'PUT',
                url: 'shifts',
                data: data,
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
});