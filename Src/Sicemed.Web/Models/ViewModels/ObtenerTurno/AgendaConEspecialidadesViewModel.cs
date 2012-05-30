using System;
using System.Collections.Generic;

namespace Sicemed.Web.Models.ViewModels.ObtenerTurno
{
    [Serializable]
    public class AgendaConEspecialidadesViewModel : InfoViewModel
    {
        public List<InfoViewModel> EspecialidadesAtendidas { get; set; }
    }
}