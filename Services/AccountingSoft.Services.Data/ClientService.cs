using AccountingSoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSoft.Services.Data
{
    public class ClientService : IClientService
    {
        public Task<Client> AddClient(Client c)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteClient(Client c)
        {
            throw new NotImplementedException();
        }

        public Task<Client> EditClient(Client c)
        {
            throw new NotImplementedException();
        }

        public Task<List<Client>> ListClients(DateTime dAfter)
        {
            throw new NotImplementedException();
        }
    }
}
