using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    [DisplayName("secretaria")]
    public class SecretariaEditModel
    {
        [HiddenInput]
        public virtual long? Id { get; set; }

        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        public DateTime FechaIngreso { get; set; }
    }
}