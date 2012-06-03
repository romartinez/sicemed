using AutoMapper;
using Sicemed.Web.Models;
using Sicemed.Web.Models.Enumerations;
using Sicemed.Web.Models.ViewModels;

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
		}
	}
}