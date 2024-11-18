function showModal(content) {
    var modal = $('#modal');

    modal.html(content);
    modal.fadeIn(100);
}

function hideModal() {
    var modal = $('#modal');

    modal.fadeOut(100, function () {
        modal.html("");
    });

}