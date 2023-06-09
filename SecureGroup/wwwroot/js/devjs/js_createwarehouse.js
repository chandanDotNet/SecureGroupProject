
$(document).ready(function () {

    $("#ddlCountry").on("change", function () {

        var valu = $("#ddlCountry").val();
        var ValueType = "State";

        $.ajax(
            {
                url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    PopulateDropDown("#ddlStateId", data);
                    //$("#ddlStateId").html(data);
                },
                error: function () {
                    alert("error");
                }
            });
    });

    $("#ddlStateId").on("change", function () {

        var valu = $("#ddlStateId").val();
        var ValueType = "City";
        //debugger;
       // alert(ValueType);
        $.ajax(
            {
                url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    PopulateDropDown("#ddlCityId", data);
                    //$("#ddlStateId").html(data);
                },
                error: function () {
                    alert("error");
                }
            });
    });

    $("#ddlUserId").on("change", function () {

        var valu = $("#ddlUserId").val();
        //debugger;
        $.ajax(
            {
                url: '/Inventory/GetUserDetailsById?Id=' + valu,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //debugger;
                    $("#AdminName").val(data.name);
                    $("#AdminContactNo").val(data.email);
                    $("#AdminEmailId").val(data.contactNo);
                },
                error: function () {
                    alert("error");
                }
            });
    });


    function PopulateDropDown(dropDownId, list, selectedId) {

        $(dropDownId).empty();
        $(dropDownId).append("<option>--Please Select--</option>")
        $.each(list, function (index, row) {
            //debugger;
            var a = row.value;
            $(dropDownId).append("<option value='" + row.value + "'>" + row.text + "</option>")
        });
    }
});


