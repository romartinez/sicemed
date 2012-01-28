var app = (function ($, app) {
    var ui = app.ui = {};

    ui.showDialog = function (options, callback) {
        var defaults = {
            modal: true,
            title: 'NBSF - Registro De Firmas',
            message: '',
            buttons: {
                "Aceptar": function () {
                    $(this).dialog("close");
                    if (callback && $.isFunction(callback)) callback();
                }
            }
        };
        $.extend(defaults, options);
        var selector = "#" + defaults.container.id;
        var div = $(selector);
        if (div.size() === 0) {
            div = $('<div id="' + defaults.container.id + '" title="' + defaults.container.title + '" class="ui-helper-hidden"><div class="' + defaults.container.clazz + ' ui-corner-all" style="padding: 0 .7em;"><p><span class="ui-icon ' + defaults.container.icon + '" style="float: left; margin-right: .3em;"></span><span></span></p></div></div>');
            div.appendTo($(document));
        }

        div.find("p span:eq(1)").html(defaults.message);

        div.dialog(defaults);
    };
    ui.showError = function (options, callback) {
        $.unblockUI();
        var defaults = {
            message: '<p>Se ha producido un error desconocido.</p><p> Por favor int&eacute;ntelo nuevamente.</p>',
            container: {
                id: 'dialog-error-desconocido',
                title: 'Error',
                clazz: 'ui-state-error',
                icon: 'ui-icon-alert'
            }
        };
        if (typeof (options) === "string") {
            defaults.message = options;
        } else {
            $.extend(defaults, options);
        }
        ui.showDialog(defaults, callback);
    };
    ui.showMessage = function (options, callback) {
        var defaults = {
            message: '',
            container: {
                id: 'dialog-ok',
                title: '',
                clazz: '',
                icon: 'ui-icon-info'
            }
        };
        if (typeof (options) === "string") {
            defaults.message = options;
        } else {
            $.extend(defaults, options);
        }
        ui.showDialog(defaults, callback);
    };
    ui.showSuccess = function (options, callback) {
        var defaults = {
            message: '<p>La operaci&oacute;n fue realizada con &eacute;xito.</p>',
            container: {
                id: 'dialog-ok',
                title: '&Eacute;xito',
                clazz: 'ui-state-highlight',
                icon: 'ui-icon-circle-check'
            }
        };
        if (typeof (options) === "string") {
            defaults.message = options;
        } else {
            $.extend(defaults, options);
        }
        ui.showDialog(defaults, callback);
    };
    ui.showInput = function (options, callback) {
        var defaults = {
            message: '',
            container: {
                id: 'dialog-message',
                title: 'Mensaje',
                clazz: '',
                icon: 'ui-icon-info'
            },
            buttons: {
                "Aceptar": function () {
                    var me = $(this);
                    var input = me.find("textarea");
                    var val = input.val();
                    input.val("");
                    me.dialog("close");
                    if (callback && $.isFunction(callback)) callback(val);
                },
                "Cancelar": function () {
                    var me = $(this);
                    me.find("textarea").val("");
                    me.dialog("close");
                }
            }
        };
        if (typeof (options) === "string") {
            defaults.message = options;
        } else {
            $.extend(defaults, options);
        }
        defaults.message += "<textarea rows='5' cols='45'></textarea>";
        ui.showDialog(defaults, callback);
    };
    ui.showConfirmation = function (options, callback) {
        var defaults = {
            message: '',
            container: {
                id: 'dialog-confirm',
                title: 'Confirmaci&oacute;n',
                clazz: '',
                icon: 'ui-icon-info'
            },
            buttons: {
                "Aceptar": function () {
                    $(this).dialog("close");
                    if (callback && $.isFunction(callback)) callback();
                },
                "Cancelar": function () {
                    $(this).dialog("close");
                }
            }
        };
        if (typeof (options) === "string") {
            defaults.message = options;
        } else {
            $.extend(defaults, options);
        }
        ui.showDialog(defaults, callback);
    };

    return app;
})(jQuery, app || {});