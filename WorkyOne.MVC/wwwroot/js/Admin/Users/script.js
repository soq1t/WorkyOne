$(function () {
    var pagination = $('#admin-users').find(".pagination");
    var controls = $('#admin-users')
        .find(".users__table")
        .find(".grid-table__item--controls");
    var dropdown = $('#admin-users').find('.users__filters-dropdown');


    var pageIndex = $(pagination)
        .find('.pagination__info-page')
        .data('page');

    var pageAmount = $(pagination)
            .find('.pagination__info-page')
        .data('amount');

    $(dropdown).find('.dropdown__header').on('click', function () {
        $(dropdown).toggleClass('active');
    });

    //$(dropdown).find('.users__filters-controls')
    //    .children('.btn').on('click', function (e) {
    //        e.preventDefault();
    //        GetUsers();
    //    });

    $(pagination)
        .children('.pagination__arrow--left')
        .on('click', function (e) {
            e.preventDefault(); 

            if (pageIndex > 1) {
                pageIndex--;

                $(pagination)
                    .find('.pagination__info')
                    .children('input')
                    .val(pageIndex);

                document.getElementById("admin-users").submit();
            }

        });

    $(pagination)
        .children('.pagination__arrow--right')
        .on('click', function (e) {
            e.preventDefault();

            if (pageIndex < pageAmount) {
                pageIndex++;
                $(pagination)
                    .find('.pagination__info')
                    .children('input')
                    .val(pageIndex);

                document.getElementById("admin-users").submit();
            }

        });

    $(controls).find(".edit").on('click', function (e) {
        var id = $(this.parentElement.parentElement).data('id');


    });

    function GetUsers() {        

        var data = {
            Page: parseInt(pageIndex),
            Filter: {
                UserName: $('#users-filters-username').val(),
                ShowActivated: $('#users-filters-showactivated').val(),
                ShowInactivated: $('#users-filters-showinactivated').val(),
            }
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