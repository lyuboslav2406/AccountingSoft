using System.Threading.Tasks;
namespace AccountingSoft.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AccountingSoft.Data.Common.Repositories;
    using AccountingSoft.Data.Models;
    using AccountingSoft.Services.Mapping;

    public class ClientService : IClientService
    {
        private readonly IDeletableEntityRepository<Client> clientRepository;

        public ClientService(IDeletableEntityRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task AddClient(Client c)
        {
            try
            {
                c.IsDeleted = false;

                await this.clientRepository.AddAsync(c);

                await this.clientRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
            }
        }

        public Task<bool> DeleteClient(Client c)
        {
            this.clientRepository.Delete(c);

            this.clientRepository.SaveChangesAsync();

            return null;
        }

        public void EditClient(Client c)
        {
            this.clientRepository.Update(c);

            this.clientRepository.SaveChangesAsync();
        }

        public IEnumerable<T> GetAllClients<T>()
        {
            var clients = this.clientRepository
                .All()
                .OrderByDescending(x => x.CreatedOn);

            var c = clients.To<T>();

            return c;
        }
    }
}
