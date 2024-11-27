$(function () {
    var personalShifts = $('#schedule-personal');

    updateEvents();

    $('#schedule-personal-add').on('click', function (e) {
        e.preventDefault();
        var scheduleId = $(personalShifts).data('schedule-id');
        $.ajax({
            method: 'POST',
            url: `/shifts/personal/partial?ScheduleId=${scheduleId}`,
            success: function (data) {
                shiftCreationConfirmation(data);
            }
        });
    });
    function shiftCreationConfirmation(data) {
        showConfirmation("Создание смены", data, function () {
            var form = $("#shift-form");
            var shift = form.serialize();

            $.ajax({
                method: 'POST',
                url: '/shifts/personal/partial/check',
                data: shift,
                success: function (data) {
                    if (typeof (data) == 'string') {
                        shiftCreationConfirmation(data);
                    } else {
                        addShift(data);
                        updateEvents();
                    }
                }
            });
        });
    };
    function updateEvents() {
        var controls = $(personalShifts).find('.shifts-table__item--controls');

        $(controls).find('.btn').off('click');

        $(controls).find('.edit').on('click', function (e) {
            e.preventDefault();

            var shift = $(this.parentElement.parentElement);

            var data = {
                ScheduleId: $(shift).children('input[name*="ScheduleId"]').attr('value'),
                Id: $(shift).children('input[name*="Id"]').attr('value'),
                Name: $(shift).children('input[name*="Name"]').attr('value'),
                Beginning: $(shift).children('input[name*="Beginning"]').attr('value'),
                Ending: $(shift).children('input[name*="Ending"]').attr('value'),
                ColorCode: $(shift).children('input[name*="ColorCode"]').attr('value'),
            }

            $.ajax({
                method: 'GET',
                url: '/shifts/personal/partial',
                data: data,
                contentType: 'application/json',
                success: function (data) {
                    updateShiftConfirmation(data, shift);
                }
            });
        });

        $(controls).find('.delete').on('click', function (e) {
            e.preventDefault();
            var shift = $(this.parentElement.parentElement);

            showConfirmation(null, `Удалить смену?`, function () {
                deleteShift(shift);
            })   
        });
    };

    function addShift(data) {
        var amount = $(personalShifts).data('amount');
        amount++;
        $(personalShifts).data('amount', amount);

        $(personalShifts).append(`
        <div data-index="${amount - 1}" data-id="${data.id}" class="shifts-table__row">
            <input hidden="hidden" name="Schedule.PersonalShifts[${amount - 1}].ScheduleId" value="${data.scheduleId}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${amount - 1}].Id" value="${data.id}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${amount - 1}].Name" value="${data.name}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${amount - 1}].Beginning" value="${data.beginning ?? ""}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${amount - 1}].Ending" value="${data.ending ?? ""}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${amount - 1}].ColorCode" value="${data.colorCode}" />
            <div class="shifts-table__item shifts-table__item--color shifts-table__item--centered">
                <span style="background-color: ${data.colorCode}"></span>
            </div>
            <div class="shifts-table__item">
                <span>${data.name}</span>
            </div>
            <div class="shifts-table__item shifts-table__item--time">
                <span>${data.beginning ?? "-"}</span>
                <span>${data.ending ?? "-"}</span>
            </div>
            <div class="shifts-table__item shifts-table__item--controls">
                <button class="material-symbols-outlined btn btn__primary btn__primary--flat edit">edit</button>
                <button class="material-symbols-outlined btn btn__danger btn__danger--flat delete">delete</button>
            </div>
        </div>`);
    };

    function deleteShift(shift) {
        var amount = $(personalShifts).data('amount');
        amount--;
        $(personalShifts).data('amount', amount);

        $(shift).remove();
    }

    function updateShiftConfirmation(data, shift) {
        showConfirmation("Изменение смены", data, function () {
            var form = $("#shift-form");
            var shiftData = form.serialize();

            $.ajax({
                method: 'POST',
                url: '/shifts/personal/partial/check',
                data: shiftData,
                success: function (data) {
                    if (typeof (data) == 'string') {
                        updateShiftConfirmation(data, shift);
                    } else {
                        updateShift(data, shift);
                        updateEvents();
                    }
                }
            });
        });
    }

    function updateShift(data, shift) {
        var index = $(shift).data('index');
        $(shift).html(`
            <input hidden="hidden" name="Schedule.PersonalShifts[${index}].ScheduleId" value="${data.scheduleId}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${index}].Id" value="${data.id}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${index}].Name" value="${data.name}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${index}].Beginning" value="${data.beginning ?? ""}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${index}].Ending" value="${data.ending ?? ""}" />
            <input hidden="hidden" name="Schedule.PersonalShifts[${index}].ColorCode" value="${data.colorCode}" />
            <div class="shifts-table__item shifts-table__item--color shifts-table__item--centered">
                <span style="background-color: ${data.colorCode}"></span>
            </div>
            <div class="shifts-table__item">
                <span>${data.name}</span>
            </div>
            <div class="shifts-table__item shifts-table__item--time">
                <span>${data.beginning ?? "-"}</span>
                <span>${data.ending ?? "-"}</span>
            </div>
            <div class="shifts-table__item shifts-table__item--controls">
                <button class="material-symbols-outlined btn btn__primary btn__primary--flat edit">edit</button>
                <button class="material-symbols-outlined btn btn__danger btn__danger--flat delete">delete</button>
            </div>`);
    }
});