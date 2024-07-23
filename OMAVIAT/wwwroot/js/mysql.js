
function SendCmd() {
	var url = "/api/mysql/cmd";
	var formData = new FormData();
	formData.append("command", $("#MySQL-query").val());
	$.ajax({
		type: 'POST',
		url: url,
		data: formData,
		processData: false,
		contentType: false,
		mimeType: "multipart/form-data",
		success: function (response) {
			$('#MySQL-output').empty()

			SendMessage("message-success");
			$('#MySQL-output').append(response); 
		},
		error: function (jqXHR, exception) {
			alert(jqXHR.status);
			if (jqXHR.status == 401) {
				SendMessage("message-fail-auth");
				window.location = "/api/logout";
			}
			else
				SendMessage("message-fail");
		}
	});
}