//Console overrides
if (!window.console) console = {};
console.log = console.log || function(){};
console.warn = console.warn || function(){};
console.error = console.error || function(){};
console.info = console.info || function(){};

//overwrite common methods
function alert(content) {
    app.ui.showMessage(content);
}

function confirm(content) {
    app.ui.showConfirmation(content, function () { });
}

function confirm(content, callback) {
    app.ui.showConfirmation(content, callback);
}

//JqGrid Formatters
jQuery.extend($.fn.fmatter, {
    dateFormatter: function (cellvalue, options, rowdata) {
        return app.helpers.evalDate(rowdata).toDateString();
    }
});

jQuery.extend($.fn.fmatter, {
    documento: function (cellvalue, options, rowdata) {
        switch (cellvalue) {
            case 3:
                return "DNI";
            default:
                return "N/A";
        }
    }
});

jQuery.extend($.fn.fmatter.dateFormatter, {
    unformat: function (cellvalue, options) {
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


//Fix validation
$.validator.methods.range = function (value, element, param) {
    var globalizedValue = value.replace(",", ".");
    return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
};

$.validator.methods.number = function (value, element) {
    return this.optional(element) || /^-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
};

$(document).ready(function () {
    $.validator.addMethod("requiredIfEnabled", app.validators.requiredIfEnabled, "El campo es requerido.");
    $.validator.addMethod("dateddMMyyyy", app.validators.datepickerIsDate, 'Por favor, ingrese una fecha válida');
});