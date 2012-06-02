using System.ComponentModel.DataAnnotations;

namespace Sicemed.Web.Infrastructure.Attributes.DataAnnotations
{
	public class DefaultStringLengthAttribute : StringLengthAttribute
	{
		public const int DEFAULT_DB_LENGTH = 255;
		public DefaultStringLengthAttribute()
			: base(DEFAULT_DB_LENGTH){}
	}
}