/* Booking.js */


document.addEventListener('DOMContentLoaded', function () {
    // Initialize AOS animation library
    AOS.init({
        duration: 800,
        easing: 'ease-in-out',
        once: true
    });

    // Scroll to top button
    const scrollToTopBtn = document.getElementById('scrollToTopBtn');
    window.addEventListener('scroll', function () {
        if (window.pageYOffset > 300) {
            scrollToTopBtn.style.display = 'block';
        } else {
            scrollToTopBtn.style.display = 'none';
        }
    });
    scrollToTopBtn.addEventListener('click', function () {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });

    // Room selection functionality
    const selectRoomBtns = document.querySelectorAll('.select-room-btn');
    const selectedRoomSummary = document.getElementById('selectedRoomSummary');
    const continueToCheckoutBtn = document.getElementById('continueToCheckout');

    let selectedRoom = null;
    let selectedRoomPrice = 0;

    selectRoomBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            // Update selected room
            selectedRoom = this.getAttribute('data-room');
            selectedRoomPrice = parseInt(this.getAttribute('data-price'));

            // Update UI to show selected room
            selectRoomBtns.forEach(b => b.classList.remove('selected'));
            this.classList.add('selected');

            // Calculate and display booking summary
            updateBookingSummary();

            // Show summary and continue button
            selectedRoomSummary.style.display = 'block';
            continueToCheckoutBtn.style.display = 'block';

            // Scroll to summary
            selectedRoomSummary.scrollIntoView({ behavior: 'smooth' });
        });
    });

    // Update booking summary based on form inputs
    function updateBookingSummary() {
        const checkIn = document.getElementById('check-in').value;
        const checkOut = document.getElementById('check-out').value;
        const adults = document.getElementById('adults').value;
        const children = document.getElementById('children').value;

        // Calculate number of nights
        let nights = 0;
        if (checkIn && checkOut) {
            const date1 = new Date(checkIn);
            const date2 = new Date(checkOut);
            const timeDiff = date2 - date1;
            nights = Math.ceil(timeDiff / (1000 * 60 * 60 * 24));
        }

        // Calculate total price
        const totalPrice = nights * selectedRoomPrice;

        // Update summary elements
        document.getElementById('summaryRoomType').textContent = selectedRoom;
        document.getElementById('summaryCheckIn').textContent = checkIn ? formatDate(checkIn) : 'Not selected';
        document.getElementById('summaryCheckOut').textContent = checkOut ? formatDate(checkOut) : 'Not selected';
        document.getElementById('summaryNights').textContent = nights;
        document.getElementById('summaryGuests').textContent = `${adults} Adults, ${children} Children`;
        document.getElementById('summaryPrice').textContent = `₱${totalPrice.toLocaleString()}`;

        // Update policy amounts in checkout modal
        document.getElementById('policyTotalAmount').textContent = `₱${totalPrice.toLocaleString()}`;
        document.getElementById('policyCancelAmount').textContent = `₱${totalPrice.toLocaleString()}`;
    }

    // Format date for display
    function formatDate(dateString) {
        const options = { year: 'numeric', month: 'long', day: 'numeric' };
        return new Date(dateString).toLocaleDateString('en-US', options);
    }

    // Update summary when form inputs change
    const formInputs = ['check-in', 'check-out', 'adults', 'children'];
    formInputs.forEach(id => {
        document.getElementById(id).addEventListener('change', function () {
            if (selectedRoom) {
                updateBookingSummary();
            }
        });
    });

    // Modal functionality
    const checkoutModal = document.getElementById('checkoutModal');
    const confirmationModal = document.getElementById('confirmationModal');
    const closeBtns = document.querySelectorAll('.close');
    const continueToCheckout = document.getElementById('continueToCheckout');
    const completeBookingBtn = document.getElementById('completeBooking');
    const closeConfirmationBtn = document.getElementById('closeConfirmation');

    // Show checkout modal when Continue button is clicked
    continueToCheckout.addEventListener('click', function () {
        checkoutModal.style.display = 'block';
        document.body.style.overflow = 'hidden'; // Prevent scrolling
    });

    // Close modals when X is clicked
    closeBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            checkoutModal.style.display = 'none';
            confirmationModal.style.display = 'none';
            document.body.style.overflow = 'auto'; // Re-enable scrolling
        });
    });

    // Close modals when clicking outside
    window.addEventListener('click', function (event) {
        if (event.target === checkoutModal) {
            checkoutModal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
        if (event.target === confirmationModal) {
            confirmationModal.style.display = 'none';
            document.body.style.overflow = 'auto';
        }
    });

    // Handle payment method selection
    const paymentMethods = document.querySelectorAll('input[name="paymentMethod"]');
    const cardPaymentDetails = document.getElementById('cardPaymentDetails');
    const eWalletDetails = document.getElementById('eWalletDetails');
    const bankTransferDetails = document.getElementById('bankTransferDetails');

    paymentMethods.forEach(method => {
        method.addEventListener('change', function () {
            cardPaymentDetails.style.display = 'none';
            eWalletDetails.style.display = 'none';
            bankTransferDetails.style.display = 'none';

            switch (this.value) {
                case 'creditCard':
                case 'debitCard':
                    cardPaymentDetails.style.display = 'block';
                    break;
                case 'eWallet':
                    eWalletDetails.style.display = 'block';
                    break;
                case 'bankTransfer':
                    bankTransferDetails.style.display = 'block';
                    break;
            }
        });
    });

    // Handle form submission
    completeBookingBtn.addEventListener('click', async function() {
        try {
            // Validate form fields
            const firstName = document.getElementById('firstName').value;
            const lastName = document.getElementById('lastName').value;
            const email = document.getElementById('email').value;
            const phone = document.getElementById('phone').value;
            const checkIn = document.getElementById('check-in').value;
            const checkOut = document.getElementById('check-out').value;
            const adults = parseInt(document.getElementById('adults').value);
            const children = parseInt(document.getElementById('children').value);
            
            // Basic validation
            if (!firstName || !lastName || !email || !phone || !checkIn || !checkOut || !selectedRoom) {
                alert('Please fill in all required fields');
                return;
            }

            updateDebugInfo('Preparing to send booking request...');

            // Create booking object
            const booking = {
                roomType: selectedRoom,
                checkInDate: new Date(checkIn).toISOString(),
                checkOutDate: new Date(checkOut).toISOString(),
                adults: adults,
                children: children || 0
            };

            updateDebugInfo('Sending booking request: ' + JSON.stringify(booking));

            // Send booking to server
            const response = await fetch('/Home/BookRoom', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'Accept': 'application/json'
                },
                body: JSON.stringify(booking)
            });

            const data = await response.json();
            
            if (!response.ok || data.error || data.errors) {
                throw new Error(data.error || (Array.isArray(data.errors) ? data.errors.join(', ') : 'Booking failed'));
            }

            updateDebugInfo('Booking successful: ' + JSON.stringify(data));

            // Show confirmation modal
            checkoutModal.style.display = 'none';
            document.getElementById('bookingReference').textContent = `PH${data.bookingId}`;
            document.getElementById('confirmRoomType').textContent = selectedRoom;
            document.getElementById('confirmCheckIn').textContent = formatDate(checkIn);
            document.getElementById('confirmCheckOut').textContent = formatDate(checkOut);
            document.getElementById('confirmAmount').textContent = document.getElementById('summaryPrice').textContent;
            confirmationModal.style.display = 'block';

        } catch (error) {
            console.error('Booking error:', error);
            updateDebugInfo('Error: ' + error.message);
            alert(`Booking failed: ${error.message}`);
        }
    });

    // Add this helper function for debugging
    function debugDatabaseConnection() {
        fetch('/Home/TestDatabaseConnection')
            .then(response => response.json())
            .then(result => {
                console.log('Database connection test:', result);
            })
            .catch(error => {
                console.error('Database connection test failed:', error);
            });
    }

    // Call this on page load
    document.addEventListener('DOMContentLoaded', function() {
        debugDatabaseConnection();
    });

    // Close confirmation modal and redirect
    closeConfirmationBtn.addEventListener('click', function () {
        confirmationModal.style.display = 'none';
        document.body.style.overflow = 'auto';
        // Redirect to homepage or wherever you want
        window.location.href = '/';
    });

    // Form submission handler
    document.getElementById('bookingForm').addEventListener('submit', function (e) {
        e.preventDefault();
        // This is handled by the room selection and checkout process
    });

    // Add event listeners for date changes
    document.getElementById('check-in').addEventListener('change', updateRoomAvailability);
    document.getElementById('check-out').addEventListener('change', updateRoomAvailability);
});

function updateRoomAvailability() {
    const checkIn = document.getElementById('check-in').value;
    const checkOut = document.getElementById('check-out').value;

    if (checkIn && checkOut) {
        fetch(`/Home/GetAvailableRooms?checkIn=${checkIn}&checkOut=${checkOut}`)
            .then(response => response.json())
            .then(data => {
                // Update room selection UI based on availability
                document.querySelectorAll('.room-option').forEach(roomOption => {
                    const roomType = roomOption.getAttribute('data-room-type');
                    const availableCount = data[roomType] || 0;
                    
                    const availabilityBadge = roomOption.querySelector('.availability-badge');
                    const selectButton = roomOption.querySelector('.select-room-btn');
                    
                    if (availableCount > 0) {
                        availabilityBadge.textContent = `${availableCount} rooms available`;
                        availabilityBadge.classList.remove('sold-out');
                        selectButton.disabled = false;
                    } else {
                        availabilityBadge.textContent = 'Sold Out';
                        availabilityBadge.classList.add('sold-out');
                        selectButton.disabled = true;
                    }
                });
            });
    }
}