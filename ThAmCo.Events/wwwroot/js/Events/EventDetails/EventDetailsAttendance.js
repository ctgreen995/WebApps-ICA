/* This method counts how many checkboxes are checked, to update the total attended 
 * guests for an event. It adds an event listener to each checkbox to be able to 
 * tell if it is checked or not. */
$(document).ready(function () {
    var checkboxes = document.querySelectorAll('input[type="checkbox"]');
    for (var i = 0; i < checkboxes.length; i++) {
        checkboxes[i].addEventListener("change", function () {
            var attendedCount = 0
            for (var i = 0; i < checkboxes.length; i++) {
                if (checkboxes[i].checked) {
                    attendedCount = attendedCount + 1;
                }
            }
        });
    }
    $("#AttendedGuests").val(attendedCount);
});