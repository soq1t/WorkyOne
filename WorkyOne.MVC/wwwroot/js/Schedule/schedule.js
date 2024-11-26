$(function () {
    var templatedShifts = $('#schedule-template-shifts');

    updateEvents();    

    $('#templated-shift-add').on('click', function (e) {

        e.preventDefault();

        var id = $('#templated-shift-select').val();

        var amount = $(templatedShifts).data('amount');
        var position = amount + 1;        


        $.ajax({
            method: 'POST',
            url: `/shifts/templated/partial?shiftId=${id}&position=${position}`,
            success: function (data) {

                $(templatedShifts).data('amount', amount + 1);
                $(templatedShifts).append(data);

                updateEvents();
            }
        });
    });
    function updateEvents() {
        var templatedControls = $(templatedShifts)
            .find('.shifts-table__item--controls');

        $(templatedControls)
            .find('.btn')
            .off('click')
            .on('click', function (e) { e.preventDefault(); });        

        $(templatedControls).find('.delete')
            .on('click', function (e) {
                var row = $(this.parentElement.parentElement);

                showConfirmation(null, 'Удалить смену из шаблона?', function () {
                    $(row).remove();

                    var amount = $(templatedShifts).data('amount');
                    $(templatedShifts).data('amount', amount - 1);

                    updateIndexes();
                });
            });
        $(templatedControls).find('.up')
            .on('click', function (e) {
                var row = $(this.parentElement.parentElement);
                var rowIndex = row.data('index');

                if (rowIndex > 2) {
                    var prev = row.prev();

                    row.insertBefore(prev);

                    updateIndexes();
                }
            });

        $(templatedControls).find('.down')
            .on('click', function (e) {
                var amount = $(templatedShifts).data('amount');

                var row = $(this.parentElement.parentElement);
                var rowIndex = row.data('index');

                if (rowIndex <= amount) {
                    var next = row.next();

                    row.insertAfter(next);

                    updateIndexes();
                }
            });
    }
    function updateIndexes() {

        var i = 1;
        $(templatedShifts).children('.shifts-table__row').each(function () {

            if (!$(this).hasClass('shifts-table__row--header')) {
                $(this).data('index', i + 1);
                $(this).children('.position').attr('value', i);

                i++;
            }
            
        })

    }
});

