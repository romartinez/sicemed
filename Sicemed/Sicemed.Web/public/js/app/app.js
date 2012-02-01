/// <reference path="jquery-1.6.2.js" />
/// <reference path="postAntiForgery.js" />

var app = (function ($, app) {
    app.initialize = function () {
        //Google map
        var myOptions = {
            center: new google.maps.LatLng(-34.397, 150.644),
            zoom: 8,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("google-map"), myOptions);

        $("#google-map-wrapper").dialog({
            autoOpen: false,
            width: 555,
            height: 400,
            modal: true,
            resizeStop: function (event, ui) { google.maps.event.trigger(map, 'resize'); },
            open: function (event, ui) { google.maps.event.trigger(map, 'resize'); }
        });

        $("#google-map-link").click(function () {
            $("#google-map-wrapper").dialog("open");            
            return false;
        });
    };
    return app;
})(jQuery, app || {});

