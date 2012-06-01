using AutoMapper;
using Sicemed.Web.Areas.Admin.Models.Clinicas;
using Sicemed.Web.Models;

namespace Sicemed.Web.Infrastructure.AutoMapper
{
    public class AdminProfile : Profile
    {
        public override string ProfileName
        {
            get { return "AdminProfile"; }
        }

        protected override void Configure()
        {
            Mapper.CreateMap<ClinicaEditViewModel, Clinica>()
                .ForAllMembers(x => x.Ignore());
            Mapper.CreateMap<Clinica, ClinicaEditViewModel>()
                .ForAllMembers(x => x.Ignore());
        }
    }
}