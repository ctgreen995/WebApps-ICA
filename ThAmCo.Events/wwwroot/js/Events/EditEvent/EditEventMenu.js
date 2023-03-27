
$(document).ready(function () {
    /*
     * If the user clisks the delete food booking button this method 
     * sends the request to the catering controller via the events 
     * controller, which will send the delete request to the catering web service.
     * It also makes visible to the user the dropdownlist of availble menus.
     */
    $("#deleteFoodBooking").click(function (e) {
        e.preventDefault();

        document.getElementById("MenuSelector").style.visibility = 'visible';

        var eventId = $("#EventId").val();
        $.ajax({
            type: "POST",
            url: '../../Events/DeleteFoodBooking',
            data: { id: eventId },
            success: function () {
                Model.FoodBooking = null;
                $("#FoodBooking").val('');
                alert("Foodbooking deleted.");
            },
            error: function (xhr, thrownError) {
                alert(thrownError + "\n\n" + xhr.responseText);
            }
        });
    });

    /*
     * When the user selects a menu this method displays the food items for that menu to the user.
     */
    $("#MenuSelector").change(function () {

        $("#FoodBooking").empty();
        $("#foodItems").empty();

        var selectedMenuId = $(this).val();

        var filteredMenus = Model.menuDTOs.filter(function (item) { return item.id == selectedMenuId });
        var menu = filteredMenus[0];

        var foodItemsDiv = document.getElementById("foodItems");

        var foodItemsHeader = document.createElement("div");
        foodItemsHeader.style.fontSize = "20px";
        foodItemsHeader.innerHTML = "Menu Food Items: ";
        foodItemsDiv.appendChild(foodItemsHeader);

        menu.foodItems.forEach(function (foodItem) {
            var foodItemDiv = document.createElement("div");
            foodItemDiv.innerHTML = "£" + foodItem.price + " " + foodItem.description;
            foodItemsDiv.appendChild(foodItemDiv);
        });
    }); 
});


