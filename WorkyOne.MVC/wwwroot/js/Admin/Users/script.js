$(function () {

    var pagination = $('#admin-users').find(".pagination");

    var pageIndex = $(pagination)
        .find('.pagination__info-page')
        .data('page');

    var pageAmount = $(pagination)
            .find('.pagination__info-page')
            .data('amount');


    $(pagination)
        .children('.pagination__arrow--left')
        .on('click', function (e) {
            if (pageIndex > 1) {
                pageIndex--;
                GetUsers();
            }

        });

    $(pagination)
        .children('.pagination__arrow--right')
        .on('click', function (e) {
            if (pageIndex < pageAmount) {
                pageIndex++;
                GetUsers();
            }

        });


    function GetUsers() {

        var data = {
            Page: parseInt(pageIndex),
        };

        $.ajax({
            method: 'GET',
            url: '',
            contentType: 'application/json',
            data: data,
            success: function (data) {
                document.open();
                document.write(data);
                document.close();
            }
        });
    }
});