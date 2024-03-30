function onLogin() {
	var username = document.getElementById("login").value;
	var password = document.getElementById("password").value;

	var formData = new FormData();
	formData.append("username", username);
	formData.append("password", password);

	POSTWithjqXHR("/api/login", formData, (response) => {
		window.location = "/admin/panel";
		},
		(jqXHR) => {
			console.log(`Your error code: ${jqXHR.status}`);
			switch (jqXHR.status) {
				case 400:
					MessageController("message-fail");
					break;
				default:
					MessageController("message-fail");
					break;
			}
		})
}