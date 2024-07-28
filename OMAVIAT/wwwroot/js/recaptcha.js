
loaded = [];

window.loadScript = function (scriptPath) {
	// check list - if already loaded we can ignore
	if (loaded[scriptPath]) {
		console.log(scriptPath + " already loaded");
		// return 'empty' promise
		return new this.Promise(function (resolve, reject) {
			resolve();
		});
	}

	return new Promise(function (resolve, reject) {
		// create JS library script element
		var script = document.createElement("script");
		script.src = scriptPath;
		script.type = "text/javascript";
		console.log(scriptPath + " created");

		// flag as loading/loaded
		loaded[scriptPath] = true;

		// if the script returns okay, return resolve
		script.onload = function () {
			console.log(scriptPath + " loaded ok");
			resolve(scriptPath);
		};

		// if it fails, return reject
		script.onerror = function () {
			console.log(scriptPath + " load failed");
			reject(scriptPath);
		}

		// scripts will load at end of body
		document["body"].appendChild(script);
	});
}

async function recaptchaCall() {
	var recaptcha_token = '';
	grecaptcha.ready(() => {
		grecaptcha.execute("6Lc1GpMpAAAAAB_4lNiPzuIGmQ6Zrb8gEgms4Gd9", { action: 'submit' }).then((token) => {
			recaptcha_token = token;
		});
	});
	while (recaptcha_token == '') {
		await new Promise(r => setTimeout(r, 100));
	}
	return recaptcha_token;
}

function onloadCallback() {
	grecaptcha.render('html_element');
}
class GreetingHelpers {
	static dotNetHelper;

	static setDotNetHelper(value) {
		GreetingHelpers.dotNetHelper = value;
	}
	static async OnComplete() {

		console.log("User complete passed ReCaptcha");
		var response = grecaptcha.getResponse();
		await GreetingHelpers.dotNetHelper.invokeMethodAsync('OnComplete', response);
	}

}

window.GreetingHelpers = GreetingHelpers;

function OnComplete() {
	window.GreetingHelpers.OnComplete();
}