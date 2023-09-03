// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict'
window.addEventListener("load", () => {
    /* E L E M E N T S */
    /* main */
    const preloader = document.getElementById("preloader");
    preloader.classList.add("end");
    let mainMenuWindow = document.getElementById("main-menu-window");
    let mainMenu = document.getElementById("main-menu");
    let menuIcon = document.getElementById("menu-icon");
    let searchIcon = document.getElementById("search-icon");
    const partnersSlider = document.querySelectorAll(".partners-list > a > img");
    let siteSearch = document.querySelector(".site-search-container");

    /* optional */
    let accordionsList = document.querySelectorAll(".accordion");
    let imagesSliders = document.querySelectorAll(".images-list-slider-container");
    let plane = document.getElementById("plane");


    /* F U N C T I O N A L */
    /* search */
    siteSearch.classList.add("site-search");
    searchIcon.addEventListener("click", () => {
        document.querySelector(".site-search-container").classList.toggle("site-search-container-active");
    });

    /* main menu */
    menuIcon.addEventListener("click", () => {
        menuIcon.classList.toggle("main-menu-icon-active");
        mainMenu.classList.toggle("main-menu-open");
        mainMenuWindow.classList.toggle("main-menu-window-open");
    });

    /* accordion */
    for (let i = 0; i < accordionsList.length; i++) {
        accordionsList[i].querySelector(".accordion-head").insertAdjacentHTML("beforeend", `<img class="icon accordion-head-icon" src="/images/basic/accordionMore.svg" alt="версия для слабовидящих">`);
        accordionsList[i].querySelector(".accordion-head-icon").addEventListener("click", () => {
            if (accordionsList[i].classList.contains("open")) {
                accordionsList[i].classList.remove("open");
            }
            else {
                accordionsList[i].classList.add("open");
                for (let j = 0; j < accordionsList.length; j++) {
                    if (j != i) {
                        accordionsList[j].classList.remove("open");
                    }
                }
            }
        });
    }

    /* partners list */
    let partnersSliderWidth = 0;
    partnersSlider.forEach(element => {
        partnersSliderWidth = partnersSliderWidth + element.offsetWidth + 20;
    });
    console.log(partnersSliderWidth);
    document.querySelector(".partners-list").style = `--width-part-block: ${partnersSliderWidth - 20}px;`;

    /* images slider */
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
        if (countOfImages==1) {
            sliderContainerNext.classList.add("images-list-slider-container-btn-inactive");
        }

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

    /* plane fly */
    if (plane) {
        plane.addEventListener("click", () => {
            plane.classList.add("fly");
            smoothScroll('#sidebar', 4500);
        });
    }

    function smoothScroll(target, duration) {
        var targetElement = document.querySelector(target);
        var targetPosition = targetElement.offsetTop;
        var startPosition = window.pageYOffset;
        var distance = targetPosition - startPosition;
        var startTime = null;

        function animation(currentTime) {
            if (startTime === null) startTime = currentTime;
            var timeElapsed = currentTime - startTime;
            var run = ease(timeElapsed, startPosition, distance, duration);
            window.scrollTo(0, run);
            if (timeElapsed < duration) requestAnimationFrame(animation);
        }

        function ease(t, b, c, d) {
            t /= d / 2;
            if (t < 1) return c / 2 * t * t + b;
            t--;
            return -c / 2 * (t * (t - 2) - 1) + b;
        }

        requestAnimationFrame(animation);
    }
});