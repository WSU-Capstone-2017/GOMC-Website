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

// 
var newAnnouncementResult = {
    Success: 0,
    SessionExpired: 1,
    InvalidSession: 2,
    MissingContent: 3
};

//
var announcementsNavState = {
    pageIndex: 0,
    pageLength: 25,
    totalLength: 0
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
    $.validator.addMethod("pattern", function (value, element, param) { // Extension for regex on names from additonal.js for jquery validate
        if (this.optional(element)) {
            return true;
        }
        if (typeof param === "string") {
            param = new RegExp("^(?:" + param + ")$");
        }
        return param.test(value);
    }, "Invalid format.");
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
$('#registrationForm').validate({ // jQuery Validate
    rules: { // rules of parameters to validate against
        userName: {
            minlength: 2,
            required: true,
            pattern: /^[a-zA-Z_]*$/
        },
        userEmail: {
            required: true,
            email: true
        },
        userAffliation: "required",
        extraComment: "required"
    }, 
    errorElement: "span", // error tag name
    errorPlacement: function (error, element) { // rules for placement of error tag
        // Add error glyph
        element.next().addClass('glyphicon glyphicon-remove');
        // Add error look
        element.parent().addClass('has-error');
        error.addClass('help-block');
        error.appendTo(element.parent());
        // Remove success
        element.next().removeClass('glyphicon-ok');
        element.parent().removeClass('has-success');
    },
    success: function (error, element) { // rules for placement of success tag
        // Add checkmark glyph
        error.prev().addClass('glyphicon glyphicon-ok');
        // add success look
        error.parent().addClass('has-success');
        // remove errors
        error.prev().removeClass('glyphicon-remove');
        error.parent().removeClass('has-error');
        error.remove();
    },
    messages: { // Different error messages for each error type
        userName: {
            required: "Please tell us who you are so we can email you!",
            minlength: "Your name should at least have 2 characters",
            pattern: "No numbers or special characters please!"
        },
        userEmail: {
            required: "We need a way to contact you, please tell us your email",
            email: "That doesn't seem quite right... Please try again"
        },
        userAffliation: "Tell us your company name or unverisity",
        extraComment: "Provide us with a brief reason as to why you want to hear from us"
    },
    submitHandler: function (form,e) { // callback triggered on successful validation
        try {
            console.log('process here');
            $('.registration-container').children().remove();
            $('.registration-container').append('<div class="loader"></div>');
            $.post('/api/Registration/Input', $(form).serialize())
                .done(function (data) {
                    $('#closeRegistration').html('Thanks for Registering! <span class="glyphicon glyphicon-ok-sign"></span> ');
                    $('#closeRegistration').addClass('btn-success');
                    $('#closeRegistration').removeClass('btn-warning');
                    $('#closeRegistration').next().slideToggle(() => {
                        $('#closeRegistration').prop('disabled', true);
                        $('.loader').remove();
                    });
                })

                .fail(function (jqXhR) {
                    console.log("Error has been thrown in registration submission:"
                        + "\nError Code: " + jqXhR.status
                        + "\nError Status: " + jqXhR.statusText
                        + "\nError Details: " + jqXhR.responseJSON.ExceptionMessage
                    ); // Adding detailed exception telemetry 
                    $('.loader').remove();

                });
        }
        catch (ex) {
            alert("The following error occured: " + ex.message + " in " + ex.fileName + " at " + ex.lineNumber);
            $('.loader').remove();

        }
        finally {
            e.preventDefault();
        }
    },
    invalidHandler: function (e, validator) { // callback triggered on failed validation
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount ===1? "You have 1 error." : "You have " + errorCount + " errors."
            window.confirm(errMessage);
        }
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
				window.location.href = "/home/admin";
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
	window.location.href = "/home/login";
});

// Post new announcement from admin page
$('#adminAnnouncement').submit(function () {

	var msgContent = $("#adminAnnouncement_Text").val();
    $("#adminAnnouncement_Text").text("");

    $.ajax({
        url: '/api/Admin/NewAnnouncement',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
			Content: msgContent
            })
        })
        .done(function(data) {
            if (data === newAnnouncementResult.Success) {
				console.log('New announcement submitted');
	            announcementsNavState.pageIndex =
		            Math.ceil(((announcementsNavState.totalLength + 1) / announcementsNavState.pageLength) - 1);
	            doFetchAnnouncements();
            } else if (data === newAnnouncementResult.MissingContent) {
	            window.alert('Message content cannot be empty.');

            } else if (data === newAnnouncementResult.InvalidSession) {
				console.log('New announcement failed to submit, bad session');
	            window.location.href = "/Home/Login";
            } else if (data === newAnnouncementResult.SessionExpired) {
				console.log('New announcement failed to submit, session expired');
	            window.location.href = "/Home/Login";
            }
        });
    return false;
});

// Change the XML config page by displaying the previous card
$('.prev-btn').click(function (e) {
    // Some sort of panel validation? Mini-forms? Sub-categories?
    var currentWorkingPanel = $('.working-panel');
    currentWorkingPanel.removeClass('working-panel');
    currentWorkingPanel.prev().addClass('working-panel');
      window.scrollTo(0, 0);
    currentWorkingPanel.slideUp('slow', () => {
        currentWorkingPanel.prev().slideDown('slow');
    });
    e.preventDefault();
});

// Change the XML config page by displaying the next card on validation success
$('.next-btn').click(function (e) {
     // Some sort of panel validation? Mini-forms? Sub-categories?
    var currentWorkingPanel = $('.working-panel');
    currentWorkingPanel.removeClass('working-panel');
    currentWorkingPanel.next().addClass('working-panel');
    window.scrollTo(0, 0);
    currentWorkingPanel.slideUp('slow', () => {
        currentWorkingPanel.next().slideDown('slow');
    });
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

function doFetchAnnouncements() {
    announcementsNavState.totalLength = 0;
    $.ajax({
        url: '/api/Admin/FetchAnnouncements',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            pageIndex: announcementsNavState.pageIndex,
            pageLength: announcementsNavState.pageLength
        })
    }).done(function (data) {
        console.log(data);
        updateNavAnnouncements(data.TotalLength);
        if (data.Result === newAnnouncementResult.Success) {
            var i = 0;
            for (i = 0; i < announcementsNavState.pageLength; i++) {
                $("#fetchAnnouncements_Message_" + i).text('');
                $("#fetchAnnouncements_Created_" + i).text('');
                $("#fetchAnnouncements_Action_" + i).text('');

                $("#fetchAnnouncements_tr_" + i).hide();
            }
            for (i = 0; i < data.Length; i++) {

                $("#fetchAnnouncements_tr_" + i).show();
                $("#fetchAnnouncements_Message_" + i).text(data.Announcements[i].Content);
                $("#fetchAnnouncements_Created_" + i).text(data.Announcements[i].Created);
                $("#fetchAnnouncements_Action_" + i).html(
                    "<a href='/api/Admin/DeleteAnnouncement' onclick='return doRemoveAnnouncement(" + data.Announcements[i].Id + ")'>Remove</a>");
            }
        } else if (data === newAnnouncementResult.InvalidSession) {
            console.log('Could not fetch announcements, bad session');
            window.location.href = "/home/login";
        } else if (data === newAnnouncementResult.SessionExpired) {
            console.log('Could not fetch announcements, session expired');
            window.location.href = "/home/login";
        }
    });
}

function updateNavAnnouncements(totalLength) {
    $("#fetchAnnouncements_Next").addClass("btn disabled");
    $("#fetchAnnouncements_Back").addClass("btn disabled");

    announcementsNavState.totalLength = totalLength;

    var maxPages = Math.ceil(announcementsNavState.totalLength / announcementsNavState.pageLength);
    if ((announcementsNavState.pageIndex + 1) < maxPages) {
        $("#fetchAnnouncements_Next").removeClass("btn disabled");
    } else {
        announcementsNavState.pageIndex = maxPages - 1;
    }
    if (announcementsNavState.pageIndex > 0) {
        $("#fetchAnnouncements_Back").removeClass("btn disabled");
    } else if (announcementsNavState.pageIndex < 0) {
        announcementsNavState.pageIndex = 0;
    }
}

function updateAnnouncementsNavStateTotalLength() {
    announcementsNavState.totalLength = 0;
    $.ajax({
        url: '/api/Admin/GetAnnouncementsCount',
        type: 'POST',
        contentType: 'application/json'
    }).done(function (data) {
        if (data.Result === newAnnouncementResult.Success) {
            updateNavAnnouncements(data.TotalLength);
        } else if (data === newAnnouncementResult.InvalidSession) {
            console.log('Could not fetch announcements count, bad session');
            window.location.href = "/home/login";
        } else if (data === newAnnouncementResult.SessionExpired) {
            console.log('Could not fetch announcements count, session expired');
            window.location.href = "/home/login";
        }
    });
}

function doNavAnnouncements(a) {
    if (a) {
        announcementsNavState.pageIndex--;
    } else {
        announcementsNavState.pageIndex++;
    }
    doFetchAnnouncements();

    return false;
}

function doRemoveAnnouncement(a) {
    $.ajax({
        url: '/api/Admin/DeleteAnnouncement',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            AnnouncementId: a
        })
    })
        .done(function (data) {
            console.log(data);
            if (data.Result === newAnnouncementResult.Success) {
                doFetchAnnouncements();
            } else if (data === newAnnouncementResult.InvalidSession) {
                console.log('Could not remove announcement, bad session');
                window.location.href = "/Home/Login";
            } else if (data === newAnnouncementResult.SessionExpired) {
                console.log('Could not remove announcement, session expired');
                window.location.href = "/Home/Login";
            }
        });
    return false;
}