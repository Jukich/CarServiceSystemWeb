
$("#EventId").on("change", function () {
    showValue($(this).val());
})

function showValue(val) {
    console.log(val);
    $.getJSON('@Url.Action("GetDropdownList", "Home")' + "?value=" + val, function (result) {
        $("#SecondDropdown").html(""); // makes select null before filling process
        var data = result.data;
        for (var i = 0; i < data.length; i++) {
            $("#SecondDropdown").append("<option>" + data[i] + "</option>")
        }

    })
}
