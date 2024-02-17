function onAuth(){
	var formData = new FormData();
	formData.append("username", $('#login').val());
	formData.append("password", $('#password').val());
	POST(
		"/api/students/login", formData,
		(response) => {
			window.location.href = "grades";
		}, 
		(code) => {
			console.log(`Your error code: ${code}`);
			MessageController("message-fail-login");
		}
	);
}