document.addEventListener('DOMContentLoaded', () => {
    const slides = document.querySelectorAll('.slide');
    const dots = document.querySelectorAll('.dot');
    const prevButton = document.querySelector('.prev');
    const nextButton = document.querySelector('.next');
    const carouselContent = document.querySelector('.carousel-content');
    let currentIndex = 0;
    const totalSlides = slides.length;

    // Function to show the current slide
    function showSlide(index) {
        if (index >= totalSlides) {
            currentIndex = 0;
        } else if (index < 0) {
            currentIndex = totalSlides - 1;
        } else {
            currentIndex = index;
        }

        // Move the slides
        const offset = -currentIndex * 100;
        carouselContent.style.transform = `translateX(${offset}%)`;

        // Update active dot
        dots.forEach(dot => dot.classList.remove('active'));
        dots[currentIndex].classList.add('active');
    }

    // Next slide
    function nextSlide() {
        showSlide(currentIndex + 1);
    }

    // Previous slide
    function prevSlide() {
        showSlide(currentIndex - 1);
    }

    // Dot navigation
    dots.forEach((dot, i) => {
        dot.addEventListener('click', () => {
            showSlide(i);
        });
    });

    // Event listeners for next/prev buttons
    nextButton.addEventListener('click', nextSlide);
    prevButton.addEventListener('click', prevSlide);

    // Auto slide change (optional)
    setInterval(nextSlide, 5000); // Change slide every 5 seconds

    // Initial display
    showSlide(currentIndex);
});
