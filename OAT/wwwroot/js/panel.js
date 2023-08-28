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

document.getElementById('news-date').valueAsDate = new Date();

function SendNews() {
    var url = "/api/news/upload";
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
        mimeType: "multipart/form-data",
        success: function (response) {
            SendMessage("message-success");
        },
        error: function (jqXHR, exception) {
            console.log(jqXHR.status);
            if (jqXHR.status == 401 || jqXHR.status == 405) {
                SendMessage("message-fail-auth");
                window.location = "/api/logout";
            }
            else if (jqXHR.status == 406 || jqXHR.status == 403) {
                SendMessage("message-fail-perms");
            }
            else
                SendMessage("message-fail");
        }
    });
}

function DeleteNews(id) {
    var url = "/api/news/" + id + "/delete";
    $.ajax({
        type: 'DELETE',
        url: url,
        processData: false,
        contentType: false,
        success: function (response) {
            SendMessage("message-success-delete");
            var element = document.getElementById(id);
            element.remove();
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401 || jqXHR.status == 405) {
                SendMessage("message-fail-auth");
                window.location = "/api/logout";
            }
            if (jqXHR.status == 204) {
                SendMessage("message-fail-delete");
            }
            if (jqXHR.status == 406 || jqXHR.status == 403) {
                SendMessage("message-fail-perms");
            }
            console.log(jqXHR.status);
        }
    });
}
function SendMessage(tag) {
    var element = document.getElementById(tag);
    element.classList.add("panel-message-active");
    setTimeout("document.getElementById(\"" + tag + "\").classList.remove(\"panel-message-active\")", 2000);
}

function UploadProfNews() {
    var url = "/api/prof/news/upload";
    var formData = new FormData();

    var fileUpload = $("#files-prof").get(0);
    var files = fileUpload.files;

    formData.append("title", $("#news-title-prof").val());
    formData.append("date", $("#news-date-prof").val());
    formData.append("text", $("#news-text-prof").val());
    for (var i = 0; i < files.length; i++) {
        formData.append("files", files[i]);
    }
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
            console.log(jqXHR.status);
            if (jqXHR.status == 401 || jqXHR.status == 405) {
                SendMessage("message-fail-auth");
                window.location = "/api/logout";
            }
            else if (jqXHR.status == 406 || jqXHR.status == 403) {
                SendMessage("message-fail-perms");
            }
            else
                SendMessage("message-fail");
        }
    });
}
function DeleteProfNews(id) {
    var url = "/api/prof/news/" + id + "/delete";
    $.ajax({
        type: 'DELETE',
        url: url,
        processData: false,
        contentType: false,
        success: function (response) {
            SendMessage("message-success-delete");
            var element = document.getElementById(id + "-prof");
            element.remove();
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401) {
                SendMessage("message-fail-auth");
                window.location = "/api/logout";
            }
            if (jqXHR.status == 204) {
                SendMessage("message-fail-delete");
            }
            if (jqXHR.status == 406 || jqXHR.status == 403) {
                SendMessage("message-fail-perms");
            }
            console.log(jqXHR.status);
        }
    });
}