document.addEventListener('DOMContentLoaded', function () {
    function ajaxifyBattleSection() {
        const battleSection = document.querySelector('#battle-section-container');
        if (!battleSection) return;

        // Attack form
        const attackForm = battleSection.querySelector('#attack-form');
        if (attackForm) {
            attackForm.addEventListener('submit', function (e) {
                e.preventDefault();
                const formData = new FormData(attackForm);
                fetch(attackForm.action, {
                    method: 'POST',
                    body: formData,
                    headers: { 'X-Requested-With': 'XMLHttpRequest' }
                })
                .then(r => r.text())
                .then(html => {
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(html, 'text/html');
                    const newBattleSection = doc.querySelector('#battle-section-container');
                    if (newBattleSection) {
                        battleSection.innerHTML = newBattleSection.innerHTML;
                    }

                    // If battle is over, reload the page
                    if (document.querySelector('#battle-over')) {
                        window.location.reload();
                        return;
                    }
                    ajaxifyBattleSection();
                });
            });
        }

        // Equipment form
        const equipmentForm = battleSection.querySelector('#equipment-form');
        if (equipmentForm) {
            equipmentForm.addEventListener('submit', function (e) {
                e.preventDefault();
                const formData = new FormData(equipmentForm);
                fetch(equipmentForm.action, {
                    method: 'POST',
                    body: formData,
                    headers: { 'X-Requested-With': 'XMLHttpRequest' }
                })
                .then(r => r.text())
                .then(html => {
                    const parser = new DOMParser();
                    const doc = parser.parseFromString(html, 'text/html');
                    const newBattleSection = doc.querySelector('#battle-section-container');
                    if (newBattleSection) {
                        battleSection.innerHTML = newBattleSection.innerHTML;
                    }

                    if (document.querySelector('#battle-over')) {
                        window.location.reload();
                        return;
                    }
                    ajaxifyBattleSection();
                });
            });
        }

        // Equipment toggle
        const toggleBtn = battleSection.querySelector('#equipment-toggle');
        const equipSection = battleSection.querySelector('#equipment-section');
        if (toggleBtn && equipSection) {
            toggleBtn.addEventListener('click', function () {
                equipSection.classList.toggle('hidden');
            });
        }
    }

    ajaxifyBattleSection();
});
