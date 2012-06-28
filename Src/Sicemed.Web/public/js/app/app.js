﻿/// <reference path="jquery-1.6.2.js" />
/// <reference path="postAntiForgery.js" />

var app = (function ($, app) {
    app.clinica = null;

    app.initControls = function () {
        //dropdown-cascading
        $(".dropdown-cascading").each(function () {
            var item = $(this);
            //Prevent multiple bindings
            if (!item.data("has-cascading")) {
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
                item.data("has-cascading", true);
            }
        });

        $("input.ctl-timespan[type=text]").timepicker({
            showHours: false
        });
        $("input[type=date]").datepicker();
        $("input[type=time]").timepicker({
            showPeriod: true,
            showLeadingZero: true
        });

        $("div.ctl-accordion").accordion({
            collapsible: true
        });

        $("div.ctl-collapsible").accordion({
            collapsible: true,
            active: false
        });

        $(".link-submit").click(function () {
            $(this).parents("form").submit();
            return false;
        });

        $(".ctl-cancelar-prompt").click(function () {
            var form = $(this).parents("form");
            app.ui.showInput("Ingrese el motivo de cancelaci&oacute;n:", function (val) {
                if (!val) {
                    app.ui.showError("El campo es obligatorio.");
                    return;
                }
                form.find("[name='prompt']").val(val);
                form.submit();
            });
            return false;
        });

        //Selecciono el primero que no este disabled
        $("div.ctl-accordion").each(function () {
            var self = $(this);
            var index = self.find("h3:not(.ui-state-disabled):first").index();
            var active = self.accordion("option", "active");
            //2 items <h3> y <div> por tab
            if (active != index) self.accordion("option", "active", (index / 2));
        });
        // Now the hack to implement the disabling functionnality
        // http: //stackoverflow.com/a/4672074
        var accordion = $("div.ctl-accordion").data("accordion");
        if (accordion) {
            accordion._std_clickHandler = accordion._clickHandler;
            accordion._clickHandler = function (event, target) {
                var clicked = $(event.currentTarget || target);
                if (!clicked.hasClass("ui-state-disabled")) {
                    this._std_clickHandler(event, target);
                }
            };
        }
    };

    app.initialize = function (o) {
        var defaults = {
            isUsingProxy: false
        };

        var options = $.extend(defaults, o);

        app.clinica = $.parseJSON(options.clinica, true);

        //Initialize controls
        app.initControls();

        if (!options.isUsingProxy) {
            //Google map            
            var clinicaLocation = new google.maps.LatLng(app.clinica.DomicilioLatitud, app.clinica.DomicilioLongitud);

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
        }
    };
    return app;
})(jQuery, app || {});

