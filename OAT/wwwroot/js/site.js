// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict'
window.addEventListener("load", () => {
    let header = document.getElementById("site-header");
    let menuIcon = document.getElementById("menu-icon");
    let dropdownMenuElements = document.querySelectorAll(".dropdown-menu-item");

    dropdownMenuElements.forEach(element => {
        element.querySelector(".dropdown-menu-item-title").addEventListener("click", () => {
            dropdownMenuElements.forEach(element => element.classList.add("dropdown-menu-item-inactive"));
            element.classList.toggle("dropdown-menu-item-inactive");
        });
    });

    menuIcon.addEventListener("click", () => {
        header.classList.toggle("header-drop");
    });
});