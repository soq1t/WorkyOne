
$(function () {
    const burger = $('.panel__burger');
    const navList = $('.nav__list');
    const navPanel = $('.nav__panel');

    burger.on("click", function () {
        burger.toggleClass('burger--active');

        var isActive = burger.hasClass('burger--active');

        if (isActive) {
            navList.addClass('nav__list--active');
            navPanel.addClass('nav__panel--active');
        } else {
            navList.removeClass('nav__list--active');
            navPanel.removeClass('nav__panel--active');
        }
        

    });
});