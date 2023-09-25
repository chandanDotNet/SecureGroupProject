$(document).ready(function () {

    //************* */
    $(document).bind("contextmenu", function (e) {
        return false;
    });


    // Check if geolocation is supported by the browser
    if ("geolocation" in navigator) {
        // Prompt user for permission to access their location
        navigator.geolocation.getCurrentPosition(
            // Success callback function
            (position) => {
                // Get the user's latitude and longitude coordinates
                const lat = position.coords.latitude;
                const lng = position.coords.longitude;

                // Do something with the location data, e.g. display on a map
                console.log(`Latitude: ${lat}, longitude: ${lng}`);
                document.getElementById("lat").value = lat;
                document.getElementById("lng").value = lng;

                //var vasteras = { lat: 22.579169, lng: 88.387761 };
                //var stockholm = { lat: lat, lng: lng };                    



                //var n = arePointsNear(vasteras, stockholm, 1);

                //console.log('Your Status - '+n);
                //alert('Your Status - ' + n);
            },
            // Error callback function
            (error) => {
                // Handle errors, e.g. user denied location sharing permissions
                console.error("Error getting user location:", error);
            }
        );
    } else {
        // Geolocation is not supported by the browser
        console.error("Geolocation is not supported by this browser.");
    }

    function arePointsNear(checkPoint, centerPoint, km) {
        var ky = 40000 / 360;
        var kx = Math.cos(Math.PI * centerPoint.lat / 180.0) * ky;
        var dx = Math.abs(centerPoint.lng - checkPoint.lng) * kx;
        var dy = Math.abs(centerPoint.lat - checkPoint.lat) * ky;
        var gg = Math.sqrt(dx * dx + dy * dy);

        //Swal.fire('Any fool can use a computer');
        // sweetAlert("Oops...", "Something went wrong!", "error");
        //alert(gg);
        return Math.sqrt(dx * dx + dy * dy) <= km;
    }

    function getAddress(latitude, longitude) {
        return new Promise(function (resolve, reject) {
            var request = new XMLHttpRequest();

            var method = 'GET';
            var url = 'http://maps.googleapis.com/maps/api/geocode/json?latlng=' + latitude + ',' + longitude + '&sensor=true';
            var async = true;

            request.open(method, url, async);
            request.onreadystatechange = function () {
                if (request.readyState == 4) {
                    if (request.status == 200) {
                        var data = JSON.parse(request.responseText);
                        var address = data.results[0];
                        resolve(address);
                    }
                    else {
                        reject(request.status);
                    }
                }
            };
            request.send();
        });
    };

    $("#show_hide_password a").on('click', function (event) {

        event.preventDefault();

        if ($('#show_hide_password input').attr("type") == "text") {

            $('#show_hide_password input').attr('type', 'password');

            $('#show_hide_password i').addClass("fa-eye-slash");

            $('#show_hide_password i').removeClass("fa-eye");

        } else if ($('#show_hide_password input').attr("type") == "password") {

            $('#show_hide_password input').attr('type', 'text');

            $('#show_hide_password i').removeClass("fa-eye-slash");

            $('#show_hide_password i').addClass("fa-eye");

        }

    });


    //Due to SSL log
    $("#btnLogin").on('click', function (event) {

        var RoleId = document.getElementById("ddlRoleId").value;
        if (RoleId != 1) {
            var lat = document.getElementById("lat").value;
            var lng = document.getElementById("lng").value;
            //var vasteras = { lat: 22.579169, lng: 88.387761 };22.613797880751562, 88.41316028586681
            //var vasteras = { lat: 22.582369, lng: 88.477582 };
            var vasteras = { lat: 22.613797880751562, lng: 88.41316028586681 };
            var stockholm = { lat: lat, lng: lng };
            if (lat != "" && lng != "") {
                //alert(lat+'-not null');


                var n = arePointsNear(vasteras, stockholm, 10);
                // getAddress(lat, lng).then(console.log).catch(console.error);
                //return false;
                if (n == false) {
                    sweetAlert("Oops...", "Your current location out of coverage area!", "error");
                    return false;
                } else {
                    //alert('Your Status - ' + n);
                    return true;
                }

            } else {
                //alert(lat + '-is null');
                sweetAlert("Oops...", "Please allow your location!", "error");
                return false;

            }
        }


    });

});