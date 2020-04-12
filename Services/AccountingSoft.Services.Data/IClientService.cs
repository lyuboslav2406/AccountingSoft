using AccountingSoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSoft.Services.Data
{
    public interface IClientService
    {
        Task<Client> AddClient(Client c);

        Task<Client> EditClient(Client c);

        Task<bool> DeleteClient(Client c);

        Task<List<Client>> ListClients(DateTime dAfter);
    }
}
