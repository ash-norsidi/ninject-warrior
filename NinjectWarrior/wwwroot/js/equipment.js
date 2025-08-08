document.addEventListener('DOMContentLoaded', function () {
    const equipmentSection = document.getElementById('equipment-section');
    if (!equipmentSection) return;

    const equipmentDropdowns = equipmentSection.querySelectorAll('.equipment-dropdown');
    const inventory = JSON.parse(equipmentSection.dataset.inventory);
    const equippedItems = JSON.parse(equipmentSection.dataset.equippedItems);

    function updateStatDifferences() {
        let strDiff = 0, defDiff = 0, evaDiff = 0, lckDiff = 0, hpDiff = 0;

        equipmentDropdowns.forEach(dropdown => {
            const selectedId = parseInt(dropdown.value);
            const slot = dropdown.dataset.slot;
            const originalItem = equippedItems[slot];
            const newItem = inventory.find(i => i.Id === selectedId);

            const originalStr = originalItem ? originalItem.StrengthBonus : 0;
            const originalDef = originalItem ? originalItem.DefenseBonus : 0;
            const originalEva = originalItem ? originalItem.EvasionBonus : 0;
            const originalLck = originalItem ? originalItem.LuckBonus : 0;
            const originalHp = originalItem ? originalItem.HealthBonus : 0;

            const newStr = newItem ? newItem.StrengthBonus : 0;
            const newDef = newItem ? newItem.DefenseBonus : 0;
            const newEva = newItem ? newItem.EvasionBonus : 0;
            const newLck = newItem ? newItem.LuckBonus : 0;
            const newHp = newItem ? newItem.HealthBonus : 0;

            strDiff += newStr - originalStr;
            defDiff += newDef - originalDef;
            evaDiff += newEva - originalEva;
            lckDiff += newLck - originalLck;
            hpDiff += newHp - originalHp;
        });

        updateDiffElement('str-diff', strDiff);
        updateDiffElement('def-diff', defDiff);
        updateDiffElement('eva-diff', evaDiff);
        updateDiffElement('lck-diff', lckDiff);
        updateDiffElement('hp-diff', hpDiff);
    }

    function updateDiffElement(elementId, diff) {
        const element = document.getElementById(elementId);
        if (!element) return;

        let indicator = '';
        let colorClass = '';

        if (diff > 0) {
            indicator = `+${diff} \u2191`;
            colorClass = 'stat-increase';
        } else if (diff < 0) {
            indicator = `${diff} \u2193`;
            colorClass = 'stat-decrease';
        } else {
            indicator = '0';
            colorClass = '';
        }
        element.textContent = indicator;
        element.className = colorClass;
    }

    equipmentDropdowns.forEach(dropdown => {
        dropdown.addEventListener('change', updateStatDifferences);
    });

    // Initial update
    updateStatDifferences();
});
