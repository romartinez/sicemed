var app = (function ($, app) {
    var grid = app.grid = {};

    grid.initialize = function (options) {

        $("#data", {
            url: options.url.list,
            datatype: 'json',
            mtype: 'POST',
            colNames: ['Id', 'Nombre', 'Nombre Corto', 'Facultades', 'Requiere Cuenta?'],
            colModel: [
                        { name: 'IdE', hidden: true },
                        { name: 'Descripcion', width: 230, resizable: false, align: 'left', sortable: false, editable: true, edittype: 'text', editoptions: { maxlength: '50', size: '50' }, editrules: { required: true }, formoptions: { label: 'Nombre (*)'} },
                        { name: 'DescripcionCorta', width: 80, resizable: false, align: 'center', sortable: false, editable: true, edittype: 'text', editoptions: { maxlength: '10', size: '10' }, formoptions: { label: 'Nombre Corto'} },
                        { name: 'Facultades', width: 310, resizable: false, align: 'left', sortable: false,
                            formatter: function (cellvalue, o, rowObject) {
                                var facultades = [];
                                if (rowObject.Facultades) {
                                    facultades = $.map(rowObject.Facultades, function (item, i) { return item.Descripcion; });
                                }
                                if (facultades.length > 0) return facultades.join(', ');
                                return '&nbsp;';
                            }
                        },
                        { name: 'RequiereCuenta', width: 110, resizable: false, align: 'center', sortable: false, editable: true, edittype: 'checkbox', editoptions: { value: 'true:false' }, formoptions: { label: 'Requiere Cuenta?' }, formatter: 'booleanFormatter' }
                    ],
            multiselect: false,
            postData: {
                count: getGridCount,
                __RequestVerificationToken: app.getAntiForgeryToken
            },
            pager: '#pager',
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
                id: "IdE"
            },
            caption: 'Gesti&oacute;n De Productos',
            emptyrecords: 'No hay Productos cargados.',
            loadComplete: function (data) {
                app.fixes.gridLoadFix($(this), data);
            }
        });

        $("#data").navGrid('#pager', { edit: false, add: false, del: false, search: false, refresh: false },
                null,
                null,
                null,
                {}, //search
                {closeOnEscape: true }
                );

    };

    var getGridCount = function () {
        var count = $("#data").data("count");
        return count === undefined ? 0 : count;
    };
    
    var getDefaultCrudOptions = function () {
        var options = {
            modal: true,
            resize: false,
            width: 400,
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
        return options;
    };

    return app;
})(jQuery, app || {});   