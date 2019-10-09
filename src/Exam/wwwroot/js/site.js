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
function hideLoader() {
    $('#loader').hide();
}
function addError(error, message) {
    $('.alertContainer').html(`
    <div class='alert alert-danger alert-dismissible fade show alertPosition' role = 'alert'>
        <strong >${error}: <br /></strong > ${message}
        <button type="button" class="close" data-dismiss="alert" aria-label="Close">
            <span aria-hidden="true">&times;</span>
        </button>
    </div >`)
    window.setTimeout(function () {
        $(".alert").slideUp(500, function () {
            $(this).remove();
        });
    }, 10000);
}
////////////////ExamApproches//////////////////////////////
class Model {
    constructor(examId, code, questions) {
        this.examId = examId;
        this.code = code;
        this.questions = questions;
    }
}
class Question {
    constructor(id, answers) {
        this.id = id;
        this.answers = answers;
    }
}
class Answer {
    constructor(id, checked) {
        this.id = id;
        this.checked = checked;
    }
}
let model = {};
function createModel(input) {
    let questions = [];
    for (let i = 0; i < input.questions.length; i++) {
        let a = [];
        for (let y = 0; y < input.questions[i].answers.length; y++) {
            a.push(new Answer(input.questions[i].answers[y].id, false))
        }
        const q = new Question(input.questions[i].id, a)
        questions.push(q);
        model = new Model(input.id, input.code, questions);
    }
    //console.log(input)
}
function setAnswer(questionIndex, answerIndex, sender, answerType) {
    if (answerType == 8) { //single
        for (let i = 0; i < model.questions[questionIndex].answers.length; i++) {
            if (i == answerIndex) {
                model.questions[questionIndex].answers[answerIndex].checked = sender.checked;
            }
            else {
                model.questions[questionIndex].answers[i].checked = false;
            }
        }
    }
    else if (answerType = 16) { //multiple
        model.questions[questionIndex].answers[answerIndex].checked = sender.checked;
    }
    $('#question_' + questionIndex).removeClass('required')
}
function validInput() {
    let notValidQuestions = []
    for (let i = 0; i < model.questions.length; i++) {
        let isValid = false;
        for (var y = 0; y < model.questions[i].answers.length; y++) {            
            if (model.questions[i].answers[y].checked) {
                isValid = true;
            }
        }
        if (!isValid) {
            notValidQuestions.push(model.questions[i])
            $('#question_' + i).addClass('required')
        }
        else {
            $('#question_' + i).removeClass('required')
        }
    }
    return notValidQuestions;   
}
function addRequired(questionNumber) {
    //$('#required_' + questionNumber).addClass('required')
}
function removeRequired(questionNumber) {
    //$('#required_' + questionNumber).removeClass('required')
}
function sendExam() {
    let len = validInput().length;
    if (len > 0) {
        let message = "Brak odpowiedzi w pytaniach: "
        for (var i = 1; i < len + 1; i++) {
            message += String(i) + "; ";           
        }
        addError('Error', message)
        return;
    }
    else {
        $('#confirmModal').hide()        
        let token = $('input[name="`__RequestVerificationToken`"]').val();
        showLoader()
        $('#sendBtn').attr('disabled', 'disabled')
        $.ajax({
            type: "POST",
            url: '/ExamApproaches/FinishExam',
            contentType: 'application/x-www-form-urlencoded; charset=utf-8',
            dataType: 'JSON',
            data: {
                __RequestVerificationToken: token,
                exam: model
            },
            success: function (result) {                
                $('#sendBtn').removeAttr('disabled')
                if (result[0]) {
                    location.href = '/StartExam/' + model.code + '/?info=' + result[1];
                }
                else {
                    location.href = '/StartExam/' + model.code + '/?error=' + result[1];
                }
            },
            error: function (e) {
                hideLoader();
                addError('Error', e);
                $('#sendBtn').removeAttr('disabled')
            }
        });
    }
}
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
