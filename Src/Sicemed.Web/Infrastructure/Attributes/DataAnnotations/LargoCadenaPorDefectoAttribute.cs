namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
	public class LargoCadenaPorDefectoAttribute : LargoCadenaAttribute
	{
		public const int DEFAULT_DB_LENGTH = 255;
		public LargoCadenaPorDefectoAttribute()
			: base(DEFAULT_DB_LENGTH){}
	}
}