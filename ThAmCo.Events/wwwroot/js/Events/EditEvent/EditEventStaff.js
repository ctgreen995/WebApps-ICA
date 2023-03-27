/*
 * This method adds or removes staff from the list boxes in the view.
 */
$(document).ready(function () {
    $("#addStaff").click(function () {
        $(".availableStaff :selected").remove().appendTo(".assignedStaff");
    });
    $("#removeStaff").click(function () {
        $(".assignedStaff :selected").remove().appendTo(".availableStaff");
    });  
});