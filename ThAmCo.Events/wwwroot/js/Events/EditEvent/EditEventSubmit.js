$(document).ready(function () {

    /* This method populates an array of ids of the staff in the 
     * assigned staff and guests list boxes for them to be posted to the controller
     * along with the values from the view model.
     */
    $(function () {
        function updateSelectedStaff() {
            var selectedValues = [];
            $(".assignedStaff option").each(function () {
                selectedValues.push($(this).val());
            });
            return selectedValues;
        }
        function updateSelectedGuests() {
            var selectedValues = [];
            $(".attendingGuests option").each(function () {
                selectedValues.push($(this).val());
            });
            return selectedValues;
        }
        $("#form").submit(function () {

            event.preventDefault();

            var formData = {
                EventId: $('#EventId').val(),
                Title: $('#Title').val(),
                EventDate: $('#EventDate').val(),
                EventType: $('#EventType').val(),
                Reservation: $('#Reservation').val(),
                SelectedVenue: $('#VenuesSelector').val(),
                NumberOfGuests: $('#numberOfGuestsSelector').val(),
                SelectedMenu: $('#MenuSelector').val(),
                FoodBooking: $('#FoodBooking').val(),
                SelectedStaff: updateSelectedStaff(),
                SelectedGuests: updateSelectedGuests()
            };

            $.ajax({
                type: "POST",
                url: $('#form').attr("action"),
                data: formData,
                success: function (response) {
                    alert("Event saved.")
                    location.href = '../../Events/ListEvents'
                },
                error: function (jqXHR, textStatus, errorThrown) {

                    alert(jqXHR.status + ' ' + errorThrown + '\n\nFailed to edit Event.');
                },
            });
        });
    });
});