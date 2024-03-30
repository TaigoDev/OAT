function onLogin() {
	var username = document.getElementById("login").value;
	var password = document.getElementById("password").value;

	var formData = new FormData();
	formData.append("username", username);
	formData.append("password", password);

	POSTWithjqXHR("api/login", formData, (response) => {
		window.location = "/admin/panel";
		},
		(jqXHR) => {
			console.log(`Your error code: ${jqXHR.status} ${url}`);
			switch (jqXHR.status) {
				case 400:
					console.log(jqXHR.responseText);
					var window = document.getElementById("file-error");
					window.classList.add("active");
					var text = document.getElementById("error-text");
					text.innerText = jqXHR.responseText;
					break;
				default:
					MessageController("message-fail");
					break;
			}
			button.textContent = "Обновить";

		})
}