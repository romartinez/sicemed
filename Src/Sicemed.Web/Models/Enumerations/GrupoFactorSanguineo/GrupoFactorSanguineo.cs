namespace Sicemed.Web.Models.Enumerations.GrupoFactorSanguineo
{
    public class GrupoFactorSanguineo : Enumeration
    {
        public static readonly GrupoFactorSanguineo  P0 = new CeroPositivo();
        public static readonly GrupoFactorSanguineo  N0 = new CeroNegativo();
        public static readonly GrupoFactorSanguineo  PA = new APositivo();
        public static readonly GrupoFactorSanguineo  NA = new ANegativo();
        public static readonly GrupoFactorSanguineo  PB = new BPositivo();
        public static readonly GrupoFactorSanguineo  NB = new BNegativo();
        public static readonly GrupoFactorSanguineo  PAB = new ABPositivo();
        public static readonly GrupoFactorSanguineo  NAB = new ABNegativo();

        public static readonly GrupoFactorSanguineo[] GrupoFactorSanguineo = new[] {P0,N0,PA,NA,PB,NB,PAB,NAB};

        public GrupoFactorSanguineo() {}
        public GrupoFactorSanguineo(long value, string displayName) : base(value, displayName) {}
    }
}