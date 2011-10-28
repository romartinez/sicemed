// Setup mocking
$.mockjax(function (settings) {
    var resource = settings.url.match(/\/ObtenerTurno\/(.*)$/);
    if (resource) {
        return {
            proxy: '/Content/mocks/ObtenerTurno/' + resource[1] + '.js',
            cache: false
        };
    }
    return;
});
//---------------------------


var obtenerTurno = (function () {
    var my = {};

    var step = function (id, name, template, model) {
        this.id = id;
        this.name = ko.observable(name);
        this.template = template;
        this.model = ko.observable(model);

        this.getTemplate = function () {
            return this.template;
        };
    };

    // Expone dos eventos:
    // * reservarTurnoLibre(firmante)
    // * elegirTurnoEnAgenda(firmante)
    var seleccionProfesionalesVm = function () {
        var self = this;
        self.especialidades = ko.observableArray([]);
        self.especialidadSeleccionada = ko.observable(null);
        self.profesionalABuscar = ko.observable('');

        self.profesionalesEncontrados = ko.observableArray([]);
        
        self.profesional

        self.buscarProfesionales = function () {
            self.profesionalesEncontrados.removeAll();
            $.getJSON('/ObtenerTurno/BuscarProfesional').done(function (data) {
                for (var i = data.length - 1; i >= 0; i--) {
                    //Agrego los metodos del firmante
                    var profesional = data[i];
                    profesional.reservarTurnoLibre = function () {
                        $(self).trigger('reservarTurnoLibre', profesional);
                    };
                    profesional.elegirTurnoEnAgenda = function () {
                        $(self).trigger('elegirTurnoEnAgenda', profesional);
                    };

                    self.profesionalesEncontrados.push(profesional);
                };
            });
        };

        //Inicializar Especialidades
        $.getJSON("/ObtenerTurno/ObtenerEspecialidades").done(function (data) {
            for (var i = data.length - 1; i >= 0; i--) {
                self.especialidades.push(data[i]);
            };
        });
    };

    var stepSeleccionProfesional = new step(1, 'Seleccion De Profesional', 'selectProfesional', new seleccionProfesionalesVm());

    var wizard = function () {
        var self = this;
        self.stepModels = ko.observableArray([
                stepSeleccionProfesional
            ]);

        self.currentStep = ko.observable(self.stepModels()[0]);

        self.getTemplate = function () {
            return self.currentStep().template;
        };

        $(stepSeleccionProfesional).bind('reservarTurnoLibre', self.reservarTurnoLibre);
        $(stepSeleccionProfesional).bind('elegirTurnoEnAgenda', self.elegirTurnoEnAgenda);

        self.reservarTurnoLibre = function (firmante) {
            console.log(firmante);
        };
        self.elegirTurnoEnAgenda = function (firmante) {
            console.log(firmante);
        };
    };

    my.vm = new wizard();

    ko.applyBindings(my.vm);

    return my;
} ());