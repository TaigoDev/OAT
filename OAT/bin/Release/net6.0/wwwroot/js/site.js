// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict'
window.addEventListener("load", () => {
    /* main elements */
    let mainMenuWindow = document.getElementById("main-menu-window");
    let mainMenu = document.getElementById("main-menu");
    let menuIcon = document.getElementById("menu-icon");
    let mainMenuElements = document.querySelectorAll(".main-menu-item");

    /* optional elements */
    let accordionsList = document.querySelectorAll(".accordion");
    let imagesSliders = document.querySelectorAll(".images-list-slider-container");

    /* main menu functional */
    mainMenuElements.forEach(element => {
        element.querySelector(".main-menu-item-title").addEventListener("click", () => {
            mainMenuElements.forEach(element => element.classList.add("main-menu-item-inactive"));
            element.classList.toggle("main-menu-item-inactive");
        });
    });

    menuIcon.addEventListener("click", () => {
        if (mainMenu.classList.contains("main-menu-open")) {
            mainMenu.classList.remove("main-menu-open");
            mainMenuElements.forEach(element => element.classList.add("main-menu-item-inactive"));
        }
        else {
            mainMenu.classList.add("main-menu-open");
        }
        mainMenuWindow.classList.toggle("main-menu-window-open");
    });

    /* accordion functional */
    accordionsList.forEach(accordion => {
        accordion.querySelector(".accordion-head").insertAdjacentHTML("beforeend", `<img id="eye-icon" class="icon accordion-head-icon" src="/images/basic/accordionMore.svg" alt="версия для слабовидящих">`);
        console.log(accordion.querySelector(".accordion-head-icon"));
        accordion.querySelector(".accordion-head-icon").addEventListener("click", () => {
            accordionsList.forEach(accordion => {
                accordion.classList.remove("open");
            });
            accordion.classList.add("open");
        });
    });

    /* images slider functional */
    for (let i = 0; i < imagesSliders.length; i++) {
        let sliderContainer = imagesSliders[i];
        sliderContainer.insertAdjacentHTML("afterbegin", `<div class="images-list-slider-container-btn prev"></div><div class="images-list-slider-container-btn next">
            </div>`);
        let sliderContainerPrev = sliderContainer.querySelector(".prev");
        let sliderContainerNext = sliderContainer.querySelector(".next");
        sliderContainerPrev.classList.add("images-list-slider-container-btn-inactive");

        let slider = sliderContainer.querySelector(".images-list-slider");
        let countOfImages = slider.querySelectorAll("img").length;
        let currentImg = 0;
        console.log(countOfImages);

        sliderContainerPrev.addEventListener("click", () => {
            if (currentImg > 0) {
                currentImg--;
                slider.style = `--images-slider-helper: ${currentImg};`;
                sliderContainerNext.classList.remove("images-list-slider-container-btn-inactive");
            }
            if (currentImg == 0) {
                sliderContainerPrev.classList.add("images-list-slider-container-btn-inactive");
            }
        });

        sliderContainerNext.addEventListener("click", () => {
            if (currentImg < countOfImages - 1) {
                currentImg++;
                slider.style = `--images-slider-helper: ${currentImg};`;
                sliderContainerPrev.classList.remove("images-list-slider-container-btn-inactive");
            }
            if (currentImg == countOfImages - 1) {
                sliderContainerNext.classList.add("images-list-slider-container-btn-inactive");
            }
        });
    }
});