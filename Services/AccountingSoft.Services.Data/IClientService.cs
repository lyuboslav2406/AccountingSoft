namespace AccountingSoft.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    using AccountingSoft.Data.Models;

    public interface IClientService
    {
        void EditClient(Client c);

        Task<bool> DeleteClient(Client c);

        IEnumerable<T> GetAllClients<T>(string search = null);

        Task<System.Guid> CreateAsync(string name, string EIK, bool DDS);

        List<Client> GetListOfClients();
    }
}
