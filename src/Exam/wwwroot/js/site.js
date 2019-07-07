function toggleTr(id, sender) {
    if ($(sender).hasClass('fa-chevron-down')) {
        $(sender).removeClass('fa-chevron-down')
        $(sender).addClass('fa-chevron-up')
    } else {
        $(sender).removeClass('fa-chevron-up')
        $(sender).addClass('fa-chevron-down')
    }
    $(id).slideToggle();
    try {
        $('html, body').animate({
            scrollTop: $(sender).offset().top - 150
        }, 500);
    } catch (e) {

    }
}
