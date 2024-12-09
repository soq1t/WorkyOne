$(function () {
    var form = $('#admin-user');
    var roles = $(form).find('.user__roles');
    var userId = $(form).data('id');

    var controls = $(form).children('.user__footer');
        
    updateEvents();
    function updateEvents() {
        $(form).find('button').off('click');

        $(form).find('button').on('click', function (e) {
            e.preventDefault();
        });

        $(controls).find('.close').on('click', function () {
            hideModal();
        });

        $(controls).find('.save').on('click', function () {
            var data = $(form).serialize();

            $.ajax({
                method: 'POST',
                url: `users/${userId}`,
                data: data,
                success: function (data) {
                    if (data.length > 0) {
                        showModal(data);
                    } else {
                        location.reload();
                    }
                }
            });
        });


        $(roles).find('.user__roles-add')
            .children('button')
            .on('click', function () {
                var role = $(this).prev().val();
                var table = $(roles).find('.user__roles-table');

                var insert = true;
                $(table).find('.grid-table__row')
                    .each(function () {
                        if ($(this).data('name') == role) {
                            insert = false;
                        }
                    })

                if (insert) {
                    var amount = $(roles).data('amount');
                    $(roles).data('amount', ++amount);
                    var row = `
                    <div data-name="${role}" class="grid-table__row">
                        <input hidden="hidden" type="text" name="User.Roles[${amount - 1}]" value="${role}" />
                        <div class="grid-table__item">${role}</div>
                        <div class="grid-table__item grid-table__item--centered">
                        <button
                            class="material-symbols-outlined btn btn__danger btn__danger--flat delete">
                                delete_forever
                            </button>
                        </div>
                    </div>
                `;

                    $(row).insertAfter($(table)
                        .find('.grid-table__body')
                        .children()
                        .last());

                    updateEvents();
                }

            });

        $(roles).find('button.delete')
            .on('click', function () {
                var row = $(this).parent().parent();
                $(row).remove();

                var amount = $(roles).data('amount');
                $(roles).data('amount', --amount);

                recalculateRoles();
            });
    }
    function recalculateRoles() {
        var i = 0;
        $(roles).find('.grid-table__body')
            .children()
            .each(function () {
                var input = $(this).children('input');

                $(input).attr('name', `User.Roles[${i}]`)
                i++;  
            });
    }
});