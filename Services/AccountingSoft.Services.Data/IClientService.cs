using AccountingSoft.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSoft.Services.Data
{
    public interface IClientService
    {
        Task AddClient(Client c);

        void EditClient(Client c);

        Task<bool> DeleteClient(Client c);
    }
}
