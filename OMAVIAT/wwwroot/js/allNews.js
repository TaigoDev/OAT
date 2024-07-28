// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

'use strict'
window.addEventListener("load", () => {
	document.querySelectorAll(".news-item-img").forEach(element => {
		let temp = element.offsetHeight / 2 > 25 ? `--block-height: ${element.offsetHeight / 2}px;` : "";
		element.style = `--block-width: ${element.offsetWidth / 2}px; ${temp}`;
	});
});