// Admin Page JS




var announcementIdMap = [];
var latexIdMap = [];
var buildAnnouncementActionType = {
    normal: 0,
    edit: 1
};
// Object of the announcement Result
var newAnnouncementResult = {
    Success: 0,
    SessionExpired: 1,
    InvalidSession: 2,
    MissingContent: 3
};

// Object of the announcement table
var announcementsNavState = {
    pageIndex: 0,
    pageLength: 5,
    uiMaxPageLength: 5,
    totalLength: 0
};

// Object of the registered users table
var registeredUsersNavState = {
    pageIndex: 0,
    pageLength: 25,
    totalLength: 0,
    isDesc: true,
    currentTh: 0,
    nameFilter: "",
    emailFilter: ""
};

// Object of announcement state
var announcementsEdit = {
    isEdit: false,
    id: 0,
    text: ""
};

// Latex file object
var latexFileData = {};

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

$("#registerdUseresFilter_Name").change(function () {
    registeredUsersNavState.nameFilter = $("#registerdUseresFilter_Name").val();
});

$("#registerdUseresFilter_Email").change(function () {
    registeredUsersNavState.emailFilter = $("#registerdUseresFilter_Email").val();
});

$("#adminAnnouncement_Text").keyup(function () {
    doPreviewAnnouncements($("#adminAnnouncement_Text").val());
});

// Listener for new tex file upload
$("#adminLatexUpload_File").change(function (e) {
    //console.log('upload change');
    latexFileData = this.files[0];
    // checkLatexUploadFormButtonDisabled();
    $('#fileNameContainer').val(latexFileData.name);
});

// Validation for Latex-upload
$('#adminLatexUpload').validate({
    debug: true,
    rules: {
        file: {
            required: true,
            extension: "tex"
            // ,accept: "application/x-latex" // Fails every-time for unknown reason? Should fix later but for now it does the basic job of stopping non-tex files
        },
        version: {
            required: true,
            pattern: /^[a-zA-Z0-9_.\/]*$/
        }
    },

    messages: {
        file: {
            extension: "Unsupported file type, you must upload a latex file"
            // ,accept: "Improper file format, please check the file and try again" // Same as issue above
        },
        version: {
            pattern: "Invalid naming convention, no whitespaces or special characters"
        }
    },

    errorElement: "span", // error tag name

    errorPlacement: function (error, element) { // rules for placement of error tag
        element.parent().parent().addClass('has-error');
        error.addClass('help-block');
        error.appendTo(element.parent());
    },

    success: function (error, element) { // rules for placement of success tag
        error.removeClass('help-block');
        error.parents('.form-group').removeClass('has-error');
        error.remove();
    },

    submitHandler: function (form, e) {
        // document.write('Good');
        e.preventDefault();
        $('#adminLatexUpload').toggle();
        $('.latex-container').append('<div class="loader center-block"></div><span class="loader-mms">Processing please wait...</span>'); // Needs to be tested
        // console.log(adminLatexUpload);
        // $("#adminLatexUpload_Submit").prop('disabled', true);
        var adminLatexUploadForm = new FormData();
        adminLatexUploadForm.append('file', latexFileData);
        adminLatexUploadForm.append('version', $("#adminLatexUpload_Version").val());
        var xhr = new XMLHttpRequest();
        xhr.open("POST", "/api/Latex/Convert", true);
        xhr.addEventListener("load",
            function (evt) {
                // console.log('load');
                // console.log(evt);
                $("#adminLatexUpload_Submit").prop('disabled', false);
                doFetchLatexUploads();
                if (xhr.status >= 200 && xhr.status < 400) {
                    $('#adminLatexUpload').toggle();
                    $('.loader').remove();
                    $('.loader-mms').remove();
                    // console.log("Processed");
                }
            }, false);
        xhr.addEventListener("error",
            function (err) {
                $('#adminLatexUpload').toggle();
                $('.loader').remove();
                $('.loader-mms').remove();
                window.confirm(err.statusText + " Please try again");
                var messageExplained = JSON.parse(err.responseJSON.Message);
                console.log(
                    "Status: " + err.status
                    + "\n Status Text: " + err.statusText
                    + "\n Full Response: " + messageExplained.general[0]
                    + "\n Check the network tab in browser debugger for more details"
                );
            },
            false);
        xhr.send(adminLatexUploadForm);
        // Below JQuery method is busted AF, I gotta figure out a better way of doing it
        //var adminLatexUploadForm = new FormData();
        //adminLatexUploadForm.append('file', $('adminLatexUpload_File').val());
        //adminLatexUploadForm.append('version', $("#adminLatexUpload_Version").val());
        // Prompt a link to do something?  On success-bar?
        //$.post('/api/Latex/Convert', adminLatexUploadForm)
        //    .done(function (data) {
        //        $('#adminLatexUpload').toggle();
        //        $('.loader').remove();
        //    })
        //    .fail(function (err) {
        //        $('#adminLatexUpload').toggle();
        //        $('.loader').remove();
        //        window.confirm(err.statusText + " Please try again");
        //        var messageExplained = JSON.parse(err.responseJSON.Message);
        //        console.log(
        //            "Status: " + err.status
        //            + "\n Status Text: " + err.statusText
        //            + "\n Full Response: " + messageExplained.general[0]
        //            + "\n Check the network tab in browser debugger for more details"
        //        );
        //    });
    },

    invalidHandler: function (e, validator) {
        var errorCount = validator.numberOfInvalids();
        if (errorCount) {
            var errMessage = errorCount === 1 ? "You have 1 error." : "You have " + errorCount + " errors.";
            window.confirm(errMessage);
        }
    }
});

function checkAnnouncementNavBounds() {
    if (announcementsNavState.pageIndex < 0) {
        announcementsNavState.pageIndex = 0;
    }
}
function doFetchAnnouncements() {
    checkAnnouncementNavBounds();
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
        announcementsDataCache = data;
        updateNavAnnouncements(data.TotalLength);
        if (data.Result === newAnnouncementResult.Success) {
            var i = 0;
            for (i = 0; i < announcementsNavState.uiMaxPageLength; i++) {
                $("#fetchAnnouncements_Message_" + i + " > div").text('');
                $("#fetchAnnouncements_Created_" + i).text('');
                $("#fetchAnnouncements_Action_" + i).text('');

                $("#fetchAnnouncements_tr_" + i).hide();
            }
            for (i = 0; i < data.Length; i++) {
                announcementIdMap[i] = data.Announcements[i].Id;
                $("#fetchAnnouncements_tr_" + i).show();
                buildAnnouncementActions(i);
                $("#fetchAnnouncements_Message_" + i + " > div").text(data.Announcements[i].Content);

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
    doFetchPreviewAnnouncements();
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
    }).done(function (data) {
        // console.log(data);
        window.alert('Publish is done!');
    });
    return false;
}

var validateSessionResultType = {
    SessionValid: 0,
    SessionExpired: 1,
    SessionInvalid: 2
};

function doFetchLatexUploads() {

    function latexUploadActions(a) {
        function atag(val, i, fn, hf) {
            hf = (typeof hf !== 'undefined') ? hf : '/api/admin';
            return "<a id='" + i + "' href='" + hf + "' onclick='return " + fn + "'>" + val + "</a>";
        }

        return "" +
            atag("Publish", 'LatexUpload_Use_' + a, 'doLatexUse(' + a + ')') +
            "<br/>" +
            atag("Get Pdf", 'LatexUpload_Pdf_' + a, 'doLatexPdf(' + a + ')');
    }

    $.ajax({
        url: '/api/Admin/FetchLatexUploads',
        type: 'POST',
        contentType: 'application/json'
    }).done(function (data) {
        // console.log(data);
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
                    // console.log($("#LatexUpload_Action_" + i).html());
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
    $("#fetchAnnouncements_Message_" + a + " > div").text(announcementsEdit.text);
    makeAnnouncementBoxEditable(a, false);
    announcementsEdit.isEdit = false;
    announcementsEdit.text = "";
    announcementsEdit.id = 0;
    buildAnnouncementActions(a);
    return false;
}

function makeAnnouncementBoxEditable(a, b) {
    var $div = $("#fetchAnnouncements_Message_" + a + " > div");
    $div.prop('contenteditable', b);
    $div.focus();
}
function doSaveAnnouncement(a) {
    var newContent = $("#fetchAnnouncements_Message_" + a + " > div").text();
    // console.log('saving: ' + newContent);
    $.ajax({
        url: '/api/admin/editannouncement',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            AnnouncementId: announcementIdMap[a],
            NewContent: newContent
        })
    })
        .done(function (data) {
            if (data === newAnnouncementResult.Success) {
                console.log('save announcement success');
                announcementsEdit.text = newContent;
                doCancelAnnouncement(a);
                doFetchPreviewAnnouncements();
            } else {
                console.log('save announcement fail');
                doCancelAnnouncement(a);
            }
        });
    return false;
}

function doEditAnnouncement(a) {
    for (var i = 0; i < announcementsNavState.uiMaxPageLength; i++)
        makeAnnouncementBoxEditable(i, i === a);

    if (announcementsEdit.isEdit === false) {
        announcementsEdit.isEdit = true;
        announcementsEdit.id = a;
        announcementsEdit.text = $("#fetchAnnouncements_Message_" + a + " > div").text();
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
        // console.log(data);
        var i = 0;
        for (i = 0; i < 5; i++) {
            if (data.length < i) {
                $("#GomcAnnouncement_" + i).hide();
            } else {
                // console.log(data[i].Content);
                $("#GomcAnnouncement_" + i).html(data[i].Content);
            }
        }
    });
}

function updateNavAnnouncements(totalLength) {
    $("#fetchAnnouncements_Next").addClass("disabled");
    $("#fetchAnnouncements_Back").addClass("disabled");

    announcementsNavState.totalLength = totalLength;

    var maxPages = Math.ceil(announcementsNavState.totalLength / announcementsNavState.pageLength);
    if ((announcementsNavState.pageIndex + 1) < maxPages) {
        $("#fetchAnnouncements_Next").removeClass("disabled");
    } else {
        announcementsNavState.pageIndex = maxPages - 1;
    }
    if (announcementsNavState.pageIndex > 0) {
        $("#fetchAnnouncements_Back").removeClass("disabled");
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
    var r = confirm("Are you sure you want to remove this item?");

    if (r === false) {
        return false;
    }

    $.ajax({
        url: '/api/Admin/DeleteAnnouncement',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            AnnouncementId: announcementIdMap[a]
        })
    })
        .done(function (data) {
            // console.log(data);
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

function exportRegisteredUsers() {
    window.location.href = "/api/admin/exportregisteredusers?" +
        "&IsDesc=" +
        registeredUsersNavState.isDesc +
        "&OrderBy=" +
        registeredUsersNavState.currentTh +
        "&nameFilter=" +
        encodeURI(registeredUsersNavState.nameFilter) +
        "&emailFilter=" +
        encodeURI(registeredUsersNavState.emailFilter);
}

function doFetchRegisteredUsers() {
    $.ajax({
        url: '/api/admin/fetchregisteredusers',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            pageIndex: registeredUsersNavState.pageIndex,
            pageLength: registeredUsersNavState.pageLength,
            FilterName: registeredUsersNavState.nameFilter,
            FilterEmail: registeredUsersNavState.emailFilter,
            IsDesc: registeredUsersNavState.isDesc,
            OrderBy: registeredUsersNavState.currentTh
        })
    }).done(function (data) {
        updateNavRegisteredUsers(data.TotalLength);
        // console.log(data);
        if (data.AuthResult === validateSessionResultType.SessionValid) {
            for (var i = 0; i < registeredUsersNavState.pageLength; i++) {
                $("#registeredUser_Name_" + i).text('');
                $("#registeredUser_Email_" + i).text('');
                $("#registeredUser_Text_" + i).text('');
                $("#registeredUser_Created_" + i).text('');
                if (i >= data.Length) {
                    $("#registeredUser_" + i).hide();
                } else {
                    $("#registeredUser_" + i).show();
                    $("#registeredUser_Name_" + i).text(data.Users[i].Name);
                    $("#registeredUser_Email_" + i).text(data.Users[i].Email);
                    $("#registeredUser_Text_" + i).text(data.Users[i].Text);

                    var dt = new Date(data.Users[i].Created);
                    $("#registeredUser_Created_" + i).text(dt.toLocaleDateString("en-US") + " " + dt.toLocaleTimeString("en-US"));
                }
            }
        } else if (data.AuthResult === validateSessionResultType.SessionInvalid) {
            console.log('Could not fetch registered users, bad session');
            window.location.href = "/home/login";
        } else if (data.AuthResult === validateSessionResultType.SessionExpired) {
            console.log('Could not fetch registered users, session expired');
            window.location.href = "/home/login";
        }
    });
}

function onRegisteredUserTh(a) {
    $(function () {
        if (a === registeredUsersNavState.currentTh) {
            registeredUsersNavState.isDesc = !registeredUsersNavState.isDesc;
        } else {
            registeredUsersNavState.isDesc = true;
        }

        registeredUsersNavState.currentTh = a;
        for (var i = 0; i < 4; i++) {
            var $spn = $("#registeredUser_th_" + i + " > a > span");
            var $th = $("#registeredUser_th_" + i);

            $th.addClass("info").removeClass("info");
            $spn.addClass("glyphicon-menu-down").removeClass("glyphicon-menu-down");
            $spn.addClass("glyphicon-menu-up").removeClass("glyphicon-menu-up");

            if (i === a) {
                $th.removeClass("info").addClass("info");
                var s = registeredUsersNavState.isDesc ? "down" : "up";
                $spn.removeClass("glyphicon-menu-" + s).addClass("glyphicon-menu-" + s);
                console.log($spn);
            }
        }

        registeredUsersNavState.pageIndex = 0;
        doFetchRegisteredUsers();
    });

    return false;
}

function doNavRegisteredUsers(a) {
    if (a) {
        registeredUsersNavState.pageIndex--;
    } else {
        registeredUsersNavState.pageIndex++;
    }
    doFetchRegisteredUsers();

    return false;
}

function updateNavRegisteredUsers(totalLength) {
    $("#registeredUsers_Next").addClass("disabled");
    $("#registeredUsers_Back").addClass("disabled");

    registeredUsersNavState.totalLength = totalLength;

    var maxPages = Math.ceil(registeredUsersNavState.totalLength / registeredUsersNavState.pageLength);
    if ((registeredUsersNavState.pageIndex + 1) < maxPages) {
        $("#registeredUsers_Next").removeClass("disabled");
    } else {
        registeredUsersNavState.pageIndex = maxPages - 1;
    }
    if (registeredUsersNavState.pageIndex > 0) {
        $("#registeredUsers_Back").removeClass("disabled");
    } else if (registeredUsersNavState.pageIndex < 0) {
        registeredUsersNavState.pageIndex = 0;
    }
}

var announcementsDataCache = {};

function doFetchPreviewAnnouncements() {
    $.ajax({
        url: '/api/admin/fetchannouncements',
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify({
            PageIndex: 0,
            PageLength: 5
        })
    }).done(function (data) {
        announcementsDataCache = data;
        if (data.Result === newAnnouncementResult.Success) {
            var newHtml = "";
            for (var i = 0; i < data.Length; i++) {
                newHtml += "<li>" + data.Announcements[i].Content + "</li>";
            }
            $("#adminAnnouncementPreview").html(newHtml);
        }
    });
}

function doPreviewAnnouncements(d) {
    var data = announcementsDataCache;
    if (data.Result === newAnnouncementResult.Success) {
        var newHtml = "<li>" + d + "</li>";
        for (var i = 0; i < data.Length; i++) {
            newHtml += "<li>" + data.Announcements[i].Content + "</li>";
        }
        $("#adminAnnouncementPreview").html(newHtml);
    }
}