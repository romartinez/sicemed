/// <reference path="jquery-1.6.2.js" />
/// <reference path="postAntiForgery.js" />

var app = (function ($, app) {
    app.clinica = null;

    var highligthMatch = function (text, term) {
        var markup = [];
        var match = text.toUpperCase().indexOf(term.toUpperCase()),
            tl = term.length;

        if (match < 0) {
            markup.push(text);
        } else {
            markup.push(text.substring(0, match));
            markup.push("<span class='select2-match'>");
            markup.push(text.substring(match, match + tl));
            markup.push("</span>");
            markup.push(text.substring(match + tl, text.length));
        }
        return markup.join("");;
    };

    app.initControls = function (opts) {
        var defaults = { isAjax: false };
        var settings = $.extend(defaults, opts);
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
            showHours: true,
            showPeriod: false,
            showPeriodLabels: false,
            showLeadingZero: true
        });
        $("input.date").each(function () {
            function getDateYymmdd(value) {
                if (value == null)
                    return null;
                return $.datepicker.parseDate("dd/mm/yy", value);
            }
            var minDate = getDateYymmdd($(this).data("val-rangedate-min"));
            var maxDate = getDateYymmdd($(this).data("val-rangedate-max"));
            $(this).datepicker({
                dateFormat: "dd/mm/yy",
                minDate: minDate,
                maxDate: maxDate
            });
        });

        $("input.time").timepicker({
            showPeriod: false,
            showPeriodLabels: false,
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
            var indexActive = self.find("h3.ui-state-highlight:first").index();
            // Check if someone is highlighted and select it
            if (indexActive && indexActive > 0) index = indexActive;
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

        if (!settings.isAjax) {
            //Searcheable
            $(".searcheable").each(function (i, el) {
                var searcheable = $(el);
                if (searcheable.data("template")) {
                    var templateName = searcheable.data("template");
                    $("#" + templateName).template(templateName);
                }
                var formatSelection = function (obj) {
                    if (searcheable.data("display-property")) {
                        return obj[searcheable.data("display-property")];
                    }
                    return obj.Descripcion;
                };
                var formatResult = function (obj, container, query) {
                    var text;
                    if (searcheable.data("template")) {
                        text = $("<div/>").append($.tmpl(searcheable.data("template"), obj)).html();
                    } else {
                        text = obj.Descripcion;
                    }
                    return highligthMatch(text, query.term);
                };
                searcheable.select2({
                    placeholder: searcheable.data("prompt"),
                    allowClear: true,
                    minimumInputLength: 3,
                    ajax: {
                        quietMillis: 500,
                        url: searcheable.data("url"),
                        dataType: "json",
                        data: function (term, page) {
                            return { filtro: term };
                        },
                        results: function (data, page) {
                            return { results: data };
                        }
                    },
                    formatResult: formatResult,
                    formatSelection: formatSelection,
                    width: '300px',
                    id: function (e) { return e.Id; },
                    formatNoMatches: function () { return "No se encuentran coincidencias."; },
                    formatInputTooShort: function (input, min) { return "Por favor ingrese " + (min - input.length) + " caracteres más."; },
                    formatSelectionTooBig: function (limit) { return "Sólo puede seleccionar " + limit + " elementos."; },
                    formatLoadMore: function (pageNumber) { return "Cargando más resultados..."; }
                });
            });
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

