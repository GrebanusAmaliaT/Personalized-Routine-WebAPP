document.addEventListener("DOMContentLoaded", () => {
    // Cere permisiune și așteaptă răspunsul
    if (Notification.permission !== "granted") {
        Notification.requestPermission().then(permission => {
            if (permission !== "granted") {
                console.warn("Notification permission not granted.");
            }
        });
    }

    const checkRoutineTime = () => {
        const now = new Date();
        const hour = now.getHours();
        const minute = now.getMinutes();

        routines.forEach(routine => {
            if (hour === routine.Hour && minute === routine.Minute) {
                showRoutineNotification(routine.RoutineType);
            }
        });
    };

    const showRoutineNotification = (routineType) => {
        if (Notification.permission === "granted") {
            new Notification("🫴 Time for your routine!", {
                body: `It's time for your ${routineType} routine!`,
                icon: "/images/routine-icon.png"
            });
        }
    };

    setInterval(checkRoutineTime, 60000);
});
