// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict'
window.addEventListener("load", () => {
    let mainMenuWindow = document.getElementById("main-menu-window");
    let mainMenu = document.getElementById("main-menu");
    let menuIcon = document.getElementById("menu-icon");
    let mainMenuElements = document.querySelectorAll(".main-menu-item");
    let accordionsList = document.querySelectorAll(".accordion");


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
});