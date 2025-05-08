





﻿document.addEventListener('DOMContentLoaded', function () {
    console.log("Booking page initialization started");


    // Initialize AOS animation library
    try {
        AOS.init({
            duration: 800,
            easing: 'ease-in-out',
            once: true
        });
        console.log("AOS initialized");
    } catch (error) {
        console.error("Error initializing AOS:", error);
    }

    // Scroll to top button
    const scrollToTopBtn = document.getElementById('scrollToTopBtn');
    if (scrollToTopBtn) {
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
        console.log("Scroll to top button initialized");
    } else {
        console.warn("Scroll to top button not found");
    }

    // Room selection functionality
    const selectRoomBtns = document.querySelectorAll('.select-room-btn');
    const selectedRoomSummary = document.getElementById('selectedRoomSummary');
    const continueToCheckoutBtn = document.getElementById('continueToCheckout');

    if (!selectRoomBtns.length) {
        console.warn("No room selection buttons found");
    }

    if (!selectedRoomSummary) {
        console.warn("Selected room summary element not found");
    }

    if (!continueToCheckoutBtn) {
        console.warn("Continue to checkout button not found");
    }

    let selectedRoom = null;
    let selectedRoomPrice = 0;

    selectRoomBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            console.log("Room selected:", this.getAttribute('data-room'));

            // Update selected room
            selectedRoom = this.getAttribute('data-room');
            selectedRoomPrice = parseInt(this.getAttribute('data-price'));

            // Update UI to show selected room
            selectRoomBtns.forEach(b => b.classList.remove('selected'));
            this.classList.add('selected');

            // Calculate and display booking summary
            updateBookingSummary();

            // Show summary and continue button
            if (selectedRoomSummary) selectedRoomSummary.style.display = 'block';
            if (continueToCheckoutBtn) continueToCheckoutBtn.style.display = 'block';

            // Scroll to summary
            if (selectedRoomSummary) selectedRoomSummary.scrollIntoView({ behavior: 'smooth' });
        });
    });

    // Update booking summary based on form inputs
    function updateBookingSummary() {
        console.log("Updating booking summary");

        const checkIn = document.getElementById('check-in')?.value;
        const checkOut = document.getElementById('check-out')?.value;
        const adults = document.getElementById('adults')?.value;
        const children = document.getElementById('children')?.value;

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
        if (document.getElementById('summaryRoomType'))
            document.getElementById('summaryRoomType').textContent = selectedRoom;

        if (document.getElementById('summaryCheckIn'))
            document.getElementById('summaryCheckIn').textContent = checkIn ? formatDate(checkIn) : 'Not selected';

        if (document.getElementById('summaryCheckOut'))
            document.getElementById('summaryCheckOut').textContent = checkOut ? formatDate(checkOut) : 'Not selected';

        if (document.getElementById('summaryNights'))
            document.getElementById('summaryNights').textContent = nights;

        if (document.getElementById('summaryGuests'))
            document.getElementById('summaryGuests').textContent = `${adults} Adults, ${children} Children`;

        if (document.getElementById('summaryPrice'))
            document.getElementById('summaryPrice').textContent = `₱${totalPrice.toLocaleString()}`;

        // Update policy amounts in checkout modal
        if (document.getElementById('policyTotalAmount'))
            document.getElementById('policyTotalAmount').textContent = `₱${totalPrice.toLocaleString()}`;

        if (document.getElementById('policyCancelAmount'))
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
        const element = document.getElementById(id);
        if (element) {
            element.addEventListener('change', function () {
                if (selectedRoom) {
                    updateBookingSummary();
                }
            });
        } else {
            console.warn(`Form input #${id} not found`);
        }
    });

    // Modal functionality
    const checkoutModal = document.getElementById('checkoutModal');
    const confirmationModal = document.getElementById('confirmationModal');
    const closeBtns = document.querySelectorAll('.close');
    const continueToCheckout = document.getElementById('continueToCheckout');
    const completeBookingBtn = document.getElementById('completeBooking');
    const closeConfirmationBtn = document.getElementById('closeConfirmation');

    // Show checkout modal when Continue button is clicked
    if (continueToCheckout) {
        continueToCheckout.addEventListener('click', function () {
            if (checkoutModal) {
                checkoutModal.style.display = 'block';
                document.body.style.overflow = 'hidden'; // Prevent scrolling
            }
        });
    }

    // Close modals when X is clicked
    closeBtns.forEach(btn => {
        btn.addEventListener('click', function () {
            if (checkoutModal) checkoutModal.style.display = 'none';
            if (confirmationModal) confirmationModal.style.display = 'none';
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
            if (cardPaymentDetails) cardPaymentDetails.style.display = 'none';
            if (eWalletDetails) eWalletDetails.style.display = 'none';
            if (bankTransferDetails) bankTransferDetails.style.display = 'none';

            switch (this.value) {
                case 'creditCard':
                case 'debitCard':
                    if (cardPaymentDetails) cardPaymentDetails.style.display = 'block';
                    break;
                case 'eWallet':
                    if (eWalletDetails) eWalletDetails.style.display = 'block';
                    break;
                case 'bankTransfer':
                    if (bankTransferDetails) bankTransferDetails.style.display = 'block';
                    break;
            }
        });
    });


    

    // Database submission handling
    if (completeBookingBtn) {
        completeBookingBtn.addEventListener('click', function () {
            console.log("Complete booking button clicked");

            // Validate form fields
            const firstName = document.getElementById('firstName')?.value;
            const lastName = document.getElementById('lastName')?.value;
            const email = document.getElementById('email')?.value;
            const phone = document.getElementById('phone')?.value;
            const address = document.getElementById('address')?.value;
            const city = document.getElementById('city')?.value;
            const zipCode = document.getElementById('zipCode')?.value;
            const country = document.getElementById('country')?.value;
            const privacyPolicy = document.getElementById('privacyPolicy')?.checked;
            const bookingConditions = document.getElementById('bookingConditions')?.checked;

            if (!firstName || !lastName || !email || !phone || !address || !city || !zipCode || !country) {
                alert('Please fill in all required personal information fields.');
                return;
            }

            if (!privacyPolicy || !bookingConditions) {
                alert('Please accept our privacy policy and booking conditions.');
                return;
            }

            // Validate payment details based on selected method
            const selectedPaymentMethod = document.querySelector('input[name="paymentMethod"]:checked')?.value;
            if (!selectedPaymentMethod) {
                alert('Please select a payment method.');
                return;
            }

            if (selectedPaymentMethod === 'creditCard' || selectedPaymentMethod === 'debitCard') {
                const cardName = document.getElementById('cardName')?.value;
                const cardNumber = document.getElementById('cardNumber')?.value;
                const expiryDate = document.getElementById('expiryDate')?.value;
                const cvv = document.getElementById('cvv')?.value;

                if (!cardName || !cardNumber || !expiryDate || !cvv) {
                    alert('Please fill in all required credit card details.');
                    return;
                }
            } else if (selectedPaymentMethod === 'eWallet') {
                const eWalletType = document.getElementById('eWalletType')?.value;
                const eWalletNumber = document.getElementById('eWalletNumber')?.value;

                if (!eWalletType || !eWalletNumber) {
                    alert('Please fill in all required e-wallet details.');
                    return;
                }
            }

            // Get booking details
            const checkIn = document.getElementById('check-in')?.value;
            const checkOut = document.getElementById('check-out')?.value;
            const adults = document.getElementById('adults')?.value;
            const children = document.getElementById('children')?.value;
            const specialRequests = document.querySelector('.special-requests textarea')?.value;
            const roomType = document.getElementById('summaryRoomType')?.textContent;
            const totalAmount = document.getElementById('summaryPrice')?.textContent.replace('₱', '').replace(',', '');

            // Calculate number of nights
            let nights = 0;
            if (checkIn && checkOut) {
                const date1 = new Date(checkIn);
                const date2 = new Date(checkOut);
                const timeDiff = date2 - date1;
                nights = Math.ceil(timeDiff / (1000 * 60 * 60 * 24));
            }

            // Create booking object
            const bookingData = {
                firstName: firstName,
                lastName: lastName,
                email: email,
                phoneNumber: phone,
                address: address,
                city: city,
                zipCode: zipCode,
                country: country,
                roomType: roomType,
                checkInDate: checkIn,
                checkOutDate: checkOut,
                numberOfAdults: parseInt(adults),
                numberOfChildren: parseInt(children),
                numberOfNights: nights,
                specialRequests: specialRequests,
                totalAmount: parseFloat(totalAmount),
                paymentMethod: selectedPaymentMethod,
                bookingDate: new Date().toISOString(),
                bookingReference: 'PH' + Math.floor(100000 + Math.random() * 900000)
            };

            console.log("Booking data:", bookingData);

            // Look for anti-forgery token
            const tokenElement = document.querySelector('input[name="__RequestVerificationToken"]');
            if (!tokenElement) {
                console.warn("Anti-forgery token not found, proceeding without it");
            }

            // Submit to server
            fetch('/Home/BookRoom', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    ...(tokenElement ? { 'RequestVerificationToken': tokenElement.value } : {})
                },
                body: JSON.stringify(bookingData)
            })
                .then(response => {
                    console.log("Response received:", response.status);
                    if (!response.ok) {
                        throw new Error('Network response was not ok, status: ' + response.status);
                    }
                    return response.text();
                })
                .then(data => {
                    console.log('Success:', data);

                    // Generate random booking reference
                    const bookingRef = 'PH' + Math.floor(100000 + Math.random() * 900000);
                    if (document.getElementById('bookingReference'))
                        document.getElementById('bookingReference').textContent = bookingRef;

                    // Hide checkout modal and show confirmation
                    if (checkoutModal) checkoutModal.style.display = 'none';
                    if (confirmationModal) confirmationModal.style.display = 'block';
                })
                .catch(error => {
                    console.error('Error:', error);
                    alert('There was an error processing your booking. Please try again.');
                });
        });
    } else {
        console.warn("Complete booking button not found");
    }


    // Handle form submission
    const bookingForm = document.getElementById('bookingForm');
    if (bookingForm) {
        bookingForm.addEventListener('submit', function (e) {
            e.preventDefault();
            // Form submission is handled by the buttons
        });
    } else {
        console.warn("Booking form not found");
    }


   

    console.log("Booking page initialization completed");
});
