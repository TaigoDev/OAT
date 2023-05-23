// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict'
window.addEventListener("load", () => {
    let header = document.getElementById("site-header");
    let menuIcon = document.getElementById("menu-icon");

    menuIcon.addEventListener("click", () => {
        header.classList.toggle("header-drop");
    });
});