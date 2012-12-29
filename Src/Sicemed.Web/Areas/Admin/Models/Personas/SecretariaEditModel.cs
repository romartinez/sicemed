using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sicemed.Web.Infrastructure.Attributes.DataAnnotations;

namespace Sicemed.Web.Areas.Admin.Models.Personas
{
    [DisplayName("secretaria")]
    public class SecretariaEditModel
    {
        [HiddenInput(DisplayValue = false)]
        public virtual long? Id { get; set; }

        [Display(Name = "Fecha Ingreso")]
        [DataType(DataType.Date)]
        [Fecha]
        public DateTime FechaIngreso { get; set; }
    }
}