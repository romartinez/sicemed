using log4net.Appender;

namespace Sicemed.Web.Infrastructure.Logs
{
    public class ConfigAdoNetAppender : AdoNetAppender
    {
        public string ConnectionStringName
        {
            set
            {
                this.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings[value].ToString();
            }
        }
    }
}