function MessageController(tag){
	var element = document.getElementById(tag);
	element.classList.add("panel-message-active");
	setTimeout("document.getElementById(\"" + tag + "\").classList.remove(\"panel-message-active\")", 2000);
}

function MessageControllerWithMSG(tag, message) {
	var element = document.getElementById(tag);
	element.textContent = message;
	element.classList.add("panel-message-active");
	setTimeout("document.getElementById(\"" + tag + "\").classList.remove(\"panel-message-active\")", 2000);
}

function POST(url, formData, success, error){
	$.ajax({
		type: 'POST',
		url: url,
		data: formData,
		processData: false,
		contentType: false,
		mimeType: "multipart/form-data",
		success: function (response) {
			success(response);
		},
		error: function (jqXHR, exception) {
			error(jqXHR.status);
		}
	});
}


function POSTWithjqXHR(url, formData, success, error) {
	$.ajax({
		type: 'POST',
		url: url,
		data: formData,
		processData: false,
		contentType: false,
		mimeType: "multipart/form-data",
		success: function (response) {
			success(response);
		},
		error: function (jqXHR, exception) {
			error(jqXHR);
		}
	});
}

function GetFile(tag){
	var fileUpload =  $("#" + tag).get(0);
	var files = fileUpload.files;
	return files[0];
}

function GET(url, success, error){
	$.ajax({
		type: 'GET',
		url: url,
		processData: false,
		contentType: false,
		success: function (response) {
			success(response);
		},
		error: function (jqXHR, exception) {
			error(jqXHR.status);
		}
	});
}

function DELETE(url, formData, success, error){
	$.ajax({
		type: 'DELETE',
		url: url,
		data: formData,
		processData: false,
		contentType: false,
		mimeType: "multipart/form-data",
		success: function (response) {
			success(response);
		},
		error: function (jqXHR, exception) {
			error(jqXHR.status);
		}
	});
}