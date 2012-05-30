/**********************************************************************
AJAX Methods
***********************************************************************/
var app = (function ($, app) {
    var ajax = app.ajax = {};
    ajax.errorXhr = function (e, xhr) {
        var d = JSON.parse(xhr.responseText);
        var msj;

        if (d.Description === "ValidationErrorException") {
            msj = "<p>Se encontraron los siguientes errores de validaci&oacute;n: </p>";
            if (d.Data && d.Data.errors && d.Data.errors.length > 0) {
                msj += "<ul>";
                for (var i = 0; i < d.Data.errors.length; i++) {
                    msj += "<li>" + d.Data.errors[i] + "</li>";
                }
                msj += "</ul>";
            }
            msj += "<p>Corrija los mismos y vuelva a intentarlo.</p>";
        } else {
            msj = "<p>Se ha producido un error:</p>";
            msj += "<p>" + d.Description + "</p>";
        }

        app.ui.showError(msj);
    };

    ajax.beginXhr = function () {
        $("#dialog-cuenta-select").remove();
        $.blockUI({ css: { backgroundColor: '#000', color: '#fff' }, message: '<h1>Espere por favor...</h1>' });
    };

    ajax.endXhr = function () {
        $.unblockUI();
    };

    $(document).ajaxError(ajax.errorXhr);
    $.ajaxSetup({
        converters: {
            "text json": function (data) {
                return $.parseJSON(data, true);
            }
        }
    });

    return app;
} (jQuery, app || {}));
