using System;

namespace Sicemed.Web.Infrastructure.Reports
{
    public class ReportInfo
    {
        public ReportInfo(string key, string title, string description, Func<object> getDataSource)
        {
            Key = key;
            Title = title;
            Description = description;
            GetDataSources = getDataSource;
            NameDataSource = key;
        }

        public string Key { get; private set; }

        public string Title { get; private set; }

        public string Description { get; private set; }

        public object DataSource
        {
            get
            {
                return GetDataSources.Invoke();
            }
        }

        public string NameDataSource { get; set; }

        private Func<object> GetDataSources { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (obj.GetType() == typeof(ReportInfo))
                return ((ReportInfo)obj).Key == this.Key;

            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }         
    }
}