/* site.js */

// Wait for DOM to load
document.addEventListener('DOMContentLoaded', function () {
    // Mobile menu toggle
    const navLinks = document.getElementById("navLinks");
    function showMenu() { navLinks.style.right = "0"; }
    function hideMenu() { navLinks.style.right = "-250px"; }

    // Scroll to top button
    const scrollToTopBtn = document.getElementById("scrollToTopBtn");
    window.addEventListener("scroll", function () {
        scrollToTopBtn.classList.toggle("show", window.scrollY > 300);
    });
    scrollToTopBtn.addEventListener("click", function () {
        window.scrollTo({ top: 0, behavior: "smooth" });
    });

    // Slideshow functionality
    let slideIndex = 0;
    const slides = document.querySelectorAll('.mySlides');
    const dots = document.querySelectorAll('.dot');

    function showSlides() {
        // Hide all slides
        slides.forEach(slide => slide.style.display = "none");

        // Remove active class from all dots
        dots.forEach(dot => dot.classList.remove("active"));

        // Show current slide
        slideIndex = (slideIndex + 1) % slides.length;
        slides[slideIndex].style.display = "block";
        dots[slideIndex].classList.add("active");

        // Continue cycling
        setTimeout(showSlides, 2000);
    }

    // Manual slide control
    function currentSlide(n) {
        slideIndex = n - 1;
        showSlides();
    }

    // Start slideshow
    showSlides();

    // Initialize AOS
    if (typeof AOS !== 'undefined') {
        AOS.init();
    }
});