// Login Page JS

// Object of the login Result from cookie
var loginResultType = {
    Success: 0,
    InvalidEmail: 1,
    InvalidPassword: 2,
    NeedCaptcha: 3,
    InvalidCaptcha: 4
};

// Login to admin page from login page
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
    errorPlacement: function (error, element) { 
        element.next().addClass('glyphicon glyphicon-remove');
        element.parents('.form-group').addClass('has-error');
        error.addClass('help-block');
        error.appendTo(element.parents('.form-group'));
        element.next().removeClass('glyphicon-ok');
        element.parents('.form-group').removeClass('has-success');
    },
    success: function (error, element) { 
        var inputGroupParent = element.parentNode;
        var glyphError = inputGroupParent.children[2];
        $(glyphError).addClass('glyphicon glyphicon-ok');
        error.parents('.form-group').addClass('has-success');
        $(glyphError).removeClass('glyphicon-remove');
        error.parents('.form-group').removeClass('has-error');
        error.remove();
    },
    submitHandler: function (form, e) {
        $('#Admin').toggle();
        $('.loader').remove();
        $('.login-container').append('<div class="loader center-block"></div>');
        $.post('/api/Login/ValidateLogin', $(form).serialize())
            .done(function (data) {
                if (data.ResultType === loginResultType.Success) {
                    $('#Admin').toggle();
                    $('.loader').toggle();
                    Cookies.set('Admin_Session_Guid', data.Session, { expires: 3 });
                    window.location.href = "/home/admin";
                } else if (data.ResultType === loginResultType.NeedCaptcha) {

                    console.log("need captcha");

                    $('#Admin').toggle();
                    $('.loader').toggle();

                    $("#loginCaptchaDiv").removeClass("hidden");

                    window.confirm("Need captcha");

                } else if (data.ResultType === loginResultType.InvalidCaptcha) {

                    console.log("Invalid captcha");

                    $('#Admin').toggle();
                    $('.loader').toggle();

                    $("#loginCaptchaDiv").removeClass("hidden");

                    window.confirm("Captcha is invalid");

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
                ); 
                $('.loader').remove();
                window.confirm("Error " + data.status + " " + data.statusText);
            });
        e.preventDefault();
    },
    invalidHandler: function (e, validator) {
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors.";
            window.confirm(errMessage);
        }
    }
});