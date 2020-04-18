namespace AccountingSoft.Web.ViewModels.Client
{
    using AccountingSoft.Services.Mapping;
    using AutoMapper;
    using ClientModel = AccountingSoft.Data.Models.Client;

    public class ClientViewModel : IMapFrom<ClientModel>, IMapTo<ClientModel>, IHaveCustomMappings
    {
        public string Name { get; set; }

        public string EIK { get; set; }

        public bool DDS { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ClientModel, ClientViewModel>();
        }

        public ClientModel ToClient(ClientViewModel cl)
        {
            ClientModel clientModel = new ClientModel()
            {
               DDS = this.DDS,
               EIK = this.EIK,
               Name = this.Name,
            };
            return clientModel;
        }

    }
}
