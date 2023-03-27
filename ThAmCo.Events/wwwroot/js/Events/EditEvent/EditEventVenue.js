$(document).ready(function () {

    /* When the user clicks to delete a reservation this method sends the request to the 
     * venues controller via the events controller to delete the reservation. It also 
     * makes visible to the user the dropdownlist to select a new venue. */
    $("#deleteReservation").click(function () {
        event.preventDefault();

        document.getElementById("selectNewVenue").style.visibility = 'visible';

        var reference = $("#Reservation").val();

        $.ajax({
            type: "POST",
            url: '../../../Events/DeleteVenueReservation',
            data: { reservation: reference },
            success: function () {

                Model.reservation = null;
                $("#Reservation").val('');
                alert("Reservation Deleted.");
            },
            error: function (xhr, thrownError) {
                alert(thrownError + "\n\n" + xhr.responseText);
            }
        });

        RetrieveVenues();
    });

    /* This method is called when the user deletes the reservation, it retrieves the availble 
     * venues from the venues controller and populates the dropdownlist and model with them.*/
    function RetrieveVenues() {

        var eventType = $("#EventType").val();
        var beginDate = $("#EventDate").val();
        var endDate = $("#EventDate").val();

        $.ajax({
            type: "GET",
            url: '../../../Events/RetrieveAvailableVenues',
            data: { selectedEventType: eventType, beginDate: beginDate, endDate: endDate },
            success: function (availabilityDetails) {

                Model.VenuesDTOs = availabilityDetails.availabilityDTOs;

                $.each(availabilityDetails.selectVenues, function (index, option) {
                    $("#VenuesSelector").append(new Option(option.text, option.value));
                });
            },
            error: function (xhr, thrownError) {
                alert(thrownError + "\n\n" + xhr.responseText);
            }
        });
    };

    /* When the user selects a venue this method displays the venue description to the user by
     * populating a div*/
    $("#VenuesSelector").change(function () {
        $("#venueDescription").empty();

        var descriptionHeader = document.createElement("div");
        descriptionHeader.style.fontSize = "20px";
        descriptionHeader.innerHTML = "Venue Description";
        $("#venueDescription").append(descriptionHeader);

        var selectedId = $('#VenuesSelector').val().slice(0, 5);

        var venue = Model.VenuesDTOs.filter(function (item) { return item.code == selectedId });

        var description = venue[0].description;
        $("#venueDescription").append(description);

        var capacity = venue[0].capacity;

        $('#numberOfGuestsSelector').attr(
            {
                'min': Math.ceil(capacity / 4),
                'max': capacity
            }
        ).val(Math.ceil(capacity / 2));
    });
});