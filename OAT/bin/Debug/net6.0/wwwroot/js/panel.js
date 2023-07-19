'use strict'
$(document).bind('dragover', function (e) {
    var dropZone = $('.zone'),
        timeout = window.dropZoneTimeout;
    if (!timeout) {
        dropZone.addClass('in');
    } else {
        clearTimeout(timeout);
    }
    var found = false,
        node = e.target;
    do {
        if (node === dropZone[0]) {
            found = true;
            break;
        }
        node = node.parentNode;
    } while (node != null);
    if (found) {
        dropZone.addClass('hover');
    } else {
        dropZone.removeClass('hover');
    }
    window.dropZoneTimeout = setTimeout(function () {
        window.dropZoneTimeout = null;
        dropZone.removeClass('in hover');
    }, 100);
});

function SendNews() {
    //Set the URL.
    var url = "https://www.oat.ru/api/news/upload";
    //Add the Field values to FormData object.
    var formData = new FormData();
    var fileUpload = $("#files").get(0);
    var files = fileUpload.files;
    formData.append("title", $("#news-title").val());
    formData.append("date", $("#news-date").val());
    formData.append("text", $("#news-text").val());
    for (var i = 0; i < files.length; i++) {
        formData.append("files", files[i]);
    }
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            alert("Новость успешно добавлена");
            location.reload();
        },
        error: function (jqXHR, exception) {
            alert(jqXHR.status);
            if (jqXHR.status == 401) {
                window.location = "https://www.oat.ru/api/logout";
            }
        }
    });
}