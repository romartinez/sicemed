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
                var events = [];
                var calendar = $("#calendar");
                calendar.fullCalendar('removeEvents');
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
                        allDay: false,
                        description: turno.Consultorio.Nombre
                    };
                    events.push(event);
                }
                calendar.fullCalendar('addEventSource', events);
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
            theme: true,
            header: {
                left: 'title',
                center: '',
                right: 'today prev,next'
            },
            weekends: false,
            defaultView: 'agendaWeek',
            allDaySlot: false,
            minTime: '7:00', //Horario apertura clinica
            maxTime: '20:00', //Horario cierre clinica
            slotMinutes: 15, //Duracion por defecto turno
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'],
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mier', 'Jue', 'Vie', 'Sab'],
            monthNames: ['Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio', 'Julio',
                        'Agosto', 'Septiembre', 'Ocubtre', 'Noviembre', 'Diciembre'],
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                        'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dec'],
            buttonText: {
                today: 'hoy',
                month: 'mes',
                week: 'semana',
                day: 'dia'
            },
            columnFormat: {
                month: 'ddd',
                week: 'ddd d/M',
                day: 'dddd d/M'
            },
            events: []
        }).hide();
    };

    my.vm = new wizard();
    ko.applyBindings(my.vm);

    return my;
} ());