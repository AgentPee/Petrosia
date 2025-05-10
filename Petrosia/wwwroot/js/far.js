// Star rating interaction
$(document).ready(function () {
    // Star rating logic
    $('.star-rating i').on('mouseenter', function () {
        var rating = $(this).data('rating');
        $('.star-rating i').each(function (index) {
            if (index < rating) {
                $(this).addClass('fas').removeClass('far');
            } else {
                $(this).addClass('far').removeClass('far');
            }
        });
    });

    $('.star-rating').on('mouseleave', function () {
        var selected = $('#Rating').val();
        $('.star-rating i').each(function (index) {
            if (index < selected) {
                $(this).addClass('fas').removeClass('far');
            } else {
                $(this).addClass('far').removeClass('fas');
            }
        });
    });

    $('.star-rating i').on('click', function () {
        var rating = $(this).data('rating');
        $('#Rating').val(rating);
        $('.rating-text').text('You rated ' + rating + ' star' + (rating > 1 ? 's' : ''));
        $('.star-rating i').each(function (index) {
            if (index < rating) {
                $(this).addClass('fas').removeClass('far');
            } else {
                $(this).addClass('far').removeClass('fas');
            }
        });
    });

    // Enhanced form submission handler
    $('#reviewForm').on('submit', function (e) {
        e.preventDefault();

        // Form validation could go here

        // Show loading state
        $('#submitBtn').prop('disabled', true).html('<i class="fas fa-spinner fa-spin"></i> Submitting...');

        // Simulate form submission (replace with actual AJAX in production)
        setTimeout(function () {
            // Hide the form
            $('#reviewForm').fadeOut(300, function () {
                // Show success message with animation
                $('#successModal').fadeIn(400);

                // Add success animation classes
                $('.modal-content').addClass('animate-success');
                $('.success-icon').addClass('animate-bounce');

                // Auto-hide after 5 seconds (optional)
                setTimeout(function () {
                    closeModal();
                }, 5000);
            });
        }, 1000); // Simulate server delay

        // In a real implementation, you would use AJAX:
        /*
        $.ajax({
            url: $(this).attr('action'),
            type: 'POST',
            data: $(this).serialize(),
            success: function(response) {
                $('#reviewForm').fadeOut(300, function() {
                    $('#successModal').fadeIn(400);
                    $('.modal-content').addClass('animate-success');
                    $('.success-icon').addClass('animate-bounce');
                });
            },
            error: function(error) {
                // Handle error
                $('#submitBtn').prop('disabled', false).html('Submit Review');
                alert('There was an error submitting your review. Please try again.');
            }
        });
        */
    });

    // Enhanced modal logic for success message
    function closeModal() {
        $('.modal-content').removeClass('animate-success').addClass('animate-out');
        setTimeout(function () {
            $('#successModal').fadeOut(300, function () {
                $('.modal-content').removeClass('animate-out');
                // Reset the form
                $('#reviewForm')[0].reset();
                $('#reviewForm').fadeIn(300);
                $('#submitBtn').prop('disabled', false).html('Submit Review');
            });
        }, 300);
    }

    // Expose closeModal to window for use in inline event handlers
    window.closeModal = closeModal;

    // Close modal events
    $('.close-modal, #closeModal').on('click', function () {
        closeModal();
    });

    // Close modal when clicking outside the content
    $('#successModal').on('click', function (e) {
        if ($(e.target).is('#successModal')) {
            closeModal();
        }
    });

    // Close modal with Escape key
    $(document).keydown(function (e) {
        if (e.key === "Escape" && $('#successModal').is(':visible')) {
            closeModal();
        }
    });

    // Load more reviews (dummy, for demo)
    $('#loadMoreBtn').on('click', function (e) {
        e.preventDefault();
        alert('Load more reviews functionality is not implemented in this demo.');
    });

    // Navigation menu toggle
    window.showMenu = function () {
        document.getElementById("navLinks").style.right = "0";
    };

    window.hideMenu = function () {
        document.getElementById("navLinks").style.right = "-200px";
    };
});