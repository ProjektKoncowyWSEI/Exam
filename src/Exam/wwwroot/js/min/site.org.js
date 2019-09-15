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
////////////////ExamApproches//////////////////////////////
function examTimer() {  
    let counter = setInterval(function () {
        let now = new Date().getTime();
        let distance = countDownDate - now;
        //var days = Math.floor(distance / (1000 * 60 * 60 * 24));
        let hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        let minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        let seconds = Math.floor((distance % (1000 * 60)) / 1000);
        if (distance < 60000) {
            document.getElementById("counter").classList.add("text-danger")
            document.getElementById("counter").classList.add("bold")
            document.getElementById("clock").classList.add("text-danger")
            document.getElementById("clock").classList.remove("orange")
        }
        let h = hours < 10 ? "0" + String(hours) : hours;
        let m = minutes < 10 ? "0" + String(minutes) : minutes;
        let s = seconds < 10 ? "0" + String(seconds) : seconds;
        document.getElementById("counter").innerHTML = h + "h " + m + "m " + s + "s";
        if (distance < 20000) {
            if (document.getElementById("counterContent").classList.contains("hidden")) {
                toogleTimer();
            }
        }
        if (distance < 0) {
            clearInterval(counter);
            document.getElementById("counter").innerHTML = "00h 00m 00s";
        }
    }, 1000);
}
function toogleTimer() {
    if (!document.getElementById("counterContent").classList.contains("hidden")) {
        document.getElementById("counterContent").classList.add("hidden")
        document.getElementById("timer").classList.add("timerSmall")
        document.getElementById("arrow").classList.remove("fa-long-arrow-right")
        document.getElementById("arrow").classList.add("fa-long-arrow-left")
    }
    else {
        document.getElementById("counterContent").classList.remove("hidden")
        document.getElementById("timer").classList.remove("timerSmall")
        document.getElementById("arrow").classList.remove("fa-long-arrow-left")
        document.getElementById("arrow").classList.add("fa-long-arrow-right")
    }
}
