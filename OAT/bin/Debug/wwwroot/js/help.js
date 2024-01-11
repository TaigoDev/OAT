'use strict'
window.addEventListener("load", () => {
    // steps
    const currentSteps = document.getElementById("help-steps");
    // buttons
    const toSecondStep = document.getElementById("to-second");
    const toFirstStep = document.getElementById("to-first");
    const toThirdStep = document.getElementById("to-third");
    const backToSecondStep = document.getElementById("back-to-second");

    // forms
    const firstStep = document.getElementById("first-step");
    const secondStep = document.getElementById("second-step");
    const thirdStep = document.getElementById("third-step");

    // first form buttons
    toSecondStep.addEventListener("click", () => {
        firstStep.classList.remove("active");
        secondStep.classList.add("active");
        currentSteps.classList.add("second");
    });
    // second form buttons
    toFirstStep.addEventListener("click", () => {
        firstStep.classList.add("active");
        secondStep.classList.remove("active");
        currentSteps.classList.remove("second");
    });
    toThirdStep.addEventListener("click", () => {
        secondStep.classList.remove("active");
        thirdStep.classList.add("active");
        currentSteps.classList.remove("second");
        currentSteps.classList.add("third");
    });
    backToSecondStep.addEventListener("click", () => {
        thirdStep.classList.remove("active");
        secondStep.classList.add("active");
        currentSteps.classList.add("second");
        currentSteps.classList.remove("third");
    });
});