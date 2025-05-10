// Star rating interaction
$(document).ready(function () {
    // Star rating logic
    $('.star-rating i').on('mouseenter', function () {
        var rating = $(this).data('rating');
        $('.star-rating i').each(function (index) {
            if (index < rating) {
                $(this).addClass('fas').removeClass('far');
            } else {
                $(this).addClass('far').removeClass('fas');
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

    // Modal logic for success message
    if ($('#successModal').is(':visible')) {
        $('#successModal').show();
    }

    $('.close-modal, #closeModal').on('click', function () {
        $('#successModal').hide();
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