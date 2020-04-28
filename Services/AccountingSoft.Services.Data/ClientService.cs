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
        private readonly IProductService productService;

        public ClientService(IDeletableEntityRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task AddClient(Client c)
        {
            await this.clientRepository.AddAsync(c);
            await this.clientRepository.SaveChangesAsync();
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

        public async Task<bool> DeleteClient(Client c)
        {
            try
            {
                this.clientRepository.Delete(c);

                await this.clientRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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

        public async Task<Client> GetSignleClient(Guid eik)
        {
            var clieent = this.clientRepository.Find(eik);

            return clieent;
        }
    }
}
