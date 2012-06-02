/// <reference path="jquery-1.6.2.js" />
/// <reference path="postAntiForgery.js" />

var app = (function ($, app) {
    app.initialize = function (o) {
        var defaults = {
            isUsingProxy: false
        };

        var options = $.extend(defaults, o);

        if (!options.isUsingProxy) {
            //Google map
            var clinicaLocation = new google.maps.LatLng(-32.92501, -60.67679);

            var mapOptions = {
                center: clinicaLocation,
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };
            var map = new google.maps.Map(document.getElementById("google-map"), mapOptions);

            var clinicaMarker = new google.maps.Marker({
                position: clinicaLocation,
                map: map
            });

            google.maps.event.addListener(clinicaMarker, 'click', function () {
                infoWindow.open(map, clinicaMarker);
            });

            var infoWindow = new google.maps.InfoWindow({
                content: "<div class='infowindow'><strong>SICEMED</strong><br/>Velez Sarsfield 982, Rosario<br/>448-7896</div>"
            });

            $("#google-map-wrapper").dialog({
                autoOpen: false,
                width: 555,
                height: 400,
                modal: true,
                resizeStop: function (event, ui) { google.maps.event.trigger(map, 'resize'); },
                open: function (event, ui) {
                    google.maps.event.trigger(map, 'resize');
                    map.setCenter(clinicaLocation);
                }
            });

            $("#google-map-link").click(function () {
                $("#google-map-wrapper").dialog("open");
                return false;
            });

            //dropdown-cascading
            $(".dropdown-cascading").each(function () {
                var item = $(this);
                var parent = item.data("cascading-parent");
                $("#" + parent).change(function () {
                    item.empty();
                    var val = $(this).val();
                    if (!val) {
                        item.append($("<option />").text(item.data("cascading-parent-prompt")));
                        item.attr("disabled", "disabled");
                        return;  
                    } 

                    var url = item.data("cascading-url");
                    var params = {};
                    params[item.data("cascading-parameter")] = val;
                    $.getJSON(url, params, function (r) {
                        if (!r || r.length == 0) {
                            item.append($("<option />").text(item.data("cascading-parent-prompt")));
                            item.attr("disabled", "disabled");
                            return;
                        } else {
                            item.append($("<option />").text(item.data("cascading-prompt")));
                            item.removeAttr("disabled");
                        }
                        $.each(r, function () {
                            item.append($("<option />").val(this.Value).text(this.Text));
                        });
                    });
                });                
            });
        }
    };
    return app;
})(jQuery, app || {});

