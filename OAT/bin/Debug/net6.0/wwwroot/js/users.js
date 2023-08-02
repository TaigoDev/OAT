function AjaxFormSubmit() {
    var url = "/api/users/new";
    var formData = new FormData();
    var e = document.getElementById("user-role");
    var value = e.value;
    var text = e.options[e.selectedIndex].text;
    formData.append("Fullname", $("#user-fn").val());
    formData.append("username", $("#user-name").val());
    formData.append("password", $("#user-password").val());
    formData.append("role", text);
    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (response) {
            SendMessage("message-success");
            location.reload();

        },
        error: function (jqXHR, exception) {
            alert(jqXHR.status);
            if (jqXHR.status == 401) {
                SendMessage("message-fail-auth");
                window.location = "https://www.oat.ru/api/logout";

            }
            if (jqXHR.status == 406) {
                SendMessage("message-fail-auth");
                window.location = "https://www.oat.ru/admin/users";
            }
        }
    });
}
function DeleteUser(id) {
    var url = "/api/users/" + id + "/delete";
    $.ajax({
        type: 'DELETE',
        url: url,
        processData: false,
        contentType: false,
        success: function (response) {
            SendMessage("message-success-delete");
            location.reload();
        },
        error: function (jqXHR, exception) {
            if (jqXHR.status == 401) {
                SendMessage("message-fail-auth");
                window.location = "https://www.oat.ru/api/logout";
            }
            if (jqXHR.status == 204) {
                SendMessage("message-fail-delete");
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
