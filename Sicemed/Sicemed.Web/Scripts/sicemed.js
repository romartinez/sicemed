var S = (function (my, $) {

    /* Public Methods
    *********************************/
    /* Helpers */
    var helpers = my.helpers = my.helpers || {};

    helpers.evalDate = function (key, value) {
        if (typeof value === 'string') {
            var a = /^(\d{4})-(\d{2})-(\d{2})T(\d{2}):(\d{2}):(\d{2}(?:\.\d*)?)(?:([\+-])(\d{2})\:(\d{2}))?Z?$/.exec(value);
            if (a) {
                var utcMilliseconds = Date.UTC(+a[1], +a[2] - 1, +a[3], +a[4], +a[5], +a[6]);
                return new Date(utcMilliseconds);
            }
        }
        return value;
    };

    helpers.getAntiForgeryToken = function () {
        var token = $.getAntiForgeryToken();
        if (token) return token.value;
        return undefined;
    };

    helpers.formatDate = function (date) {
        return $.datepicker.formatDate("dd/mm/yy", date);
    };


    /* UI */
    var ui = my.ui = my.ui || {};

    ui.showDialog = function (options, callback) {
        var defaults = {
            modal: true,
            title: 'SICEMED - ',
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
        var defaults = {
            message: 'Se ha producido un error desconocido.<br/> Por favor intentelo nuevamente.',
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
        this.showDialog(defaults, callback);
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
        this.showDialog(defaults, callback);
    };

    ui.showSuccess = function (options, callback) {
        var defaults = {
            message: 'La operaci&oacute;n fue realizada con &eacute;xito.',
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
        this.showDialog(defaults, callback);
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
        this.showDialog(defaults, callback);
    };

    /* Private Methods
    *********************************/
    var font_improvement = function ($selectors) {
        jQuery($selectors).each(function () {
            $size = parseInt(jQuery(this).css('fontSize'));
            jQuery(this).css('fontSize', $size * 1.4)
        });

        Cufon.replace($selectors, { fontFamily: 'geosans' });
    };


    /* Ready 
    *********************************/
    jQuery(document).ready(function () {
        // cufon font replacement
        font_improvement('h1, #featured:not(.curtain, .accordion, .newsslider) .sliderheading');
    });

    return my;

} (S || {}, jQuery));


jQuery.extend(jQuery, {
    parseJSON: function (data) {
        return JSON.parse(data, isoDateReviver);
    }
});


/* Ajax defaults
*********************************/
jQuery(document).ajaxError(function (event, xhr, ajaxOptions, thrownError) {
    jQuery.unblockUI();
    var msg = "Se ha producido el siguiente error: <br/>" + thrownError;
    S.ui.showError(msg);
});

jQuery.parseJSON = function (str) {
    return JSON.parse(str, S.helpers.evalDate);
};


/* Overwrite common window methods
*********************************/
function alert(content) {
    S.ui.showMessage(content);
}

function confirm(content) {
    S.ui.showConfirmation(content, function () { });
}

function confirm(content, callback) {
    S.ui.showConfirmation(content, callback);
}


/* jqGrid Formatters
*********************************/
jQuery.extend($.fn.fmatter, {
    dateFormatter: function (cellvalue, options, rowdata) {
        return S.helpers.formatDate(cellvalue);
    }
});

jQuery.extend($.fn.fmatter.dateFormatter, {
    unformat: function (cellvalue, options) {
        console.log(cellvalue);
        return new Date(cellvalue);
    }
});

jQuery.extend($.fn.fmatter, {
    booleanFormatter: function (cellvalue, options, rowdata) {
        return cellvalue === true ? "Si" : "No";
    }
});

jQuery.extend($.fn.fmatter.booleanFormatter, {
    unformat: function (cellvalue, options) {
        return (cellvalue === "Si").toString(); //Fix de los checkboxes
    }
});

/* jqGrid general options
*********************************/
jQuery.extend(jQuery.jgrid.defaults, {
    datatype: "json",
    jsonReader: { repeatitems: false, id: "Id" },
    postData: {
        __RequestVerificationToken: S.helpers.getAntiForgeryToken
    },
    rowNum: 5,
    pager: "#ajaxGridPager",
    width: "600px",
    height: "10.5em"
});