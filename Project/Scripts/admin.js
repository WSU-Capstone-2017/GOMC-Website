// Admin Page JS

$(document).ready(function () {
    doFetchAnnouncements();
    doFetchLatexUploads();
    doFetchRegisteredUsers();
    doFetchPreviewAnnouncements();

    $("#registerdUseresFilter_Name").change(function () {
        registeredUsersNavState.nameFilter = $("#registerdUseresFilter_Name").val();
    });

    $("#registerdUseresFilter_Email").change(function () {
        registeredUsersNavState.emailFilter = $("#registerdUseresFilter_Email").val();
    });

    $("#adminAnnouncement_Text").keyup(function () {
        doPreviewAnnouncements($("#adminAnnouncement_Text").val());
    });
});