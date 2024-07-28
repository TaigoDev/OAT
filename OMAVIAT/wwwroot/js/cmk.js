function ShowForm(id) {
	document.getElementById("descriptionOfWork-form").classList.add("hide");
	document.getElementById("history-form").classList.add("hide");
	document.getElementById("achievements-form").classList.add("hide");
	document.getElementById("people-form").classList.add("hide");
	document.getElementById("plans-form").classList.add("hide");
	document.getElementById(GetId(id)).classList.remove("hide");
}

function GetId(name) {
	if (name == "Описание работы ЦМК")
		return "descriptionOfWork-form";
	else if (name == "Члены ЦМК")
		return "people-form";
	else if (name == "Достижения ЦМК")
		return "achievements-form";
	else if (name == "История")
		return "history-form";
	else if (name == "Решаемые задачи и планы")
		return "plans-form";
	return "404";
}