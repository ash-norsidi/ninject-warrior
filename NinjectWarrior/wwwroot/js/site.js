document.addEventListener('DOMContentLoaded', function () {
    const equipmentToggle = document.getElementById('equipment-toggle');
    const equipmentSection = document.getElementById('equipment-section');

    if (equipmentToggle && equipmentSection) {
        equipmentToggle.addEventListener('click', function () {
            const isHidden = equipmentSection.classList.contains('hidden');
            if (isHidden) {
                equipmentSection.classList.remove('hidden');
                equipmentToggle.textContent = 'Close';
            } else {
                equipmentSection.classList.add('hidden');
                equipmentToggle.textContent = 'Equipment';
            }
        });
    }
});
