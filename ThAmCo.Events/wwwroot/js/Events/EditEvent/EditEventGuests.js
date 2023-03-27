/* This method is used to add or remove guests from an event by adding or removing 
 * the guests from list boxes in the view.
 */
$(document).ready(function () {
    $("#addGuest").click(function () {
        $(".availableGuests :selected").remove().appendTo(".attendingGuests");
    });
    $("#removeGuest").click(function () {
        $(".attendingGuests :selected").remove().appendTo(".availableGuests");
    });
});