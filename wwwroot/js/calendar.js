document.addEventListener("DOMContentLoaded", function () {
    const toggle = document.getElementById("calendarToggle");
    const calendarContainer = document.getElementById("calendarContainer");
    const selectedDateInput = document.getElementById("selectedDate");
    const selectedDateDisplay = document.getElementById("selectedDateDisplay");
    const eventForm = document.getElementById("eventForm");
    const eventTextarea = document.getElementById("eventDescription");
    const eventList = document.getElementById("eventItems");

    toggle.addEventListener("click", () => {
        calendarContainer.style.display = calendarContainer.style.display === "none" ? "block" : "none";
    });

    flatpickr("#eventCalendar", {
        inline: true,
        onReady: loadEvents,
        onMonthChange: loadEvents,
        onChange: function (selectedDates, dateStr) {
            selectedDateInput.value = dateStr;
            selectedDateDisplay.textContent = dateStr;
            eventTextarea.value = "";
            eventForm.style.display = "block";
        }
    });

    document.getElementById("eventSubmitForm").addEventListener("submit", function (e) {
        e.preventDefault();
        const date = selectedDateInput.value;
        const description = eventTextarea.value.trim();

        if (!description) return alert("Please write an event description.");

        fetch("/Calendar/AddEvent", {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "RequestVerificationToken": document.querySelector('input[name="__RequestVerificationToken"]').value
            },
            body: JSON.stringify({ date, description })
        }).then(resp => {
            if (resp.ok) {
                loadEvents();
                eventTextarea.value = "";
                alert("Event saved!");
            } else {
                alert("Error saving event.");
            }
        });
    });

    function loadEvents() {
        fetch("/Calendar/GetEvents")
            .then(resp => resp.json())
            .then(data => {
                eventList.innerHTML = "";
                const now = new Date();
                data.sort((a, b) => new Date(a.date) - new Date(b.date));

                data.forEach(ev => {
                    const evDate = new Date(ev.date);
                    if (evDate > now) {
                        const li = document.createElement("li");
                        li.className = "list-group-item";
                        li.textContent = `${ev.date} - ${ev.description}`;
                        eventList.appendChild(li);
                    }
                });
            });
    }

});