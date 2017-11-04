// gomc.js is the main javascript file for the web application

// Global Vars
var currentWidth = 0;

var registrationString = {
    init: '<span class="glyphicon glyphicon-collapse-down"></span> Close Form and go straight to download',
    fin: '<span class="glyphicon glyphicon-collapse-up"></span> Open Form and Register'
};
// Global Object events
$(function(){
	console.log('READY');
});

// Event Listeners
$('#btn').click(function(){
	if($('#btn').children().hasClass('glyphicon-align-justify')){
	  $('#btn').children().removeClass('glyphicon-align-justify');
	  $('#btn').children().addClass('glyphicon-remove');
	  $('header').css('margin-top', '22.5em');
	}
	else {
	   $('#btn').children().removeClass('glyphicon-remove');
	   $('#btn').children().addClass('glyphicon-align-justify');
	   $('header').css('margin-top', '6.5em');
	}
	$('#btn').css('color', '#FFFFFF');
	$('#btn').css('backgroundColor', '#2C3539');
});

//$('#xmlTrigger').click(function(){
//   $('#xmlDataContainer').slideToggle(700,function(){
//	$('#xmlDataContainer').toggleClass('hidden-until');
//	$('#xmlTrigger').slideUp(500, 'linear', morphXmlTrigger);
//  });
//});

$('#closeRegistration').click(function () {
    $(this).next().slideToggle(() => {
        $(this).html((count, words)=>{
            return words == '<span class="glyphicon glyphicon-collapse-down"></span> Close Form and go straight to download' ? registrationString.fin : registrationString.init;
        });
    });
});

$('#registrationForm').submit(function (e) {
    try {
        $.post('/api/Registration/Input', $('#registrationForm').serialize())
            .done(function (data) {
                $('#closeRegistration').html('Thanks for Registering! <span class="glyphicon glyphicon-ok-sign"></span> ');
                $('#closeRegistration').addClass('btn-success');
                $('#closeRegistration').removeClass('btn-warning');
                $('#closeRegistration').next().slideToggle(() => {
                $('#closeRegistration').prop('disabled', true);
                });
            })

            .fail(function (jqXhR) {
                console.log("Error has been thrown");
                // $("#gomc_config_input_error").html(JSON.parse(jqXhR.responseText)["Message"]); // implementing error handling later
            });
    }
    catch (ex) {
        alert("The following error occured: " + ex.message + " in " + ex.fileName + " at " + ex.lineNumber);
    }
    finally {
        e.preventDefault();
    }
});

$('#Admin').submit(function (e) {
    $('.form-group').removeClass('has-error');
    $('.help-block').remove();
    $.post('/api/Login/ValidateLogin', $(this).serialize())
        .done(function (guidString) {
			// cookie for admin login session and expires in 3 days
            Cookies.set('Admin_Session_Guid', guidString, { expires: 3 });
            window.location.href = "/Home/Admin";
        })
        .fail(function (data) {
            console.log(data.statusText);
            $('.form-group').addClass('has-error');
            $('.form-group').append('<span class="help-block">Invalid credentials</span>');
        });
    e.preventDefault();
});

$('#adminLogout').click(function () {
	// remove cookie for admin login session
    Cookies.remove('Admin_Session_Guid');
    window.location.href = "/Home/Login";
});

$('#adminAnnouncement').submit(function () {

});

function checkAdminLoginSession() {
	var loginSession = Cookies.get('Admin_Session_Guid');

	if (typeof loginSession === "undefined") {
	    return false;
	} else {
	    // TODO: call /api/Login/ValidateSession instead
	    return true;
	}
}

// Callback methods
//function morphXmlTrigger(){
//  if($('#xmlDataContainer').hasClass('hidden-until')){
//	$('#xmlTrigger').removeClass('btn-danger');
//	$('#xmlTrigger').addClass('btn-primary');
//	$('#xmlTrigger').html('Start');
//	removeButtons();
//  }
//  else{
//	$('#xmlTrigger').removeClass('btn-primary');
//	$('#xmlTrigger').addClass('btn-danger');
//	$('#xmlTrigger').html('<span class="glyphicon glyphicon-remove"></span>');
//	addButtons();
//  }
//  $(this).slideDown();
//}

function addButtons(){
	$('.panel-body').append('<button class=" btn btn-success form-left-nav"><span class="glyphicon glyphicon-menu-left"></span></button>');
	$('.panel-body').append('<button class=" btn btn-success form-right-nav"><span class="glyphicon glyphicon-menu-right"></span></button>');
	$('.panel-first').addClass('in-focus');
	$('.panel-first').find('.form-left-nav').prop('disabled', true);
	$('.panel-eigth').find('.form-right-nav').css('display', 'none');
	currentWidth+=12.5;
	var temp = currentWidth + '%'
	$('#userProgress').html(parseInt(currentWidth) + '%');
	$('#userProgress').css('width', temp);
	$('.panel').toggle();
	$('.panel-first').toggle();
	displayMenuChunks();
}

function removeButtons(){
	$('.panel-body').remove('form-left-nav');
	$('.panel-body').remove('form-right-nav');
}

//function displayMenuChunks() {
//	$('.form-left-nav').click(function(e){
//		 e.preventDefault();
//		var currentCard = $('.in-focus');
//		currentCard.removeClass('in-focus');
//		currentCard.prev().addClass('in-focus');
//		currentCard.toggle();
//		currentCard.prev().toggle();
//		adjustBar(false);
//	});

//	$('.form-right-nav').click(function(e){
//		e.preventDefault();
//		var currentCard = $('.in-focus');
//		currentCard.removeClass('in-focus');
//		currentCard.next().addClass('in-focus');
//		currentCard.toggle();
//		currentCard.next().toggle();
//		adjustBar(true);
//	});
//}
// Maybe this should get adjusted on input as opposed to on panel rotation
//function adjustBar(operation) {
//	if(operation == true){ // increase
//		currentWidth = currentWidth + 12.5;
//	   var newWidth = currentWidth + '%';
//		$('#userProgress').css('width', newWidth);
//		$('#userProgress').html(parseInt(newWidth)+ '%');

//	}
//	else { // decrease
//		currentWidth = currentWidth - 12.5;
//	   var newWidth = currentWidth + '%';
//		$('#userProgress').css('width', newWidth);
//		$('#userProgress').html(parseInt(newWidth)+ '%');
//	}
//}

function captchaSelect(captchaResponse) {
    $('#submitRegistration').prop('disabled', false);
}

function refreshLatex() {
    console.log("Refresh Latex Clicked!");
}

function refreshDownloads() {
    console.log("Refresh Downloads Clicked!");
}

function refreshExamples() {
    console.log("Refresh Examples Clicked!");
}