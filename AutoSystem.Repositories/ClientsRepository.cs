using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;

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
    }    
}
