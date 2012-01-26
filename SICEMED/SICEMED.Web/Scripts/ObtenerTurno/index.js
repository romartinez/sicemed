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

var holder = {};

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
    // * reservarTurnoLibre(profesional)
    // * elegirTurnoEnAgenda(profesional)
    var seleccionProfesionalesVm = function () {
        var self = this;
        self.especialidades = ko.observableArray([]);
        self.especialidadSeleccionada = ko.observable(null);
        self.profesionalABuscar = ko.observable('');

        self.profesionalesEncontrados = ko.observableArray([]);

        self.buscarProfesionales = function () {
            self.profesionalesEncontrados.removeAll();
            $.getJSON('/ObtenerTurno/BuscarProfesional').done(function (data) {
                for (var i = data.length - 1; i >= 0; i--) {
                    //Agrego los metodos del firmante
                    var profesional = data[i];
                    profesional.reservarTurnoLibre = function () {
                        $(self).trigger('reservarTurnoLibre', [profesional]);
                    };
                    profesional.elegirTurnoEnAgenda = function () {
                        $(self).trigger('elegirTurnoEnAgenda', [profesional]);
                    };

                    self.profesionalesEncontrados.push(profesional);
                };
            });
        };

        self.init = function () {
            //Inicializar Especialidades
            $.getJSON("/ObtenerTurno/ObtenerEspecialidades").done(function (data) {
                for (var i = data.length - 1; i >= 0; i--) {
                    self.especialidades.push(data[i]);
                }
                ;
            });
        };
    };

    // Expone un evento
    // * reservarTurno(turno)
    var seleccionTurnoVm = function () {
        var self = this;

        self.turnosDisponibles = ko.observableArray([]);

        self.init = function () {
            //Incializar agenda
            $.getJSON("/ObtenerTurno/ObtenerAgendaProfesional").done(function (data) {
                for (var i = data.length - 1; i >= 0; i--) {
                    var turno = data[i];
                    turno.reservarTurno = function () {
                        $(self).trigger('reservarTurno', [turno]);
                    };
                    self.turnosDisponibles.push(turno);
                    var event = {
                        title: 'Reservar...',
                        start: turno.FechaTurnoInicial,
                        end: turno.FechaTurnoFinal,
                        description: turno.Consultorio.Nombre
                    };
                    $("#calendar").fullCalendar('addEvent', event);
                    $("#calendar").fullCalendar('renderEvent', event);
                }
            });
        };
    };


    var comprobanteVm = function () {
        var self = this;

        self.init = function () {

        };

        self.imprimir = function () {
            console.log("Imprimir");
        };
    };

    var stepSeleccionProfesional = new step(1, 'Seleccion De Profesional', 'selectProfesional', new seleccionProfesionalesVm());
    var stepSeleccionTurno = new step(2, 'Seleccion De Turno', 'selectTurno', new seleccionTurnoVm());
    var stepComprobante = new step(3, 'Comprobante', 'selectComprobante', new comprobanteVm());

    var wizard = function () {
        var self = this;
        self.stepModels = ko.observableArray([
                stepSeleccionProfesional,
                stepSeleccionTurno,
                stepComprobante
            ]);

        self.currentStep = ko.observable(self.stepModels()[0]);

        self.profesionalSeleccionado = ko.observable(null);
        self.turnoSeleccionado = ko.observable(null);

        self.getTemplate = function () {
            return self.currentStep().template;
        };

        self.reservarTurnoLibre = function (e, profesional) {
            self.profesionalSeleccionado(profesional);
            self.turnoSeleccionado(profesional.ProximoTurno);
            self.currentStep(self.stepModels()[2]);
        };
        self.elegirTurnoEnAgenda = function (e, profesional) {
            self.profesionalSeleccionado(profesional);
            self.currentStep(self.stepModels()[1]);
            $('#calendar').show();
        };
        self.reservarTurno = function (e, turno) {
            self.turnoSeleccionado(turno);
            self.currentStep(self.stepModels()[2]);
        };

        $(stepSeleccionProfesional.model()).bind('reservarTurnoLibre', self.reservarTurnoLibre);
        $(stepSeleccionProfesional.model()).bind('elegirTurnoEnAgenda', self.elegirTurnoEnAgenda);

        $(stepSeleccionTurno.model()).bind('reservarTurno', self.reservarTurno);

        //Render del calendar
        $('#calendar').fullCalendar({
            events: []
        }).hide();
    };

    my.vm = new wizard();
    ko.applyBindings(my.vm);

    return my;
} ());