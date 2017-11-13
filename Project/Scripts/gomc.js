/*
gomc.js is the main javascript file for the GOMC website
    Some of the pages in this website have their own embedded javascript rendered in each section
    At the top we have our global variables and objects, followed by event handlers, followed by callbacks
    This javascript was made to be compatible with jQuery 3.2.1 with jQuery-Validate v1.16.0
*/

// Global Vars

// Width of the progress bar in XML.cshtml
var currentWidth = 0;

// 
var announcementIdMap = [];
var latexIdMap = [];

//
var buildAnnouncementActionType = {
    normal: 0,
    edit: 1
};

// Object of the login Result from cookie
var loginResultType = {
    Success: 0,
    InvalidEmail: 1,
    InvalidPassword: 2
};

// Object of the announcement Result
var newAnnouncementResult = {
    Success: 0,
    SessionExpired: 1,
    InvalidSession: 2,
    MissingContent: 3
};

// Object of the announcement table length
var announcementsNavState = {
    pageIndex: 0,
    pageLength: 5,
	uiMaxPageLength: 5,
    totalLength: 0
};

// Object of announcement state
var announcementsEdit = {
	isEdit: false,
	id: 0,
	text: ""
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
    }, "Invalid pattern");

    // Extension methods for each panel in XML-Config
    //$.validator.addMethod("nowhitespace", function (val, item) { // Extension to remove whitespace from xml input forms, from additional.js for jQuery validate
    //    return this.optional(element) || /^\S+$/i.test(val);
    //}, "Input cannot have whitespace");
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
            pattern: /^[a-zA-Z0-9_.\/]*$/
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
$('#Admin').validate({
    rules: {
        uName: {
            required: true,
            email: true
        },
        pCode: "required"
    },
    messages: {
        uName: {
            required: "Please enter your email",
            email: "Please enter a valid email"
        },
        pCode: "Please enter your password"
    },
    errorElement: "span",
    errorPlacement: function (error, element) { // rules for placement of error tag
        // Add error glyph
        element.next().addClass('glyphicon glyphicon-remove');
        // Add error look
        element.parents('.form-group').addClass('has-error');
        error.addClass('help-block');
        error.appendTo(element.parents('.form-group'));
        // Remove success
        element.next().removeClass('glyphicon-ok');
        element.parents('.form-group').removeClass('has-success');
    },
    success: function (error, element) { // rules for placement of success tag
        // Add checkmark glyph
        var inputGroupParent = element.parentNode;
        var glyphError = inputGroupParent.children[2];
        $(glyphError).addClass('glyphicon glyphicon-ok');
        // add success look
        error.parents('.form-group').addClass('has-success');
        // remove errors
        $(glyphError).removeClass('glyphicon-remove');
        error.parents('.form-group').removeClass('has-error');
        error.remove();
    },
    submitHandler: function (form, e) { // callback triggered on successful validation
        $('#Admin').toggle();
        $('.loader').remove();
        $('.login-container').append('<div class="loader center-block"></div>');
        $.post('/api/Login/ValidateLogin', $(form).serialize())
            .done(function (data) {
                if (data.ResultType === loginResultType.Success) {
                    $('#Admin').toggle();
                    $('.loader').toggle();
                    // cookie for admin login session and expires in 3 days
                    Cookies.set('Admin_Session_Guid', data.Session, { expires: 3 });
                    window.location.href = "/home/admin";
                } else {
                    var failMms = data.ResultType;
                    switch (failMms) {
                        case 1:
                            window.confirm("Invalid email");
                            break;
                        case 2:
                            window.confirm("Invalid password");
                            break;
                        default:
                            window.confirm('An error has occured please try again');
                            break;
                    }
                    location.reload();
                }
            })
            .fail(function (data) {
                $('#Admin').toggle();
                $('.loader').toggle();
                console.log("Error has been thrown in login processing:"
                    + "\nError Code: " + data.status
                    + "\nError Status: " + data.statusText
                    + "\nError Details: " + data.responseJSON.ExceptionMessage
                ); // Adding detailed exception telemetry 
                $('.loader').remove();
                window.confirm("Error " + data.status + " " + data.statusText);
            });
        e.preventDefault();
    },
    invalidHandler: function (e, validator) { 
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors."
            window.confirm(errMessage);
        }
    }
});

// Logout from admin
$('#adminLogout').click(function () {
	Cookies.remove('Admin_Session_Guid');
	window.location.href = "/home/login";
});

// Post new announcement from admin page
$('#adminAnnouncement').submit(function () {
    $('#adminAnnouncement').toggle();
    $('.announcement-container').append('<div class="loader center-block"></div>');
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
        .done(function (data) {
            $('#adminAnnouncement').toggle();
            $('.loader').remove();
            if (data === newAnnouncementResult.Success) {
				console.log('New announcement submitted');
	            announcementsNavState.pageIndex = 0;
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
    var currentWorkingPanel = $('.working-panel');
    currentWorkingPanel.removeClass('working-panel');
    currentWorkingPanel.prev().addClass('working-panel');
      window.scrollTo(0, 0);
    currentWorkingPanel.slideUp('slow', () => {
        currentWorkingPanel.prev().slideDown('slow');
        currentWidth -= 25;
        updateBar(currentWidth);
    });
    // e.preventDefault();
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
    // e.preventDefault();
});

$('#xmlForm1Save').click(function () {
    $('#xmlForm1').validate();
    if ($('#xmlForm1').valid()) {
        var currentWorkingPanel = $('.working-panel');
        currentWorkingPanel.removeClass('working-panel');
        currentWorkingPanel.next().addClass('working-panel');
        window.scrollTo(0, 0);
        currentWorkingPanel.slideUp('slow', () => {
            currentWorkingPanel.next().slideDown('slow');
            currentWidth += 25;
            updateBar(currentWidth);
        });
    }
});

$('#xmlForm1').validate({
    rules: {
        gomc_config_input_Ensemble: "required",
        gomc_config_input_Restart: "required",
        gomc_config_input_Prng: "required",
        gomc_config_input_RandomSeed: {
            min: 1
        },
        gomc_config_input_ParaType: "required",
        gomc_config_input_ParametersFileName: {
            required: true,
			pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_Coordinates_1: {
			required: true,
			pattern: /^[a-zA-Z0-9_.\/]*$/ // pattern to cover the issue of no whitespace 
        },
        gomc_config_input_Coordinates_2: {
            required: true,
            //nowhitespace: true
			pattern: /^[a-zA-Z0-9_.\/]*$/ // pattern to cover the issue of no whitespace 
        },
        gomc_config_input_Structures_1: {
			required: true,
			pattern: /^[a-zA-Z0-9_.\/]*$/ // pattern to cover the issue of no whitespace 
        },
        gomc_config_input_Structures_2: {
            required: true,
            //nowhitespace: true
            pattern: /^[a-zA-Z0-9_.\/]*$/ // pattern to cover the issue of no whitespace 
        }

    },
    messages: {
        gomc_config_input_RandomSeed: {
            min: "Please input a positive number"
        },
        gomc_config_input_ParametersFileName: {
            required: "File-name required",
            pattern: "No numbers or special characters please!"
        },
		gomc_config_input_Coordinates_1: {
			required: "File-name required",
			//whitespace: "Please enter the characters without any white space"
			pattern: "No numbers or special characters please!"
        },
        gomc_config_input_Coordinates_2: {
            required: "File-name required",
            //whitespace: "Please enter the characters without any white space"
            pattern: "No numbers or special characters please!"
        },
		gomc_config_input_Structures_1: {
			required: "File-name required",
			//whitespace: "Please enter the characters without any white space"
			pattern: "No numbers or special characters please!"
        },
        gomc_config_input_Structures_2: {
            required: "File-name required",
            //whitespace: "Please enter the characters without any white space"
            pattern: "No numbers or special characters please!"
        }
    },
    errorElement: "span", // error tag name
    errorPlacement: function (error, element) { // rules for placement of error tag
        // Needs custom work for those stupid radio buttons
        // Add error glyph
        // element.next().addClass('glyphicon glyphicon-remove');
        // Add error look
        if (element.is(':radio')) {
            error.addClass('help-block');
            error.css('color', '#a94442');
            error.prependTo(element.parent().parent());
        }
        else {
            element.parent().addClass('has-error');
            error.addClass('help-block');
            error.appendTo(element.parent());
            // Remove success
            // element.next().removeClass('glyphicon-ok');
            element.parent().removeClass('has-success');
        }
    },
    success: function (error, element) { // rules for placement of success tag
        // Add checkmark glyph
        // error.prev().addClass('glyphicon glyphicon-ok');
        // add success look
        error.parent().addClass('has-success');
        // remove errors
       // error.prev().removeClass('glyphicon-remove');
        error.parent().removeClass('has-error');
        error.remove();
    },
    submitHandler: function (form, e) { // callback triggered on successful validation
      
    },
    invalidHandler: function (e, validator) {
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors."
            window.confirm(errMessage);
        }
    }
});

$('#xmlForm2Save').click(function () {
    $('#xmlForm2').validate();
    if ($('#xmlForm2').valid()) {
        var currentWorkingPanel = $('.working-panel');
        currentWorkingPanel.removeClass('working-panel');
        currentWorkingPanel.next().addClass('working-panel');
        window.scrollTo(0, 0);
        currentWorkingPanel.slideUp('slow', () => {
            currentWorkingPanel.next().slideDown('slow');
            currentWidth += 25;
            updateBar(currentWidth);
        });
    }
});

$('#xmlForm2').validate({
    rules: {
        gomc_config_input_Temperature: {
            min: 0,
            required: true
        },
        gomc_config_input_Rcut: {
            min: 0,
            required: true
        },
        gomc_config_input_RcutLow: {
            min: 0,
            required: true
        },
        gomc_config_input_Lrc: "required",
        gomc_config_input_Exclude: "required",
        gomc_config_input_Potential: "required",
        gomc_config_input_Rswitch: {
            min: 0,
            required: true
        },
        gomc_config_input_ElectroStatic: "required",
        gomc_config_input_Ewald: "required",
        gomc_config_input_CachedFourier: "required",
        gomc_config_input_Tolerance: {
            min: 0,
            required: true
        },
        gomc_config_input_Dielectric: {
            min: 0,
            required: true
        },
        gomc_config_input_OneFourScaling: {
            min: 0,
            required: true
        },
        gomc_config_input_RunSteps: {
            min: 0,
            required: true
        },
        gomc_config_input_EqSteps: {
            min: 0,
            required: true
        },
        gomc_config_input_AdjSteps: {
            min: 0,
            required: true
        },
        gomc_config_input_ChemPot_ResName: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_ChemPot_Value: {
            min: -99999,
            required: true
        },
        gomc_config_input_Fugacity_ResName: {
            required: false,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
		gomc_config_input_Fugacity_Value: {
			min: -99999,
            required: false
        },
        gomc_config_input_DisFreq: {
            min: 0,
            required: true
        },
        gomc_config_input_RotFreq: {
            min: 0,
            required: true
        },
        gomc_config_input_IntraSwapFreq: {
            min: 0,
            required: true
        },
        gomc_config_input_VolFreq: {
            min: 0,
            required: true
        },
        gomc_config_input_SwapFreq: {
            min: 0,
            required: true
        }
    },
    messages: {
        gomc_config_input_Temperature: {
            min: "Please input a positive number"
        },
        gomc_config_input_Rcut: {
            min: "Please input a positive number"
        },
        gomc_config_input_RcutLow: {
            min: "Please input a positive number"
        },
        gomc_config_input_Rswitch: {
            min: "Please input a positive number"
        },
        gomc_config_input_Tolerance: {
            min: "Please input a positive number"
        },
        gomc_config_input_Dielectric: {
            min: "Please input a positive number"
        },
        gomc_config_input_OneFourScaling: {
            min: "Please input a positive number"
        },
        gomc_config_input_RunSteps: {
            min: "Please input a positive number"
        },
        gomc_config_input_EqSteps: {
            min: "Please input a positive number"
        },
        gomc_config_input_AdjSteps: {
            min: "Please input a positive number"
        },
        gomc_config_input_ChemPot_ResName: {
            pattern: "No numbers or special characters please!"
        },
        gomc_config_input_ChemPot_Value: {
            min: "Please input a positive number"
        },
        gomc_config_input_Fugacity_ResName: {
            pattern: "No numbers or special characters please!"
        },
        gomc_config_input_Fugacity_Value: {
            min: "Please input a positive number"
        },
        gomc_config_input_DisFreq: {
            min: "Please input a positive number"
        },
        gomc_config_input_RotFreq: {
            min: "Please input a positive number"
        },
        gomc_config_input_IntraSwapFreq: {
            min: "Please input a positive number"
        },
        gomc_config_input_VolFreq: {
            min: "Please input a positive number"
        },
        gomc_config_input_SwapFreq: {
            min: "Please input a positive number"
        }
    },
    errorElement: "span", // error tag name
    errorPlacement: function (error, element) { // rules for placement of error tag
        // Needs custom work for those stupid radio buttons
        // Add error glyph
        // element.next().addClass('glyphicon glyphicon-remove');
        // Add error look
        if (element.is(':radio')) {
            error.addClass('help-block');
            error.css('color', '#a94442');
            error.prependTo(element.parent().parent());
        }
        else {
            element.parent().addClass('has-error');
            error.addClass('help-block');
            error.appendTo(element.parent());
            // Remove success
            // element.next().removeClass('glyphicon-ok');
            element.parent().removeClass('has-success');
        }
    },
    success: function (error, element) { // rules for placement of success tag
        // Add checkmark glyph
        // error.prev().addClass('glyphicon glyphicon-ok');
        // add success look
        error.parent().addClass('has-success');
        // remove errors
        // error.prev().removeClass('glyphicon-remove');
        error.parent().removeClass('has-error');
        error.remove();
    },
    submitHandler: function (form, e) { // callback triggered on successful validation

    },
    invalidHandler: function (e, validator) {
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors."
            window.confirm(errMessage);
        }
    }
});

$('#xmlForm3Save').click(function () {
    $('#xmlForm3').validate();
    if ($('#xmlForm3').valid()) {
        var currentWorkingPanel = $('.working-panel');
        currentWorkingPanel.removeClass('working-panel');
        currentWorkingPanel.next().addClass('working-panel');
        window.scrollTo(0, 0);
        currentWorkingPanel.slideUp('slow', () => {
            currentWorkingPanel.next().slideDown('slow');
            currentWidth += 25;
            updateBar(currentWidth);
        });
    }
});

$('#xmlForm3').validate({
    rules: {
        gomc_config_input_UseConstantArea: "required",
        gomc_config_input_FixVolBox0: "required",
        gomc_config_input_BoxDim_1_XAxis: {
            required: true,
            min: 0
        },
        gomc_config_input_BoxDim_1_YAxis: {
            required: true,
            min: 0
        },
        gomc_config_input_BoxDim_1_ZAxis: {
            required: true,
            min: 0
        },
        gomc_config_input_CbmcFirst: {
            required: true,
            min: 0
        },
        gomc_config_input_CbmcNth: {
            required: true,
            min: 0
        },
        gomc_config_input_CbmcAng: {
            required: true,
            min: 0
        },
        gomc_config_input_CbmcDih: {
            required: true,
            min: 0
        },
        gomc_config_input_OutputName: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_CoordinatesFreqValue: {
            required: true,
            min: 0
        },
        gomc_config_input_RestartFreq_Enabled: "required",
        gomc_config_input_RestartFreq_Value: {
            required: true,
            min: 0
        },
        gomc_config_input_ConsoleFreq_Enabled: "required",
        gomc_config_input_ConsoleFreq_Value: {
            required: true,
            min: 0
        },
        gomc_config_input_BlockAverageFreq_Enabled: "required",
        gomc_config_input_BlockAverageFreq_Value: {
            required: true,
            min: 0
        },
        gomc_config_input_HistogramFreq_Enabled: "required",
        gomc_config_input_HistogramFreq_Value: {
            required: true,
            min: 0
        }
    },
    messages: {
        gomc_config_input_BoxDim_1_XAxis: {
            min: "Please input a postive number"
        },
        gomc_config_input_BoxDim_1_YAxis: {
            min: "Please input a postive number"
        },
        gomc_config_input_BoxDim_1_ZAxis: {
            min: "Please input a postive number"
        },
        gomc_config_input_CbmcFirst: {
            min: "Please input a postive number"
        },
        gomc_config_input_CbmcNth: {
            min: "Please input a postive number"
        },
        gomc_config_input_CbmcAng: {
            min: "Please input a postive number"
        },
        gomc_config_input_CbmcDih: {
            min: "Please input a postive number"
        },
        gomc_config_input_OutputName: {
            pattern: "No whitespace, numbers or special characters"
        },
        gomc_config_input_CoordinatesFreqValue: {
            min: "Please input a postive number"
        },
        gomc_config_input_RestartFreq_Value: {
            min: "Please input a postive number"
        },
        gomc_config_input_ConsoleFreq_Value: {
            min: "Please input a postive number"
        },
        gomc_config_input_BlockAverageFreq_Value: {
            min: "Please input a postive number"
        },
        gomc_config_input_HistogramFreq_Value: {
            min: "Please input a postive number"
        }
    },
    errorElement: "span", // error tag name
    errorPlacement: function (error, element) { // rules for placement of error tag
        // Needs custom work for those stupid radio buttons
        // Add error glyph
        // element.next().addClass('glyphicon glyphicon-remove');
        // Add error look
        if (element.is(':radio')) {
            error.addClass('help-block');
            error.css('color', '#a94442');
            error.prependTo(element.parent().parent());
        }
        else {
            element.parent().addClass('has-error');
            error.addClass('help-block');
            error.appendTo(element.parent());
            // Remove success
            // element.next().removeClass('glyphicon-ok');
            element.parent().removeClass('has-success');
        }
    },
    success: function (error, element) { // rules for placement of success tag
        // Add checkmark glyph
        // error.prev().addClass('glyphicon glyphicon-ok');
        // add success look
        error.parent().addClass('has-success');
        // remove errors
        // error.prev().removeClass('glyphicon-remove');
        error.parent().removeClass('has-error');
        error.remove();
    },
    submitHandler: function (form, e) { // callback triggered on successful validation

    },
    invalidHandler: function (e, validator) {
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors."
            window.confirm(errMessage);
        }
    }
});

// Submit the XML config form with all data
//$('#xmlConfig').click(function () {
//    $('#xmlConfig').validate();
//    if ($('#xmlConfig').valid()) {
//        var currentWorkingPanel = $('.working-panel');
//        window.scrollTo(0, 0);
//        currentWorkingPanel.slideUp('slow', () => {
//            currentWorkingPanel.next().slideDown('slow');
//        });
//    }
//});

$('#xmlConfig').validate({
    rules: {
        gomc_config_input_DistName: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_HistName: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_RunNumber: {
            required: true,
            min: 0
        },
        gomc_config_input_RunLetter: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        },
        gomc_config_input_SampleFreq: {
            required: true,
            min: 0
        },
        gomc_config_input_OutEnergy_1: "required",
        gomc_config_input_OutEnergy_2: "required",
        gomc_config_input_OutPressure_1: "required",
        gomc_config_input_OutPressure_2: "required",
        gomc_config_input_OutMolNumber_1: "required",
        gomc_config_input_OutMolNumber_2: "required",
        gomc_config_input_OutDensity_1: "required",
        gomc_config_input_OutDensity_2: "required",
        gomc_config_input_OutVolume_1: "required",
        gomc_config_input_OutVolume_2: "required",
        gomc_config_input_OutSurfaceTension_1: "required",
        gomc_config_input_OutSurfaceTension_2: "required"
    },
    messages: {
        gomc_config_input_DistName: {
            pattern: "No whitespace, numbers or special characters"
        },
        gomc_config_input_HistName: {
            pattern: "No whitespace, numbers or special characters"
        },
        gomc_config_input_RunNumber: {
            min: "Input a positive number"
        },
        gomc_config_input_RunLetter: {
            pattern: "No whitespace, numbers or special characters"
        },
        gomc_config_input_SampleFreq: {
            min: "Input a positive number"
        },
    },
    errorElement: "span", // error tag name
    errorPlacement: function (error, element) { // rules for placement of error tag
        // Needs custom work for those stupid radio buttons
        // Add error glyph
        // element.next().addClass('glyphicon glyphicon-remove');
        // Add error look
        if (element.is(':radio')) {
            error.addClass('help-block');
            error.css('color', '#a94442');
            error.prependTo(element.parent().parent());
        }
        else {
            element.parent().addClass('has-error');
            error.addClass('help-block');
            error.appendTo(element.parent());
            // Remove success
            // element.next().removeClass('glyphicon-ok');
            element.parent().removeClass('has-success');
        }
    },
    success: function (error, element) { // rules for placement of success tag
        // Add checkmark glyph
        // error.prev().addClass('glyphicon glyphicon-ok');
        // add success look
        error.parent().addClass('has-success');
        // remove errors
        // error.prev().removeClass('glyphicon-remove');
        error.parent().removeClass('has-error');
        error.remove();
    },
	submitHandler: function (form, e) {
		console.log($('#xmlForm1').serialize());
		console.log($('#xmlForm2').serialize());
		console.log($('#xmlForm3').serialize());
		console.log($('#xmlFonfig').serialize());

		var xmlData = $('#xmlForm1').serialize() + '&' + $('#xmlForm2').serialize() + '&' + $('#xmlForm3').serialize() + '&' + $('#xmlConfig').serialize();

		console.log(xmlData);

        $.post('/api/configinput/FormPost', xmlData) 
            .done(function (data) {
                var newUrl = '/api/configinput/DownloadFromGuid?guid=' + data;
                window.location.replace(newUrl); // Purpose of this?
                // Perhaps add a thank you message?
                var currentWorkingPanel = $('.working-panel');
                currentWorkingPanel.removeClass('working-panel');
                currentWorkingPanel.next().addClass('working-panel');
                window.scrollTo(0, 0);
                currentWorkingPanel.slideUp('slow', () => {
                    currentWorkingPanel.next().slideDown('slow');
                    currentWidth += 25;
                    updateBar(currentWidth);
                });
            })
            .fail(function (err) {
                window.confirm(err.statusText + " Please try again");
                var messageExplained = JSON.parse(err.responseJSON.Message);
                console.log(
                    "Status: " + err.status
                    + "\n Status Text: " + err.statusText
                    + "\n Full Response: " + messageExplained.general[0]
                    + "\n Check the network tab in browser debugger for more details"
                );
            });
    },
    invalidHandler: function (e, validator) {
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors."
            window.confirm(errMessage);
        }
    }
});

// Callback methods: Support Event Listeners and provide further UI behaviors


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
            for (i = 0; i < announcementsNavState.uiMaxPageLength; i++) {
                $("#fetchAnnouncements_Message_" + i).text('');
                $("#fetchAnnouncements_Created_" + i).text('');
                $("#fetchAnnouncements_Action_" + i).text('');

                $("#fetchAnnouncements_tr_" + i).hide();
            }
			for (i = 0; i < data.Length; i++) {
				announcementIdMap[i] = data.Announcements[i].Id;
                $("#fetchAnnouncements_tr_" + i).show();
	            buildAnnouncementActions(i);
				$("#fetchAnnouncements_Message_" + i).text(data.Announcements[i].Content);

				var dt = new Date(data.Announcements[i].Created);
				$("#fetchAnnouncements_Created_" + i).text(dt.toLocaleDateString("en-US") + " " + dt.toLocaleTimeString("en-US"));
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
function doLatexPdf(i) {
	window.location.href = '/api/admin/downloadlatexfile?latexId=' + latexIdMap[i] + '&Kind=Pdf';
	//$.ajax({
	//	url: '/api/admin/downloadlatexfile',
	//	type: 'POST',
	//	contentType: 'application/json',
	//	data: JSON.stringify({
	//		Kind: "Pdf",
	//		LatexUploadId: latexIdMap[i]
	//	})
	//});
	return false;
}
function doLatexUse(i) {
	$.ajax({
		url: '/api/admin/publishlatexupload?latexId=' + latexIdMap[i],
		type: 'GET'
	}).done(function(data) {
		console.log(data);
		window.alert('Publish is done!');
	});
	return false;
}

function doFetchLatexUploads() {

	var validateSessionResultType = {
		SessionValid: 0,
		SessionExpired: 1,
		SessionInvalid: 2
	};

	function latexUploadActions(a) {
		function atag(val, i, fn, hf) {
			hf = (typeof hf !== 'undefined') ? hf : '/api/admin';
			return "<a id='" + i + "' href='" + hf + "' onclick='return " + fn + "'>" + val + "</a>";
		}

		return "" +
			atag("Publish", 'LatexUpload_Use_' + a, 'doLatexUse(' + a + ')') +
			" " +
			atag("Get Pdf", 'LatexUpload_Pdf_' + a, 'doLatexPdf(' + a + ')');
	}

	$.ajax({
		url: '/api/Admin/FetchLatexUploads',
		type: 'POST',
		contentType: 'application/json'
	}).done(function (data) {
		console.log(data);
		if (data.AuthResult === validateSessionResultType.SessionValid) {
			var i = 0;
			for (i = 0; i < 5; i++) {
				$("#LatexUpload_Version_" + i).text('');
				$("#LatexUpload_Created_" + i).text('');
				$("#LatexUpload_Action_" + i).text('');

				if (i >= data.Length) {
					$("#LatexUpload_" + i).hide();
				} else {
					latexIdMap[i] = data.Uploads[i].Id;
					$("#LatexUpload_" + i).show();
					$("#LatexUpload_Version_" + i).text(data.Uploads[i].Version);
					var dt = new Date(data.Uploads[i].Created);
					$("#LatexUpload_Created_" + i).text(dt.toLocaleDateString("en-US") + " " + dt.toLocaleTimeString("en-US"));
					$("#LatexUpload_Action_" + i).html(latexUploadActions(i));
					console.log($("#LatexUpload_Action_" + i).html());
				}
			}
		} else if (data.AuthResult === validateSessionResultType.SessionInvalid) {
			console.log('Could not fetch latex uploads, bad session');
			window.location.href = "/home/login";
		} else if (data.AuthResult === validateSessionResultType.SessionExpired) {
			console.log('Could not fetch latex uploads, session expired');
			window.location.href = "/home/login";
		}
	});
}
function buildAnnouncementActions(a) {
	function atag(val, i, fn, hf) {
		hf = (typeof hf !== 'undefined') ? hf : '/api/admin';
		return "<a id='" + i + "' href='" + hf + "' onclick='return " + fn + "'>" + val + "</a>";
	}

	var spc = "  ";
	if (announcementsEdit.isEdit === false) {
		$("#fetchAnnouncements_Action_" + a).html(
			atag('Edit', 'AnnouncementEdit_' + a, 'doEditAnnouncement(' + a + ')') + spc +
			atag('Remove', 'AnnouncementRemove_' + a, 'doRemoveAnnouncement(' + a + ')')
		);
	} else {
		$("#fetchAnnouncements_Action_" + a).html(
			atag('Save', 'AnnouncementSave_' + a, 'doSaveAnnouncement(' + a + ')') + spc +
			atag('Cancel', 'AnnouncementCancel_' + a, 'doCancelAnnouncement(' + a + ')') + spc +
			atag('Remove', 'AnnouncementRemove_' + a, 'doRemoveAnnouncement(' + a + ')')
		);
	}

}

function doCancelAnnouncement(a) {
	$("#fetchAnnouncements_Message_" + a).text(announcementsEdit.text);
	announcementsEdit.isEdit = false;
	announcementsEdit.text = "";
	announcementsEdit.id = 0;
	buildAnnouncementActions(a);
	return false;
}

function doSaveAnnouncement(a) {
	var newContent = $("#EditAnnouncementText").text();
	$.ajax({
			url: '/api/admin/editannouncement',
			type: 'POST',
			contentType: 'application/json',
			data: JSON.stringify({
				AnnouncementId: announcementIdMap[a],
				NewContent: newContent
			})
		})
		.done(function(data) {
			if (data === newAnnouncementResult.Success) {
				console.log('save announcement success');
				announcementsEdit.text = newContent;
				doCancelAnnouncement(a);
			} else {
				console.log('save announcement fail');
				doCancelAnnouncement(a);
			}
		});
	return false;
}

function doEditAnnouncement(a) {
	if (announcementsEdit.isEdit === false) {
		announcementsEdit.isEdit = true;
		announcementsEdit.id = a;
		announcementsEdit.text = $("#fetchAnnouncements_Message_" + a).text();
		$("#fetchAnnouncements_Message_" + a).html("<textarea id='EditAnnouncementText'></textarea>");
		$("#EditAnnouncementText").text(announcementsEdit.text);
		buildAnnouncementActions(a);
	} else {
		doCancelAnnouncement(announcementsEdit.id);
		doEditAnnouncement(a);
	}
	return false;
}

function doFetchGomcAnnouncements() {
	$.ajax({
		url: '/api/HomeApi/FetchAnnouncements',
		type: 'POST',
		contentType: 'application/json'
	}).done(function (data) {
		console.log(data);
		var i = 0;
		for (i = 0; i < 5; i++) {
			if (data.length < i) {
				$("#GomcAnnouncement_" + i).hide();
			} else {
				console.log(data[i].Content);
				$("#GomcAnnouncement_" + i).html(data[i].Content);
			}
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
			AnnouncementId: announcementIdMap[a]
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

// Callback to update the progress bar in the xml config form
function updateBar(currentWidth) {
    var newWidth = currentWidth + '%';
    $('#userProgress').css('width', newWidth);
}