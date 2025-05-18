document.querySelectorAll('.reaction-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        const span = btn.querySelector('span');
        let count = parseInt(span.innerText);
        span.innerText = count + 1;

        // Dacă vrei: salvează reacția prin fetch la un controller (ex: /Community/AddReaction)
        // Sau păstrează local temporar (localStorage dacă vrei blocare dublu click)
    });
});
