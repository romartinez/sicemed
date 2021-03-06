﻿var app = (function ($, app) {
    var grid = app.grid = {};

    grid.initialize = function (options) {
        var defaults = {
            grid: '#data',
            pager: '#pager',
            width: 950,
            url: {
                edit: 'Editar',
                add: 'Nuevo',
                del: 'Eliminar',
                list: 'List'
            },
            buttons: [],
            params: {
                modalWidth: 400,
                editable: true,
                addable: true,
                deleteable: true,
                refresheable: true,
                loadComplete: null,
                rowNum: 10,
                rowList: [10, 25, 50],
                serializeGridData: function (postData) {
                    return $.param(postData);
                }
            }
        };

        var settings = $.extend(true, {}, defaults, options);

        var getGridCount = function () {
            var count = $(settings.grid).data("count");
            return count === undefined ? 0 : count;
        };

        var defaultCrudOptions = {
            modal: true,
            afterShowForm: function () {
                $("jqmOverlay").hide();
                $('body').prepend('<div class="ui-widget-overlay" id="jqgrid-overlay" style="left: 0px; top: 0px; width: 100%; height: 100%; position: fixed; z-index: 949; opacity: 0.3;"/>');
            },
            onClose: function () {
                $('#jqgrid-overlay').remove();
            },
            resize: false,
            width: settings.params.modalWidth,
            mtype: 'POST',
            closeAfterAdd: true,
            closeAfterEdit: true,
            savekey: [true, 13],
            reloadAfterSubmit: true,
            recreateForm: true,
            serializeEditData: $.appendAntiForgeryToken,
            serializeDelData: $.appendAntiForgeryToken,
            errorTextFormat: function (err) {
                return "Se produjo un error.";
            }
        };

        var getEditOptions = function () {
            var options = {
                url: settings.url.edit
            };
            return $.extend(options, defaultCrudOptions);
        };

        var getAddOptions = function () {
            var options = {
                url: settings.url.add,
                afterSubmit: function (response, postData) {
                    var grid = $(settings.grid);
                    grid.data("count", grid.data("count") + 1);
                    return [true, ""];
                }
            };
            return $.extend(options, defaultCrudOptions);
        };

        var getDeleteOptions = function () {
            var options = {
                url: settings.url.del,
                afterSubmit: function (response, postData) {
                    var grid = $(settings.grid);
                    grid.data("count", grid.data("count") - 1);
                    return [true, ""];
                }
            };
            return $.extend(options, defaultCrudOptions);
        };

        $(settings.grid).jqGrid({
            url: settings.url.list,
            datatype: 'json',
            mtype: 'POST',
            colNames: settings.params.colNames,
            colModel: settings.params.colModel,
            multiselect: false,
            postData: {
                count: getGridCount,
                __RequestVerificationToken: app.helpers.getAntiForgeryToken
            },
            serializeGridData: settings.params.serializeGridData,
            pager: settings.pager,
            rowNum: settings.params.rowNum,
            rowList: settings.params.rowList,
            viewrecords: true,
            height: 'auto',
            width: settings.width.toString(),
            jsonReader: {
                page: "Page",
                total: "Total",
                records: "Records",
                root: "Rows",
                repeatitems: false,
                id: "Id"
            },
            caption: settings.params.caption,
            emptyrecords: settings.params.emptyrecords,
            loadComplete: function (data) {
                app.fixes.gridLoadFix($(this), data);
                if ($.isFunction(settings.params.loadComplete)) settings.params.loadComplete.call(this, data);
            }
        });

        var navGrid = $(settings.grid).navGrid(
            settings.pager,
            {
                edit: settings.params.editable,
                add: settings.params.addable,
                del: settings.params.deleteable,
                search: true,
                refresh: settings.params.refresheable
            },
            getEditOptions(),
            getAddOptions(),
            getDeleteOptions(),
            {width: 685, closeOnEscape: true, multipleSearch: false }
        );

        if (settings.buttons) {
            $.each(settings.buttons, function () {
                navGrid.navButtonAdd(settings.pager, this);
            });
        }
    };

    return app;
})(jQuery, app || {});   