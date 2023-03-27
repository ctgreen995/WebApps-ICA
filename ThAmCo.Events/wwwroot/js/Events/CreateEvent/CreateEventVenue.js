$(document).ready(function () {

    /*
     * This method retrieves the available venues from the venues controller, via the events controller.
     * It populates a dropdownlist with available venues and also adds the available venues DTOs to the Model
     * to allow for extracting information about the venues to display to the user.
     */
    $("#submitDates").click(function (e) {
        e.preventDefault();

        var eventType = $("#SelectedEventType").val();
        var beginDate = $("#BeginDate").val();
        var endDate = $("#EndDate").val();
        $.ajax({
            type: "GET",
            url: '../../../Events/RetrieveAvailableVenues',
            data: { selectedEventType: eventType, beginDate: beginDate, endDate: endDate },
            success: function (availabilityDetails) {

                Model.AvailableVenues = availabilityDetails.availabilityDTOs;

                $.each(availabilityDetails.selectVenues, function (index, option) {
                    $("#SelectedAvailability").append(new Option(option.text, option.value));
                });

                $('#SelectedAvailability option:first').prop('selected', true);
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert(xhr.responseText + "\n\n" + thrownError);
            }
        });
    });

    /* When a venue has been selected this method populates
     * a div with child divs containing a description of the venue.
     * It also sets the event date to the date on which the selected
     * venue is available and then finally sets a max and minimum
     * number of guests for the guests selector, with a starting value of 
     * half of the capacity. 
     */
    $("#SelectedAvailability").change(function () {

        $("#venueDescription").empty();

        var descriptionHeader = document.createElement("div");
        descriptionHeader.style.fontSize = "20px";
        descriptionHeader.innerHTML = "Venue Description";
        $("#venueDescription").append(descriptionHeader);

        var selectedId = $('#SelectedAvailability').val().slice(0, 5);
        var selectedDate = $('#SelectedAvailability').val().slice(5);

        var venue = Model.AvailableVenues.filter(function (item) { return item.code == selectedId });

        var description = venue[0].description;
        $("#venueDescription").append(description);

        Model.eventDate = selectedDate;
        $("#EventDate").val(selectedDate);

        var capacity = venue[0].capacity;

        $('#numberOfGuestsSelector').attr(
            {
                'min': Math.ceil(capacity / 4),
                'max': capacity
            }
        ).val(Math.ceil(capacity / 2));
    });
});