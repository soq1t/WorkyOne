$(function () {

    $('#schedules-create').on('click', function () {
        function addEvents() {
            $('.schedule__new').find('.close').on('click', hideModal);
            $('.schedule__new').on('submit', function (e) {
                e.preventDefault(); 0

                var $form = $(this);
                var formData = $form.serialize();

                $.ajax({
                    type: 'POST',
                    url: 'schedules/create',
                    data: formData,
                    success: function (data) {
                        if (data == "Success") {
                            hideModal();
                            window.location.reload();
                        } else {
                            showModal(data);
                            addEvents();
                        }
                    }
                });
            });
        }

        $.ajax({
            method: 'GET',
            url: 'schedules/create',
            success: function (data) {
                showModal(data);
                addEvents();
            }
        });
    });

    

    $('.schedules__list').find('.edit').on('click', function () {
        var id = $(this.parentElement.parentElement).data("id");

        window.location.replace('/schedules/' + id);
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

        showConfirmation(null, confirmationMessage, function () {
            $.ajax({
                url: "schedules/" + id + "/favorite",
                type: 'PUT',
                success: function (data) {
                    ToggleFavorite(favItem);
                },
                error: function () {

                }
            });
        });

        //if (confirm(confirmationMessage)) {
        //    $.ajax({
        //        url: "schedules/" + id + "/favorite",
        //        type: 'PUT',
        //        success: function (data) {
        //            ToggleFavorite(favItem);
        //        },
        //        error: function () {

        //        }
        //    });
        //}        
    });

    $('.schedules__list').find('.delete').on('click', function () {
        var id = $(this.parentElement.parentElement).data("id");
        var name = $(this.parentElement.parentElement).data("name");

        var message = 'Удалить расписание "' + name + '"?';

        showConfirmation(null, message, function () {

            $.ajax({
                method: 'DELETE',
                url: 'schedules/' + id,
                success: function (data) {
                    window.location.reload();
                }
            });
        });

        //if (confirm('Удалить расписание "' + name + '"?')) {
        //    $.ajax({
        //        method: 'DELETE',
        //        url: 'schedules/' + id,
        //        success: function (data) {
        //            window.location.reload();
        //        }
        //    });
        //}        
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