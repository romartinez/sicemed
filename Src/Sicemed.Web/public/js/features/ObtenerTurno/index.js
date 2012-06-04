var model = (function () {

    var ViewModel = function () {
        var self = this;

        //#region Wizard
        var step = function (id, name, template, initFunction) {
            this.id = id;
            this.name = ko.observable(name);
            this.template = template;

            this.getTemplate = function () {
                return this.template;
            };

            this.init = initFunction || function () { };
        };

        var stepSeleccionProfesional = new step(1, 'Seleccion De Profesional', 'selectProfesional', function () {
            //Inicializar Especialidades
            $.getJSON("/ObtenerTurno/ObtenerEspecialidades").done(function (data) {
                for (var i = data.length - 1; i >= 0; i--) {
                    self.especialidades.push(data[i]);
                }
            });
        });

        var stepSeleccionTurno = new step(2, 'Seleccion De Turno', 'selectTurno', function () {
            $('#calendar').show();
            //Incializar agenda
            $.getJSON("/ObtenerTurno/ObtenerAgendaProfesional",
                {
                    profesionalId: self.profesionalSeleccionado().Id
                }
            ).done(function (data) {
                var events = [];
                var calendar = $("#calendar");
                calendar.fullCalendar('removeEvents');
                for (var i = data.length - 1; i >= 0; i--) {
                    var turno = data[i];
                    turno.reservarTurno = function () {
                        self.turnoSeleccionado(turno);
                        self.currentStep(stepComprobante);
                    };
                    self.turnosDisponibles.push(turno);
                    var event = {
                        title: (turno.Paciente == null).toString(),
                        start: turno.FechaTurnoInicial,
                        end: turno.FechaTurnoFinal,
                        allDay: false,
                        description: turno.Consultorio.Nombre,
                        color: turno.Paciente ? '#F00' : null,
                        libre: !turno.Paciente,
                        turno: turno
                    };
                    console.log(event);
                    events.push(event);
                }
                calendar.fullCalendar('addEventSource', events);
            });
        });

        var stepComprobante = new step(3, 'Comprobante', 'selectComprobante', function () {
            $('#calendar').hide();
        });

        self.stepModels = ko.observableArray([
                stepSeleccionProfesional,
                stepSeleccionTurno,
                stepComprobante
            ]);

        self.currentStep = ko.observable(stepSeleccionProfesional);

        self.getClass = function (stepToCheck) {
            if (self.currentStep().id == stepToCheck.id) return 'active-step';
            if (self.currentStep().id > stepToCheck.id) return 'completed-step';
            return '';
        };

        self.changeStep = function (stepToChange) {
            //TODO: No se si lo voy a implementar.
            //            if (self.currentStep().id > stepToChange.id) {
            //                $('#calendar').hide();
            //                self.currentStep(stepToChange);
            //            } 
        };

        self.getTemplate = function () {
            return self.currentStep().template;
        };

        //#endregion

        //#region Propiedades
        self.especialidades = ko.observableArray([]);
        self.profesionalesEncontrados = ko.observableArray([]);
        self.turnosDisponibles = ko.observableArray([]);
        self.profesionalABuscar = ko.observable('');

        self.especialidadSeleccionada = ko.observable(null);
        self.especialidadAgendaSeleccionada = ko.observable(null);
        self.profesionalSeleccionado = ko.observable(null);
        self.turnoSeleccionado = ko.observable(null);
        self.turnoAsignado = ko.observable(null);
        //#endregion

        //#region Metodos
        self.buscarProfesionales = function () {
            self.profesionalesEncontrados.removeAll();
            $.getJSON('/ObtenerTurno/BuscarProfesional',
                {
                    especialidadId: self.especialidadSeleccionada() ? self.especialidadSeleccionada().Id : null,
                    nombre: self.profesionalABuscar()
                }
            ).done(function (data) {
                for (var i = data.length - 1; i >= 0; i--) {
                    //Agrego los metodos del firmante
                    var profesional = data[i];
                    profesional.reservarTurnoLibre = function () {
                        self.profesionalSeleccionado(profesional);
                        self.turnoSeleccionado(profesional.ProximoTurnoLibre);
                        self.currentStep(stepComprobante);
                    };
                    profesional.elegirTurnoEnAgenda = function () {
                        self.profesionalSeleccionado(profesional);
                        self.currentStep(stepSeleccionTurno);
                    };

                    self.profesionalesEncontrados.push(profesional);
                };
            });
        };

        self.reservar = function () {
            $.post('/ObtenerTurno/ReservarTurno', {
                especialidadId: self.especialidadAgendaSeleccionada() ?
                                    self.especialidadAgendaSeleccionada().Id
                                    : self.especialidadSeleccionada().Id,
                profesionalId: self.profesionalSeleccionado().Id,
                fecha: app.format.fulldate(self.turnoSeleccionado().FechaTurnoInicial),
                consultorioId: self.turnoSeleccionado().Consultorio.Id,
                agendaId: self.turnoSeleccionado().Agenda.Id
            }).done(function (d) {
                self.turnoAsignado(d);
            });
        };

        self.imprimir = function () {
            window.open("/ObtenerTurno/ImprimirComprobante?turnoId=" + self.turnoAsignado().Id);
        };
        //#endregion

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
            eventClick: function (calEvent, jsEvent, view) {
                if (calEvent.libre) {
                    self.turnoSeleccionado(calEvent.turno);
                    self.currentStep(stepComprobante);
                }
            },
            eventMouseout: function (event, jsEvent, view) {
                $(this).css('cursor', 'auto');
            },
            eventMouseover: function (event, jsEvent, view) {
                if (event.libre) $(this).css('cursor', 'pointer');
            },
            events: []
        }).hide();
    };

    var vm = new ViewModel();
    ko.applyBindings(vm);

    return vm;
} ());