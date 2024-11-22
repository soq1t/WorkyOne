function showModal(content) {
    var modal = $('#modal-global');

    modal.html(content);

    modal.css('display', 'flex')
        .addClass('modal--active');
}

function hideModal() {
    var modal = $('#modal-global');

    modal.removeClass('modal--active')
        .delay(500)
        .css('display', 'none')
        .html('');

}