
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

function onloadFunction() {
	if (window.smartCaptcha) {
		const container = document.getElementById('captcha-container');
		const widgetId = window.smartCaptcha.render(container, {
			sitekey: "ysc1_I9g75xZMUZMaxIq7RxnauDYz5QP3UwZPMpFcIWg657404671"
		});

		const unsubscribe = window.smartCaptcha.subscribe(
			widgetId,
			'success',
			() => GreetingHelpers.OnComplete()
		);
	}
}
class GreetingHelpers {
	static dotNetHelper;

	static setDotNetHelper(value) {
		GreetingHelpers.dotNetHelper = value;
	}
	static async OnComplete() {

		console.log("User complete passed ReCaptcha");
		var response = window.smartCaptcha.getResponse();
		await GreetingHelpers.dotNetHelper.invokeMethodAsync('OnComplete', response);
	}

}

window.GreetingHelpers = GreetingHelpers;

function OnComplete() {
	window.GreetingHelpers.OnComplete();
}