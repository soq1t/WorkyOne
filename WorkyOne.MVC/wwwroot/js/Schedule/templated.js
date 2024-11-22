$(function () {

    addClickEvents();

});

function moveShift(id, direction) {   
    var loading = $('#templated-shifts-table').find('.shifts-table__loading');
    loading.addClass('active');

    $.ajax({
        method: 'GET',
        url:'shifts/templated/' + id + '/move?steps=' + direction,
        success: function (data) {
            updateTable();
        },
        error: function () {
            loading.removeClass('active');
        }
    });

}

function addClickEvents() {
    var table = $('#templated-shifts-table');

    $(table).find('.shift__controls-up').on('click', function () {
        let id = $(this).parent().data('id');
        
        moveShift(id, -1);
    });

    $(table).find('.shift__controls-down').on('click', function () {
        let id = $(this).parent().data('id');

        moveShift(id, 1);
    });
}

function updateTable() {

    var table = $('#templated-shifts-table');
    var scheduleId = $('#schedule').data('id');


    $.ajax({
        method: 'GET',
        url: scheduleId + '/templated/partial',
        success: function (data) {
            $(table).replaceWith(data);
        },
        error: function () {
            $(table).find('.shifts-table__loading').removeClass('active');
        }
    });
}