
$(document).ready(function () {
    $("#ddlProductList").on("change", function () {

        var valu = $("#ddlProductList").val();
        var ValueType = "SubProduct";
       // debugger;
        $.ajax(
            {
                url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    PopulateDropDown("#ddlSubProductList", data);

                },
                error: function () {
                    alert("error");
                }
            });
    });

    $("#ddlSubProductList").on("change", function () {

        var valu = $("#ddlSubProductList").val();
        var ValueType = "SubProduct";
       
        $.ajax(
            {
                url: '/Inventory/GetSubProductDetailsById?Id=' + valu,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {                    
                    var UnitData = data.unit;
                    $("#lblQuantityUnit").html(UnitData);                   
                },
                error: function () {
                    alert("error");
                }
            });
    });

    $("#ddlTransferType").on("change", function () {

        var valu = $("#ddlTransferType").val();

        PopulateDropDownbyTransferType(valu);       
    });


    //$("#TransferQuantity").keydown(function () {
    //    debugger;
    //    var val = this.value;
    //    $("#TransferQuantity").css("background-color", "yellow");
    //});

    $('#TransferQuantity').keyup(function () {

        var productQuantity = $(this).val();
        CheckDependentValue(productQuantity);
        //alert($(this).val());
    });
    function PopulateDropDown(dropDownId, list, selectedId) {

        $(dropDownId).empty();
        //$(dropDownId).append("<option>--Please Select--</option>")
        $.each(list, function (index, row) {
            //debugger;
            var a = row.value;
            $(dropDownId).append("<option value='" + row.value + "'>" + row.text + "</option>")
        });
    }

    function PopulateDropDownbyTransferType(valu) {

        if (valu == 1) {
            //WH-WH
           // debugger;
            $("#lblSource").empty();
            $("#lblSource").html("Source- WareHouse");
            $("#lblDestination").html("Destination- WareHouse");
            var ValueType = "WareHouse";
           // debugger;
            $.ajax(
                {
                    url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType,
                    type: 'GET',
                    data: "",
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        PopulateDropDown("#ddlSource", data);
                        PopulateDropDown("#ddlDestination", data);

                    },
                    error: function () {
                        alert("error");
                    }
                });

           

        } else if (valu == 2) {
            //WH-Site           
            var ValueType1 = "WareHouse";
            var ValueType2 = "Site";
            $("#lblSource").html("Source- WareHouse");
            $("#lblDestination").html("Destination- Site");

            $.ajax(
                {
                    url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType1,
                    type: 'GET',
                    data: "",
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        PopulateDropDown("#ddlSource", data);                       

                    },
                    error: function () {
                        alert("error");
                    }
                });
           
            $.ajax(
                {
                    url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType2,
                    type: 'GET',
                    data: "",
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        PopulateDropDown("#ddlDestination", data);                      

                    },
                    error: function () {
                        alert("error");
                    }
                });

        } else if (valu == 3) {
            //Site-WH
            var ValueType1 = "Site";
            var ValueType2 = "WareHouse";

            $("#lblSource").html("Source- Site");
            $("#lblDestination").html("Destination- WareHouse");
            $.ajax(
                {
                    url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType1,
                    type: 'GET',
                    data: "",
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        PopulateDropDown("#ddlSource", data);
                    },
                    error: function () {
                        alert("error");
                    }
                });
            
            $.ajax(
                {
                    url: '/Inventory/GetDropdownListDataById?Id=' + valu + '&DropdownDataType=' + ValueType2,
                    type: 'GET',
                    data: "",
                    contentType: 'application/json; charset=utf-8',
                    success: function (data) {
                        PopulateDropDown("#ddlDestination", data);

                    },
                    error: function () {
                        alert("error");
                    }
                });

        }


    }


    function CheckDependentValue(productQuantity) {

        var ddlTransferType = $("#ddlTransferType").val();
        var ddlSource = $("#ddlSource").val();
        var ddlDestination = $("#ddlDestination").val();
        var ddlProductId = $("#ddlProductList").val();
        var ddlSubProduct = $("#ddlSubProductList").val();
        var Quantity = productQuantity;
        if (ddlTransferType > 0) {

            if (ddlSource > 0) {

                if (ddlDestination > 0) {

                    if (ddlProductId > 0) {

                        if (ddlSubProduct > 0) {

                          
                            CheckProductAvailable(ddlTransferType, ddlSource, ddlDestination, ddlProductId, ddlSubProduct, Quantity);

                        } else {
                            alert("Please select Sub Product");
                        }

                    } else {
                        alert("Please select Product");
                    }

                } else {
                    alert("Please select Destination");
                }
            } else {
                alert("Please select Source");
            }

        } else {
            alert("Please select Transfer Type");
        }

    }


    function CheckProductAvailable(ddlTransferType, ddlSource, ddlDestination, ddlProductId, ddlSubProduct, Quantity) {
        
        $.ajax(
            {
                url: '/Inventory/CheckProductAvailable?TransferType=' + ddlTransferType + '&Source=' + ddlSource + '&Destination=' + ddlDestination + '&ProductId=' + ddlProductId + '&SubProduct=' + ddlSubProduct + '&Quantity=' + Quantity,
                type: 'GET',
                data: "",
                contentType: 'application/json; charset=utf-8',
                success: function (data) {
                    //PopulateDropDown("#ddlSubProductList", data);
                    debugger;
                    if (data < 0) {
                        //debugger;
                        $("#TransferQuantity").empty();
                        $("#TransferQuantity").val("");
                        alert("You don't have sufficient product quantity for transfer");
                       
                    }
                },
                error: function () {
                    alert("error");
                }
            });
    }

});