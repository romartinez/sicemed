using System;
using System.Collections.Generic;
using System.Linq;

namespace Sicemed.Web.Models.ViewModels.Menu
{
    [Serializable]
    public class PageViewModel
    {
        public PageViewModel()
        {
            Childs = new List<PageViewModel>();
        }

        public string Name { get; set; }
        public string Url { get; set; }
        public PageViewModel Parent { get; set; }
        public List<PageViewModel> Childs { get; set; }
        public bool IsParent { get { return Childs.Any(); } }
        public bool IsLast { get; set; }
        public bool IsFirst { get; set; }
        public bool IsCurrent { get; set; }
        public bool IsCurrentItem { get; set; }
        public int Order { get; set; }
    }
}