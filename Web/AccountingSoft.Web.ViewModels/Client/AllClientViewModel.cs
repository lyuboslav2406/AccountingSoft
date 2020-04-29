using System;
using System.Collections.Generic;
using System.Text;

namespace AccountingSoft.Web.ViewModels.Client
{
    public class AllClientViewModel
    {
        public IEnumerable<ClientViewModel> Clients { get; set; }

        public string Name { get; set; }

        public string EIK { get; set; }

        public bool DDS { get; set; }

        public int PagesCount { get; set; }

        public int CurrentPage { get; set; }
    }
}
