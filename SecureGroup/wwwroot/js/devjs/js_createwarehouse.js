
$(document).ready(function () {

    $("#ddlCountry").on("change", function () {

        //debugger;
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
                    $("#AdminContactNo").val(data.contactNo);
                    $("#AdminEmailId").val(data.email);
                },
                error: function () {
                    alert("error");
                }
            });
    });



    $("#btnShow").click(function () {

        $.ajax({
            type: "GET",
            url: "/Inventory/CreateWarehouse",
            success: function (response) {
                //debugger;
                $('#popupdata').html(response);
                //$('#dialog').dialog('open');
            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });

    $("#btnAddProductItem").click(function () {
        debugger;
        $.ajax({
            type: "GET",
            url: "/Inventory/AddProductItem",
            success: function (response) {
                //debugger;
                $('#divProductItemAdd').html(response);

            },
            failure: function (response) {
                alert(response.responseText);
            },
            error: function (response) {
                alert(response.responseText);
            }
        });
    });

    //$('#Delete').click(function () {
    //    alert("dd");
    //   // $('#dialog-form').dialog('open');
    //});


    $("#APProductId").on("change", function () {

        var valu = $("#APProductId").val();
        var ValueType = "SubProduct";
        debugger;
        $.ajax(
            {
                url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    PopulateDropDown("#APSubProductId", data);

                },
                error: function () {
                    alert("error");
                }
            });

        $.ajax(
            {
                url: '/Master/GetProductGSTDetails?Id=' + valu,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {

                    debugger;
                    var ab = data.gstPercen;
                    var GstTypeId = data.gstTypeId;
                    $("#GSTPercen").val(ab);
                    $("#GSTTypeId").val(GstTypeId);
                    $("#lblgsttype").text("(" + data.gstTypeName + ")");

                    if (GstTypeId == 19) {
                        $(".CSGSTDiv").show();
                       
                    } else {
                        $(".CSGSTDiv").hide();
                    }
                },
                error: function () {
                    alert("error");
                }
            });
    });


    $('#APUnitPrice').keyup(function () {

        var UnitPrice = $(this).val();
        var Quantity = $("#APQuantity").val();
        var Amount = 0.00;
        var percent = $("#GSTPercen").val();
        //$('#APAmount').val(11.10);
        funCalculateAmount(UnitPrice, Quantity, Amount, percent);
        //alert($(this).val());
    });
    $('#APQuantity').keyup(function () {

        var Quantity = $(this).val();
        var UnitPrice = $("#APUnitPrice").val();
        var Amount = 0.00;
        var PerAmount = 0.00;
        var percent = $("#GSTPercen").val();
        //$('#APAmount').val(11.10);
        funCalculateAmount(UnitPrice, Quantity, Amount, percent);
        //alert($(this).val());
    });

    function funCalculateAmount(UnitPrice, Quantity, Amount, percent) {

        var percentVal = percent;
       var ProductAmount = UnitPrice * Quantity;
        var PercentageAmount = percentage(percentVal, ProductAmount);
        Amount = (parseFloat(ProductAmount) + parseFloat(PercentageAmount));
        debugger;
        var CSGSTAmount = (parseFloat(PercentageAmount) / 2);
        //$("#APAmount$").html(Amount);
        $("#APAmount").val(Amount);
        $("#GSTAmount").val(PercentageAmount);
        $("#CGSTAmount").val(CSGSTAmount);
        $("#SGSTAmount").val(CSGSTAmount);
    };
    function percentage(percent, total) {
        var perAmount = ((percent / 100) * total).toFixed(2);

        return perAmount;
    };

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


