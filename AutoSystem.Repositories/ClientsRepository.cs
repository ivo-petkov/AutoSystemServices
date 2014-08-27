using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Linq;

namespace AutoSystem.Repositories
{
    public class ClientsRepository : EfRepository<Client>
    {
        private AutoSystemContext dbContext;

        public ClientsRepository(AutoSystemContext context)
            : base(context)
        {
            this.dbContext = context;
        }

        public Client CheckIfClientExist(Client client)
        {
            return dbContext.Clients.FirstOrDefault(u => (u.Name == client.Name && u.Telephone == client.Telephone));
        }
    }    
}
