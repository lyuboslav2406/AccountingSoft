namespace AccountingSoft.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using AccountingSoft.Data.Models;

    public interface IClientService
    {
        Task AddClient(Client c);

        void EditClient(Client c);

        Task<bool> DeleteClient(Client c);

        IEnumerable<T> GetAllClients<T>(string search = null, int? take = null, int skip = 0);

        Task<System.Guid> CreateAsync(string name, string EIK, bool DDS);

        List<Client> GetListOfClients();

        Task<Client> GetSignleClient(Guid client);

        int GetCount();
    }
}
