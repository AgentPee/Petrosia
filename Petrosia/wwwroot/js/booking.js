document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS
    AOS.init({
        duration: 1200,
        delay: 150,
        easing: "ease-in-out",
        once: true
    });

    // Scroll to top button
    const scrollToTopBtn = document.getElementById("scrollToTopBtn");

    window.addEventListener("scroll", function () {
        if (window.scrollY > 300) {
            scrollToTopBtn.classList.add("show");
            scrollToTopBtn.classList.remove("hidden");
        } else {
            scrollToTopBtn.classList.add("hidden");
            scrollToTopBtn.classList.remove("show");
        }
    });

    scrollToTopBtn.addEventListener("click", function () {
        window.scrollTo({
            top: 0,
            behavior: "smooth"
        });
    });

    // Room selection functionality
    const roomButtons = document.querySelectorAll('.room-price button');

    roomButtons.forEach(button => {
        button.addEventListener('click', function () {
            // Remove active class from all buttons
            roomButtons.forEach(btn => {
                btn.textContent = 'Select Room';
                btn.style.backgroundColor = '#a58a5e';
            });

            // Add active state to clicked button
            this.textContent = 'Selected ✓';
            this.style.backgroundColor = '#4CAF50';
        });
    });

    // Set minimum date for check-in (today)
    const today = new Date().toISOString().split('T')[0];
    document.getElementById('check-in').min = today;

    // Update check-out min date when check-in changes
    document.getElementById('check-in').addEventListener('change', function () {
        const checkInDate = this.value;
        document.getElementById('check-out').min = checkInDate;
    });
});