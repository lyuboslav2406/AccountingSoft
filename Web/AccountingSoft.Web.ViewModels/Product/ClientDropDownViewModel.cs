namespace AccountingSoft.Web.ViewModels.Product
{
    using AccountingSoft.Data.Models;

    using AccountingSoft.Services.Mapping;
    using System.ComponentModel.DataAnnotations;

    public class ClientDropDownViewModel : IMapFrom<Client>
    {
        public System.Guid Id { get; set; }

        [MaxLength(50)]
        [MinLength(3)]
        public string Name { get; set; }

        public bool DDS { get; set; }
    }
}
