document.addEventListener('DOMContentLoaded', function () {
    // Find any temporary alert boxes on the page
    const alerts = document.querySelectorAll('#tempAlertError, #tempAlertSuccess');
    
    alerts.forEach(alertEl => {
        // Wait 4 seconds (4000ms), then trigger Bootstrap's alert close
        setTimeout(() => {
            const bsAlert = new bootstrap.Alert(alertEl);
            bsAlert.close();
        }, 3000);
    });
});