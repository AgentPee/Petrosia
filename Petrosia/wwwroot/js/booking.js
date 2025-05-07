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
    completeBookingBtn.addEventListener('click', function () {
        // Validate form
        const firstName = document.getElementById('firstName').value;
        const lastName = document.getElementById('lastName').value;
        const email = document.getElementById('email').value;
        const phone = document.getElementById('phone').value;
        const privacyPolicy = document.getElementById('privacyPolicy').checked;
        const bookingConditions = document.getElementById('bookingConditions').checked;

        if (!firstName || !lastName || !email || !phone) {
            alert('Please fill in all required personal information fields.');
            return;
        }

        if (!privacyPolicy || !bookingConditions) {
            alert('Please accept our privacy policy and booking conditions.');
            return;
        }

        // Validate payment details based on selected method
        const selectedPaymentMethod = document.querySelector('input[name="paymentMethod"]:checked').value;

        if (selectedPaymentMethod === 'creditCard' || selectedPaymentMethod === 'debitCard') {
            const cardName = document.getElementById('cardName').value;
            const cardNumber = document.getElementById('cardNumber').value;
            const expiryDate = document.getElementById('expiryDate').value;
            const cvv = document.getElementById('cvv').value;

            if (!cardName || !cardNumber || !expiryDate || !cvv) {
                alert('Please fill in all required credit card details.');
                return;
            }
        } else if (selectedPaymentMethod === 'eWallet') {
            const eWalletType = document.getElementById('eWalletType').value;
            const eWalletNumber = document.getElementById('eWalletNumber').value;

            if (!eWalletType || !eWalletNumber) {
                alert('Please fill in all required e-wallet details.');
                return;
            }
        }

        // If all validations pass, show confirmation modal
        checkoutModal.style.display = 'none';

        // Update confirmation details
        document.getElementById('confirmRoomType').textContent = selectedRoom;
        document.getElementById('confirmCheckIn').textContent = document.getElementById('summaryCheckIn').textContent;
        document.getElementById('confirmCheckOut').textContent = document.getElementById('summaryCheckOut').textContent;
        document.getElementById('confirmAmount').textContent = document.getElementById('summaryPrice').textContent;

        // Generate random booking reference
        const bookingRef = 'PH' + Math.floor(100000 + Math.random() * 900000);
        document.getElementById('bookingReference').textContent = bookingRef;

        confirmationModal.style.display = 'block';
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
});