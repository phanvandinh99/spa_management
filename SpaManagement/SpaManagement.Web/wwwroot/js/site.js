// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Hiệu ứng thu nhỏ navbar khi cuộn
window.addEventListener('scroll', function() {
    var navbar = document.querySelector('.custom-navbar');
    var mainContent = document.querySelector('.main-content');
    if (window.scrollY > 10) {
        navbar.classList.add('shrink');
        if(mainContent) mainContent.classList.add('shrink');
    } else {
        navbar.classList.remove('shrink');
        if(mainContent) mainContent.classList.remove('shrink');
    }
});
