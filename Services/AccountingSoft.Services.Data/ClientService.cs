using AccountingSoft.Data.Common.Repositories;
using AccountingSoft.Data.Models;
using System.Threading.Tasks;

namespace AccountingSoft.Services.Data
{
    public class ClientService : IClientService
    {
        private readonly IDeletableEntityRepository<Client> clientRepository;

        public ClientService(IDeletableEntityRepository<Client> clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public async Task AddClient(Client c)
        {
            await this.clientRepository.AddAsync(c);

            await this.clientRepository.SaveChangesAsync();
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
    }
}
