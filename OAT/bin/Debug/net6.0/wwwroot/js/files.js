function UploadScheduleChanges(files_id) {
    var url = "/api/schedule/changes/" + $('#corpus').find(":selected").val() + "/upload";
    var formData = new FormData();

    var fileUpload = $("#" + files_id).get(0);
    var files = fileUpload.files;
    formData.append("file", files[0]);
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        mimeType: "multipart/form-data",
        success: function (response) {
            SendMessage("message-success");
        },
        error: function (jqXHR, exception) {
            alert(jqXHR.status);
            if (jqXHR.status == 401) {
                SendMessage("message-fail-auth");
                window.location = "https://www.oat.ru/api/logout";
            }
            else
                SendMessage("message-fail");
        }
    });
}
function SendMessage(tag) {
    var element = document.getElementById(tag);
    element.classList.add("panel-message-active");
    setTimeout("document.getElementById(\"" + tag + "\").classList.remove(\"panel-message-active\")", 2000);
}