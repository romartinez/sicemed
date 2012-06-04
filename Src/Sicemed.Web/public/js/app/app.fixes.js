var app = (function ($, app) {

    // Fixes **********************************************************************    
    var fixes = app.fixes = {};

    fixes.gridCount = function (grid) {
        grid.trigger("reloadGrid");
        var pager = $(grid.getGridParam('pager'));
        var center = pager.find("#pager_center");
        if (grid.getGridParam('reccount') === 0) {
            if (center.find(".no-text").size() === 0) {
                center.append($("<div style='display: none;' class='no-text'>" + grid.getGridParam('emptyrecords') + "</div>"));
            }

            pager.removeClass('ui-state-default')
                .addClass('ui-state-highlight');

            pager.find("#pager_right .ui-paging-info").html("");

            center.find("table").hide()
                .parent().find(".no-text").show();
        } else {
            pager
                .addClass('ui-state-default')
                .removeClass('ui-state-highlight');

            center
                .find(".no-text").hide()
                .parent().find("table").show();
        }
    };
    fixes.gridLoadFix = function (grid, data) {
        if (!grid.data("count")) {
            grid.data("count", data.Records);
        }
        //Recargo la grilla (página actual)
        if (data.Rows.length === 0 && data.Page !== 1) {
            //No hay records así que vamos a la página anterior
            grid.get(0).p.page = grid.get(0).p.page > 1 ? grid.get(0).p.page - 1 : 1;
            setTimeout(function () { grid.get(0).grid.populate(); }, 500);
        }
    };

    // Helpers **********************************************************************    
    var helpers = app.helpers = {};
    helpers.getAntiForgeryToken = function () {
        return $.getAntiForgeryToken().value;
    };
    helpers.evalDate = function (date) {
        var epoch = (new RegExp('/Date\\((-?[0-9]+)\\)/')).exec(date);
        return new Date(parseInt(epoch[1]));
    };

    // Formatters **********************************************************************    
    var format = app.format = {};
    format.date = function (date) {
        return $.fullCalendar.formatDate(date, "dd/MM/yy");
    };
    format.fulldate = function (date) {
        return $.fullCalendar.formatDate(date, "dd/MM/yy HH:mm");
    };
    format.hour = function (date) {
        return $.fullCalendar.formatDate(date, "HH:mm");
    };


    // Validators **********************************************************************        
    var validators = app.validators = {};
    validators.requiredIfEnabled = function (value, element) {
        if (element.disabled)
            return true;
        return $.validator.methods.required.call(this, $.trim(element.value), element);
    };
    validators.datepickerIsDate = function (value, element) {
        try { $.datepicker.parseDate('dd/mm/yy', value); return true; } catch (error) { return false; }
    };

    return app;
})(jQuery, app || {});