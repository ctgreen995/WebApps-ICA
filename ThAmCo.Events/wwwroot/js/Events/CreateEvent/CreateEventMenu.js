
/*
 * When a user selects a menu from the dropdown this method populates
 * a div with child divs containing the food items for that menu. 
 */
$(document).ready(function () {

    $("#SelectedMenu").change(function () {

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
        })
    });
});