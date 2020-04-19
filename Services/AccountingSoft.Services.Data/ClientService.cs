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

        public async Task<System.Guid> CreateAsync(string name, string EIK, bool DDS)
        {
            var client = new Client
            {
                Name = name,
                EIK = EIK,
                DDS = DDS,
            };

            await this.clientRepository.AddAsync(client);
            await this.clientRepository.SaveChangesAsync();
            return client.Id;
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

        public IEnumerable<T> GetAllClients<T>(string search = null)
        {
            IQueryable<Client> query =
               this.clientRepository.All().OrderBy(x => x.Name);

            if (search != null)
            {
                query = query.Where(a => a.EIK.Contains(search));
            }

            return query.To<T>().ToList();
        }

        public List<Client> GetListOfClients()
        {
            var list = this.clientRepository.GetListOfClients();

            return list;
        }
    }
}
