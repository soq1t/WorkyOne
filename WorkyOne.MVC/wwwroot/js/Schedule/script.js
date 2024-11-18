$(function () {
    $('.edit').on('click', function () {
        var id = $(this.parentElement.parentElement).data("id");

        $.ajax({
            url: "schedules/" + id,
            type: 'GET',
            success: function (data) {
                showModal(data);
            },
            error: function () {

            }
        });
    });
});