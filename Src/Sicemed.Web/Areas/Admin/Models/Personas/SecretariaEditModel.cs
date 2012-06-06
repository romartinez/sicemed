using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    [DisplayName("secretaria")]
    public class SecretariaEditModel
    {
        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime FechaNacimiento { get; set; }
    }
}