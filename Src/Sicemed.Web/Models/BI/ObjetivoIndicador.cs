namespace Sicemed.Web.Models.BI
{
    public class ObjetivoIndicador : Entity
    {
        public virtual Indicador Indicador { get; set; }
        public virtual double Objetivo { get; set; }        
    }
}