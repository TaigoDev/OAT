'use strict'
window.addEventListener("load", () => {
	let selectors = document.querySelectorAll(".selector-items");
	let map = document.querySelector(".map-container");


	let currentCorpus = null;
	let avaliableFloors = [];

	selectors.forEach((selector) => {
		let selectorItems = selector.querySelectorAll(".selector-items li");
		selectorItems.forEach((selectorItem) => {
			selectorItem.addEventListener("click", () => {
				selectorItems.forEach((selectorItem) => {
					selectorItem.classList.remove("active");
				});
				selectorItem.classList.add("active");


				if (selectorItem.hasAttribute("data-target-corpus-name")) {
					document.getElementById("choose-floor").classList.remove("hide");
					currentCorpus = map.querySelector(`.map-corpus[data-corpus-name=${selectorItem.getAttribute("data-target-corpus-name")}]`);
					currentCorpus.querySelectorAll(".map-corpus-floor").forEach((floor) => {
						avaliableFloors.push(floor.getAttribute("data-corpus-floor"));
					})
					console.log(avaliableFloors);
					let floors = document.querySelectorAll(".selector-items.floors li");

					floors.forEach((floor) => {
						// по умолчанию предполагается, что этаж недоступен
						let logic = false;
						avaliableFloors.forEach((avaliableFloor) => {
							if (floor.getAttribute("data-target-corpus-floor") == avaliableFloor) {
								logic = true;
							}
						})
						// если этаж недоступен
						if (!logic) {
							floor.classList.add("hide");
						}
						// если этаж доступен
						else {
							floor.classList.remove("hide");
						}
					});
					avaliableFloors = [];
				}
				if (selectorItem.hasAttribute("data-target-corpus-floor")) {
					map.querySelectorAll(".map-corpus-floor").forEach((floor) => {
						floor.classList.remove("active");
					});
					console.log(selectorItem.getAttribute("data-target-corpus-floor"));
					console.log(currentCorpus);
					currentCorpus.querySelector(`.map-corpus-floor[data-corpus-floor="${selectorItem.getAttribute("data-target-corpus-floor")}"]`).classList.add("active");
				}
			});
		});
	})
});