using System.Collections.Generic;
using System.Threading.Tasks;
using ClientModel = AccountingSoft.Data.Models.Client;

namespace AccountingSoft.Web.ViewModels
{
    public class LayoutViewModel
    {
        public List<ClientModel> Clients { get; set; }
    }
}
