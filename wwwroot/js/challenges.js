const challenges = [
    "Drink 2 liters of water 💧",
    "Write down 3 things you love about yourself 💕",
    "Take a 10-minute walk outside 🚶‍♀️",
    "Put your phone away for 30 minutes 📵",
    "Listen to your favorite song 🎧",
    "Do 10 deep breaths 🌬️",
    "Say no to something that drains you ❌",
    "Compliment someone today 😊",
    "Eat something nourishing 🍎",
    "Stretch for 5 minutes 🧘‍♀️"
    ];

    function getDailyChallenge() {
        const today = new Date();
    const index = today.getDate() % challenges.length;
    return challenges[index];
    }

    document.addEventListener("DOMContentLoaded", function () {
        const challengeEl = document.getElementById("challengeText");
    const doneButton = document.getElementById("doneButton");
    const status = document.getElementById("challengeStatus");
    const badgeCounter = document.getElementById("badgeCounter");

    const today = new Date().toISOString().split("T")[0]; // ex: 2025-05-17
    const storageKey = "completedChallenges";
    const stored = JSON.parse(localStorage.getItem(storageKey)) || { };

    // afișare challenge
    challengeEl.textContent = getDailyChallenge();

    // badge inițial
        const count = Object.keys(stored).length;

        // Actualizează badge counter text
        badgeCounter.textContent = `🌟 ${count} self-care ${count === 1 ? "victory" : "victories"}!`;

        // Actualizează bara de progres
        const progressBar = document.getElementById("progressBar");
        const progressPercent = Math.min((count / 30) * 100, 100);
        progressBar.style.width = `${progressPercent}%`;
        progressBar.textContent = `${count}/30 days`;

        // Afișează badge câștigat
        const badgeMessage = document.getElementById("earnedBadge");
        if (count >= 30) {
            badgeMessage.textContent = "🏆 Ultimate self-care master!";
        } else if (count >= 14) {
            badgeMessage.textContent = "🥇 Gold badge unlocked!";
        } else if (count >= 7) {
            badgeMessage.textContent = "🥈 Silver badge unlocked!";
        } else if (count >= 3) {
            badgeMessage.textContent = "🥉 Bronze badge unlocked!";
        } else {
            badgeMessage.textContent = "";
        }

    // ascunde butonul dacă deja a fost bifat
    if (stored[today]) {
        doneButton.disabled = true;
    status.textContent = "✅ Already completed today. Great job!";
        }

        doneButton.addEventListener("click", () => {
        stored[today] = true;
    localStorage.setItem(storageKey, JSON.stringify(stored));

    const newCount = Object.keys(stored).length;
    badgeCounter.textContent = `🌟 ${newCount} self-care ${newCount === 1 ? "victory" : "victories"}!`;
    doneButton.disabled = true;
    status.textContent = "✅ Challenge saved. Keep it up!";
        });
    });

document.getElementById("resetProgress").addEventListener("click", () => {
    if (confirm("Are you sure you want to reset your progress? This can't be undone! :(")) {
        localStorage.removeItem("completedChallenges");
        location.reload();
    }
});

Object.keys(stored).sort().forEach(date => {
    const li = document.createElement("li");
    li.className = "list-group-item";
    li.textContent = `✅ ${date}`;
    document.getElementById("historyList").appendChild(li);
});



