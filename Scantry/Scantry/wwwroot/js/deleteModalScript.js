document.addEventListener('DOMContentLoaded', function () {
    const universalDeleteModal = document.getElementById('universalDeleteModal');
    if (universalDeleteModal) {
        universalDeleteModal.addEventListener('show.bs.modal', function (event) {
            // Find the button (whether the user clicked the button or the inner <i> icon)
            const button = event.relatedTarget.closest('button');
            if (!button) return;

            const id = button.getAttribute('data-id');
            const actionUrl = button.getAttribute('data-action-url');
            const title = button.getAttribute('data-title');
            const message = button.getAttribute('data-message');

            document.getElementById('universalDeleteForm').action = actionUrl;
            document.getElementById('deleteTargetId').value = id;
            document.getElementById('deleteModalTitle').textContent = title || 'Confirm Deletion';
            document.getElementById('deleteModalMessage').textContent = message || 'Are you sure you want to delete this item?';
        });
    }
});