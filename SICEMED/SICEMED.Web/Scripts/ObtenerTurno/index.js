// Setup mocking

$.mockjax(function(settings) {
  var resource = settings.url.match(/\/ObtenerTurno\/(.*)$/);
  if ( resource ) {
    return {
      proxy: '/Content/mocks/ObtenerTurno/' + resource[1] + '.js'
    };
  }
  return;  
});
//---------------------------


var obtenerTurno = (function () {
	var my = {};		

	var step = function(id, name, template, model){		
		this.id = id;
		this.name = ko.observable(name);
		this.template = template;
		this.model = ko.observable(model);

    this.getTemplate = function(){
			return this.template;	
		};
	};

	var seleccionProfesionalesVm = function(){
    var self = this;
		self.especialidades = ko.observableArray([]);
		self.especialidadSeleccionada = ko.observable(null);
		self.profesionalABuscar = ko.observable('');
		
		self.profesionalesEncontrados = ko.observableArray([]);		

		this.buscarProfesionales = function(){			      
			self.profesionalesEncontrados.removeAll();      
      $.getJSON('/ObtenerTurno/BuscarProfesional').done(function(data){
        console.log(data);
        for (var i = data.length - 1; i >= 0; i--) {
          self.profesionalesEncontrados.push(data[i]);
        };
      });
		};

    $.getJSON("/ObtenerTurno/ObtenerEspecialidades").done(function(data){        
        for (var i = data.length - 1; i >= 0; i--) {
          self.especialidades.push(data[i]);
        };      
    });
	};

	var wizard = function(){
		this.stepModels = ko.observableArray([
			new step(1, 'Seleccion De Profesional', 'selectProfesional',  new seleccionProfesionalesVm())
		]);

    this.currentStep = ko.observable(this.stepModels()[0]);
    
    this.getTemplate = function(){                 
      return this.currentStep().template;
    };
	}

  my.vm = new wizard();  
	
  ko.applyBindings(my.vm);
  
	return my;
}());