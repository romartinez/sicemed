using System.Linq;
using AutoMapper;
using Castle.Core;
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
            Mapper.CreateMap<Clinica, ClinicaEditViewModel>()                
                .ForMember(d => d.TiposDocumentosHabilitados, m => m.Ignore())
                .ForMember(d => d.LocalidadesHabilitadas, m => m.Ignore())
                .ForMember(d => d.ProvinciasHabilitadas, m => m.Ignore());

            Mapper.CreateMap<ClinicaEditViewModel, Clinica>()
                .ForMember(d => d.Telefonos, m => m.Ignore())
                .ForMember(d => d.Domicilio, m => m.Ignore())
                .ForMember(d => d.ObrasSocialesAtendidas, m => m.Ignore())
                .ForMember(d => d.Pacientes, m => m.Ignore())
                .ForMember(d => d.Profesionales, m => m.Ignore())
                .ForMember(d => d.Secretarias, m => m.Ignore())
                .ForMember(d => d.Partners, m => m.Ignore())
                .ForMember(d => d.Id, m => m.Ignore())
                .ForMember(d => d.Documento, m => m.Ignore())
                .AfterMap((v, m) =>
                              {
                                  //Note: m.Telefonos.Clear(); no funciona!
                                  var telefonosAQuitar = m.Telefonos.ToList();
                                  telefonosAQuitar.ForEach(t => m.QuitarTelefono(t));
                                  v.Telefonos.ForEach(t => m.AgregarTelefono(t));
                              });
        }
    }
}