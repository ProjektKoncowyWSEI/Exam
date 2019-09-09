function filter(searchId, clearId, searchIn, searchWhat) {
    let input = document.querySelector('#' + searchId).value
    if (input.length > 0) {
        document.querySelector('#' + clearId).classList.remove('hidden')
    }
    else {
        document.querySelector('#' + clearId).classList.add('hidden')
    }
    let element = document.querySelector(searchIn);
    let elements = element.querySelectorAll(searchWhat);
    for (let i = 0; i < elements.length; i++) {
        if (!elements[i].textContent.toLowerCase().includes(input.toLowerCase()) && !elements[i].outerText.toLowerCase().includes(input.toLowerCase())) {
            hideBlock(elements[i]);
        }
        else {
            showBlock(elements[i]);
        }
    }
}
function showBlock(item) {
    item.classList.remove('hidden')
}
function hideBlock(item) {
    item.classList.add('hidden')
}
function clearFilter(searchId, clearId, searchIn, searchWhat) {
    document.querySelector('#' + searchId).value = null
    filter(searchId, clearId, searchIn, searchWhat)
    document.querySelector('#' + searchId).select();
}
function escapeSearch(event, searchId, clearId, searchIn, searchWhat) {    
    if (event.keyCode == 27) {
        clearFilter(searchId, clearId, searchIn, searchWhat)
    }
}
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
function toggleTrs(id, id2, sender) {
    if ($(sender).hasClass('fa-chevron-down')) {
        $(sender).removeClass('fa-chevron-down')
        $(sender).addClass('fa-chevron-up')
    } else {
        $(sender).removeClass('fa-chevron-up')
        $(sender).addClass('fa-chevron-down')
    }
    $(id).slideToggle();
    $(id2).slideToggle();
    try {
        $('html, body').animate({
            scrollTop: $(sender).offset().top - 150
        }, 500);
    } catch (e) {

    }
}
function hideElement(id) {
    showLoader();
    $(id).hide();
}
function showLoader() {    
    $('#loader').show();
}
