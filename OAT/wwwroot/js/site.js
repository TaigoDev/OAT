// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict'
window.addEventListener("load", () => {
    let mainMenuWindow = document.getElementById("main-menu-window");
    let mainMenu = document.getElementById("main-menu");
    let menuIcon = document.getElementById("menu-icon");
    let mainMenuElements = document.querySelectorAll(".main-menu-item");

    mainMenuElements.forEach(element => {
        element.querySelector(".main-menu-item-title").addEventListener("click", () => {
            mainMenuElements.forEach(element => element.classList.add("main-menu-item-inactive"));
            element.classList.toggle("main-menu-item-inactive");
        });
    });

    menuIcon.addEventListener("click", () => {
        mainMenu.classList.toggle("main-menu-open");
        mainMenuWindow.classList.toggle("main-menu-window-open");
    });
});