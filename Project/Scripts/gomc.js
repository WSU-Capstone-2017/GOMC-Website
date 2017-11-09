// gomc.js is the main javascript file for the web application

// Global Vars
// Width of the progress bar in XML.cshtml
var currentWidth = 0;
// Object of the login Result from cookie
var loginResultType = {
    Success: 0,
    InvalidEmail: 1,
    InvalidPassword: 2
};
// Object to appear and validate against in the orange button of Registration.cshtml
var registrationString = {
	init: '<span class="glyphicon glyphicon-collapse-down"></span> Close Form and go straight to download',
	fin: '<span class="glyphicon glyphicon-collapse-up"></span> Open Form and Register'
};

// Global Object events & telemetry
$(function () {
    console.log('READY');
    $('[data-toggle="tooltip"]').tooltip();  // Extend jQuery with bootstrap.js for tooltip functionality
});

// Event Listeners
//Navigation button in the Responsive view of all cshtml pages
$('#btn').click(function () {
	if ($('#btn').children().hasClass('glyphicon-align-justify')) {
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

// Orange button on downloads.cshtml
$('#closeRegistration').click(function () {
	$(this).next().slideToggle(() => {
		$(this).html((count, words) => {
			return words == '<span class="glyphicon glyphicon-collapse-down"></span> Close Form and go straight to download' ? registrationString.fin : registrationString.init;
		});
	});
});

// Registration form on the downloads.cshtml
$('#registrationForm').submit(function (e) {
    try {
        console.log('process here');
        // jQuery validate prompt
        // Validate name
        // Validate email
        // Validate affliation
        // Validate Comments
        // Perhaps make below code a callback on validation success? Otherwise it should stop here
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
                console.log("Error has been thrown"); // Needs better handling
            });
    }
    catch (ex) {
        alert("The following error occured: " + ex.message + " in " + ex.fileName + " at " + ex.lineNumber);
    }
    finally {
        e.preventDefault();
    }
});
// Login from admin page
$('#Admin').submit(function (e) {
	$('.form-group').removeClass('has-error');
    $('.help-block').remove();
    $('#Admin').toggle();
    $('.loader').remove();
    $('.login-container').append('<div class="loader center-block"></div>');
    $.post('/api/Login/ValidateLogin', $(this).serialize())
		.done(function (data) {
			if (data.ResultType === loginResultType.Success) {
                $('#Admin').toggle();
                $('.loader').toggle();
			    // cookie for admin login session and expires in 3 days
			    Cookies.set('Admin_Session_Guid', data.Session, { expires: 3 });
			    window.location.href = "/Home/Admin";
            } else {
                $('#Admin').toggle();
                $('.loader').toggle();
				console.log('data.ResultType = ' + data.ResultType);
			    $('.form-group').addClass('has-error');
			    $('.form-group').append('<span class="help-block">Invalid credentials</span>');
			}
		})
        .fail(function (data) {
            $('#Admin').toggle();
            $('.loader').toggle();
			console.log(data.statusText);
			$('.form-group').addClass('has-error');
			$('.form-group').append('<span class="help-block">Invalid credentials</span>');
		});
	e.preventDefault();
});
// Logout from admin
$('#adminLogout').click(function () {
	// remove cookie for admin login session
	Cookies.remove('Admin_Session_Guid');
	window.location.href = "/Home/Login";
});

// Post new announcement from admin page
$('#adminAnnouncement').submit(function () {
    var newAnnouncementResult = {
        Success: 0,
        SessionExpired: 1,
        InvalidSession: 2
    };

    $("#adminAnnouncement_Text").text("");

    $.ajax({
        url: '/api/Admin/NewAnnouncement',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
                content: $("#adminAnnouncement_Text").val()
            })
        })
        .done(function(data) {
            if (data === newAnnouncementResult.Success) {
                window.alert('New announcement submitted');
            } else if (data === newAnnouncementResult.InvalidSession) {
                window.alert('New announcement failed to submit, bad session');
            } else if (data === newAnnouncementResult.SessionExpired) {
                window.alert('New announcement failed to submit, session expired');
            }
        });
    return false;
});

// Change the XML config page by displaying the previous card
$('.prev-btn').click(function (e) {
    var currentWorkingPanel = $('.working-panel');
    currentWorkingPanel.removeClass('working-panel');
    currentWorkingPanel.prev().addClass('working-panel');
    // Perhaps wrap the following in a timeout or something? Slow the animation to be less visually jarring as per feedback from Dr.Sam
    currentWorkingPanel.toggle();
    currentWorkingPanel.prev().toggle();
    e.preventDefault();
});

// Change the XML config page by displaying the next card on validation success
$('.next-btn').click(function (e) {
    var currentWorkingPanel = $('.working-panel');
    currentWorkingPanel.removeClass('working-panel');
    currentWorkingPanel.next().addClass('working-panel');
    // Perhaps wrap the following in a timeout or something? Slow the animation to be less visually jarring as per feedback from Dr.Sam
    currentWorkingPanel.toggle();
    currentWorkingPanel.next().toggle();
    e.preventDefault();
});

// Submit the XML config form with all data
$('#xmlConfig').submit(function () {
   $.post('/api/configinput/FormPost', frm.serialize())
       .done(function(data) {
           var newUrl = '/api/configinput/DownloadFromGuid?guid=' + data;
           window.location.replace(newUrl); // Purpose of this?
           // Perhaps add a thank you message?
       })
       .fail(function(jqXhR) {
            var errMessage = JSON.parse(jqXhR.responseText)["Message"];
            window.confirm(errMessage);
       });
});

// Callback methods: Support Event Listeners and provide further UI behaviors
// Call-back from any admin interation, validates logged in status
function checkAdminLoginSession() {
    var loginSession = Cookies.get('Admin_Session_Guid');

    if (typeof loginSession === "undefined") {
        return false;
    } else {
        // TODO: call /api/Login/ValidateSession instead
        return true;
    }
}
//// Call-back to add buttons to XML page on slide down
//function addButtons() {
//	$('.panel-body').append('<button class=" btn btn-success form-left-nav"><span class="glyphicon glyphicon-menu-left"></span></button>');
//	$('.panel-body').append('<button class=" btn btn-success form-right-nav"><span class="glyphicon glyphicon-menu-right"></span></button>');
//	$('.panel-first').addClass('in-focus');
//	$('.panel-first').find('.form-left-nav').prop('disabled', true);
//	$('.panel-eigth').find('.form-right-nav').css('display', 'none');
//	currentWidth += 12.5;
//	var temp = currentWidth + '%'
//	$('#userProgress').html(parseInt(currentWidth) + '%');
//	$('#userProgress').css('width', temp);
//	$('.panel').toggle();
//	$('.panel-first').toggle();
//	displayMenuChunks();
//}

// Removes buttons from XML page on form close
//function removeButtons() {
//	$('.panel-body').remove('form-left-nav');
//	$('.panel-body').remove('form-right-nav');
//}

// Callback to analyze status of captcha, if it is not valid, user cannot register on website
function captchaSelect(captchaResponse) {
	$('#submitRegistration').prop('disabled', false);
}

// Callback for Latex webhook
function refreshLatex() {
	console.log("Refresh Latex Clicked!");
}

// Callback for Downloads webhook
function refreshDownloads() {
	console.log("Refresh Downloads Clicked!");
}

// Callback for the Refresh page webhook
function refreshExamples() {
	console.log("Refresh Examples Clicked!");
}