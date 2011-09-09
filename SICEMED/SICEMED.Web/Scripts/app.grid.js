var app = (function ($, app) {
    var grid = app.grid = {};

    grid.initialize = function (options) {
        var defaults = {
            grid: '#data',
            pager: '#pager',
            url: {
                edit: 'Editar',
                add: 'Nuevo',
                del: 'Eliminar',
                list: 'List'
            },
            params: {
                modalWidth: 600,
                editable: true,
                addable: true,
                deleteable: true,
                refresheable: true
            }
        };

        var settings = $.extend(true, {}, defaults, options);

        var getGridCount = function () {
            var count = $(settings.grid).data("count");
            return count === undefined ? 0 : count;
        };

        var defaultCrudOptions = {
            modal: true,
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
            errorTextFormat: function(err) {
                return "Se produjo un error.";
            }
        };

        var getEditOptions = function(){
            var options = {
                url: settings.url.edit
            };
            return $.extend(options, defaultCrudOptions);                
        };

        var getAddOptions = function(){
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

        var getDeleteOptions = function(){
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
            pager: settings.pager,
            rowNum: 10,
            rowList: [10, 25, 50],
            viewrecords: true,
            height: 'auto',
            width: '730',
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
            }                
        });

        $(settings.grid).navGrid(
            settings.pager, 
            { 
                edit: settings.params.editable,
                add: settings.params.addable, 
                del: settings.params.deleteable, 
                search: false, 
                refresh: settings.params.refresheable 
            },
            getEditOptions(),
            getAddOptions(),
            getDeleteOptions(),
            {}, //search
            {closeOnEscape: true }
        );                    
    }

    return app;
})(jQuery, app || {});   