'use strict'
window.addEventListener("load", () => {
    let newsSlider = document.getElementById("news-slider");
    let sliderPrev = document.getElementById("slider-prev");
    let sliderNext = document.getElementById("slider-next");

    let slidePosition = 0;
    sliderPrev.addEventListener("click", () => {
        if (slidePosition > 1) {
            slidePosition++;
            newsSlider.style = `--news-offset: ${100 * slidePosition}%; transform: translateX(calc(var(--news-offset) - 90px));`;
        }
        if (slidePosition == 1) {
            slidePosition++;
            newsSlider.style = `--news-offset: ${100 * slidePosition}%; transform: translateX(var(--news-offset));`;
        }
    });
    sliderNext.addEventListener("click", () => {
        if (slidePosition < 5) {
            slidePosition--;
            newsSlider.style = `--news-offset: ${100 * slidePosition}%; transform: translateX(calc(var(--news-offset) - 90px));`;
        }
    });
});