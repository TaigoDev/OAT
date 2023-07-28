function UploadSchedule(building, files_id) {
    var url = "https://www.oat.ru/api/schedule/" + building + "/upload";
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