function onSelectPurpose() {
	var myselect = document.getElementById("appointment");
	if (myselect.selectedIndex == 4) {

		$("#container-donationt").css("display", "");
		$("#container-documentId").css("display", "none");
		$("#container-documentDate").css("display", "none");
		$("#container-studentFullName").css("display", "none");
		$("#container-group").css("display", "none");
	} else {
		$("#container-donationt").css("display", "none");
		$("#container-documentId").css("display", "");
		$("#container-documentDate").css("display", "");
		$("#container-studentFullName").css("display", "");
		$("#container-group").css("display", "");
	}
}

function RedirectToPrint() {
	if (!$('#agreement').is(":checked"))
		SendMessage("message-fail-checkbox");
	else {
		var myselect = document.getElementById("appointment");
		if (myselect.selectedIndex == 4) {

			if (!Check("FullName", "cost", "email", "phone")) {
				SendMessage("message-fail-fields");
				return;
			}

			window.location.href = "print?purpose=" + myselect.selectedIndex +
				"&Voluntary_Purpose=" + document.getElementById("donationt").selectedIndex +
				"&FullName=" + document.getElementById("FullName").value +
				"&summa=" + document.getElementById("cost").value +
				"&email=" + document.getElementById("email").value +
				"&phone=" + document.getElementById("phone").value;
		} else {
			if (!Check("FullName", "cost", "email", "phone", "documentId", "documentDate", "studentFullName", "group")) {
				SendMessage("message-fail-fields");
				return;
			}

			if (!IsContract()) {
				SendMessage("message-fail-contract");
				return;
			}

			window.location.href = "print?purpose=" + myselect.selectedIndex +
				"&documentId=" + document.getElementById("documentId").value +
				"&documentDate=" + ConvertData(document.getElementById("documentDate").value) +
				"&studentFullName=" + document.getElementById("studentFullName").value +
				"&group=" + document.getElementById("group").value +
				"&FullName=" + document.getElementById("FullName").value +
				"&summa=" + document.getElementById("cost").value +
				"&email=" + document.getElementById("email").value +
				"&phone=" + document.getElementById("phone").value;
		}
	}
}

function Check(...ids) {
	for (var arg of ids) {
		if (document.getElementById(arg).value == '')
			return false;
	}
	return true;
}

function SendMessage(tag) {
	var element = document.getElementById(tag);
	element.classList.add("panel-message-active");
	setTimeout("document.getElementById(\"" + tag + "\").classList.remove(\"panel-message-active\")", 2000);
}

function IsContract() {
	var url = "/pay/contract/search" +
		"?documentId=" + document.getElementById("documentId").value +
		"&documentDate=" + ConvertData(document.getElementById("documentDate").value) +
		"&studentFullName=" + document.getElementById("studentFullName").value +
		"&group=" + document.getElementById("group").value +
		"&FullName=" + document.getElementById("FullName").value
	var answer = false;

	$.ajax({
		type: 'GET',
		url: url,
		processData: false,
		contentType: false,
		async: false,
		mimeType: "multipart/form-data",
		success: function (response) {
			answer = (response === 'true');
		},
		error: function (jqXHR, exception) {
			alert(jqXHR.status);
			answer = false;
		}
	});
	return answer;
}


function ConvertData(date) {
	const oldDate = date;
	const arr = oldDate.split('-');
	return arr[2] + '.' + arr[1] + '.' + arr[0];
}

onSelectPurpose();

