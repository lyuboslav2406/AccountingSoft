namespace AccountingSoft.Web.ViewModels.Product
{
    using AccountingSoft.Data.Models;

    using AccountingSoft.Services.Mapping;

    public class ClientDropDownViewModel : IMapFrom<Client>
    {
        public System.Guid Id { get; set; }

        public string Name { get; set; }
    }
}
