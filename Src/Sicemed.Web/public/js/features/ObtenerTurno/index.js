var model = (function () {

    var ViewModel = function () {
        var self = this;

        var attachTooltips = function () {
            $(".hasTooltip[title]")
                .tooltip({ effect: 'slide', offset: [10, 2], position: 'bottom rigth' })
                .on("mouseover", function () {
                    $(this).addClass("event-hover");
                })
                .on("mouseout", function () {
                    $(this).removeClass("event-hover");
                });
        };

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

        var stepSeleccionProfesional = new step(1, 'Seleccion De Profesional', 'stepSeleccionProfesional', function () {
            //Inicializar Especialidades
            $.getJSON("/ObtenerTurno/ObtenerEspecialidades").done(function (data) {
                for (var i = data.length - 1; i >= 0; i--) {
                    self.especialidades.push(data[i]);
                }
            });
        });

        var stepSeleccionTurno = new step(2, 'Seleccion De Turno', 'stepSeleccionTurno', function () {
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
                        self.currentStep(stepConfirmar);
                    };
                    self.turnosDisponibles.push(turno);
                    var especialidadesAtendidasTurno = $.map(turno.EspecialidadesAtendidas, function (especialidad) {
                        return especialidad.Descripcion;
                    }).join(",");
                    var description = "<div class='turno-libre'>"
                        + "<span class='text-blue'>Fecha Turno:</span><span>" + app.format.date(turno.FechaTurnoInicial) + "</span><br/>"
                        + "<span class='text-blue'>Horario:</span><span>" + app.format.hour(turno.FechaTurnoInicial) + " / " + app.format.hour(turno.FechaTurnoFinal) + "</span><br/>"
                        + "<span class='text-blue'>Duracion:</span><span>" + turno.DuracionTurno +  "</span><br/>"
                        + "<span class='text-blue'>Consultorio:</span><span>" + turno.Consultorio.Descripcion + "</span><br/>"
                        + "<span class='text-blue'>Especialidades:</span><span>" + especialidadesAtendidasTurno + "</span>"
                        + "</div>";
                    var event = {
                        title: turno.Paciente ? turno.Paciente.Descripcion : "Reservar...",
                        start: turno.FechaTurnoInicial,
                        end: turno.FechaTurnoFinal,
                        duracion:turno.DuracionTurno,
                        allDay: false,
                        description: turno.Paciente ? '' : description,
                        color: turno.Paciente ? '#F00' : null,
                        libre: !turno.Paciente,
                        turno: turno
                    };
                    events.push(event);
                }
                calendar.fullCalendar('addEventSource', events);
                //Tooltip
                attachTooltips();
            });
        });

        var stepConfirmar = new step(3, 'Reservar', 'stepConfirmar', function () {
            $('#calendar').hide();
            //Si no tiene especialidad elegida y,
            //si la agenda del turno seleccionado tiene una sola especialidad se la asigno
            if (!self.especialidadSeleccionada() && self.turnoSeleccionado().EspecialidadesAtendidas.length === 1) {
                self.especialidadSeleccionada(self.turnoSeleccionado().EspecialidadesAtendidas[0]);
            }
        });

        var stepComprobante = new step(4, 'Comprobante', 'stepComprobante', function () { });

        self.stepModels = ko.observableArray([
                stepSeleccionProfesional,
                stepSeleccionTurno,
                stepConfirmar,
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
                        self.profesionalSeleccionado(this);
                        self.turnoSeleccionado(this.ProximoTurnoLibre);
                        self.currentStep(stepConfirmar);
                    };
                    profesional.elegirTurnoEnAgenda = function () {
                        self.profesionalSeleccionado(this);
                        self.currentStep(stepSeleccionTurno);
                    };
                    self.profesionalesEncontrados.push(profesional);
                };
            });
        };

        self.reservar = function () {
            $.post('/ObtenerTurno/ReservarTurno', {
                especialidadId: self.especialidadAgendaSeleccionada() ? self.especialidadAgendaSeleccionada().Id : self.especialidadSeleccionada().Id,
                profesionalId: self.profesionalSeleccionado().Id,
                fecha: app.format.fulldate(self.turnoSeleccionado().FechaTurnoInicial),
                duracion: self.turnoSeleccionado().DuracionTurno,
                consultorioId: self.turnoSeleccionado().Consultorio.Id
             }).done(function (d) {
                self.turnoAsignado(d);
                self.currentStep(stepComprobante);
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
                    self.currentStep(stepConfirmar);
                }
            },
            eventMouseout: function (event, jsEvent, view) {
                $(this).css('cursor', 'auto');
            },
            eventMouseover: function (event, jsEvent, view) {
                if (event.libre) $(this).css('cursor', 'pointer');
            },
            viewDisplay: function () {
                attachTooltips();
            },
            events: []
        }).hide();
    };

    var vm = new ViewModel();
    ko.applyBindings(vm);

    return vm;
} ());
