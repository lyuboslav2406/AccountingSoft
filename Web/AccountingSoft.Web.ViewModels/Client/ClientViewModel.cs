namespace AccountingSoft.Web.ViewModels.Client
{
    using ClientModel = AccountingSoft.Data.Models.Client;

    public class ClientViewModel
    {
        public string Name { get; set; }

        public string EIK { get; set; }

        public bool DDS { get; set; }

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
