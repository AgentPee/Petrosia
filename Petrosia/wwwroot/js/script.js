let currentPage = 1;

function nextPage() {
    if (currentPage === 1) {
        document.getElementById("guestTable").style.display = "none";
        document.getElementById("roomServiceTable").style.display = "table";
        document.getElementById("roomsTable").style.display = "none"; 
        document.getElementById("recordTitle").textContent = "Employee's Records";
        currentPage = 2;
    } else if (currentPage === 2) {
        document.getElementById("roomServiceTable").style.display = "none";
        document.getElementById("roomsTable").style.display = "table";
        document.getElementById("recordTitle").textContent = "Rooms";
        currentPage = 3;

}
}

function prevPage() {
    if (currentPage === 4) {
        document.getElementById("webTeamTable").style.display = "none";
        document.getElementById("roomsTable").style.display = "table";
        document.getElementById("recordTitle").textContent = "Rooms";
        currentPage = 3;
    } else if (currentPage === 3) {
        document.getElementById("roomsTable").style.display = "none";
        document.getElementById("roomServiceTable").style.display = "table";
        document.getElementById("recordTitle").textContent = "Employee's Records";
        currentPage = 2;
    } else if (currentPage === 2) {
        document.getElementById("roomServiceTable").style.display = "none";
        document.getElementById("guestTable").style.display = "table";
        document.getElementById("recordTitle").textContent = "Guest Records";
        currentPage = 1;
    }
}


function editRecord(button) {
    let row = button.parentElement.parentElement;
    let cells = row.querySelectorAll("td:not(:last-child)");
    
    let newValues = Array.from(cells).map(cell => prompt("Edit value:", cell.textContent));
    if (newValues.some(val => val === null)) return;

    cells.forEach((cell, index) => cell.textContent = newValues[index]);

    // Handle status updates for employees
    if (currentPage === 2) {
        let statusCell = row.cells[4];
        let status = statusCell.textContent.toLowerCase();

        statusCell.className = ""; // Reset class

        if (status === "on duty") {
            statusCell.classList.add("status-OnDuty");
        } else if (status === "excused") {
            statusCell.classList.add("status-Excused");
        } else if (status === "absent") {
            statusCell.classList.add("status-Absent");
        }
    }
}

function deleteRecord(button) {
    if (confirm("Are you sure you want to delete this record?")) {
        let row = button.parentElement.parentElement;
        row.remove();
    }
}

// 🔍 SEARCH FUNCTION
function searchRecord() {
    let query = document.getElementById("search").value.toLowerCase();
    let table = currentPage === 1 ? document.getElementById("guestTable") : document.getElementById("roomServiceTable");
    let rows = table.getElementsByTagName("tr");

    for (let i = 1; i < rows.length; i++) {
        let cells = rows[i].getElementsByTagName("td");
        let matchFound = false;

        for (let j = 0; j < cells.length - 1; j++) { // Ignore actions column
            if (cells[j].textContent.toLowerCase().includes(query)) {
                matchFound = true;
                break;
            }
        }

        rows[i].style.display = matchFound ? "" : "none";
    }
}
function searchTable() {
    let input = document.getElementById("searchInput").value.toLowerCase();
    let tables = document.querySelectorAll("table");
    
    tables.forEach((table) => {
        if (table.style.display !== "none") { // Only search in the visible table
            let rows = table.getElementsByTagName("tr");

            for (let i = 1; i < rows.length; i++) { // Start from 1 to skip headers
                let cells = rows[i].getElementsByTagName("td");
                let match = false;

                for (let j = 0; j < cells.length; j++) {
                    if (cells[j].textContent.toLowerCase().includes(input)) {
                        match = true;
                        break;
                    }
                }

                rows[i].style.display = match ? "" : "none";
            }
        }
    });
}
function toggleWebTeam() {
    var table = document.getElementById("webTeamTable");
    table.style.display = (table.style.display === "none") ? "table" : "none";
 }
 document.addEventListener("DOMContentLoaded", function () {
    // Select all rows in the table
    let rows = document.querySelectorAll("#roomServiceTable tbody tr");

    rows.forEach(row => {
        let timeInCell = row.cells[4]; // The "Time In" column (index 4)
        let statusCell = row.cells[4]; // The "Status" column (index 7)

        let timeInText = timeInCell.textContent.trim(); // Get time value
        let timeIn = convertTo24Hour(timeInText); // Convert to 24-hour format

        if (timeIn > "08:05") { 
            statusCell.textContent = "Late"; // Update status text
            statusCell.style.color = "red"; // Make text red
            statusCell.style.fontWeight = "bold"; // Make text bold
        }
    });
});

// Function to convert AM/PM time to 24-hour format (HH:MM)
function convertTo24Hour(time) {
    let match = time.match(/(\d+):(\d+)(AM|PM)/);
    if (!match) return time;

    let hours = parseInt(match[1]);
    let minutes = match[2];
    let period = match[3];

    if (period === "PM" && hours !== 12) {
        hours += 12;
    } else if (period === "AM" && hours === 12) {
        hours = 0;
    }

    return hours.toString().padStart(2, '0') + ":" + minutes;
}
