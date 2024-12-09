function submitRegistration() {
    var data = $('#register-form').serialize();

    $.ajax({
        method: 'POST',
        data: data,
        success: function (data) {
            if (data.length > 0) {
                document.open();
                document.write(data);
                document.close();
            } else {
                showConfirmation("Успех",
                    "Ваш аккаунт был успешно зарегистрирован. Ожидайте активации аккаунта администратором",
                    function () {
                        location.replace("/");
                    });
            }
        }
    });
}