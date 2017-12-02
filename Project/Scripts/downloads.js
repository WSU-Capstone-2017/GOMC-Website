// Downloads Page JS
$(function () {
    //console.log('READY');
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

    $.validator.addMethod("accept", function (value, element, param) {

        // Split mime on commas in case we have multiple types we can accept
        var typeParam = typeof param === "string" ? param.replace(/\s/g, "") : "image/*",
            optionalValue = this.optional(element),
            i, file, regex;

        // Element is optional
        if (optionalValue) {
            return optionalValue;
        }

        if ($(element).attr("type") === "file") {
            // Escape string to be used in the regex
            // see: https://stackoverflow.com/questions/3446170/escape-string-for-use-in-javascript-regex
            // Escape also "/*" as "/.*" as a wildcard
            typeParam = typeParam
                .replace(/[\-\[\]\/\{\}\(\)\+\?\.\\\^\$\|]/g, "\\$&")
                .replace(/,/g, "|")
                .replace(/\/\*/g, "/.*");

            // Check if the element has a FileList before checking each file
            if (element.files && element.files.length) {
                regex = new RegExp(".?(" + typeParam + ")$", "i");
                for (i = 0; i < element.files.length; i++) {
                    file = element.files[i];

                    // Grab the mimetype from the loaded file, verify it matches
                    if (!file.type.match(regex)) {
                        return false;
                    }
                }
            }
        }
        // Either return true because we've validated each file, or because the
        // browser does not support element.files and the FileList feature
        return true;
    }, $.validator.format("Please enter a value with a valid mimetype."));

    // Older "accept" file extension method. Old docs: http://docs.jquery.com/Plugins/Validation/Methods/accept
    $.validator.addMethod("extension", function (value, element, param) {
        param = typeof param === "string" ? param.replace(/,/g, "|") : "png|jpe?g|gif";
        return this.optional(element) || value.match(new RegExp("\\.(" + param + ")$", "i"));
    }, $.validator.format("Please enter a value with a valid extension."));

    // Extension methods for each panel in XML-Config
    //$.validator.addMethod("nowhitespace", function (val, item) { // Extension to remove whitespace from xml input forms, from additional.js for jQuery validate
    //    return this.optional(element) || /^\S+$/i.test(val);
    //}, "Input cannot have whitespace");
});

// Object to appear and validate against in the orange button of Registration.cshtml
var registrationString = {
    init: '<span class="glyphicon glyphicon-collapse-down"></span> Close Form and proceed without registering',
    fin: '<span class="glyphicon glyphicon-collapse-up"></span> Open Form and Register'
};

// Orange button on downloads.cshtml
// Button removed as per client request
//$('#closeRegistration').click(function () {
//    $(this).next().slideToggle(() => {
//        $(this).html((count, words) => {
//            return words === '<span class="glyphicon glyphicon-collapse-down"></span> Close Form and proceed without registering' ? registrationString.fin : registrationString.init;
//        });
//    });
//});

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
    submitHandler: function (form, e) { // callback triggered on successful validation
        try {
            console.log('process here');
            $('.registration-container').children().remove();
            $('.registration-container').append('<div class="loader"></div>');
            $.post('/api/Registration/Input', $(form).serialize())
                .done(function (data) {
                    $('.registration-container').html('Thanks for Registering! <span class="glyphicon glyphicon-ok-sign"></span> ');
                    $('.registration-container').css({
                        "backgroundColor": "#3C763D",
                        "color": "#FFF"
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
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors.";
            window.confirm(errMessage);
        }
    }

});

// Callback to analyze status of captcha, if it is not valid, user cannot register on website
function captchaSelect(captchaResponse) {
    $('#submitRegistration').prop('disabled', false);
}