using AutoMapper;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.ViewModels;
using Sicemed.Web.Models.ViewModels.Cuenta;

namespace Sicemed.Web.Infrastructure.AutoMapper
{
	public class GenericProfile : Profile
	{
		public override string ProfileName
		{
			get { return "GenericProfile"; }
		}

		protected override void Configure()
		{
			Mapper.CreateMap<Enumeration, InfoViewModel>()
				.ForMember(d => d.Id, m => m.MapFrom(o => o.Value))
				.ForMember(d => d.Descripcion, m => m.MapFrom(o => o.DisplayName));
		    
            Mapper.CreateMap<Clinica, ClinicaViewModel>();

		    Mapper.CreateMap<RegistroPersonaViewModel, Persona>()
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.Membership, m => m.Ignore())
                .ForMember(d => d.Documento, m => m.Ignore())
                .ForMember(d => d.Domicilio, m => m.Ignore())
		        .ForMember(d => d.Roles, m => m.Ignore());
		}
	}
}