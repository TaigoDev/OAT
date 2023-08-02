function Upload() {
    var formData = new FormData();
    var purpose = $("#appointment").find(":selected").val();
    if (purpose == "Добровольное пожертвование") {
        formData.append("title", $("#news-title-prof").val());
        formData.append("date", $("#news-date-prof").val());
        formData.append("text", document.getElementById('#news-text-prof').val());

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
    else {
        formData.append("documentId", $("#documentId").val());
        formData.append("documentDate", $("#documentDate").val());
        formData.append("studentFullName", $("#studentFullName").val());
        formData.append("FullName", $("#FullName").val());
        formData.append("group", $("#group").val());
        formData.append("summa", $("#cost").val());
        formData.append("purpose", purpose);
        formData.append("email", $("#email").val());
        formData.append("phone", $("#phone").val());


        $.ajax({
            type: 'POST',
            url: "/api/oplata/receipt",
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
}

