'use strict'
window.addEventListener("load", () => {
	let newsSlider = document.getElementById("news-slider");
	let sliderPrev = document.getElementById("slider-prev");
	let sliderNext = document.getElementById("slider-next");

	let slidePosition = 0;
	sliderPrev.addEventListener("click", () => {
		if (slidePosition == 1) {
			slidePosition--;
			newsSlider.style = `--news-offset: ${-100 * slidePosition}%; transform: translateX(var(--news-offset));`;
			sliderNext.classList.remove("inactive");
		}
		if (slidePosition > 1) {
			slidePosition--;
			newsSlider.style = `--news-offset: ${-100 * slidePosition}%; transform: translateX(calc(var(--news-offset) - var(--news-offset-helper) * ${slidePosition}));`;
			sliderNext.classList.remove("inactive");
		}
		else {
			sliderPrev.classList.add("inactive");
		}
	});
	sliderNext.addEventListener("click", () => {
		if (slidePosition < 2) {
			slidePosition++;
			newsSlider.style = `--news-offset: ${-100 * slidePosition}%; transform: translateX(calc(var(--news-offset) - var(--news-offset-helper) * ${slidePosition}));`;
			sliderPrev.classList.remove("inactive");
		}
		if (slidePosition == 2) {
			sliderNext.classList.add("inactive");
		}
	});
});