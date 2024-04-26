function ShowForm(id) {
	document.getElementById("news-form").classList.add("hide");
	document.getElementById("history-form").classList.add("hide");
	document.getElementById("files-form").classList.add("hide");
	document.getElementById("people-form").classList.add("hide");
	document.getElementById(GetId(id)).classList.remove("hide");
}

function GetId(name) {
	if (name == "Новости")
		return "news-form";
	else if (name == "Члены ЦМК")
		return "people-form";
	else if (name == "Документы")
		return "files-form";
	else if (name == "История")
		return "history-form";
	return "404";
}