/**
 * Petrosia Hotel - Feedback and Reviews JavaScript
 * This script handles the functionality of the Feedback and Reviews page
 */

document.addEventListener('DOMContentLoaded', function () {
    // Mobile Menu Toggle
    const showMenu = () => {
        document.getElementById('navLinks').style.right = '0';
    };

    const hideMenu = () => {
        document.getElementById('navLinks').style.right = '-200px';
    };

    // Make these functions globally available for the onclick attributes
    window.showMenu = showMenu;
    window.hideMenu = hideMenu;

    // Star Rating Functionality
    const stars = document.querySelectorAll('.star-rating i');
    const ratingInput = document.getElementById('rating');
    const ratingText = document.querySelector('.rating-text');

    const ratingTexts = [
        'Select your rating',
        'Poor - 1 star',
        'Fair - 2 stars',
        'Average - 3 stars',
        'Good - 4 stars',
        'Excellent - 5 stars'
    ];

    stars.forEach(star => {
        star.addEventListener('click', () => {
            const rating = parseInt(star.getAttribute('data-rating'));
            ratingInput.value = rating;

            // Update text based on rating
            ratingText.textContent = ratingTexts[rating];

            // Reset all stars
            stars.forEach(s => s.classList.remove('selected'));
            stars.forEach(s => s.classList.remove('fas'));
            stars.forEach(s => s.classList.add('far'));

            // Fill in stars up to the selected rating
            for (let i = 0; i < stars.length; i++) {
                const starValue = parseInt(stars[i].getAttribute('data-rating'));
                if (starValue <= rating) {
                    stars[i].classList.remove('far');
                    stars[i].classList.add('fas');
                    stars[i].classList.add('selected');
                }
            }
        });

        // Hover effects
        star.addEventListener('mouseover', () => {
            const rating = parseInt(star.getAttribute('data-rating'));

            // Reset all stars
            stars.forEach(s => s.classList.remove('fas'));
            stars.forEach(s => s.classList.add('far'));

            // Fill in stars up to the hovered rating
            for (let i = 0; i < stars.length; i++) {
                const starValue = parseInt(stars[i].getAttribute('data-rating'));
                if (starValue <= rating) {
                    stars[i].classList.remove('far');
                    stars[i].classList.add('fas');
                }
            }
        });

        // Reset to selected state on mouseout
        star.addEventListener('mouseout', () => {
            const selectedRating = parseInt(ratingInput.value);

            // Reset all stars
            stars.forEach(s => s.classList.remove('fas'));
            stars.forEach(s => s.classList.add('far'));

            // Fill in stars up to the selected rating
            for (let i = 0; i < stars.length; i++) {
                const starValue = parseInt(stars[i].getAttribute('data-rating'));
                if (starValue <= selectedRating) {
                    stars[i].classList.remove('far');
                    stars[i].classList.add('fas');
                }
            }
        });
    });

    // Form Submission
    const reviewForm = document.getElementById('reviewForm');
    const successModal = document.getElementById('successModal');
    const closeModal = document.getElementsByClassName('close-modal')[0];
    const closeModalBtn = document.getElementById('closeModal');

    // Set the API endpoint for form submission
    const API_ENDPOINT = '/api/submit-review'; // Replace with your actual API endpoint

    if (reviewForm) {
        reviewForm.addEventListener('submit', function (e) {
            e.preventDefault();

            // Validate form
            if (validateForm()) {
                // Create FormData object
                const formData = new FormData(reviewForm);

                // Show loading state
                const submitBtn = reviewForm.querySelector('button[type="submit"]');
                const originalBtnText = submitBtn.textContent;
                submitBtn.textContent = 'Submitting...';
                submitBtn.disabled = true;

                // Send data to server using fetch API
                fetch(API_ENDPOINT, {
                    method: 'POST',
                    body: formData,
                    // For JSON submission instead of FormData, use the following:
                    // headers: {
                    //     'Content-Type': 'application/json',
                    // },
                    // body: JSON.stringify(Object.fromEntries(formData))
                })
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Network response was not ok');
                        }
                        return response.json();
                    })
                    .then(data => {
                        // Show success modal
                        successModal.style.display = 'flex';

                        // Reset form
                        reviewForm.reset();
                        ratingInput.value = 0;
                        ratingText.textContent = ratingTexts[0];

                        // Reset stars
                        stars.forEach(s => s.classList.remove('selected'));
                        stars.forEach(s => s.classList.remove('fas'));
                        stars.forEach(s => s.classList.add('far'));

                        console.log('Success:', data);
                    })
                    .catch((error) => {
                        console.error('Error:', error);
                        alert('There was a problem submitting your review. Please try again later.');
                    })
                    .finally(() => {
                        // Reset button state
                        submitBtn.textContent = originalBtnText;
                        submitBtn.disabled = false;
                    });
            }
        });
    }

    // Validate form fields
    function validateForm() {
        let isValid = true;

        // Get form fields
        const name = document.getElementById('name');
        const email = document.getElementById('email');
        const stayDate = document.getElementById('stayDate');
        const rating = document.getElementById('rating');
        const reviewTitle = document.getElementById('reviewTitle');
        const reviewText = document.getElementById('reviewText');

        // Validate each field
        if (!name.value.trim()) {
            highlightError(name, 'Please enter your name');
            isValid = false;
        } else {
            removeError(name);
        }

        if (!email.value.trim()) {
            highlightError(email, 'Please enter your email');
            isValid = false;
        } else if (!isValidEmail(email.value)) {
            highlightError(email, 'Please enter a valid email address');
            isValid = false;
        } else {
            removeError(email);
        }

        if (!stayDate.value) {
            highlightError(stayDate, 'Please select your stay date');
            isValid = false;
        } else {
            removeError(stayDate);
        }

        if (rating.value === '0') {
            highlightError(document.querySelector('.rating-group'), 'Please select a rating');
            isValid = false;
        } else {
            removeError(document.querySelector('.rating-group'));
        }

        if (!reviewTitle.value.trim()) {
            highlightError(reviewTitle, 'Please enter a review title');
            isValid = false;
        } else {
            removeError(reviewTitle);
        }

        if (!reviewText.value.trim()) {
            highlightError(reviewText, 'Please enter your review');
            isValid = false;
        } else {
            removeError(reviewText);
        }

        return isValid;
    }

    // Validate email format
    function isValidEmail(email) {
        const re = /^[\w-]+(\.[\w-]+)*@([\w-]+\.)+[a-zA-Z]{2,7}$/;
        return re.test(email);
    }

    // Highlight error on form field
    function highlightError(element, message) {
        // Add error class to parent
        element.parentElement.classList.add('error');

        // Create error message if it doesn't exist
        let errorElement = element.parentElement.querySelector('.error-message');
        if (!errorElement) {
            errorElement = document.createElement('span');
            errorElement.className = 'error-message';
            element.parentElement.appendChild(errorElement);
        }

        // Set error message
        errorElement.textContent = message;
    }

    // Remove error styling
    function removeError(element) {
        element.parentElement.classList.remove('error');

        const errorElement = element.parentElement.querySelector('.error-message');
        if (errorElement) {
            errorElement.remove();
        }
    }

    // Close modal
    if (closeModal) {
        closeModal.addEventListener('click', () => {
            successModal.style.display = 'none';
        });
    }

    if (closeModalBtn) {
        closeModalBtn.addEventListener('click', () => {
            successModal.style.display = 'none';
        });
    }

    // Close modal when clicking outside
    window.addEventListener('click', (e) => {
        if (e.target === successModal) {
            successModal.style.display = 'none';
        }
    });

    // Review filtering and sorting
    const sortSelect = document.getElementById('sortReviews');
    const reviewsList = document.getElementById('reviewsList');
    const reviewCards = document.querySelectorAll('.review-card');

    if (sortSelect) {
        sortSelect.addEventListener('change', () => {
            const sortValue = sortSelect.value;
            const reviewsArray = Array.from(reviewCards);

            switch (sortValue) {
                case 'newest':
                    // Sort by date (newest first)
                    reviewsArray.sort((a, b) => {
                        const dateA = new Date(a.querySelector('.review-date').textContent);
                        const dateB = new Date(b.querySelector('.review-date').textContent);
                        return dateB - dateA;
                    });
                    break;
                case 'highest':
                    // Sort by rating (highest first)
                    reviewsArray.sort((a, b) => {
                        const ratingA = a.querySelectorAll('.fa-star.fas').length;
                        const ratingB = b.querySelectorAll('.fa-star.fas').length;
                        return ratingB - ratingA;
                    });
                    break;
                case 'lowest':
                    // Sort by rating (lowest first)
                    reviewsArray.sort((a, b) => {
                        const ratingA = a.querySelectorAll('.fa-star.fas').length;
                        const ratingB = b.querySelectorAll('.fa-star.fas').length;
                        return ratingA - ratingB;
                    });
                    break;
            }

            // Clear current reviews
            reviewsList.innerHTML = '';

            // Append sorted reviews
            reviewsArray.forEach(review => {
                reviewsList.appendChild(review);
            });
        });
    }

    // Load more reviews button
    const loadMoreBtn = document.getElementById('loadMoreBtn');
    let page = 1;

    if (loadMoreBtn) {
        loadMoreBtn.addEventListener('click', () => {
            // Increment page number
            page++;

            // Show loading state
            loadMoreBtn.textContent = 'Loading...';
            loadMoreBtn.disabled = true;

            // Fetch more reviews from the server
            fetch(`/api/reviews?page=${page}`)
                .then(response => {
                    if (!response.ok) {
                        throw new Error('Network response was not ok');
                    }
                    return response.json();
                })
                .then(data => {
                    if (data.reviews && data.reviews.length > 0) {
                        // Add fetched reviews to the list
                        data.reviews.forEach(review => {
                            const reviewElement = createReviewElement(review);
                            reviewsList.appendChild(reviewElement);
                        });

                        // Hide load more button if no more reviews
                        if (!data.hasMore) {
                            loadMoreBtn.style.display = 'none';
                        }
                    } else {
                        // No more reviews
                        loadMoreBtn.style.display = 'none';
                    }
                })
                .catch(error => {
                    console.error('Error loading more reviews:', error);

                    // Fallback to mock data if API fails (remove in production)
                    const mockReviews = [
                        {
                            name: 'David M.',
                            avatar: '../images/avatars/avatar4.jpg',
                            date: 'March 28, 2025',
                            rating: 5,
                            title: 'Wonderful experience!',
                            content: 'The location is perfect, just a short walk to all the main attractions. The room was spacious and immaculately clean. Staff were always helpful and went above and beyond to make our stay special.',
                            tags: ['Location', 'Cleanliness', 'Value for Money'],
                            image: null
                        },
                        {
                            name: 'Sophia L.',
                            avatar: '../images/avatars/avatar5.jpg',
                            date: 'March 15, 2025',
                            rating: 4,
                            title: 'Lovely hotel with great amenities',
                            content: 'We enjoyed our stay at Petrosia Hotel. The rooms are beautifully decorated and comfortable. The swimming pool and spa facilities were excellent. Breakfast had a good variety of options. Only downside was some noise from the street.',
                            tags: ['Amenities', 'Room Comfort'],
                            image: '../images/reviews/review5.jpg'
                        }
                    ];

                    mockReviews.forEach(review => {
                        const reviewElement = createReviewElement(review);
                        reviewsList.appendChild(reviewElement);
                    });

                    // Hide load more button after loading mock data
                    loadMoreBtn.style.display = 'none';
                })
                .finally(() => {
                    // Reset button state
                    loadMoreBtn.textContent = 'Load More Reviews';
                    loadMoreBtn.disabled = false;
                });
        });
    }

    // Create review element from data
    function createReviewElement(review) {
        const reviewCard = document.createElement('div');
        reviewCard.className = 'review-card new-review';

        // Create review HTML structure
        let reviewHTML = `
            <div class="review-header">
                <img src="${review.avatar}" alt="Guest" class="reviewer-avatar">
                <div class="reviewer-info">
                    <h3>${review.name}</h3>
                    <div class="review-rating">
                        <div class="stars">
        `;

        // Add stars based on rating
        for (let i = 1; i <= 5; i++) {
            if (i <= review.rating) {
                reviewHTML += `<i class="fas fa-star"></i>`;
            } else {
                reviewHTML += `<i class="far fa-star"></i>`;
            }
        }

        reviewHTML += `
                        </div>
                        <span class="review-date">${review.date}</span>
                    </div>
                </div>
            </div>
            <h4 class="review-title">${review.title}</h4>
            <p class="review-content">${review.content}</p>
            <div class="review-tags">
        `;

        // Add tags
        review.tags.forEach(tag => {
            reviewHTML += `<span class="tag">${tag}</span>`;
        });

        reviewHTML += `</div>`;

        // Add image if exists
        if (review.image) {
            reviewHTML += `
                <div class="review-image">
                    <img src="${review.image}" alt="Review Photo">
                </div>
            `;
        }

        reviewCard.innerHTML = reviewHTML;
        return reviewCard;
    }
});

// Function to handle the contact email button click
function goToFeedbackPage() {
    window.location.href = '/Home/Feedback';
}