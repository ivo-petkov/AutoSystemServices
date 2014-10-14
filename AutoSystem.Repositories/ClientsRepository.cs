using AutoSystem.DataLayer;
using AutoSystem.Models;
using System;
using System.Collections.Generic;
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

        public Client GetById(int clientId)
        {
            return dbContext.Clients.FirstOrDefault(u => u.ClientId == clientId);
        }
        
        public bool CheckForCars(int id)
        {
            var clent = this.Get(id);

            if (clent.Cars == null || clent.Cars.Count == 0)
            {
                return false;
            }
            return true;
        }

        public Client GetByName(string name)
        {
            return dbContext.Clients.FirstOrDefault(u => u.Name == name);
        }

        public bool EditPerformers(System.Collections.Generic.List<Performer> editedPerformers, int id)
        {
            var client = this.Get(id);
            if (client == null)
            {
                return false;
            }

            int count = client.Performers.Count;
            var clientPerformers = client.Performers.ToList();
            for (int i = 0; i < count; i++)
            {
                if (!editedPerformers.Contains(clientPerformers[i]))
                {
                    client.Performers.Remove(clientPerformers[i]);
                }
            }

            foreach (var performer in editedPerformers)
            {
                if (!client.Performers.Contains(performer))
                {
                    client.Performers.Add(performer);
                }
            }  

            //List<Performer> performers = new List<Performer>(client.Performers);
            client.Performers = editedPerformers;
            dbContext.SaveChanges();
            return true;
        }
    }    
}
