$(function () {
    $('#modal-global-confirmation-cancel').on('click', hideConfirmation);
});

function showModal(content) {
    var modal = $('#modal-global');

    modal.html(content)

    modal.css('display', 'flex')
        .addClass('modal--active');    
}

function hideModal() {
    var modal = $('#modal-global');

    modal.removeClass('modal--active')
        .delay(550)
        .css('display', 'none').html(null);
}

function showConfirmation(title, content, callback) {
    var modal = $('#modal-global-confirmation');

    if (title == null) {
        title = "Подтвердите действие";
    }

    modal.find('.modal__confirmation__title').html(title);
    modal.find('.modal__confirmation-content').html(content);

    modal.css('display', 'flex')
        .addClass('modal--active');

    $('#modal-global-confirmation-ok').on('click', callback);
    $('#modal-global-confirmation-ok').on('click', hideConfirmation);
}

function hideConfirmation() {
    var modal = $('#modal-global-confirmation');

    modal.find('.modal__confirmation__title').html(null);
    modal.find('.modal__confirmation-content').html(null);

    modal.removeClass('modal--active')
        .delay(550)
        .css('display', 'none');

    $('#modal-global-confirmation-ok').off();
}