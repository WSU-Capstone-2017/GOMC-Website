// Downloads Page JS
$(function () {
    //console.log('READY');
    $('[data-toggle="tooltip"]').tooltip();  

    $.validator.addMethod("pattern", function (value, element, param) { 
        if (this.optional(element)) {
            return true;
        }
        if (typeof param === "string") {
            param = new RegExp("^(?:" + param + ")$");
        }
        return param.test(value);
    }, "Invalid pattern");

    $.validator.addMethod("accept", function (value, element, param) {
        var typeParam = typeof param === "string" ? param.replace(/\s/g, "") : "image/*",
            optionalValue = this.optional(element),
            i, file, regex;

        if (optionalValue) {
            return optionalValue;
        }

        if ($(element).attr("type") === "file") {
            typeParam = typeParam
                .replace(/[\-\[\]\/\{\}\(\)\+\?\.\\\^\$\|]/g, "\\$&")
                .replace(/,/g, "|")
                .replace(/\/\*/g, "/.*");

            if (element.files && element.files.length) {
                regex = new RegExp(".?(" + typeParam + ")$", "i");
                for (i = 0; i < element.files.length; i++) {
                    file = element.files[i];

                    if (!file.type.match(regex)) {
                        return false;
                    }
                }
            }
        }
        return true;
    }, $.validator.format("Please enter a value with a valid mimetype."));

    $.validator.addMethod("extension", function (value, element, param) {
        param = typeof param === "string" ? param.replace(/,/g, "|") : "png|jpe?g|gif";
        return this.optional(element) || value.match(new RegExp("\\.(" + param + ")$", "i"));
    }, $.validator.format("Please enter a value with a valid extension."));
});

// Object to appear and validate against in the orange button of Registration.cshtml
var registrationString = {
    init: '<span class="glyphicon glyphicon-collapse-down"></span> Close Form and proceed without registering',
    fin: '<span class="glyphicon glyphicon-collapse-up"></span> Open Form and Register'
};

// Registration form on the downloads.cshtml
$('#registrationForm').validate({ 
    rules: { 
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
    errorElement: "span", 
    errorPlacement: function (error, element) {        
        element.next().addClass('glyphicon glyphicon-remove');
        element.parent().addClass('has-error');
        error.addClass('help-block');
        error.appendTo(element.parent());
        element.next().removeClass('glyphicon-ok');
        element.parent().removeClass('has-success');
    },
    success: function (error, element) { 
        error.prev().addClass('glyphicon glyphicon-ok');
        error.parent().addClass('has-success');
        error.prev().removeClass('glyphicon-remove');
        error.parent().removeClass('has-error');
        error.remove();
    },
    messages: { 
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
    submitHandler: function (form, e) { 
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
                    ); 
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
    invalidHandler: function (e, validator) { 
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