$(function () {
    $('.schedules__list').find('.edit').on('click', function () {
        var id = $(this.parentElement.parentElement).data("id");

        window.location.replace('/schedules/' + id);
        //$.ajax({
        //    url: "schedules/" + id,
        //    type: 'GET',
        //    success: function (data) {
        //        showModal(data);
        //    },
        //    error: function () {

        //    }
        //});
    });

    $('.schedules__list').find('.favorite').on('click', function ()
    {
        var id = $(this.parentElement.parentElement).data("id");

        var favItem = this;

        var confirmationMessage = "";

        if ($(favItem).hasClass('favorite--active')) {
            confirmationMessage = 'Удалить избранное расписание?';
        } else {
            confirmationMessage = 'Изменить избранное расписание?';
        }

        if (confirm(confirmationMessage)) {
            $.ajax({
                url: "schedules/" + id + "/favorite",
                type: 'PUT',
                success: function (data) {
                    ToggleFavorite(favItem);
                },
                error: function () {

                }
            });
        }        
    });
});

function ToggleFavorite(favItem)
{
    if ($(favItem).hasClass('favorite--active')) {
        $(favItem).removeClass('favorite--active');
    } else {       
        $('.schedules__list').find('.favorite').removeClass('favorite--active');
        $(favItem).addClass('favorite--active');
    }

}