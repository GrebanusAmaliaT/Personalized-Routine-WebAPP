document.addEventListener("DOMContentLoaded", function () {
    const toggle = document.getElementById("calendarToggle");
    const calendarContainer = document.getElementById("calendarContainer");
    const selectedDateInput = document.getElementById("selectedDate");
    const selectedDateDisplay = document.getElementById("selectedDateDisplay");
    const eventForm = document.getElementById("eventForm");
    const eventTextarea = document.getElementById("eventDescription");

    if (!toggle || !calendarContainer) {
        console.error("Missing elements: calendarToggle or calendarContainer");
        return;
    }

    toggle.addEventListener("click", function () {
        if (calendarContainer.style.display === "none" || calendarContainer.style.display === "") {
            calendarContainer.style.display = "block";
        } else {
            calendarContainer.style.display = "none";
        }
    });

    flatpickr("#eventCalendar", {
        inline: true,
        dateFormat: "Y-m-d",
        onChange: function (selectedDates, dateStr) {
            selectedDateInput.value = dateStr;
            selectedDateDisplay.textContent = dateStr;
            eventTextarea.value = "";
            eventForm.style.display = "block";
        }
    });

    const submitForm = document.getElementById("eventSubmitForm");

    if (submitForm) {
        submitForm.addEventListener("submit", function (e) {
            e.preventDefault();

            const date = selectedDateInput.value;
            const description = eventTextarea.value.trim();

            if (!description) {
                alert("Please write an event description.");
                return;
            }

            fetch("/Calendar/AddEvent", {
                method: "POST",
                headers: {
                    "Content-Type": "application/json",
                    "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({ date, description })
            }).then(resp => {
                if (resp.ok) {
                    window.location.reload();
                } else {
                    alert("Error saving event.");
                }
            });
        });
    }
});
