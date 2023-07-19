function AjaxFormSubmit() {
    var url = "https://www.oat.ru/api/users/new";
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
            alert("Пользователь успешно добавлен");
            location.reload();
        },
        error: function (jqXHR, exception) {
            alert(jqXHR.status);
            if (jqXHR.status == 401) {
                window.location = "https://www.oat.ru/api/logout";
            }
            if (jqXHR.status == 406) {
                window.location = "https://www.oat.ru/admin/users";
            }
        }
    });
}